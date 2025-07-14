using Microsoft.Extensions.Logging;
using AutoMapper;
using BARQ.Core.Interfaces;
using BARQ.Core.Entities;
using BARQ.Core.Repositories;
using BARQ.Core.Enums;
using BARQ.Infrastructure.MultiTenancy;

namespace BARQ.Application.Services.Workflows;

public class WorkflowService : IWorkflowService
{
    private readonly IRepository<WorkflowInstance> _workflowInstanceRepository;
    private readonly IRepository<WorkflowTemplate> _workflowTemplateRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<AuditLog> _auditLogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<WorkflowService> _logger;
    private readonly ITenantProvider _tenantProvider;

    public WorkflowService(
        IRepository<WorkflowInstance> workflowInstanceRepository,
        IRepository<WorkflowTemplate> workflowTemplateRepository,
        IRepository<User> userRepository,
        IRepository<AuditLog> auditLogRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<WorkflowService> logger,
        ITenantProvider tenantProvider)
    {
        _workflowInstanceRepository = workflowInstanceRepository;
        _workflowTemplateRepository = workflowTemplateRepository;
        _userRepository = userRepository;
        _auditLogRepository = auditLogRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _tenantProvider = tenantProvider;
    }

    public async Task<WorkflowExecutionResult> CreateWorkflowInstanceAsync(CreateWorkflowInstanceRequest request)
    {
        try
        {
            var tenantId = _tenantProvider.GetTenantId();
            
            var template = await _workflowTemplateRepository.GetByIdAsync(request.TemplateId);
            if (template == null)
            {
                return new WorkflowExecutionResult
                {
                    Success = false,
                    ErrorMessage = "Workflow template not found"
                };
            }

            var workflowInstance = new WorkflowInstance
            {
                Id = Guid.NewGuid(),
                TemplateId = request.TemplateId,
                Name = request.Name ?? template.Name,
                Description = request.Description ?? template.Description,
                Status = WorkflowStatus.Created,
                Priority = request.Priority ?? WorkflowPriority.Medium,
                CreatedBy = request.CreatedBy,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                TenantId = tenantId,
                EntityId = request.EntityId,
                EntityType = request.EntityType,
                Data = System.Text.Json.JsonSerializer.Serialize(request.InitialData ?? new Dictionary<string, object>())
            };

            await _workflowInstanceRepository.AddAsync(workflowInstance);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("WORKFLOW_CREATED", $"Workflow instance created: {workflowInstance.Name}", workflowInstance.Id);

            _logger.LogInformation("Workflow instance created: {WorkflowId}", workflowInstance.Id);

            return new WorkflowExecutionResult
            {
                Success = true,
                WorkflowInstanceId = workflowInstance.Id,
                Status = workflowInstance.Status,
                Message = "Workflow instance created successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating workflow instance: {TemplateName}", request.Name);
            return new WorkflowExecutionResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<WorkflowExecutionResult> StartWorkflowAsync(Guid workflowInstanceId, Guid startedBy)
    {
        try
        {
            var workflowInstance = await _workflowInstanceRepository.GetByIdAsync(workflowInstanceId);
            if (workflowInstance == null)
            {
                return new WorkflowExecutionResult
                {
                    Success = false,
                    ErrorMessage = "Workflow instance not found"
                };
            }

            if (workflowInstance.Status != WorkflowStatus.Created)
            {
                return new WorkflowExecutionResult
                {
                    Success = false,
                    ErrorMessage = $"Cannot start workflow in status: {workflowInstance.Status}"
                };
            }

            workflowInstance.Status = WorkflowStatus.InProgress;
            workflowInstance.StartedAt = DateTime.UtcNow;
            workflowInstance.StartedBy = startedBy;
            workflowInstance.UpdatedAt = DateTime.UtcNow;

            await _workflowInstanceRepository.UpdateAsync(workflowInstance);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("WORKFLOW_STARTED", $"Workflow started: {workflowInstance.Name}", workflowInstanceId);

            _logger.LogInformation("Workflow started: {WorkflowId}", workflowInstanceId);

            return new WorkflowExecutionResult
            {
                Success = true,
                WorkflowInstanceId = workflowInstanceId,
                Status = workflowInstance.Status,
                Message = "Workflow started successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting workflow: {WorkflowId}", workflowInstanceId);
            return new WorkflowExecutionResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<WorkflowExecutionResult> ApproveWorkflowStepAsync(Guid workflowInstanceId, string stepId, Guid approvedBy, string? comments = null)
    {
        try
        {
            var workflowInstance = await _workflowInstanceRepository.GetByIdAsync(workflowInstanceId);
            if (workflowInstance == null)
            {
                return new WorkflowExecutionResult
                {
                    Success = false,
                    ErrorMessage = "Workflow instance not found"
                };
            }

            if (workflowInstance.Status != WorkflowStatus.InProgress && workflowInstance.Status != WorkflowStatus.PendingApproval)
            {
                return new WorkflowExecutionResult
                {
                    Success = false,
                    ErrorMessage = $"Cannot approve workflow step in status: {workflowInstance.Status}"
                };
            }

            workflowInstance.Status = WorkflowStatus.InProgress;
            workflowInstance.UpdatedAt = DateTime.UtcNow;

            await _workflowInstanceRepository.UpdateAsync(workflowInstance);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("WORKFLOW_STEP_APPROVED", $"Workflow step approved: {stepId} by {approvedBy}", workflowInstanceId);

            _logger.LogInformation("Workflow step approved: {WorkflowId}, Step: {StepId}", workflowInstanceId, stepId);

            return new WorkflowExecutionResult
            {
                Success = true,
                WorkflowInstanceId = workflowInstanceId,
                Status = workflowInstance.Status,
                Message = "Workflow step approved successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving workflow step: {WorkflowId}, Step: {StepId}", workflowInstanceId, stepId);
            return new WorkflowExecutionResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<WorkflowExecutionResult> RejectWorkflowStepAsync(Guid workflowInstanceId, string stepId, Guid rejectedBy, string reason)
    {
        try
        {
            var workflowInstance = await _workflowInstanceRepository.GetByIdAsync(workflowInstanceId);
            if (workflowInstance == null)
            {
                return new WorkflowExecutionResult
                {
                    Success = false,
                    ErrorMessage = "Workflow instance not found"
                };
            }

            workflowInstance.Status = WorkflowStatus.Rejected;
            workflowInstance.UpdatedAt = DateTime.UtcNow;

            await _workflowInstanceRepository.UpdateAsync(workflowInstance);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("WORKFLOW_STEP_REJECTED", $"Workflow step rejected: {stepId} by {rejectedBy}. Reason: {reason}", workflowInstanceId);

            _logger.LogInformation("Workflow step rejected: {WorkflowId}, Step: {StepId}", workflowInstanceId, stepId);

            return new WorkflowExecutionResult
            {
                Success = true,
                WorkflowInstanceId = workflowInstanceId,
                Status = workflowInstance.Status,
                Message = "Workflow step rejected"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting workflow step: {WorkflowId}, Step: {StepId}", workflowInstanceId, stepId);
            return new WorkflowExecutionResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<WorkflowExecutionResult> CancelWorkflowAsync(Guid workflowInstanceId, Guid cancelledBy, string? reason = null)
    {
        try
        {
            var workflowInstance = await _workflowInstanceRepository.GetByIdAsync(workflowInstanceId);
            if (workflowInstance == null)
            {
                return new WorkflowExecutionResult
                {
                    Success = false,
                    ErrorMessage = "Workflow instance not found"
                };
            }

            if (workflowInstance.Status == WorkflowStatus.Completed || workflowInstance.Status == WorkflowStatus.Cancelled)
            {
                return new WorkflowExecutionResult
                {
                    Success = false,
                    ErrorMessage = $"Cannot cancel workflow in status: {workflowInstance.Status}"
                };
            }

            workflowInstance.Status = WorkflowStatus.Cancelled;
            workflowInstance.CompletedAt = DateTime.UtcNow;
            workflowInstance.UpdatedAt = DateTime.UtcNow;

            await _workflowInstanceRepository.UpdateAsync(workflowInstance);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("WORKFLOW_CANCELLED", $"Workflow cancelled by {cancelledBy}. Reason: {reason}", workflowInstanceId);

            _logger.LogInformation("Workflow cancelled: {WorkflowId}", workflowInstanceId);

            return new WorkflowExecutionResult
            {
                Success = true,
                WorkflowInstanceId = workflowInstanceId,
                Status = workflowInstance.Status,
                Message = "Workflow cancelled successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling workflow: {WorkflowId}", workflowInstanceId);
            return new WorkflowExecutionResult
            {
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<WorkflowInstanceStatus> GetWorkflowStatusAsync(Guid workflowInstanceId)
    {
        try
        {
            var workflowInstance = await _workflowInstanceRepository.GetByIdAsync(workflowInstanceId);
            if (workflowInstance == null)
            {
                return new WorkflowInstanceStatus
                {
                    WorkflowInstanceId = workflowInstanceId,
                    Status = WorkflowStatus.Unknown,
                    ErrorMessage = "Workflow instance not found"
                };
            }

            return new WorkflowInstanceStatus
            {
                WorkflowInstanceId = workflowInstanceId,
                Status = workflowInstance.Status,
                CurrentStep = "Step1",
                Progress = CalculateProgress(workflowInstance),
                CreatedAt = workflowInstance.CreatedAt,
                StartedAt = workflowInstance.StartedAt,
                CompletedAt = workflowInstance.CompletedAt,
                LastUpdated = workflowInstance.UpdatedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting workflow status: {WorkflowId}", workflowInstanceId);
            return new WorkflowInstanceStatus
            {
                WorkflowInstanceId = workflowInstanceId,
                Status = WorkflowStatus.Unknown,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<List<WorkflowHistoryEntry>> GetWorkflowHistoryAsync(Guid workflowInstanceId)
    {
        try
        {
            var auditLogs = await _auditLogRepository.FindAsync(a => 
                a.EntityId == workflowInstanceId && 
                a.EntityName == "WorkflowInstance");

            var historyEntries = auditLogs.Select(log => new WorkflowHistoryEntry
            {
                Id = log.Id,
                WorkflowInstanceId = workflowInstanceId,
                Action = log.Action,
                Description = log.Description,
                UserId = log.UserId,
                Timestamp = log.Timestamp,
                Details = new Dictionary<string, object>
                {
                    { "IpAddress", log.IpAddress ?? "Unknown" },
                    { "UserAgent", "Unknown" }
                }
            }).OrderByDescending(h => h.Timestamp).ToList();

            _logger.LogInformation("Retrieved {Count} history entries for workflow: {WorkflowId}", historyEntries.Count, workflowInstanceId);
            return historyEntries;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting workflow history: {WorkflowId}", workflowInstanceId);
            return new List<WorkflowHistoryEntry>();
        }
    }

    public async Task<bool> CheckSlaBreachAsync(Guid workflowInstanceId)
    {
        try
        {
            var workflowInstance = await _workflowInstanceRepository.GetByIdAsync(workflowInstanceId);
            if (workflowInstance == null)
            {
                _logger.LogWarning("Workflow instance not found for SLA check: {WorkflowId}", workflowInstanceId);
                return false;
            }

            var template = await _workflowTemplateRepository.GetByIdAsync(workflowInstance.TemplateId);
            if (template == null || !template.SlaHours.HasValue)
            {
                return false;
            }

            var slaDeadline = workflowInstance.CreatedAt.AddHours(template.SlaHours.Value);
            var isBreached = DateTime.UtcNow > slaDeadline && workflowInstance.Status != WorkflowStatus.Completed;

            if (isBreached)
            {
                await LogAuditAsync("SLA_BREACH_DETECTED", $"SLA breach detected for workflow: {workflowInstance.Name}", workflowInstanceId);
                _logger.LogWarning("SLA breach detected for workflow: {WorkflowId}", workflowInstanceId);
            }

            return isBreached;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking SLA breach: {WorkflowId}", workflowInstanceId);
            return false;
        }
    }

    public async Task SendWorkflowNotificationAsync(Guid workflowInstanceId, string notificationType, List<Guid> recipientIds, string? customMessage = null)
    {
        try
        {
            var workflowInstance = await _workflowInstanceRepository.GetByIdAsync(workflowInstanceId);
            if (workflowInstance == null)
            {
                _logger.LogWarning("Workflow instance not found for notification: {WorkflowId}", workflowInstanceId);
                return;
            }

            foreach (var recipientId in recipientIds)
            {
                var recipient = await _userRepository.GetByIdAsync(recipientId);
                if (recipient != null)
                {
                    _logger.LogInformation("Sending {NotificationType} notification to {Email} for workflow {WorkflowId}", 
                        notificationType, recipient.Email, workflowInstanceId);
                }
            }

            await LogAuditAsync("WORKFLOW_NOTIFICATION_SENT", $"Notification sent: {notificationType} to {recipientIds.Count} recipients", workflowInstanceId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending workflow notification: {WorkflowId}", workflowInstanceId);
        }
    }

    public async Task<List<WorkflowInstanceStatus>> GetActiveWorkflowsAsync(Guid? userId = null)
    {
        try
        {
            var query = _workflowInstanceRepository.GetQueryable()
                .Where(w => w.Status == WorkflowStatus.InProgress || w.Status == WorkflowStatus.PendingApproval);

            if (userId.HasValue)
            {
                query = query.Where(w => w.CreatedBy == userId.Value || w.StartedBy == userId.Value);
            }

            var workflows = await _workflowInstanceRepository.FindAsync(query);

            var workflowStatuses = workflows.Select(w => new WorkflowInstanceStatus
            {
                WorkflowInstanceId = w.Id,
                Status = w.Status,
                CurrentStep = "Step1",
                Progress = CalculateProgress(w),
                CreatedAt = w.CreatedAt,
                StartedAt = w.StartedAt,
                CompletedAt = w.CompletedAt,
                LastUpdated = w.UpdatedAt
            }).ToList();

            _logger.LogInformation("Retrieved {Count} active workflows", workflowStatuses.Count);
            return workflowStatuses;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active workflows");
            return new List<WorkflowInstanceStatus>();
        }
    }

    public async Task<List<WorkflowInstanceStatus>> GetWorkflowsByStatusAsync(WorkflowStatus status, int limit = 100)
    {
        try
        {
            var workflows = await _workflowInstanceRepository.FindAsync(w => w.Status == status);
            var limitedWorkflows = workflows.Take(limit);

            var workflowStatuses = limitedWorkflows.Select(w => new WorkflowInstanceStatus
            {
                WorkflowInstanceId = w.Id,
                Status = w.Status,
                CurrentStep = "Step1",
                Progress = CalculateProgress(w),
                CreatedAt = w.CreatedAt,
                StartedAt = w.StartedAt,
                CompletedAt = w.CompletedAt,
                LastUpdated = w.UpdatedAt
            }).ToList();

            _logger.LogInformation("Retrieved {Count} workflows with status {Status}", workflowStatuses.Count, status);
            return workflowStatuses;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting workflows by status: {Status}", status);
            return new List<WorkflowInstanceStatus>();
        }
    }

    private decimal CalculateProgress(WorkflowInstance workflowInstance)
    {
        return workflowInstance.Status switch
        {
            WorkflowStatus.Created => 0,
            WorkflowStatus.InProgress => 50,
            WorkflowStatus.PendingApproval => 75,
            WorkflowStatus.Completed => 100,
            WorkflowStatus.Cancelled => 0,
            WorkflowStatus.Rejected => 0,
            _ => 0
        };
    }

    private async Task LogAuditAsync(string action, string description, Guid? entityId = null)
    {
        try
        {
            var auditLog = new AuditLog
            {
                Id = Guid.NewGuid(),
                Action = action,
                EntityName = "WorkflowInstance",
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
