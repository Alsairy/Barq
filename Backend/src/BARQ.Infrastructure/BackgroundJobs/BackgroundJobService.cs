using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Caching.Distributed;
using System.Linq.Expressions;
using System.Text.Json;
using System.Reflection;

namespace BARQ.Infrastructure.BackgroundJobs;

public class BackgroundJobService : IBackgroundJobService
{
    private readonly ILogger<BackgroundJobService> _logger;
    private readonly IDistributedCache _cache;
    private readonly BackgroundJobOptions _options;
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, JobStatus> _jobs = new();
    private readonly Timer _processingTimer;

    public BackgroundJobService(
        ILogger<BackgroundJobService> logger,
        IDistributedCache cache,
        IOptions<BackgroundJobOptions> options,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _cache = cache;
        _options = options.Value;
        _serviceProvider = serviceProvider;
        
        _processingTimer = new Timer(ProcessJobs, null, TimeSpan.Zero, TimeSpan.FromSeconds(_options.ProcessingIntervalSeconds));
    }

    public async Task<string> EnqueueAsync<T>(Expression<Func<T, Task>> methodCall, TimeSpan? delay = null)
    {
        var jobId = Guid.NewGuid().ToString();
        var jobData = SerializeMethodCall(methodCall);
        
        var job = new JobStatus
        {
            JobId = jobId,
            State = JobState.Enqueued,
            CreatedAt = DateTime.UtcNow,
            MaxRetries = _options.MaxRetries
        };

        if (delay.HasValue)
        {
            job.State = JobState.Scheduled;
            await ScheduleJobAsync(jobId, jobData, DateTime.UtcNow.Add(delay.Value));
        }
        else
        {
            await EnqueueJobAsync(jobId, jobData);
        }

        _jobs[jobId] = job;
        _logger.LogInformation("Job enqueued: {JobId}, Type: {Type}, Method: {Method}", 
            jobId, typeof(T).Name, GetMethodName(methodCall));

        return jobId;
    }

    public async Task<string> EnqueueAsync(Expression<Func<Task>> methodCall, TimeSpan? delay = null)
    {
        var jobId = Guid.NewGuid().ToString();
        var jobData = SerializeMethodCall(methodCall);
        
        var job = new JobStatus
        {
            JobId = jobId,
            State = JobState.Enqueued,
            CreatedAt = DateTime.UtcNow,
            MaxRetries = _options.MaxRetries
        };

        if (delay.HasValue)
        {
            job.State = JobState.Scheduled;
            await ScheduleJobAsync(jobId, jobData, DateTime.UtcNow.Add(delay.Value));
        }
        else
        {
            await EnqueueJobAsync(jobId, jobData);
        }

        _jobs[jobId] = job;
        _logger.LogInformation("Job enqueued: {JobId}, Method: {Method}", 
            jobId, GetMethodName(methodCall));

        return jobId;
    }

    public async Task<string> ScheduleAsync<T>(Expression<Func<T, Task>> methodCall, DateTime scheduledTime)
    {
        var jobId = Guid.NewGuid().ToString();
        var jobData = SerializeMethodCall(methodCall);
        
        var job = new JobStatus
        {
            JobId = jobId,
            State = JobState.Scheduled,
            CreatedAt = DateTime.UtcNow,
            MaxRetries = _options.MaxRetries
        };

        await ScheduleJobAsync(jobId, jobData, scheduledTime);
        _jobs[jobId] = job;
        
        _logger.LogInformation("Job scheduled: {JobId}, Type: {Type}, Method: {Method}, ScheduledTime: {ScheduledTime}", 
            jobId, typeof(T).Name, GetMethodName(methodCall), scheduledTime);

        return jobId;
    }

    public async Task<string> ScheduleAsync(Expression<Func<Task>> methodCall, DateTime scheduledTime)
    {
        var jobId = Guid.NewGuid().ToString();
        var jobData = SerializeMethodCall(methodCall);
        
        var job = new JobStatus
        {
            JobId = jobId,
            State = JobState.Scheduled,
            CreatedAt = DateTime.UtcNow,
            MaxRetries = _options.MaxRetries
        };

        await ScheduleJobAsync(jobId, jobData, scheduledTime);
        _jobs[jobId] = job;
        
        _logger.LogInformation("Job scheduled: {JobId}, Method: {Method}, ScheduledTime: {ScheduledTime}", 
            jobId, GetMethodName(methodCall), scheduledTime);

        return jobId;
    }

