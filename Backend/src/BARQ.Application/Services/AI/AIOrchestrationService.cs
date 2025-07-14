using Microsoft.Extensions.Logging;
using AutoMapper;
using BARQ.Core.Interfaces;
using BARQ.Core.Entities;
using BARQ.Core.Repositories;
using BARQ.Core.Enums;
using BARQ.Infrastructure.MultiTenancy;

namespace BARQ.Application.Services.AI;

public class AIOrchestrationService : IAIOrchestrationService
{
    private readonly IRepository<AITask> _aiTaskRepository;
    private readonly IRepository<AIProviderConfiguration> _providerRepository;
    private readonly IRepository<AuditLog> _auditLogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<AIOrchestrationService> _logger;
    private readonly ITenantProvider _tenantProvider;

    public AIOrchestrationService(
        IRepository<AITask> aiTaskRepository,
        IRepository<AIProviderConfiguration> providerRepository,
        IRepository<AuditLog> auditLogRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<AIOrchestrationService> logger,
        ITenantProvider tenantProvider)
    {
        _aiTaskRepository = aiTaskRepository;
        _providerRepository = providerRepository;
        _auditLogRepository = auditLogRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _tenantProvider = tenantProvider;
    }

    public async Task<AITaskResult> ExecuteTaskAsync(AITaskRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var tenantId = _tenantProvider.GetTenantId();
            
            var aiTask = new AITask
            {
                Id = Guid.NewGuid(),
                TaskType = request.TaskType,
                Priority = request.Priority,
                Status = AITaskStatus.Pending,
                Input = System.Text.Json.JsonSerializer.Serialize(request.Input),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                ProjectId = request.ProjectId,
                UserId = request.UserId,
                TenantId = tenantId,
                MaxRetries = request.MaxRetries ?? 3,
                RetryCount = 0
            };

            await _aiTaskRepository.AddAsync(aiTask);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("AI_TASK_CREATED", $"AI task created: {request.TaskType}", aiTask.Id);

            var bestProvider = await GetBestProviderAsync(request.TaskType, request.Requirements, cancellationToken);
            if (bestProvider == null)
            {
                aiTask.Status = AITaskStatus.Failed;
                aiTask.ErrorMessage = "No suitable AI provider found";
                await _aiTaskRepository.UpdateAsync(aiTask);
                await _unitOfWork.SaveChangesAsync();

                return new AITaskResult
                {
                    TaskId = aiTask.Id,
                    Status = AITaskStatus.Failed,
                    ErrorMessage = "No suitable AI provider found",
                    ExecutionTime = TimeSpan.Zero,
                    Cost = 0,
                    QualityScore = 0
                };
            }

            aiTask.ProviderId = bestProvider.Id;
            aiTask.Status = AITaskStatus.Running;
            aiTask.StartedAt = DateTime.UtcNow;
            await _aiTaskRepository.UpdateAsync(aiTask);
            await _unitOfWork.SaveChangesAsync();

            var result = await ExecuteTaskWithProviderAsync(aiTask, bestProvider, cancellationToken);

            aiTask.Status = result.Status;
            aiTask.Output = result.Output;
            aiTask.ErrorMessage = result.ErrorMessage;
            aiTask.CompletedAt = DateTime.UtcNow;
            aiTask.ExecutionTime = result.ExecutionTime;
            aiTask.Cost = result.Cost;
            aiTask.QualityScore = result.QualityScore;

            await _aiTaskRepository.UpdateAsync(aiTask);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("AI_TASK_COMPLETED", $"AI task completed: {aiTask.Status}", aiTask.Id);

            _logger.LogInformation("AI task executed: {TaskId} with status {Status}", aiTask.Id, result.Status);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing AI task: {TaskType}", request.TaskType);
            return new AITaskResult
            {
                TaskId = Guid.NewGuid(),
                Status = AITaskStatus.Failed,
                ErrorMessage = ex.Message,
                ExecutionTime = TimeSpan.Zero,
                Cost = 0,
                QualityScore = 0
            };
        }
    }

    public async Task<List<AITaskResult>> ExecuteMultipleTasksAsync(List<AITaskRequest> requests, CancellationToken cancellationToken = default)
    {
        try
        {
            var results = new List<AITaskResult>();
            var tasks = new List<Task<AITaskResult>>();

            foreach (var request in requests)
            {
                tasks.Add(ExecuteTaskAsync(request, cancellationToken));
            }

            var completedTasks = await Task.WhenAll(tasks);
            results.AddRange(completedTasks);

            _logger.LogInformation("Executed {Count} AI tasks in batch", requests.Count);
            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing multiple AI tasks");
            return new List<AITaskResult>();
        }
    }

    public async Task<AIProviderConfiguration?> GetBestProviderAsync(AITaskType taskType, AITaskRequirements? requirements = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var tenantId = _tenantProvider.GetTenantId();
            var providers = await _providerRepository.FindAsync(p => 
                p.TenantId == tenantId && 
                p.IsActive && 
                p.IsHealthy &&
                p.SupportedTaskTypes.Contains(taskType.ToString()));

            if (!providers.Any())
            {
                _logger.LogWarning("No active providers found for task type: {TaskType}", taskType);
                return null;
            }

            var bestProvider = providers
                .Where(p => requirements == null || MeetsRequirements(p, requirements))
                .OrderByDescending(p => p.SuccessRate ?? 0)
                .ThenBy(p => p.AverageCost ?? decimal.MaxValue)
                .ThenByDescending(p => p.AverageResponseTime ?? TimeSpan.MaxValue)
                .FirstOrDefault();

            if (bestProvider != null)
            {
                _logger.LogInformation("Selected provider {ProviderId} for task type {TaskType}", bestProvider.Id, taskType);
            }

            return bestProvider;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting best provider for task type: {TaskType}", taskType);
            return null;
        }
    }

    public async Task<AITaskStatus> GetTaskStatusAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        try
        {
            var task = await _aiTaskRepository.GetByIdAsync(taskId);
            if (task == null)
            {
                _logger.LogWarning("AI task not found: {TaskId}", taskId);
                return AITaskStatus.Failed;
            }

            return task.Status;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting task status: {TaskId}", taskId);
            return AITaskStatus.Failed;
        }
    }

    public async Task<AITaskResult?> GetTaskResultAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        try
        {
            var task = await _aiTaskRepository.GetByIdAsync(taskId);
            if (task == null)
            {
                _logger.LogWarning("AI task not found: {TaskId}", taskId);
                return null;
            }

            return new AITaskResult
            {
                TaskId = task.Id,
                Status = task.Status,
                Output = task.Output,
                ErrorMessage = task.ErrorMessage,
                ExecutionTime = task.ExecutionTime ?? TimeSpan.Zero,
                Cost = task.Cost ?? 0,
                QualityScore = task.QualityScore ?? 0,
                ProviderId = task.ProviderId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting task result: {TaskId}", taskId);
            return null;
        }
    }

    public async Task<bool> CancelTaskAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        try
        {
            var task = await _aiTaskRepository.GetByIdAsync(taskId);
            if (task == null)
            {
                _logger.LogWarning("AI task not found for cancellation: {TaskId}", taskId);
                return false;
            }

            if (task.Status == AITaskStatus.Completed || task.Status == AITaskStatus.Failed || task.Status == AITaskStatus.Cancelled)
            {
                _logger.LogWarning("Cannot cancel task in status: {Status}", task.Status);
                return false;
            }

            task.Status = AITaskStatus.Cancelled;
            task.CompletedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;

            await _aiTaskRepository.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("AI_TASK_CANCELLED", $"AI task cancelled: {taskId}", taskId);

            _logger.LogInformation("AI task cancelled: {TaskId}", taskId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling task: {TaskId}", taskId);
            return false;
        }
    }

    public async Task<AIProviderValidationResult> ValidateProviderConfigurationAsync(Guid providerId, CancellationToken cancellationToken = default)
    {
        try
        {
            var provider = await _providerRepository.GetByIdAsync(providerId);
            if (provider == null)
            {
                return new AIProviderValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Provider not found"
                };
            }

            var validationErrors = new List<string>();

            if (string.IsNullOrEmpty(provider.Name))
                validationErrors.Add("Provider name is required");

            if (string.IsNullOrEmpty(provider.ApiEndpoint))
                validationErrors.Add("API endpoint is required");

            if (string.IsNullOrEmpty(provider.ApiKey))
                validationErrors.Add("API key is required");

            if (!provider.SupportedTaskTypes.Any())
                validationErrors.Add("At least one supported task type is required");

            var isValid = !validationErrors.Any();

            return new AIProviderValidationResult
            {
                IsValid = isValid,
                ErrorMessage = isValid ? null : string.Join("; ", validationErrors),
                ValidationErrors = validationErrors
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating provider configuration: {ProviderId}", providerId);
            return new AIProviderValidationResult
            {
                IsValid = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<AIProviderHealthStatus> CheckProviderHealthAsync(Guid providerId, CancellationToken cancellationToken = default)
    {
        try
        {
            var provider = await _providerRepository.GetByIdAsync(providerId);
            if (provider == null)
            {
                return new AIProviderHealthStatus
                {
                    ProviderId = providerId,
                    IsHealthy = false,
                    Status = "Provider not found",
                    LastChecked = DateTime.UtcNow
                };
            }

            var healthStatus = new AIProviderHealthStatus
            {
                ProviderId = providerId,
                IsHealthy = provider.IsHealthy,
                Status = provider.IsHealthy ? "Healthy" : "Unhealthy",
                ResponseTime = provider.AverageResponseTime ?? TimeSpan.Zero,
                LastChecked = DateTime.UtcNow,
                SuccessRate = provider.SuccessRate ?? 0,
                ErrorRate = 100 - (provider.SuccessRate ?? 0)
            };

            return healthStatus;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking provider health: {ProviderId}", providerId);
            return new AIProviderHealthStatus
            {
                ProviderId = providerId,
                IsHealthy = false,
                Status = $"Health check failed: {ex.Message}",
                LastChecked = DateTime.UtcNow
            };
        }
    }

    public async Task UpdateProviderMetricsAsync(Guid providerId, AIProviderMetrics metrics, CancellationToken cancellationToken = default)
    {
        try
        {
            var provider = await _providerRepository.GetByIdAsync(providerId);
            if (provider == null)
            {
                _logger.LogWarning("Provider not found for metrics update: {ProviderId}", providerId);
                return;
            }

            provider.SuccessRate = metrics.SuccessRate;
            provider.AverageResponseTime = metrics.AverageResponseTime;
            provider.AverageCost = metrics.AverageCost;
            provider.IsHealthy = metrics.IsHealthy;
            provider.UpdatedAt = DateTime.UtcNow;

            await _providerRepository.UpdateAsync(provider);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("PROVIDER_METRICS_UPDATED", $"Provider metrics updated: {providerId}", providerId);

            _logger.LogInformation("Provider metrics updated: {ProviderId}", providerId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating provider metrics: {ProviderId}", providerId);
        }
    }

    public async Task<List<AITaskResult>> GetTaskHistoryAsync(Guid? projectId = null, Guid? userId = null, int limit = 100, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _aiTaskRepository.GetQueryable();

            if (projectId.HasValue)
                query = query.Where(t => t.ProjectId == projectId.Value);

            if (userId.HasValue)
                query = query.Where(t => t.UserId == userId.Value);

            var tasks = await _aiTaskRepository.FindAsync(
                query.OrderByDescending(t => t.CreatedAt).Take(limit).ToList().AsQueryable());

            var results = tasks.Select(task => new AITaskResult
            {
                TaskId = task.Id,
                Status = task.Status,
                Output = task.Output,
                ErrorMessage = task.ErrorMessage,
                ExecutionTime = task.ExecutionTime ?? TimeSpan.Zero,
                Cost = task.Cost ?? 0,
                QualityScore = task.QualityScore ?? 0,
                ProviderId = task.ProviderId
            }).ToList();

            _logger.LogInformation("Retrieved {Count} task history records", results.Count);
            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting task history");
            return new List<AITaskResult>();
        }
    }

    private async Task<AITaskResult> ExecuteTaskWithProviderAsync(AITask task, AIProviderConfiguration provider, CancellationToken cancellationToken)
    {
        try
        {
            var startTime = DateTime.UtcNow;

            await Task.Delay(1000, cancellationToken);

            var endTime = DateTime.UtcNow;
            var executionTime = endTime - startTime;

            var result = new AITaskResult
            {
                TaskId = task.Id,
                Status = AITaskStatus.Completed,
                Output = $"Mock AI task result for {task.TaskType}",
                ExecutionTime = executionTime,
                Cost = 0.10m,
                QualityScore = 0.95m,
                ProviderId = provider.Id
            };

            await LogAuditAsync("AI_TASK_EXECUTED", $"AI task executed with provider {provider.Name}", task.Id);

            return result;
        }
        catch (OperationCanceledException)
        {
            return new AITaskResult
            {
                TaskId = task.Id,
                Status = AITaskStatus.Cancelled,
                ErrorMessage = "Task was cancelled",
                ExecutionTime = TimeSpan.Zero,
                Cost = 0,
                QualityScore = 0,
                ProviderId = provider.Id
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing task with provider: {ProviderId}", provider.Id);
            return new AITaskResult
            {
                TaskId = task.Id,
                Status = AITaskStatus.Failed,
                ErrorMessage = ex.Message,
                ExecutionTime = TimeSpan.Zero,
                Cost = 0,
                QualityScore = 0,
                ProviderId = provider.Id
            };
        }
    }

    private bool MeetsRequirements(AIProviderConfiguration provider, AITaskRequirements requirements)
    {
        if (requirements.MaxCost.HasValue && provider.AverageCost > requirements.MaxCost.Value)
            return false;

        if (requirements.MaxResponseTime.HasValue && provider.AverageResponseTime > requirements.MaxResponseTime.Value)
            return false;

        if (requirements.MinQualityScore.HasValue && (provider.SuccessRate ?? 0) < requirements.MinQualityScore.Value)
            return false;

        if (requirements.RequiredCapabilities?.Any() == true)
        {
            var providerCapabilities = provider.Capabilities ?? new List<string>();
            if (!requirements.RequiredCapabilities.All(cap => providerCapabilities.Contains(cap)))
                return false;
        }

        return true;
    }

    private async Task LogAuditAsync(string action, string description, Guid? entityId = null)
    {
        try
        {
            var auditLog = new AuditLog
            {
                Id = Guid.NewGuid(),
                Action = action,
                EntityName = "AITask",
                EntityId = entityId,
                Description = description,
                UserId = _tenantProvider.GetCurrentUserId(),
                TenantId = _tenantProvider.GetTenantId(),
                Timestamp = DateTime.UtcNow,
                IpAddress = "127.0.0.1"
            };

            await _auditLogRepository.AddAsync(auditLog);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging audit: {Action}", action);
        }
    }
}
