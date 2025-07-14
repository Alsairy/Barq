using Microsoft.Extensions.Logging;
using AutoMapper;
using BARQ.Core.Interfaces;
using BARQ.Core.Entities;
using BARQ.Core.Repositories;
using BARQ.Core.Enums;
using BARQ.Core.Models.Requests;
using BARQ.Core.Services;

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

    public async Task<AITaskResult> ExecuteTaskAsync(AITask task, CancellationToken cancellationToken = default)
    {
        try
        {
            var tenantId = _tenantProvider.GetTenantId();
            
            task.Status = AITaskStatus.Running;
            task.UpdatedAt = DateTime.UtcNow;
            
            await _aiTaskRepository.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("AI_TASK_STARTED", $"AI task started: {task.TaskType}", task.Id);

            var bestProvider = await GetBestProviderAsync(task.TaskType, tenantId, null);
            if (bestProvider == null)
            {
                task.Status = AITaskStatus.Failed;
                task.ErrorMessage = "No suitable AI provider found";
                await _aiTaskRepository.UpdateAsync(task);
                await _unitOfWork.SaveChangesAsync();

                return new AITaskResult
                {
                    TaskId = task.Id,
                    Success = false,
                    ErrorMessage = "No suitable AI provider found",
                    ExecutionTimeMs = 0,
                    Provider = AIProvider.OpenAI
                };
            }

            task.AIProvider = bestProvider.Provider;
            task.Status = AITaskStatus.Running;
            task.StartedAt = DateTime.UtcNow;
            await _aiTaskRepository.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();

            var result = await ExecuteTaskWithProviderAsync(task, bestProvider, cancellationToken);

            task.Status = result.Success ? AITaskStatus.Completed : AITaskStatus.Failed;
            task.OutputData = result.ResultData;
            task.ErrorMessage = result.ErrorMessage;
            task.CompletedAt = DateTime.UtcNow;
            task.UpdatedAt = DateTime.UtcNow;

            await _aiTaskRepository.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("AI_TASK_COMPLETED", $"AI task completed: {task.Status}", task.Id);

            _logger.LogInformation("AI task executed: {TaskId} with status {Status}", task.Id, task.Status);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing AI task: {TaskType}", task.TaskType);
            return new AITaskResult
            {
                TaskId = task.Id,
                Success = false,
                ErrorMessage = ex.Message,
                ExecutionTimeMs = 0,
                Provider = AIProvider.OpenAI
            };
        }
    }

    public async Task<IEnumerable<AITaskResult>> ExecuteTasksAsync(IEnumerable<AITask> tasks, CancellationToken cancellationToken = default)
    {
        try
        {
            var results = new List<AITaskResult>();
            var taskList = new List<Task<AITaskResult>>();

            foreach (var task in tasks)
            {
                taskList.Add(ExecuteTaskAsync(task, cancellationToken));
            }

            var completedTasks = await Task.WhenAll(taskList);
            results.AddRange(completedTasks);

            _logger.LogInformation("Executed {Count} AI tasks in batch", tasks.Count());
            return results;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing multiple AI tasks");
            return new List<AITaskResult>();
        }
    }

    public async Task<AIProviderConfiguration?> GetBestProviderAsync(AITaskType taskType, Guid tenantId, AITaskRequirements? requirements = null)
    {
        try
        {
            var providers = await _providerRepository.FindAsync(p => 
                p.TenantId == tenantId && 
                p.IsActive && 
                p.IsHealthy &&
                p.SupportedTaskTypes != null && p.SupportedTaskTypes.Contains(taskType.ToString()));

            if (!providers.Any())
            {
                _logger.LogWarning("No active providers found for task type: {TaskType}", taskType);
                return null;
            }

            var bestProvider = providers
                .Where(p => requirements == null || MeetsRequirements(p, requirements))
                .OrderByDescending(p => p.SuccessRate ?? 0)
                .ThenBy(p => p.CostPerToken ?? decimal.MaxValue)
                .ThenByDescending(p => p.AverageResponseTimeMs ?? long.MaxValue)
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

    public async Task<AITaskStatus> GetTaskStatusAsync(Guid taskId)
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
                Success = task.Status == AITaskStatus.Completed,
                ResultData = task.OutputData,
                ErrorMessage = task.ErrorMessage,
                ExecutionTimeMs = task.ProcessingTimeMs ?? 0,
                Cost = task.Cost ?? 0,
                QualityScore = task.QualityScore ?? 0,
                Provider = AIProvider.OpenAI
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting task result: {TaskId}", taskId);
            return null;
        }
    }

    public async Task<bool> CancelTaskAsync(Guid taskId)
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

    public Task<AIProviderValidationResult> ValidateProviderAsync(AIProviderConfiguration configuration, CancellationToken cancellationToken = default)
    {
        try
        {
            var validationErrors = new List<string>();

            if (string.IsNullOrEmpty(configuration.Name))
                validationErrors.Add("Provider name is required");

            if (string.IsNullOrEmpty(configuration.EndpointUrl))
                validationErrors.Add("API endpoint is required");

            if (string.IsNullOrEmpty(configuration.ApiKey))
                validationErrors.Add("API key is required");

            if (configuration.SupportedTaskTypes == null || !configuration.SupportedTaskTypes.Any())
                validationErrors.Add("At least one supported task type is required");

            var isValid = !validationErrors.Any();

            return Task.FromResult(new AIProviderValidationResult
            {
                IsValid = isValid,
                Errors = validationErrors
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating provider configuration: {ProviderId}", configuration.Id);
            return Task.FromResult(new AIProviderValidationResult
            {
                IsValid = false,
                Errors = new List<string> { ex.Message }
            });
        }
    }

    public async Task<AIProviderHealthStatus> GetProviderHealthAsync(Guid providerId, CancellationToken cancellationToken = default)
    {
        try
        {
            var provider = await _providerRepository.GetByIdAsync(providerId);
            if (provider == null)
            {
                return new AIProviderHealthStatus
                {
                    IsHealthy = false,
                    CheckedAt = DateTime.UtcNow,
                    Issues = new List<string> { "Provider not found" }
                };
            }

            var healthStatus = new AIProviderHealthStatus
            {
                IsHealthy = provider.IsHealthy,
                CheckedAt = DateTime.UtcNow,
                ResponseTimeMs = provider.AverageResponseTimeMs ?? 0,
                ErrorRate = 100 - (provider.SuccessRate ?? 0),
                AvailableCapacity = 100,
                Issues = provider.IsHealthy ? new List<string>() : new List<string> { "Provider is unhealthy" }
            };

            return healthStatus;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking provider health: {ProviderId}", providerId);
            return new AIProviderHealthStatus
            {
                IsHealthy = false,
                CheckedAt = DateTime.UtcNow,
                Issues = new List<string> { $"Health check failed: {ex.Message}" }
            };
        }
    }

    public async Task UpdateProviderMetricsAsync(Guid providerId, AIProviderMetrics metrics)
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
            provider.AverageResponseTimeMs = (long)metrics.AverageResponseTimeMs;
            provider.TotalCost = metrics.TotalCost;
            provider.IsHealthy = metrics.SuccessRate > 80;
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

            var tasks = await _aiTaskRepository.GetAsync(
                projectId.HasValue ? t => t.ProjectId == projectId.Value : null,
                orderBy: q => q.OrderByDescending(t => t.CreatedAt),
                take: limit);

            var results = tasks.Select(task => new AITaskResult
            {
                TaskId = task.Id,
                Success = task.Status == AITaskStatus.Completed,
                ResultData = task.OutputData,
                ErrorMessage = task.ErrorMessage,
                ExecutionTimeMs = task.ProcessingTimeMs ?? 0,
                Cost = task.Cost ?? 0,
                QualityScore = task.QualityScore ?? 0,
                Provider = AIProvider.OpenAI
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
                Success = true,
                ResultData = $"Mock AI task result for {task.TaskType}",
                ExecutionTimeMs = (long)executionTime.TotalMilliseconds,
                Cost = 0.10m,
                Provider = AIProvider.OpenAI
            };

            await LogAuditAsync("AI_TASK_EXECUTED", $"AI task executed with provider {provider.Name}", task.Id);

            return result;
        }
        catch (OperationCanceledException)
        {
            return new AITaskResult
            {
                TaskId = task.Id,
                Success = false,
                ErrorMessage = "Task was cancelled",
                ExecutionTimeMs = 0,
                Provider = AIProvider.OpenAI
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing task with provider: {ProviderId}", provider.Id);
            return new AITaskResult
            {
                TaskId = task.Id,
                Success = false,
                ErrorMessage = ex.Message,
                ExecutionTimeMs = 0,
                Provider = AIProvider.OpenAI
            };
        }
    }

    private bool MeetsRequirements(AIProviderConfiguration provider, AITaskRequirements requirements)
    {
        if (requirements.MaxCost.HasValue && provider.AverageCost > requirements.MaxCost.Value)
            return false;


        return true;
    }

    public async Task<AITaskResult> RetryTaskAsync(Guid taskId, CancellationToken cancellationToken = default)
    {
        try
        {
            var task = await _aiTaskRepository.GetByIdAsync(taskId);
            if (task == null)
            {
                _logger.LogWarning("AI task not found for retry: {TaskId}", taskId);
                return new AITaskResult
                {
                    TaskId = taskId,
                    Success = false,
                    ErrorMessage = "Task not found",
                    ExecutionTimeMs = 0,
                    Provider = AIProvider.OpenAI
                };
            }

            if (task.Status != AITaskStatus.Failed)
            {
                _logger.LogWarning("Cannot retry task that is not in failed status: {Status}", task.Status);
                return new AITaskResult
                {
                    TaskId = taskId,
                    Success = false,
                    ErrorMessage = "Task is not in failed status",
                    ExecutionTimeMs = 0,
                    Provider = AIProvider.OpenAI
                };
            }

            task.Status = AITaskStatus.Pending;
            task.ErrorMessage = null;
            task.UpdatedAt = DateTime.UtcNow;

            await _aiTaskRepository.UpdateAsync(task);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("AI_TASK_RETRY", $"AI task retry initiated: {taskId}", taskId);

            var result = await ExecuteTaskAsync(task, cancellationToken);

            _logger.LogInformation("AI task retried: {TaskId} with status {Status}", taskId, result.Success);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrying task: {TaskId}", taskId);
            return new AITaskResult
            {
                TaskId = taskId,
                Success = false,
                ErrorMessage = ex.Message,
                ExecutionTimeMs = 0,
                Provider = AIProvider.OpenAI
            };
        }
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
                EntityId = entityId ?? Guid.Empty,
                Description = description,
                UserId = _tenantProvider.GetCurrentUserId(),
                TenantId = _tenantProvider.GetTenantId(),
                Timestamp = DateTime.UtcNow,
                IPAddress = "127.0.0.1"
            };

            await _auditLogRepository.AddAsync(auditLog);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging audit: {Action}", action);
        }
    }

    public async Task<AITaskResult> CreateAITaskAsync(CreateAITaskRequest request)
    {
        await Task.CompletedTask;
        throw new NotImplementedException("AI task creation not yet implemented");
    }

    public async Task<AITaskResult> ExecuteAITaskAsync(Guid taskId)
    {
        await Task.CompletedTask;
        throw new NotImplementedException("AI task execution not yet implemented");
    }

    public async Task<AITaskStatus> GetAITaskStatusAsync(Guid taskId)
    {
        await Task.CompletedTask;
        throw new NotImplementedException("AI task status retrieval not yet implemented");
    }

    public async Task<bool> CancelAITaskAsync(Guid taskId)
    {
        await Task.CompletedTask;
        throw new NotImplementedException("AI task cancellation not yet implemented");
    }

    public async Task<AITaskResult> GetAITaskResultsAsync(Guid taskId)
    {
        await Task.CompletedTask;
        throw new NotImplementedException("AI task results retrieval not yet implemented");
    }

    public async Task<IEnumerable<AITask>> GetProjectAITasksAsync(Guid projectId)
    {
        await Task.CompletedTask;
        throw new NotImplementedException("Project AI tasks retrieval not yet implemented");
    }

    public async Task<object> GetAITaskAnalyticsAsync()
    {
        await Task.CompletedTask;
        throw new NotImplementedException("AI task analytics not yet implemented");
    }

    public async Task<IEnumerable<AIProviderConfiguration>> GetAvailableProvidersAsync()
    {
        await Task.CompletedTask;
        throw new NotImplementedException("Available providers retrieval not yet implemented");
    }

    public async Task<AIProviderHealthStatus> CheckProviderHealthAsync(Guid providerId)
    {
        await Task.CompletedTask;
        throw new NotImplementedException("Provider health check not yet implemented");
    }

    public async Task<AIProviderConfiguration> ConfigureProviderAsync(ConfigureAIProviderRequest request)
    {
        await Task.CompletedTask;
        throw new NotImplementedException("Provider configuration not yet implemented");
    }

    public async Task<object> ExecuteBatchTasksAsync(ExecuteBatchAITasksRequest request)
    {
        await Task.CompletedTask;
        throw new NotImplementedException("Batch tasks execution not yet implemented");
    }

    public async Task<object> GetQueueStatusAsync()
    {
        await Task.CompletedTask;
        throw new NotImplementedException("Queue status retrieval not yet implemented");
    }

    public async Task<object> GetCostAnalysisAsync()
    {
        await Task.CompletedTask;
        throw new NotImplementedException("Cost analysis not yet implemented");
    }
}
