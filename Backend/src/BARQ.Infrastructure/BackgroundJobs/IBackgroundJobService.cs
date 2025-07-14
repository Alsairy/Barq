using System.Linq.Expressions;

namespace BARQ.Infrastructure.BackgroundJobs;

public interface IBackgroundJobService
{
    Task<string> EnqueueAsync<T>(Expression<Func<T, Task>> methodCall, TimeSpan? delay = null);
    Task<string> EnqueueAsync(Expression<Func<Task>> methodCall, TimeSpan? delay = null);
    Task<string> ScheduleAsync<T>(Expression<Func<T, Task>> methodCall, DateTime scheduledTime);
    Task<string> ScheduleAsync(Expression<Func<Task>> methodCall, DateTime scheduledTime);
    Task<string> ScheduleRecurringAsync<T>(string jobId, Expression<Func<T, Task>> methodCall, string cronExpression);
    Task<string> ScheduleRecurringAsync(string jobId, Expression<Func<Task>> methodCall, string cronExpression);
    Task<bool> DeleteAsync(string jobId);
    Task<JobStatus> GetJobStatusAsync(string jobId);
    Task<IEnumerable<JobInfo>> GetJobsAsync(JobState? state = null, int skip = 0, int take = 50);
    Task<JobStatistics> GetStatisticsAsync();
}

public interface IBackgroundJobProcessor
{
    Task ProcessJobAsync(string jobId, string jobData, CancellationToken cancellationToken = default);
    Task<bool> CanProcessJobAsync(string jobType);
}

public enum JobState
{
    Enqueued,
    Processing,
    Succeeded,
    Failed,
    Deleted,
    Scheduled
}

public class JobStatus
{
    public string JobId { get; set; } = string.Empty;
    public JobState State { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? ErrorMessage { get; set; }
    public int RetryCount { get; set; }
    public int MaxRetries { get; set; }
    public Dictionary<string, object> Properties { get; set; } = new();
}

public class JobInfo
{
    public string JobId { get; set; } = string.Empty;
    public string JobType { get; set; } = string.Empty;
    public string MethodName { get; set; } = string.Empty;
    public JobState State { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public TimeSpan? Duration { get; set; }
    public string? ErrorMessage { get; set; }
    public int RetryCount { get; set; }
}

public class JobStatistics
{
    public long EnqueuedJobs { get; set; }
    public long ProcessingJobs { get; set; }
    public long SucceededJobs { get; set; }
    public long FailedJobs { get; set; }
    public long ScheduledJobs { get; set; }
    public long DeletedJobs { get; set; }
    public double AverageProcessingTime { get; set; }
    public DateTime LastUpdated { get; set; }
}
