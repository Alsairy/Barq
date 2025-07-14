using Microsoft.Extensions.Logging;
using AutoMapper;
using BARQ.Core.Interfaces;
using BARQ.Core.Entities;
using BARQ.Core.Repositories;
using BARQ.Core.Enums;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;
using BARQ.Core.Services;
using System.Text.Json;

namespace BARQ.Application.Services.Workflows;

public class WorkflowService : IWorkflowService
{
    private readonly IRepository<WorkflowInstance> _workflowInstanceRepository;
    private readonly IRepository<WorkflowTemplate> _workflowTemplateRepository;
    private readonly IRepository<WorkflowStep> _workflowStepRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<AuditLog> _auditLogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<WorkflowService> _logger;
    private readonly ITenantProvider _tenantProvider;
    private readonly IWorkflowEngine _workflowEngine;

    public WorkflowService(
        IRepository<WorkflowInstance> workflowInstanceRepository,
        IRepository<WorkflowTemplate> workflowTemplateRepository,
        IRepository<WorkflowStep> workflowStepRepository,
        IRepository<User> userRepository,
        IRepository<AuditLog> auditLogRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<WorkflowService> logger,
        ITenantProvider tenantProvider,
        IWorkflowEngine workflowEngine)
    {
        _workflowInstanceRepository = workflowInstanceRepository;
        _workflowTemplateRepository = workflowTemplateRepository;
        _workflowStepRepository = workflowStepRepository;
        _userRepository = userRepository;
        _auditLogRepository = auditLogRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _tenantProvider = tenantProvider;
        _workflowEngine = workflowEngine;
    }