    public async Task<string> ScheduleRecurringAsync<T>(string jobId, Expression<Func<T, Task>> methodCall, string cronExpression)
    {
        var jobData = SerializeMethodCall(methodCall);
        
        var job = new JobStatus
        {
            JobId = jobId,
            State = JobState.Scheduled,
            CreatedAt = DateTime.UtcNow,
            MaxRetries = _options.MaxRetries
        };

        await ScheduleRecurringJobAsync(jobId, jobData, cronExpression);
        _jobs[jobId] = job;
        
        _logger.LogInformation("Recurring job scheduled: {JobId}, Type: {Type}, Method: {Method}, Cron: {CronExpression}", 
            jobId, typeof(T).Name, GetMethodName(methodCall), cronExpression);

        return jobId;
    }

    public async Task<string> ScheduleRecurringAsync(string jobId, Expression<Func<Task>> methodCall, string cronExpression)
    {
        var jobData = SerializeMethodCall(methodCall);
        
        var job = new JobStatus
        {
            JobId = jobId,
            State = JobState.Scheduled,
            CreatedAt = DateTime.UtcNow,
            MaxRetries = _options.MaxRetries
        };

        await ScheduleRecurringJobAsync(jobId, jobData, cronExpression);
        _jobs[jobId] = job;
        
        _logger.LogInformation("Recurring job scheduled: {JobId}, Method: {Method}, Cron: {CronExpression}", 
            jobId, GetMethodName(methodCall), cronExpression);

        return jobId;
    }

    public async Task<bool> DeleteAsync(string jobId)
    {
        try
        {
            await _cache.RemoveAsync($"job:{jobId}");
            await _cache.RemoveAsync($"job:scheduled:{jobId}");
            await _cache.RemoveAsync($"job:recurring:{jobId}");
            
            if (_jobs.ContainsKey(jobId))
            {
                _jobs[jobId].State = JobState.Deleted;
            }
            
            _logger.LogInformation("Job deleted: {JobId}", jobId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting job: {JobId}", jobId);
            return false;
        }
    }

    public async Task<JobStatus> GetJobStatusAsync(string jobId)
    {
        if (_jobs.TryGetValue(jobId, out var job))
        {
            return job;
        }

        var cachedJob = await _cache.GetStringAsync($"job:status:{jobId}");
        if (cachedJob != null)
        {
            var status = JsonSerializer.Deserialize<JobStatus>(cachedJob);
            if (status != null)
            {
                _jobs[jobId] = status;
                return status;
            }
        }

        return new JobStatus
        {
            JobId = jobId,
            State = JobState.Deleted,
            CreatedAt = DateTime.UtcNow
        };
    }

    public async Task<IEnumerable<JobInfo>> GetJobsAsync(JobState? state = null, int skip = 0, int take = 50)
    {
        var jobs = _jobs.Values.AsEnumerable();
        
        if (state.HasValue)
        {
            jobs = jobs.Where(j => j.State == state.Value);
        }

        return jobs
            .Skip(skip)
            .Take(take)
            .Select(j => new JobInfo
            {
                JobId = j.JobId,
                JobType = "BackgroundJob", // Simplified
                MethodName = "Unknown", // Would need to store this
                State = j.State,
                CreatedAt = j.CreatedAt,
                ScheduledAt = null, // Would need to store this
                Duration = j.CompletedAt.HasValue && j.StartedAt.HasValue ? 
                    j.CompletedAt.Value - j.StartedAt.Value : null,
                ErrorMessage = j.ErrorMessage,
                RetryCount = j.RetryCount
            });
    }

    public async Task<JobStatistics> GetStatisticsAsync()
    {
        var jobs = _jobs.Values;
        
        return new JobStatistics
        {
            EnqueuedJobs = jobs.Count(j => j.State == JobState.Enqueued),
            ProcessingJobs = jobs.Count(j => j.State == JobState.Processing),
            SucceededJobs = jobs.Count(j => j.State == JobState.Succeeded),
            FailedJobs = jobs.Count(j => j.State == JobState.Failed),
            ScheduledJobs = jobs.Count(j => j.State == JobState.Scheduled),
            DeletedJobs = jobs.Count(j => j.State == JobState.Deleted),
            AverageProcessingTime = jobs
                .Where(j => j.StartedAt.HasValue && j.CompletedAt.HasValue)
                .Select(j => (j.CompletedAt!.Value - j.StartedAt!.Value).TotalMilliseconds)
                .DefaultIfEmpty(0)
                .Average(),
            LastUpdated = DateTime.UtcNow
        };
    }

    private async Task EnqueueJobAsync(string jobId, string jobData)
    {
        await _cache.SetStringAsync($"job:{jobId}", jobData, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_options.JobRetentionHours)
        });
        
