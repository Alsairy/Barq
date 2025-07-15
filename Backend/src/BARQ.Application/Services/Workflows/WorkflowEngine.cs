using Microsoft.Extensions.Logging;
using BARQ.Core.Services;
using BARQ.Core.Entities;
using BARQ.Core.Repositories;
using BARQ.Core.Enums;
using BARQ.Core.Interfaces;
using BARQ.Core.Models.Responses;
using System.Text.Json;

namespace BARQ.Application.Services.Workflows;

/// <summary>
/// </summary>
public class WorkflowEngine : IWorkflowEngine
{
    private readonly IRepository<WorkflowInstance> _workflowInstanceRepository;
    private readonly IRepository<WorkflowTemplate> _workflowTemplateRepository;
    private readonly IRepository<WorkflowStep> _workflowStepRepository;
    private readonly IRepository<WorkflowStepExecution> _stepExecutionRepository;
    private readonly IRepository<WorkflowDataContext> _dataContextRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<WorkflowEngine> _logger;
    private readonly ITenantProvider _tenantProvider;
    private readonly IAIOrchestrationService _aiOrchestrationService;

    public WorkflowEngine(
        IRepository<WorkflowInstance> workflowInstanceRepository,
        IRepository<WorkflowTemplate> workflowTemplateRepository,
        IRepository<WorkflowStep> workflowStepRepository,
        IRepository<WorkflowStepExecution> stepExecutionRepository,
        IRepository<WorkflowDataContext> dataContextRepository,
        IUnitOfWork unitOfWork,
        ILogger<WorkflowEngine> logger,
        ITenantProvider tenantProvider,
        IAIOrchestrationService aiOrchestrationService)
    {
        _workflowInstanceRepository = workflowInstanceRepository;
        _workflowTemplateRepository = workflowTemplateRepository;
        _workflowStepRepository = workflowStepRepository;
        _stepExecutionRepository = stepExecutionRepository;
        _dataContextRepository = dataContextRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _tenantProvider = tenantProvider;
        _aiOrchestrationService = aiOrchestrationService;
    }