    public async Task<WorkflowInstance> CreateWorkflowInstanceAsync(Guid templateId, Guid initiatorId, object? workflowData = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var tenantId = _tenantProvider.GetTenantId();
            
            var template = await _workflowTemplateRepository.GetByIdAsync(templateId);
            if (template == null)
            {
                throw new InvalidOperationException("Workflow template not found");
            }

            var workflowInstance = new WorkflowInstance
            {
                Id = Guid.NewGuid(),
                WorkflowTemplateId = templateId,
                Name = template.Name,
                Description = template.Description,
                Status = WorkflowStatus.Pending,
                Priority = ProjectPriority.Medium,
                InitiatorId = initiatorId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                TenantId = tenantId,
                WorkflowData = System.Text.Json.JsonSerializer.Serialize(workflowData ?? new Dictionary<string, object>())
            };

            await _workflowInstanceRepository.AddAsync(workflowInstance);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("WORKFLOW_CREATED", $"Workflow instance created: {workflowInstance.Name}", workflowInstance.Id);

            _logger.LogInformation("Workflow instance created: {WorkflowId}", workflowInstance.Id);

            return workflowInstance;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating workflow instance: {TemplateId}", templateId);
            throw;
        }
    }

    public async Task<WorkflowExecutionResult> StartWorkflowAsync(Guid workflowInstanceId, CancellationToken cancellationToken = default)
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

            if (workflowInstance.Status != WorkflowStatus.Pending)
            {
                return new WorkflowExecutionResult
                {
                    IsSuccess = false,
                    ErrorDetails = $"Cannot start workflow in status: {workflowInstance.Status}"
                };
            }

            workflowInstance.Status = WorkflowStatus.InProgress;
            workflowInstance.StartedAt = DateTime.UtcNow;
            workflowInstance.UpdatedAt = DateTime.UtcNow;

            await _workflowInstanceRepository.UpdateAsync(workflowInstance);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("WORKFLOW_STARTED", $"Workflow started: {workflowInstance.Name}", workflowInstanceId);

            _logger.LogInformation("Workflow started: {WorkflowId}", workflowInstanceId);

            return new WorkflowExecutionResult
            {
                IsSuccess = true,
                Status = WorkflowStepStatus.Completed,
                Message = "Workflow started successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error starting workflow: {WorkflowId}", workflowInstanceId);
            return new WorkflowExecutionResult
            {
                IsSuccess = false,
                ErrorDetails = ex.Message
            };
        }
    }

    public async Task<WorkflowExecutionResult> ApproveStepAsync(Guid workflowInstanceId, Guid stepId, string? comments = null, CancellationToken cancellationToken = default)
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

            if (workflowInstance.Status != WorkflowStatus.InProgress && workflowInstance.Status != WorkflowStatus.WaitingForApproval)
            {
                return new WorkflowExecutionResult
                {
                    IsSuccess = false,
                    ErrorDetails = $"Cannot approve workflow step in status: {workflowInstance.Status}"
                };
            }

            workflowInstance.Status = WorkflowStatus.InProgress;
            workflowInstance.UpdatedAt = DateTime.UtcNow;

            await _workflowInstanceRepository.UpdateAsync(workflowInstance);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("WORKFLOW_STEP_APPROVED", $"Workflow step approved: {stepId}", workflowInstanceId);

            _logger.LogInformation("Workflow step approved: {WorkflowId}, Step: {StepId}", workflowInstanceId, stepId);

            return new WorkflowExecutionResult
            {
                IsSuccess = true,
                Status = WorkflowStepStatus.Completed,
                Message = "Workflow step approved successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving workflow step: {WorkflowId}, Step: {StepId}", workflowInstanceId, stepId);
            return new WorkflowExecutionResult
            {
                IsSuccess = false,
                ErrorDetails = ex.Message
            };
        }
    }

    public async Task<WorkflowExecutionResult> RejectStepAsync(Guid workflowInstanceId, Guid stepId, string reason, CancellationToken cancellationToken = default)
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

            workflowInstance.Status = WorkflowStatus.Rejected;
            workflowInstance.UpdatedAt = DateTime.UtcNow;

            await _workflowInstanceRepository.UpdateAsync(workflowInstance);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("WORKFLOW_STEP_REJECTED", $"Workflow step rejected: {stepId}. Reason: {reason}", workflowInstanceId);

            _logger.LogInformation("Workflow step rejected: {WorkflowId}, Step: {StepId}", workflowInstanceId, stepId);

            return new WorkflowExecutionResult
            {
                IsSuccess = true,
                Status = WorkflowStepStatus.Completed,
                Message = "Workflow step rejected"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting workflow step: {WorkflowId}, Step: {StepId}", workflowInstanceId, stepId);
            return new WorkflowExecutionResult
            {
                IsSuccess = false,
                ErrorDetails = ex.Message
            };
        }
    }

    public async Task<WorkflowExecutionResult> CancelWorkflowAsync(Guid workflowInstanceId, Guid userId, string? reason = null, CancellationToken cancellationToken = default)
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

            if (workflowInstance.Status == WorkflowStatus.Approved || workflowInstance.Status == WorkflowStatus.Cancelled)
            {
                return new WorkflowExecutionResult
                {
                    IsSuccess = false,
                    ErrorDetails = $"Cannot cancel workflow in status: {workflowInstance.Status}"
                };
            }

            workflowInstance.Status = WorkflowStatus.Cancelled;
            workflowInstance.CompletedAt = DateTime.UtcNow;
            workflowInstance.UpdatedAt = DateTime.UtcNow;

            await _workflowInstanceRepository.UpdateAsync(workflowInstance);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("WORKFLOW_CANCELLED", $"Workflow cancelled by {userId}. Reason: {reason}", workflowInstanceId);

            _logger.LogInformation("Workflow cancelled: {WorkflowId}", workflowInstanceId);

            return new WorkflowExecutionResult
            {
                IsSuccess = true,
                Status = WorkflowStepStatus.Completed,
                Message = "Workflow cancelled successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling workflow: {WorkflowId}", workflowInstanceId);
            return new WorkflowExecutionResult
            {
                IsSuccess = false,
                ErrorDetails = ex.Message
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
                InstanceId = workflowInstanceId,
                Status = workflowInstance.Status,
                CurrentStepIndex = 1,
                CurrentStepName = "Step1",
                ProgressPercentage = CalculateProgress(workflowInstance),
                TotalSteps = 5,
                CompletedSteps = 1
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting workflow status: {WorkflowId}", workflowInstanceId);
            return new WorkflowInstanceStatus
            {
                InstanceId = workflowInstanceId,
                Status = WorkflowStatus.Created,
                CurrentStepIndex = 0,
                CurrentStepName = "Error",
                ProgressPercentage = 0,
                TotalSteps = 0,
                CompletedSteps = 0
            };
        }
    }

    public async Task<IEnumerable<WorkflowHistoryEntry>> GetWorkflowHistoryAsync(Guid workflowInstanceId)
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
                Action = log.Action ?? "Unknown",
                Description = log.Description ?? "No description",
                UserId = log.UserId,
                Timestamp = log.Timestamp,
                Details = System.Text.Json.JsonSerializer.Serialize(new Dictionary<string, object>
                {
                    { "IPAddress", log.IPAddress ?? "Unknown" },
                    { "UserAgent", "Unknown" }
                })
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

            var template = await _workflowTemplateRepository.GetByIdAsync(workflowInstance.WorkflowTemplateId);
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

            var workflows = await _workflowInstanceRepository.GetAsync(w => 
                userId.HasValue ? (w.InitiatorId == userId.Value || w.CurrentAssigneeId == userId.Value) : true);

            var workflowStatuses = workflows.Select(w => new WorkflowInstanceStatus
            {
                InstanceId = w.Id,
                Status = w.Status,
                CurrentStepIndex = 1,
                CurrentStepName = "Step1",
                ProgressPercentage = CalculateProgress(w),
                TotalSteps = 5,
                CompletedSteps = 1
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
                LastUpdated = w.UpdatedAt ?? DateTime.UtcNow
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

    public async Task<WorkflowExecutionResult> RequestChangesAsync(Guid workflowInstanceId, Guid userId, string reason, CancellationToken cancellationToken = default)
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

            workflowInstance.Status = WorkflowStatus.PendingApproval;
            workflowInstance.UpdatedAt = DateTime.UtcNow;

            await _workflowInstanceRepository.UpdateAsync(workflowInstance);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("WORKFLOW_CHANGES_REQUESTED", $"Changes requested by {userId}. Reason: {reason}", workflowInstanceId);

            _logger.LogInformation("Changes requested for workflow: {WorkflowId}", workflowInstanceId);

            return new WorkflowExecutionResult
            {
                IsSuccess = true,
                Status = WorkflowStepStatus.Completed,
                Message = "Changes requested successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error requesting changes for workflow: {WorkflowId}", workflowInstanceId);
            return new WorkflowExecutionResult
            {
                IsSuccess = false,
                ErrorDetails = ex.Message
            };
        }
    }

    public async Task<WorkflowExecutionResult> EscalateWorkflowAsync(Guid workflowInstanceId, string reason, CancellationToken cancellationToken = default)
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

            workflowInstance.Priority = ProjectPriority.High;
            workflowInstance.UpdatedAt = DateTime.UtcNow;

            await _workflowInstanceRepository.UpdateAsync(workflowInstance);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("WORKFLOW_ESCALATED", $"Workflow escalated. Reason: {reason}", workflowInstanceId);

            _logger.LogInformation("Workflow escalated: {WorkflowId}", workflowInstanceId);

            return new WorkflowExecutionResult
            {
                IsSuccess = true,
                Status = WorkflowStepStatus.Completed,
                Message = "Workflow escalated successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error escalating workflow: {WorkflowId}", workflowInstanceId);
            return new WorkflowExecutionResult
            {
                IsSuccess = false,
                ErrorDetails = ex.Message
            };
        }
    }

    public async Task<IEnumerable<WorkflowInstance>> GetPendingApprovalsAsync(Guid userId, WorkflowType? workflowType = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var query = _workflowInstanceRepository.GetQueryable()
                .Where(w => w.Status == WorkflowStatus.PendingApproval);

            var workflows = await _workflowInstanceRepository.GetAsync(w => 
                w.Status == WorkflowStatus.PendingApproval && 
                (!workflowType.HasValue || w.WorkflowTemplate.Name == workflowType.Value.ToString()));

            _logger.LogInformation("Retrieved {Count} pending approvals for user: {UserId}", workflows.Count(), userId);
            return workflows;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pending approvals for user: {UserId}", userId);
            return new List<WorkflowInstance>();
        }
    }

    public async Task<bool> UpdateWorkflowDataAsync(Guid workflowInstanceId, object workflowData, CancellationToken cancellationToken = default)
    {
        try
        {
            var workflowInstance = await _workflowInstanceRepository.GetByIdAsync(workflowInstanceId);
            if (workflowInstance == null)
            {
                return false;
            }

            workflowInstance.Data = System.Text.Json.JsonSerializer.Serialize(workflowData);
            workflowInstance.UpdatedAt = DateTime.UtcNow;

            await _workflowInstanceRepository.UpdateAsync(workflowInstance);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("WORKFLOW_DATA_UPDATED", "Workflow data updated", workflowInstanceId);

            _logger.LogInformation("Workflow data updated: {WorkflowId}", workflowInstanceId);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating workflow data: {WorkflowId}", workflowInstanceId);
            return false;
        }
    }

    public async Task<int> ProcessSLABreachesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var activeWorkflows = await _workflowInstanceRepository.FindAsync(w => 
                w.Status == WorkflowStatus.InProgress || w.Status == WorkflowStatus.PendingApproval);

            var breachedWorkflows = new List<WorkflowInstance>();

            foreach (var workflow in activeWorkflows)
            {
                var isBreached = await CheckSlaBreachAsync(workflow.Id);
                if (isBreached)
                {
                    breachedWorkflows.Add(workflow);
                }
            }

            foreach (var workflow in breachedWorkflows)
            {
                await SendWorkflowNotificationAsync(workflow.Id, "SLA_BREACH", new List<Guid> { workflow.InitiatorId }, "SLA breach detected");
            }

            _logger.LogInformation("Processed {Count} SLA breaches", breachedWorkflows.Count);
            return breachedWorkflows.Count;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing SLA breaches");
            return 0;
        }
    }

    public async Task<bool> SendWorkflowNotificationAsync(Guid workflowInstanceId, WorkflowNotificationType notificationType, CancellationToken cancellationToken = default)
    {
        try
        {
            var workflowInstance = await _workflowInstanceRepository.GetByIdAsync(workflowInstanceId);
            if (workflowInstance == null)
            {
                _logger.LogWarning("Workflow instance not found for notification: {WorkflowId}", workflowInstanceId);
                return false;
            }

            var recipientIds = new List<Guid> { workflowInstance.InitiatorId };
            if (workflowInstance.CurrentAssigneeId.HasValue)
            {
                recipientIds.Add(workflowInstance.CurrentAssigneeId.Value);
            }

            await SendWorkflowNotificationAsync(workflowInstanceId, notificationType.ToString(), recipientIds);

            await LogAuditAsync("WORKFLOW_NOTIFICATION_SENT", $"Notification sent: {notificationType}", workflowInstanceId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending workflow notification: {WorkflowId}", workflowInstanceId);
            return false;
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
                EntityName = "WorkflowInstance",
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

    public async Task<WorkflowInstance> CreateWorkflowAsync(CreateWorkflowRequest request)
    {
        try
        {
            var template = await _workflowTemplateRepository.GetByIdAsync(request.TemplateId);
            if (template == null)
            {
                throw new ArgumentException("Workflow template not found");
            }

            var workflowInstance = new WorkflowInstance
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                WorkflowTemplateId = request.TemplateId,
                Status = WorkflowStatus.Created,
                Priority = request.Priority,
                WorkflowData = JsonSerializer.Serialize(request.Data ?? new Dictionary<string, object>()),
                DueDate = request.DueDate,
                InitiatorId = request.InitiatorId,
                CurrentAssigneeId = request.AssigneeId,
                TenantId = _tenantProvider.GetTenantId(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                CurrentStepIndex = 0
            };

            await _workflowInstanceRepository.AddAsync(workflowInstance);
            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("WORKFLOW_CREATED", "Workflow instance created", workflowInstance.Id);

            _logger.LogInformation("Workflow instance created: {WorkflowId}", workflowInstance.Id);

            return workflowInstance;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating workflow instance");
            throw;
        }
    }

    public async Task<WorkflowExecutionResult> ApproveWorkflowAsync(ApproveWorkflowRequest request)
    {
        try
        {
            var result = await ApproveStepAsync(request.WorkflowInstanceId, request.ApproverId, request.Comments);
            
            if (result.IsSuccess && result.Status == WorkflowStepStatus.Running)
            {
                return await _workflowEngine.ExecuteWorkflowAsync(request.WorkflowInstanceId);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving workflow: {WorkflowId}", request.WorkflowInstanceId);
            return new WorkflowExecutionResult
            {
                IsSuccess = false,
                ErrorDetails = ex.Message
            };
        }
    }

    public async Task<WorkflowExecutionResult> RejectWorkflowAsync(RejectWorkflowRequest request)
    {
        try
        {
            return await RejectStepAsync(request.WorkflowInstanceId, request.ReviewerId, request.Reason);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting workflow: {WorkflowId}", request.WorkflowInstanceId);
            return new WorkflowExecutionResult
            {
                IsSuccess = false,
                ErrorDetails = ex.Message
            };
        }
    }

    public async Task<IEnumerable<WorkflowInstance>> GetProjectWorkflowsAsync(Guid projectId)
    {
        try
        {
            var workflows = await _workflowInstanceRepository.FindAsync(w => 
                w.ProjectId == projectId && w.TenantId == _tenantProvider.GetTenantId());
            
            return workflows.OrderByDescending(w => w.CreatedAt);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving project workflows: {ProjectId}", projectId);
            return Enumerable.Empty<WorkflowInstance>();
        }
    }

    public async Task<object> GetWorkflowAnalyticsAsync()
    {
        try
        {
            var tenantId = _tenantProvider.GetTenantId();
            var workflows = await _workflowInstanceRepository.FindAsync(w => w.TenantId == tenantId);
            var workflowsList = workflows.ToList();

            var analytics = new
            {
                TotalWorkflows = workflowsList.Count,
                CompletedWorkflows = workflowsList.Count(w => w.Status == WorkflowStatus.Completed),
                InProgressWorkflows = workflowsList.Count(w => w.Status == WorkflowStatus.InProgress),
                PendingApprovalWorkflows = workflowsList.Count(w => w.Status == WorkflowStatus.WaitingForApproval),
                RejectedWorkflows = workflowsList.Count(w => w.Status == WorkflowStatus.Rejected),
                CancelledWorkflows = workflowsList.Count(w => w.Status == WorkflowStatus.Cancelled),
                AverageCompletionTime = CalculateAverageCompletionTime(workflowsList),
                WorkflowsByType = workflowsList.GroupBy(w => w.WorkflowTemplate?.WorkflowType)
                    .Select(g => new { Type = g.Key, Count = g.Count() }),
                WorkflowsByPriority = workflowsList.GroupBy(w => w.Priority)
                    .Select(g => new { Priority = g.Key, Count = g.Count() }),
                RecentWorkflows = workflowsList.OrderByDescending(w => w.CreatedAt)
                    .Take(10)
                    .Select(w => new { w.Id, w.Name, w.Status, w.CreatedAt })
            };

            return analytics;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflow analytics");
            return new { Error = "Failed to retrieve analytics" };
        }
    }

    public async Task<IEnumerable<WorkflowTemplate>> GetWorkflowTemplatesAsync()
    {
        try
        {
            var templates = await _workflowTemplateRepository.FindAsync(t => 
                t.TenantId == _tenantProvider.GetTenantId() && t.IsActive);
            
            return templates.OrderBy(t => t.Name);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflow templates");
            return Enumerable.Empty<WorkflowTemplate>();
        }
    }

    public async Task<WorkflowTemplate> CreateWorkflowTemplateAsync(CreateWorkflowTemplateRequest request)
    {
        try
        {
            var template = new WorkflowTemplate
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                WorkflowType = request.WorkflowType,
                Version = "1.0",
                WorkflowDefinition = JsonSerializer.Serialize(request.Steps),
                ApprovalSteps = JsonSerializer.Serialize(request.Steps.Where(s => s.RequiresApproval)),
                SlaHours = request.SlaHours,
                IsActive = true,
                IsDefault = false,
                TenantId = _tenantProvider.GetTenantId(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _workflowTemplateRepository.AddAsync(template);

            for (int i = 0; i < request.Steps.Count; i++)
            {
                var stepRequest = request.Steps[i];
                var step = new WorkflowStep
                {
                    Id = Guid.NewGuid(),
                    Name = stepRequest.Name,
                    Description = stepRequest.Description,
                    StepType = Enum.Parse<WorkflowStepType>(stepRequest.Type),
                    Order = i,
                    Configuration = JsonSerializer.Serialize(stepRequest.Configuration),
                    RequiresApproval = stepRequest.RequiresApproval,
                    IsActive = true,
                    WorkflowTemplateId = template.Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _workflowStepRepository.AddAsync(step);
            }

            await _unitOfWork.SaveChangesAsync();

            await LogAuditAsync("WORKFLOW_TEMPLATE_CREATED", "Workflow template created", template.Id);

            _logger.LogInformation("Workflow template created: {TemplateId}", template.Id);

            return template;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating workflow template");
            throw;
        }
    }

    public async Task<object> CheckSlaBreachesAsync()
    {
        try
        {
            var breachedCount = await ProcessSLABreachesAsync();
            
            var tenantId = _tenantProvider.GetTenantId();
            var activeWorkflows = await _workflowInstanceRepository.FindAsync(w => 
                w.TenantId == tenantId && 
                (w.Status == WorkflowStatus.InProgress || w.Status == WorkflowStatus.WaitingForApproval));

            var slaStatus = new List<object>();
            
            foreach (var workflow in activeWorkflows)
            {
                var isBreached = await CheckSlaBreachAsync(workflow.Id);
                var timeRemaining = CalculateTimeRemaining(workflow);
                
                slaStatus.Add(new
                {
                    WorkflowId = workflow.Id,
                    WorkflowName = workflow.Name,
                    Status = workflow.Status,
                    DueDate = workflow.DueDate,
                    IsBreached = isBreached,
                    TimeRemaining = timeRemaining,
                    Priority = workflow.Priority
                });
            }

            return new
            {
                TotalBreachedWorkflows = breachedCount,
                SlaStatus = slaStatus.OrderBy(s => ((dynamic)s).TimeRemaining)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking SLA breaches");
            return new { Error = "Failed to check SLA breaches" };
        }
    }

    public async Task<object> GetWorkflowPerformanceAsync()
    {
        try
        {
            var tenantId = _tenantProvider.GetTenantId();
            var workflows = await _workflowInstanceRepository.FindAsync(w => w.TenantId == tenantId);
            var workflowsList = workflows.ToList();

            var completedWorkflows = workflowsList.Where(w => w.Status == WorkflowStatus.Completed).ToList();
            
            var performance = new
            {
                TotalWorkflows = workflowsList.Count,
                CompletedWorkflows = completedWorkflows.Count,
                CompletionRate = workflowsList.Count > 0 ? (decimal)completedWorkflows.Count / workflowsList.Count * 100 : 0,
                AverageCompletionTime = CalculateAverageCompletionTime(completedWorkflows),
                FastestCompletion = CalculateFastestCompletion(completedWorkflows),
                SlowestCompletion = CalculateSlowestCompletion(completedWorkflows),
                WorkflowsCompletedOnTime = CalculateOnTimeCompletions(completedWorkflows),
                PerformanceByType = workflowsList.GroupBy(w => w.WorkflowTemplate?.WorkflowType)
                    .Select(g => new
                    {
                        Type = g.Key,
                        Total = g.Count(),
                        Completed = g.Count(w => w.Status == WorkflowStatus.Completed),
                        AverageTime = CalculateAverageCompletionTime(g.Where(w => w.Status == WorkflowStatus.Completed).ToList())
                    }),
                MonthlyTrends = CalculateMonthlyTrends(workflowsList)
            };

            return performance;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving workflow performance");
            return new { Error = "Failed to retrieve performance metrics" };
        }
    }

    #region Private Helper Methods

    private TimeSpan? CalculateAverageCompletionTime(List<WorkflowInstance> workflows)
    {
        var completedWorkflows = workflows.Where(w => 
            w.StartedAt.HasValue && w.CompletedAt.HasValue).ToList();
        
        if (!completedWorkflows.Any())
            return null;

        var totalTicks = completedWorkflows.Sum(w => 
            (w.CompletedAt!.Value - w.StartedAt!.Value).Ticks);
        
        return new TimeSpan(totalTicks / completedWorkflows.Count);
    }

    private TimeSpan? CalculateFastestCompletion(List<WorkflowInstance> workflows)
    {
        var completedWorkflows = workflows.Where(w => 
            w.StartedAt.HasValue && w.CompletedAt.HasValue).ToList();
        
        if (!completedWorkflows.Any())
            return null;

        return completedWorkflows.Min(w => w.CompletedAt!.Value - w.StartedAt!.Value);
    }

    private TimeSpan? CalculateSlowestCompletion(List<WorkflowInstance> workflows)
    {
        var completedWorkflows = workflows.Where(w => 
            w.StartedAt.HasValue && w.CompletedAt.HasValue).ToList();
        
        if (!completedWorkflows.Any())
            return null;

        return completedWorkflows.Max(w => w.CompletedAt!.Value - w.StartedAt!.Value);
    }

    private int CalculateOnTimeCompletions(List<WorkflowInstance> workflows)
    {
        return workflows.Count(w => 
            w.CompletedAt.HasValue && 
            w.DueDate.HasValue && 
            w.CompletedAt.Value <= w.DueDate.Value);
    }

    private object CalculateMonthlyTrends(List<WorkflowInstance> workflows)
    {
        var last12Months = Enumerable.Range(0, 12)
            .Select(i => DateTime.UtcNow.AddMonths(-i))
            .Select(date => new
            {
                Month = date.ToString("yyyy-MM"),
                Created = workflows.Count(w => w.CreatedAt.Year == date.Year && w.CreatedAt.Month == date.Month),
                Completed = workflows.Count(w => w.CompletedAt?.Year == date.Year && w.CompletedAt?.Month == date.Month)
            })
            .OrderBy(x => x.Month)
            .ToList();

        return last12Months;
    }

    private TimeSpan? CalculateTimeRemaining(WorkflowInstance workflow)
    {
        if (!workflow.DueDate.HasValue)
            return null;

        var remaining = workflow.DueDate.Value - DateTime.UtcNow;
        return remaining.TotalMilliseconds > 0 ? remaining : TimeSpan.Zero;
    }

    #endregion
}