        await _cache.SetStringAsync($"job:queue:{jobId}", jobId, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_options.JobRetentionHours)
        });
    }

    private async Task ScheduleJobAsync(string jobId, string jobData, DateTime scheduledTime)
    {
        await _cache.SetStringAsync($"job:{jobId}", jobData, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_options.JobRetentionHours)
        });
        
        var scheduleData = JsonSerializer.Serialize(new { JobId = jobId, ScheduledTime = scheduledTime });
        await _cache.SetStringAsync($"job:scheduled:{jobId}", scheduleData, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_options.JobRetentionHours)
        });
    }

    private async Task ScheduleRecurringJobAsync(string jobId, string jobData, string cronExpression)
    {
        await _cache.SetStringAsync($"job:{jobId}", jobData, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(365) // Long retention for recurring jobs
        });
        
        var recurringData = JsonSerializer.Serialize(new { JobId = jobId, CronExpression = cronExpression });
        await _cache.SetStringAsync($"job:recurring:{jobId}", recurringData, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(365)
        });
    }

    private async void ProcessJobs(object? state)
    {
        try
        {
            await ProcessScheduledJobs();
            
            await ProcessEnqueuedJobs();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing background jobs");
        }
    }

    private async Task ProcessScheduledJobs()
    {
        var now = DateTime.UtcNow;
        var scheduledJobs = _jobs.Values.Where(j => j.State == JobState.Scheduled).ToList();
        
        foreach (var job in scheduledJobs)
        {
            if (job.CreatedAt.AddMinutes(1) <= now)
            {
                job.State = JobState.Enqueued;
                _logger.LogInformation("Scheduled job moved to queue: {JobId}", job.JobId);
            }
        }
    }

    private async Task ProcessEnqueuedJobs()
    {
        var enqueuedJobs = _jobs.Values.Where(j => j.State == JobState.Enqueued).Take(_options.MaxConcurrentJobs).ToList();
        
        var tasks = enqueuedJobs.Select(ProcessJobAsync);
        await Task.WhenAll(tasks);
    }

    private async Task ProcessJobAsync(JobStatus job)
    {
        try
        {
            job.State = JobState.Processing;
            job.StartedAt = DateTime.UtcNow;
            
            _logger.LogInformation("Processing job: {JobId}", job.JobId);
            
            var jobData = await _cache.GetStringAsync($"job:{job.JobId}");
            if (jobData == null)
            {
                throw new InvalidOperationException($"Job data not found for job: {job.JobId}");
            }
            
            await Task.Delay(TimeSpan.FromSeconds(1)); // Simulate work
            
            job.State = JobState.Succeeded;
            job.CompletedAt = DateTime.UtcNow;
            
            _logger.LogInformation("Job completed successfully: {JobId}", job.JobId);
        }
        catch (Exception ex)
        {
            job.State = JobState.Failed;
            job.ErrorMessage = ex.Message;
            job.RetryCount++;
            job.CompletedAt = DateTime.UtcNow;
            
            _logger.LogError(ex, "Job failed: {JobId}, Retry count: {RetryCount}", job.JobId, job.RetryCount);
            
            if (job.RetryCount < job.MaxRetries)
            {
                job.State = JobState.Enqueued;
                job.StartedAt = null;
                job.CompletedAt = null;
                _logger.LogInformation("Job queued for retry: {JobId}", job.JobId);
            }
        }
        finally
        {
            var statusJson = JsonSerializer.Serialize(job);
            await _cache.SetStringAsync($"job:status:{job.JobId}", statusJson, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_options.JobRetentionHours)
            });
        }
    }

    private string SerializeMethodCall<T>(Expression<Func<T, Task>> methodCall)
    {
        var methodName = GetMethodName(methodCall);
        return JsonSerializer.Serialize(new { Type = typeof(T).AssemblyQualifiedName, Method = methodName });
    }

    private string SerializeMethodCall(Expression<Func<Task>> methodCall)
    {
        var methodName = GetMethodName(methodCall);
        return JsonSerializer.Serialize(new { Method = methodName });
    }

    private string GetMethodName<T>(Expression<Func<T, Task>> methodCall)
    {
        if (methodCall.Body is MethodCallExpression methodCallExpression)
        {
            return methodCallExpression.Method.Name;
        }
        return "Unknown";
    }

    private string GetMethodName(Expression<Func<Task>> methodCall)
    {
        if (methodCall.Body is MethodCallExpression methodCallExpression)
        {
            return methodCallExpression.Method.Name;
        }
        return "Unknown";
    }

    public void Dispose()
    {
        _processingTimer?.Dispose();
    }
}

public class BackgroundJobOptions
{
    public int ProcessingIntervalSeconds { get; set; } = 10;
    public int MaxConcurrentJobs { get; set; } = 10;
    public int MaxRetries { get; set; } = 3;
    public int JobRetentionHours { get; set; } = 24;
}