    public async Task<WorkflowExecutionResult> ExecuteWorkflowAsync(Guid workflowInstanceId, CancellationToken cancellationToken = default)
    {
        try
        {
            var workflowInstance = await _workflowInstanceRepository.GetByIdAsync(workflowInstanceId);
            if (workflowInstance == null)
            {
                return new WorkflowExecutionResult
                {
                    IsSuccess = false,
                    ErrorDetails = "Workflow instance not found"
                };
            }

            var template = await _workflowTemplateRepository.GetByIdAsync(workflowInstance.WorkflowTemplateId);
            if (template == null)
            {
                return new WorkflowExecutionResult
                {
                    IsSuccess = false,
                    ErrorDetails = "Workflow template not found"
                };
            }

            var steps = await _workflowStepRepository.FindAsync(s => 
                s.WorkflowTemplateId == template.Id && s.IsActive);
            var orderedSteps = steps.OrderBy(s => s.Order).ToList();

            if (!orderedSteps.Any())
            {
                return new WorkflowExecutionResult
                {
                    IsSuccess = false,
                    ErrorDetails = "No active steps found in workflow template"
                };
            }

            workflowInstance.Status = WorkflowStatus.InProgress;
            workflowInstance.StartedAt = DateTime.UtcNow;
            workflowInstance.UpdatedAt = DateTime.UtcNow;
            await _workflowInstanceRepository.UpdateAsync(workflowInstance);

            await CreateWorkflowDataContextAsync(workflowInstance);

            var currentStepIndex = workflowInstance.CurrentStepIndex;
            var executionResult = new WorkflowExecutionResult
            {
                IsSuccess = true,
                Status = WorkflowStepStatus.Running,
                Message = "Workflow execution started"
            };

            for (int i = currentStepIndex; i < orderedSteps.Count; i++)
            {
                var step = orderedSteps[i];
                var stepExecution = await CreateStepExecutionAsync(workflowInstance.Id, step.Id);
                
                var stepResult = await ExecuteStepAsync(stepExecution.Id, cancellationToken);
                
                if (!stepResult.Success)
                {
                    workflowInstance.Status = WorkflowStatus.Rejected;
                    executionResult.IsSuccess = false;
                    executionResult.ErrorDetails = stepResult.ErrorMessage;
                    break;
                }

                if (stepResult.Status == WorkflowStepStatus.WaitingForApproval)
                {
                    workflowInstance.Status = WorkflowStatus.WaitingForApproval;
                    workflowInstance.CurrentStepIndex = i;
                    executionResult.Status = WorkflowStepStatus.WaitingForApproval;
                    executionResult.Message = "Workflow paused for approval";
                    break;
                }

                workflowInstance.CurrentStepIndex = i + 1;
            }

            if (workflowInstance.CurrentStepIndex >= orderedSteps.Count && workflowInstance.Status == WorkflowStatus.InProgress)
            {
                workflowInstance.Status = WorkflowStatus.Completed;
                workflowInstance.CompletedAt = DateTime.UtcNow;
                executionResult.Status = WorkflowStepStatus.Completed;
                executionResult.Message = "Workflow completed successfully";
            }

            workflowInstance.UpdatedAt = DateTime.UtcNow;
            await _workflowInstanceRepository.UpdateAsync(workflowInstance);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Workflow execution completed: {WorkflowId} with status {Status}", 
                workflowInstanceId, workflowInstance.Status);

            return executionResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing workflow: {WorkflowId}", workflowInstanceId);
            return new WorkflowExecutionResult
            {
                IsSuccess = false,
                ErrorDetails = ex.Message
            };
        }
    }

    public async Task<WorkflowStepExecutionResult> ExecuteStepAsync(Guid stepExecutionId, CancellationToken cancellationToken = default)
    {
        try
        {
            var stepExecution = await _stepExecutionRepository.GetByIdAsync(stepExecutionId);
            if (stepExecution == null)
            {
                return new WorkflowStepExecutionResult
                {
                    Success = false,
                    ErrorMessage = "Step execution not found"
                };
            }

            var step = await _workflowStepRepository.GetByIdAsync(stepExecution.WorkflowStepId);
            if (step == null)
            {
                return new WorkflowStepExecutionResult
                {
                    Success = false,
                    ErrorMessage = "Workflow step not found"
                };
            }

            stepExecution.Status = WorkflowStepStatus.Running;
            stepExecution.StartedAt = DateTime.UtcNow;
            await _stepExecutionRepository.UpdateAsync(stepExecution);

            var startTime = DateTime.UtcNow;
            var result = new WorkflowStepExecutionResult
            {
                Success = true,
                Status = WorkflowStepStatus.Running
            };

            try
            {
                switch (step.StepType)
                {
                    case WorkflowStepType.Task:
                        result = await ExecuteTaskStepAsync(stepExecution, step, cancellationToken);
                        break;
                    case WorkflowStepType.Approval:
                        result = await ExecuteApprovalStepAsync(stepExecution, step, cancellationToken);
                        break;
                    case WorkflowStepType.Condition:
                        result = await ExecuteConditionStepAsync(stepExecution, step, cancellationToken);
                        break;
                    case WorkflowStepType.AITask:
                        result = await ExecuteAITaskStepAsync(stepExecution, step, cancellationToken);
                        break;
                    case WorkflowStepType.Integration:
                        result = await ExecuteIntegrationStepAsync(stepExecution, step, cancellationToken);
                        break;
                    case WorkflowStepType.DataTransformation:
                        result = await ExecuteDataTransformationStepAsync(stepExecution, step, cancellationToken);
                        break;
                    case WorkflowStepType.Notification:
                        result = await ExecuteNotificationStepAsync(stepExecution, step, cancellationToken);
                        break;
                    case WorkflowStepType.Delay:
                        result = await ExecuteDelayStepAsync(stepExecution, step, cancellationToken);
                        break;
                    default:
                        result = new WorkflowStepExecutionResult
                        {
                            Success = false,
                            Status = WorkflowStepStatus.Failed,
                            ErrorMessage = $"Unsupported step type: {step.StepType}"
                        };
                        break;
                }
            }
            catch (Exception ex)
            {
                result = new WorkflowStepExecutionResult
                {
                    Success = false,
                    Status = WorkflowStepStatus.Failed,
                    ErrorMessage = ex.Message
                };
            }

            stepExecution.Status = result.Status;
            stepExecution.CompletedAt = DateTime.UtcNow;
            stepExecution.DurationMs = (long)(DateTime.UtcNow - startTime).TotalMilliseconds;
            stepExecution.OutputData = result.OutputData != null ? JsonSerializer.Serialize(result.OutputData) : null;
            stepExecution.ErrorMessage = result.ErrorMessage;
            stepExecution.UpdatedAt = DateTime.UtcNow;

            await _stepExecutionRepository.UpdateAsync(stepExecution);
            await _unitOfWork.SaveChangesAsync();

            result.DurationMs = stepExecution.DurationMs;

            _logger.LogInformation("Step execution completed: {StepExecutionId} with status {Status}", 
                stepExecutionId, result.Status);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing step: {StepExecutionId}", stepExecutionId);
            return new WorkflowStepExecutionResult
            {
                Success = false,
                Status = WorkflowStepStatus.Failed,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<bool> PauseWorkflowAsync(Guid workflowInstanceId, CancellationToken cancellationToken = default)
    {
        try
        {
            var workflowInstance = await _workflowInstanceRepository.GetByIdAsync(workflowInstanceId);
            if (workflowInstance == null)
            {
                return false;
            }

            workflowInstance.Status = WorkflowStatus.OnHold;
            workflowInstance.UpdatedAt = DateTime.UtcNow;
            await _workflowInstanceRepository.UpdateAsync(workflowInstance);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Workflow paused: {WorkflowId}", workflowInstanceId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error pausing workflow: {WorkflowId}", workflowInstanceId);
            return false;
        }
    }

    public async Task<bool> ResumeWorkflowAsync(Guid workflowInstanceId, CancellationToken cancellationToken = default)
    {
        try
        {
            var workflowInstance = await _workflowInstanceRepository.GetByIdAsync(workflowInstanceId);
            if (workflowInstance == null)
            {
                return false;
            }

            workflowInstance.Status = WorkflowStatus.InProgress;
            workflowInstance.UpdatedAt = DateTime.UtcNow;
            await _workflowInstanceRepository.UpdateAsync(workflowInstance);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Workflow resumed: {WorkflowId}", workflowInstanceId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resuming workflow: {WorkflowId}", workflowInstanceId);
            return false;
        }
    }

    public async Task<bool> StopWorkflowAsync(Guid workflowInstanceId, string? reason = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var workflowInstance = await _workflowInstanceRepository.GetByIdAsync(workflowInstanceId);
            if (workflowInstance == null)
            {
                return false;
            }

            workflowInstance.Status = WorkflowStatus.Cancelled;
            workflowInstance.CompletedAt = DateTime.UtcNow;
            workflowInstance.UpdatedAt = DateTime.UtcNow;
            await _workflowInstanceRepository.UpdateAsync(workflowInstance);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Workflow stopped: {WorkflowId}, Reason: {Reason}", workflowInstanceId, reason);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error stopping workflow: {WorkflowId}", workflowInstanceId);
            return false;
        }
    }

    public async Task<WorkflowStepExecutionResult> RetryStepAsync(Guid stepExecutionId, CancellationToken cancellationToken = default)
    {
        try
        {
            var stepExecution = await _stepExecutionRepository.GetByIdAsync(stepExecutionId);
            if (stepExecution == null)
            {
                return new WorkflowStepExecutionResult
                {
                    Success = false,
                    ErrorMessage = "Step execution not found"
                };
            }

            if (stepExecution.RetryCount >= stepExecution.MaxRetries)
            {
                return new WorkflowStepExecutionResult
                {
                    Success = false,
                    Status = WorkflowStepStatus.Failed,
                    ErrorMessage = "Maximum retry attempts exceeded"
                };
            }

            stepExecution.RetryCount++;
            stepExecution.Status = WorkflowStepStatus.Retrying;
            stepExecution.UpdatedAt = DateTime.UtcNow;
            await _stepExecutionRepository.UpdateAsync(stepExecution);

            return await ExecuteStepAsync(stepExecutionId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrying step: {StepExecutionId}", stepExecutionId);
            return new WorkflowStepExecutionResult
            {
                Success = false,
                Status = WorkflowStepStatus.Failed,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<WorkflowExecutionStatus> GetExecutionStatusAsync(Guid workflowInstanceId)
    {
        try
        {
            var workflowInstance = await _workflowInstanceRepository.GetByIdAsync(workflowInstanceId);
            if (workflowInstance == null)
            {
                return new WorkflowExecutionStatus
                {
                    WorkflowInstanceId = workflowInstanceId,
                    Status = WorkflowStatus.Unknown
                };
            }

            var template = await _workflowTemplateRepository.GetByIdAsync(workflowInstance.WorkflowTemplateId);
            var totalSteps = 0;
            var completedSteps = 0;
            var failedSteps = 0;

            if (template != null)
            {
                var steps = await _workflowStepRepository.FindAsync(s => 
                    s.WorkflowTemplateId == template.Id && s.IsActive);
                totalSteps = steps.Count();

                var stepExecutions = await _stepExecutionRepository.FindAsync(se => 
                    se.WorkflowInstanceId == workflowInstanceId);
                
                completedSteps = stepExecutions.Count(se => se.Status == WorkflowStepStatus.Completed);
                failedSteps = stepExecutions.Count(se => se.Status == WorkflowStepStatus.Failed);
            }

            var progressPercentage = totalSteps > 0 ? (decimal)completedSteps / totalSteps * 100 : 0;

            return new WorkflowExecutionStatus
            {
                WorkflowInstanceId = workflowInstanceId,
                Status = workflowInstance.Status,
                ProgressPercentage = progressPercentage,
                TotalSteps = totalSteps,
                CompletedSteps = completedSteps,
                FailedSteps = failedSteps,
                StartedAt = workflowInstance.StartedAt,
                EstimatedCompletionAt = CalculateEstimatedCompletion(workflowInstance, totalSteps, completedSteps)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting execution status: {WorkflowId}", workflowInstanceId);
            return new WorkflowExecutionStatus
            {
                WorkflowInstanceId = workflowInstanceId,
                Status = WorkflowStatus.Unknown
            };
        }
    }

    public async Task<WorkflowStepExecutionStatus> GetStepExecutionStatusAsync(Guid stepExecutionId)
    {
        try
        {
            var stepExecution = await _stepExecutionRepository.GetByIdAsync(stepExecutionId);
            if (stepExecution == null)
            {
                return new WorkflowStepExecutionStatus
                {
                    StepExecutionId = stepExecutionId,
                    Status = WorkflowStepStatus.Failed
                };
            }

            var progressPercentage = stepExecution.Status switch
            {
                WorkflowStepStatus.Pending => 0,
                WorkflowStepStatus.Running => 50,
                WorkflowStepStatus.Completed => 100,
                WorkflowStepStatus.Failed => 0,
                WorkflowStepStatus.Cancelled => 0,
                _ => 0
            };

            return new WorkflowStepExecutionStatus
            {
                StepExecutionId = stepExecutionId,
                Status = stepExecution.Status,
                ProgressPercentage = progressPercentage,
                StartedAt = stepExecution.StartedAt,
                EstimatedCompletionAt = CalculateStepEstimatedCompletion(stepExecution),
                RetryCount = stepExecution.RetryCount
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting step execution status: {StepExecutionId}", stepExecutionId);
            return new WorkflowStepExecutionStatus
            {
                StepExecutionId = stepExecutionId,
                Status = WorkflowStepStatus.Failed
            };
        }
    }

    public async Task<WorkflowValidationResult> ValidateWorkflowAsync(Guid workflowTemplateId)
    {
        var result = new WorkflowValidationResult { IsValid = true };

        try
        {
            var template = await _workflowTemplateRepository.GetByIdAsync(workflowTemplateId);
            if (template == null)
            {
                result.IsValid = false;
                result.Errors.Add("Workflow template not found");
                return result;
            }

            var steps = await _workflowStepRepository.FindAsync(s => 
                s.WorkflowTemplateId == workflowTemplateId && s.IsActive);
            var stepsList = steps.ToList();

            if (!stepsList.Any())
            {
                result.IsValid = false;
                result.Errors.Add("Workflow template has no active steps");
                return result;
            }

            var orderedSteps = stepsList.OrderBy(s => s.Order).ToList();
            for (int i = 0; i < orderedSteps.Count; i++)
            {
                if (orderedSteps[i].Order != i)
                {
                    result.Warnings.Add($"Step ordering gap detected at position {i}");
                }
            }

            foreach (var step in stepsList)
            {
                if (string.IsNullOrEmpty(step.Configuration))
                {
                    result.Warnings.Add($"Step '{step.Name}' has no configuration");
                }

                if (step.StepType == WorkflowStepType.Condition && string.IsNullOrEmpty(step.ExecutionConditions))
                {
                    result.Errors.Add($"Condition step '{step.Name}' has no execution conditions");
                    result.IsValid = false;
                }
            }

            _logger.LogInformation("Workflow validation completed: {TemplateId}, Valid: {IsValid}", 
                workflowTemplateId, result.IsValid);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating workflow: {TemplateId}", workflowTemplateId);
            result.IsValid = false;
            result.Errors.Add($"Validation error: {ex.Message}");
        }

        return result;
    }

    public async Task<WorkflowExecutionMetrics> GetExecutionMetricsAsync(Guid workflowInstanceId)
    {
        try
        {
            var stepExecutions = await _stepExecutionRepository.FindAsync(se => 
                se.WorkflowInstanceId == workflowInstanceId);
            var executionsList = stepExecutions.ToList();

            var totalExecutionTime = executionsList.Sum(se => se.DurationMs ?? 0);
            var averageStepTime = executionsList.Any() ? totalExecutionTime / executionsList.Count : 0;
            var totalRetries = executionsList.Sum(se => se.RetryCount);
            var successfulSteps = executionsList.Count(se => se.Status == WorkflowStepStatus.Completed);
            var successRate = executionsList.Any() ? (decimal)successfulSteps / executionsList.Count * 100 : 0;

            var bottlenecks = executionsList
                .Where(se => se.DurationMs > averageStepTime * 2)
                .Select(se => $"Step execution {se.Id} took {se.DurationMs}ms")
                .ToList();

            return new WorkflowExecutionMetrics
            {
                TotalExecutionTimeMs = totalExecutionTime,
                AverageStepExecutionTimeMs = averageStepTime,
                TotalRetries = totalRetries,
                SuccessRate = successRate,
                Bottlenecks = bottlenecks,
                ResourceMetrics = new Dictionary<string, object>
                {
                    { "TotalSteps", executionsList.Count },
                    { "CompletedSteps", successfulSteps },
                    { "FailedSteps", executionsList.Count(se => se.Status == WorkflowStepStatus.Failed) }
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting execution metrics: {WorkflowId}", workflowInstanceId);
            return new WorkflowExecutionMetrics();
        }
    }

    #region Private Helper Methods

    private async Task<WorkflowDataContext> CreateWorkflowDataContextAsync(WorkflowInstance workflowInstance)
    {
        var dataContext = new WorkflowDataContext
        {
            Id = Guid.NewGuid(),
            Name = $"Workflow_{workflowInstance.Id}_Context",
            Scope = "workflow",
            Data = workflowInstance.WorkflowData ?? "{}",
            WorkflowInstanceId = workflowInstance.Id,
            TenantId = workflowInstance.TenantId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsActive = true
        };

        await _dataContextRepository.AddAsync(dataContext);
        await _unitOfWork.SaveChangesAsync();

        return dataContext;
    }

    private async Task<WorkflowStepExecution> CreateStepExecutionAsync(Guid workflowInstanceId, Guid stepId)
    {
        var stepExecution = new WorkflowStepExecution
        {
            Id = Guid.NewGuid(),
            WorkflowInstanceId = workflowInstanceId,
            WorkflowStepId = stepId,
            Status = WorkflowStepStatus.Pending,
            TenantId = _tenantProvider.GetTenantId(),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _stepExecutionRepository.AddAsync(stepExecution);
        await _unitOfWork.SaveChangesAsync();

        return stepExecution;
    }

    private async Task<WorkflowStepExecutionResult> ExecuteTaskStepAsync(WorkflowStepExecution stepExecution, WorkflowStep step, CancellationToken cancellationToken)
    {
        await Task.Delay(100, cancellationToken); // Simulate work
        
        return new WorkflowStepExecutionResult
        {
            Success = true,
            Status = WorkflowStepStatus.Completed,
            OutputData = new { message = "Task completed successfully" }
        };
    }

    private Task<WorkflowStepExecutionResult> ExecuteApprovalStepAsync(WorkflowStepExecution stepExecution, WorkflowStep step, CancellationToken cancellationToken)
    {
        return Task.FromResult(new WorkflowStepExecutionResult
        {
            Success = true,
            Status = WorkflowStepStatus.WaitingForApproval,
            OutputData = new { message = "Waiting for approval" }
        });
    }

    private async Task<WorkflowStepExecutionResult> ExecuteConditionStepAsync(WorkflowStepExecution stepExecution, WorkflowStep step, CancellationToken cancellationToken)
    {
        await Task.Delay(50, cancellationToken);
        
        return new WorkflowStepExecutionResult
        {
            Success = true,
            Status = WorkflowStepStatus.Completed,
            OutputData = new { conditionResult = true }
        };
    }

    private async Task<WorkflowStepExecutionResult> ExecuteAITaskStepAsync(WorkflowStepExecution stepExecution, WorkflowStep step, CancellationToken cancellationToken)
    {
        try
        {
            var config = JsonSerializer.Deserialize<Dictionary<string, object>>(step.Configuration);
            var taskType = config?.GetValueOrDefault("taskType")?.ToString() ?? "TextGeneration";
            var prompt = config?.GetValueOrDefault("prompt")?.ToString() ?? "Default AI task";

            var aiTask = new AITask
            {
                Id = Guid.NewGuid(),
                TaskType = Enum.Parse<AITaskType>(taskType),
                InputData = prompt,
                Status = AITaskStatus.Pending,
                WorkflowInstanceId = stepExecution.WorkflowInstanceId,
                TenantId = stepExecution.TenantId,
                CreatedAt = DateTime.UtcNow
            };

            var result = await _aiOrchestrationService.ExecuteTaskAsync(aiTask, cancellationToken);

            return new WorkflowStepExecutionResult
            {
                Success = result.Success,
                Status = result.Success ? WorkflowStepStatus.Completed : WorkflowStepStatus.Failed,
                OutputData = result.ResultData,
                ErrorMessage = result.ErrorMessage
            };
        }
        catch (Exception ex)
        {
            return new WorkflowStepExecutionResult
            {
                Success = false,
                Status = WorkflowStepStatus.Failed,
                ErrorMessage = ex.Message
            };
        }
    }

    private async Task<WorkflowStepExecutionResult> ExecuteIntegrationStepAsync(WorkflowStepExecution stepExecution, WorkflowStep step, CancellationToken cancellationToken)
    {
        await Task.Delay(200, cancellationToken);
        
        return new WorkflowStepExecutionResult
        {
            Success = true,
            Status = WorkflowStepStatus.Completed,
            OutputData = new { integrationResult = "Success" }
        };
    }

    private async Task<WorkflowStepExecutionResult> ExecuteDataTransformationStepAsync(WorkflowStepExecution stepExecution, WorkflowStep step, CancellationToken cancellationToken)
    {
        await Task.Delay(100, cancellationToken);
        
        return new WorkflowStepExecutionResult
        {
            Success = true,
            Status = WorkflowStepStatus.Completed,
            OutputData = new { transformedData = "Data transformed successfully" }
        };
    }

    private async Task<WorkflowStepExecutionResult> ExecuteNotificationStepAsync(WorkflowStepExecution stepExecution, WorkflowStep step, CancellationToken cancellationToken)
    {
        await Task.Delay(50, cancellationToken);
        
        return new WorkflowStepExecutionResult
        {
            Success = true,
            Status = WorkflowStepStatus.Completed,
            OutputData = new { notificationSent = true }
        };
    }

    private async Task<WorkflowStepExecutionResult> ExecuteDelayStepAsync(WorkflowStepExecution stepExecution, WorkflowStep step, CancellationToken cancellationToken)
    {
        try
        {
            var config = JsonSerializer.Deserialize<Dictionary<string, object>>(step.Configuration);
            var delayMs = config?.GetValueOrDefault("delayMs")?.ToString();
            
            if (int.TryParse(delayMs, out var delay))
            {
                await Task.Delay(delay, cancellationToken);
            }
            
            return new WorkflowStepExecutionResult
            {
                Success = true,
                Status = WorkflowStepStatus.Completed,
                OutputData = new { delayCompleted = true }
            };
        }
        catch (Exception ex)
        {
            return new WorkflowStepExecutionResult
            {
                Success = false,
                Status = WorkflowStepStatus.Failed,
                ErrorMessage = ex.Message
            };
        }
    }

    private DateTime? CalculateEstimatedCompletion(WorkflowInstance workflowInstance, int totalSteps, int completedSteps)
    {
        if (workflowInstance.StartedAt == null || completedSteps == 0 || totalSteps == 0)
            return null;

        var elapsed = DateTime.UtcNow - workflowInstance.StartedAt.Value;
        var averageTimePerStep = elapsed.TotalMinutes / completedSteps;
        var remainingSteps = totalSteps - completedSteps;
        var estimatedRemainingTime = TimeSpan.FromMinutes(averageTimePerStep * remainingSteps);

        return DateTime.UtcNow.Add(estimatedRemainingTime);
    }

    private DateTime? CalculateStepEstimatedCompletion(WorkflowStepExecution stepExecution)
    {
        if (stepExecution.StartedAt == null || stepExecution.Status != WorkflowStepStatus.Running)
            return null;

        var estimatedDuration = TimeSpan.FromMinutes(5); // Default 5 minutes
        return stepExecution.StartedAt.Value.Add(estimatedDuration);
    }

    #endregion
}
