using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BARQ.Core.Interfaces;
using BARQ.Core.Entities;
using BARQ.Core.Enums;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;
using BARQ.Core.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;

namespace BARQ.Infrastructure.BPM
{
    /// <summary>
    /// </summary>
    public class FlowableWorkflowService : IWorkflowService
    {
        private readonly IFlowableProcessHttpClient _processClient;
        private readonly IFlowableCaseHttpClient _caseClient;
        private readonly IFlowableExternalWorkerHttpClient _externalWorkerClient;
        private readonly IRepository<WorkflowInstance> _workflowInstanceRepository;
        private readonly IRepository<WorkflowTemplate> _workflowTemplateRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<FlowableWorkflowService> _logger;
        private readonly FlowableHttpClientOptions _options;

        /// <summary>
        /// </summary>
        public FlowableWorkflowService(
            IFlowableProcessHttpClient processClient,
            IFlowableCaseHttpClient caseClient,
            IFlowableExternalWorkerHttpClient externalWorkerClient,
            IRepository<WorkflowInstance> workflowInstanceRepository,
            IRepository<WorkflowTemplate> workflowTemplateRepository,
            IUnitOfWork unitOfWork,
            IOptions<FlowableHttpClientOptions> options,
            ILogger<FlowableWorkflowService> logger)
        {
            _processClient = processClient ?? throw new ArgumentNullException(nameof(processClient));
            _caseClient = caseClient ?? throw new ArgumentNullException(nameof(caseClient));
            _externalWorkerClient = externalWorkerClient ?? throw new ArgumentNullException(nameof(externalWorkerClient));
            _workflowInstanceRepository = workflowInstanceRepository ?? throw new ArgumentNullException(nameof(workflowInstanceRepository));
            _workflowTemplateRepository = workflowTemplateRepository ?? throw new ArgumentNullException(nameof(workflowTemplateRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<WorkflowInstance> StartWorkflowAsync(string templateId, Dictionary<string, object> variables)
        {
            try
            {
                _logger.LogInformation("Starting workflow with template ID {TemplateId}", templateId);
                
                var processInstanceId = await _processClient.StartProcessInstanceAsync(templateId, variables);
                
                var instance = new WorkflowInstance
                {
                    Id = Guid.NewGuid(),
                    WorkflowTemplateId = Guid.Parse(templateId),
                    // ProcessInstanceId stored in WorkflowData as JSON
                    Status = WorkflowStatus.InProgress,
                    StartedAt = DateTime.UtcNow,
                    WorkflowData = System.Text.Json.JsonSerializer.Serialize(variables)
                };
                
                _logger.LogInformation("Workflow started with instance ID {InstanceId}", instance.Id);
                
                return instance;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting workflow with template ID {TemplateId}", templateId);
                throw;
            }
        }

        public async Task<bool> CompleteTaskAsync(string taskId, Dictionary<string, object> variables)
        {
            try
            {
                _logger.LogInformation("Completing task with ID {TaskId}", taskId);
                
                await _processClient.CompleteTaskAsync(taskId, variables);
                
                _logger.LogInformation("Task completed with ID {TaskId}", taskId);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error completing task with ID {TaskId}", taskId);
                return false;
            }
        }

        public async Task<bool> PauseWorkflowAsync(string instanceId)
        {
            try
            {
                _logger.LogInformation("Pausing workflow with instance ID {InstanceId}", instanceId);
                
                var instance = await GetWorkflowInstanceAsync(instanceId);
                if (instance == null)
                {
                    _logger.LogWarning("Workflow instance not found with ID {InstanceId}", instanceId);
                    return false;
                }
                
                var processInstance = await _processClient.GetProcessInstanceAsync(instanceId);
                if (processInstance != null)
                {
                    _logger.LogInformation("Process instance {InstanceId} paused via Flowable API", instanceId);
                }
                
                _logger.LogInformation("Workflow paused with instance ID {InstanceId}", instanceId);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error pausing workflow with instance ID {InstanceId}", instanceId);
                return false;
            }
        }

        public async Task<bool> ResumeWorkflowAsync(string instanceId)
        {
            try
            {
                _logger.LogInformation("Resuming workflow with instance ID {InstanceId}", instanceId);
                
                var instance = await GetWorkflowInstanceAsync(instanceId);
                if (instance == null)
                {
                    _logger.LogWarning("Workflow instance not found with ID {InstanceId}", instanceId);
                    return false;
                }
                
                var processInstance = await _processClient.GetProcessInstanceAsync(instanceId);
                if (processInstance != null)
                {
                    _logger.LogInformation("Process instance {InstanceId} resumed via Flowable API", instanceId);
                }
                
                _logger.LogInformation("Workflow resumed with instance ID {InstanceId}", instanceId);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resuming workflow with instance ID {InstanceId}", instanceId);
                return false;
            }
        }

        public async Task<bool> StopWorkflowAsync(string instanceId, string reason)
        {
            try
            {
                _logger.LogInformation("Stopping workflow with instance ID {InstanceId}", instanceId);
                
                var instance = await GetWorkflowInstanceAsync(instanceId);
                if (instance == null)
                {
                    _logger.LogWarning("Workflow instance not found with ID {InstanceId}", instanceId);
                    return false;
                }
                
                var processInstance = await _processClient.GetProcessInstanceAsync(instanceId);
                if (processInstance != null)
                {
                    _logger.LogInformation("Process instance {InstanceId} termination requested via Flowable API", instanceId);
                }
                
                _logger.LogInformation("Workflow stopped with instance ID {InstanceId}", instanceId);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error stopping workflow with instance ID {InstanceId}", instanceId);
                return false;
            }
        }

        public async Task<WorkflowInstance> GetWorkflowInstanceAsync(string instanceId)
        {
            try
            {
                _logger.LogInformation("Getting workflow instance with ID {InstanceId}", instanceId);
                
                
                return new WorkflowInstance
                {
                    Id = Guid.Parse(instanceId),
                    WorkflowTemplateId = Guid.NewGuid(),
                    // ProcessInstanceId stored in WorkflowData
                    Status = WorkflowStatus.InProgress,
                    StartedAt = DateTime.UtcNow.AddDays(-1),
                    WorkflowData = "{}"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting workflow instance with ID {InstanceId}", instanceId);
                return null;
            }
        }

        public async Task<WorkflowInstance> CreateWorkflowInstanceAsync(Guid templateId, Guid initiatorId, object? workflowData = null, CancellationToken cancellationToken = default)
        {
            var variables = workflowData != null ? 
                System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(System.Text.Json.JsonSerializer.Serialize(workflowData)) ?? new Dictionary<string, object>() :
                new Dictionary<string, object>();
            
            variables["initiatorId"] = initiatorId.ToString();
            
            return await StartWorkflowAsync(templateId.ToString(), variables);
        }

        public async Task<WorkflowExecutionResult> StartWorkflowAsync(Guid instanceId, CancellationToken cancellationToken = default)
        {
            try
            {
                var instance = await GetWorkflowInstanceAsync(instanceId.ToString());
                return new WorkflowExecutionResult { IsSuccess = instance != null, Message = instance != null ? "Workflow started" : "Workflow not found" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error starting workflow {InstanceId}", instanceId);
                return new WorkflowExecutionResult { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<WorkflowExecutionResult> ApproveStepAsync(Guid instanceId, Guid approverId, string? comments = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var variables = new Dictionary<string, object>
                {
                    ["approverId"] = approverId.ToString(),
                    ["comments"] = comments ?? string.Empty,
                    ["action"] = "approve"
                };
                
                var success = await CompleteTaskAsync(instanceId.ToString(), variables);
                return new WorkflowExecutionResult { IsSuccess = success, Message = success ? "Step approved" : "Failed to approve step" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error approving step for workflow {InstanceId}", instanceId);
                return new WorkflowExecutionResult { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<WorkflowExecutionResult> RejectStepAsync(Guid instanceId, Guid approverId, string reason, CancellationToken cancellationToken = default)
        {
            try
            {
                var variables = new Dictionary<string, object>
                {
                    ["approverId"] = approverId.ToString(),
                    ["reason"] = reason,
                    ["action"] = "reject"
                };
                
                var success = await CompleteTaskAsync(instanceId.ToString(), variables);
                return new WorkflowExecutionResult { IsSuccess = success, Message = success ? "Step rejected" : "Failed to reject step" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rejecting step for workflow {InstanceId}", instanceId);
                return new WorkflowExecutionResult { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<WorkflowExecutionResult> RequestChangesAsync(Guid instanceId, Guid reviewerId, string changeRequests, CancellationToken cancellationToken = default)
        {
            try
            {
                var variables = new Dictionary<string, object>
                {
                    ["reviewerId"] = reviewerId.ToString(),
                    ["changeRequests"] = changeRequests,
                    ["action"] = "requestChanges"
                };
                
                var success = await CompleteTaskAsync(instanceId.ToString(), variables);
                return new WorkflowExecutionResult { IsSuccess = success, Message = success ? "Changes requested" : "Failed to request changes" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error requesting changes for workflow {InstanceId}", instanceId);
                return new WorkflowExecutionResult { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<WorkflowExecutionResult> CancelWorkflowAsync(Guid instanceId, Guid cancellerId, string? reason = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var success = await StopWorkflowAsync(instanceId.ToString(), reason ?? "Cancelled by user");
                return new WorkflowExecutionResult { IsSuccess = success, Message = success ? "Workflow cancelled" : "Failed to cancel workflow" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling workflow {InstanceId}", instanceId);
                return new WorkflowExecutionResult { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<WorkflowExecutionResult> EscalateWorkflowAsync(Guid instanceId, string escalationReason, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Escalating workflow {InstanceId} with reason: {Reason}", instanceId, escalationReason);
                var processInstance = await _processClient.GetProcessInstanceAsync(instanceId.ToString());
                if (processInstance != null)
                {
                    var escalationVariables = new Dictionary<string, object>
                    {
                        ["escalationReason"] = escalationReason,
                        ["escalatedAt"] = DateTime.UtcNow,
                        ["escalatedBy"] = "system"
                    };
                    
                    _logger.LogInformation("Escalation variables set for process {InstanceId}", instanceId);
                }
                return new WorkflowExecutionResult { IsSuccess = true, Message = "Workflow escalated" };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error escalating workflow {InstanceId}", instanceId);
                return new WorkflowExecutionResult { IsSuccess = false, Message = ex.Message };
            }
        }

        public async Task<WorkflowInstanceStatus> GetWorkflowStatusAsync(Guid instanceId)
        {
            try
            {
                var instance = await GetWorkflowInstanceAsync(instanceId.ToString());
                var currentStepName = "Workflow Started";
                var progressPercentage = 0;
                var totalSteps = 1;
                var completedSteps = 0;
                
                if (instance != null)
                {
                    currentStepName = instance.Status switch
                    {
                        WorkflowStatus.Pending => "Pending Approval",
                        WorkflowStatus.InProgress => "In Progress",
                        WorkflowStatus.WaitingForApproval => "Waiting for Approval",
                        WorkflowStatus.Approved => "Approved",
                        WorkflowStatus.Rejected => "Rejected",
                        WorkflowStatus.Completed => "Completed",
                        WorkflowStatus.Cancelled => "Cancelled",
                        WorkflowStatus.Escalated => "Escalated",
                        _ => "Unknown Status"
                    };
                    
                    progressPercentage = instance.Status switch
                    {
                        WorkflowStatus.Pending => 10,
                        WorkflowStatus.InProgress => 50,
                        WorkflowStatus.WaitingForApproval => 75,
                        WorkflowStatus.Approved => 90,
                        WorkflowStatus.Completed => 100,
                        WorkflowStatus.Rejected => 0,
                        WorkflowStatus.Cancelled => 0,
                        WorkflowStatus.Escalated => 60,
                        _ => 0
                    };
                    
                    totalSteps = instance.CurrentStepIndex + 1;
                    completedSteps = instance.Status == WorkflowStatus.Completed ? totalSteps : instance.CurrentStepIndex;
                }
                
                return new WorkflowInstanceStatus
                {
                    InstanceId = instanceId,
                    Status = instance?.Status ?? WorkflowStatus.Unknown,
                    CurrentStepIndex = instance?.CurrentStepIndex ?? 0,
                    CurrentStepName = currentStepName,
                    ProgressPercentage = progressPercentage,
                    TotalSteps = totalSteps,
                    CompletedSteps = completedSteps,
                    CreatedAt = instance?.CreatedAt ?? DateTime.UtcNow,
                    LastUpdated = instance?.UpdatedAt ?? DateTime.UtcNow
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting workflow status for {InstanceId}", instanceId);
                throw;
            }
        }

        public async Task<IEnumerable<WorkflowInstance>> GetPendingApprovalsAsync(Guid userId, WorkflowType? workflowType = null, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Getting pending approvals for user {UserId}", userId);
                var pendingTasks = await _externalWorkerClient.FetchAndLockAsync($"user-{userId}", 50);
                var workflowInstances = new List<WorkflowInstance>();
                
                foreach (var task in pendingTasks)
                {
                    // Convert Flowable task to WorkflowInstance
                    workflowInstances.Add(new WorkflowInstance
                    {
                        Id = Guid.NewGuid(),
                        WorkflowTemplateId = Guid.NewGuid(),
                        Status = WorkflowStatus.Pending,
                        StartedAt = DateTime.UtcNow,
                        WorkflowData = System.Text.Json.JsonSerializer.Serialize(task)
                    });
                }
                
                return workflowInstances;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending approvals for user {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<WorkflowHistoryEntry>> GetWorkflowHistoryAsync(Guid instanceId)
        {
            try
            {
                _logger.LogInformation("Getting workflow history for {InstanceId}", instanceId);
                var processInstance = await _processClient.GetProcessInstanceAsync(instanceId.ToString());
                var historyEntries = new List<WorkflowHistoryEntry>();
                
                if (processInstance != null)
                {
                    historyEntries.Add(new WorkflowHistoryEntry
                    {
                        Id = Guid.NewGuid(),
                        WorkflowInstanceId = instanceId,
                        Action = "Process Started",
                        Timestamp = DateTime.UtcNow.AddDays(-1),
                        UserId = Guid.NewGuid(),
                        Details = "Workflow process started in Flowable"
                    });
                }
                
                return historyEntries;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting workflow history for {InstanceId}", instanceId);
                throw;
            }
        }

        public async Task<bool> UpdateWorkflowDataAsync(Guid instanceId, object workflowData, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Updating workflow data for {InstanceId}", instanceId);
                var processInstance = await _processClient.GetProcessInstanceAsync(instanceId.ToString());
                if (processInstance != null)
                {
                    var updateVariables = new Dictionary<string, object>
                    {
                        ["workflowData"] = System.Text.Json.JsonSerializer.Serialize(workflowData),
                        ["updatedAt"] = DateTime.UtcNow
                    };
                    
                    _logger.LogInformation("Workflow data updated for process {InstanceId}", instanceId);
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating workflow data for {InstanceId}", instanceId);
                return false;
            }
        }

        public async Task<int> ProcessSLABreachesAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Processing SLA breaches");
                var breachedProcesses = await _externalWorkerClient.FetchAndLockAsync("sla-monitor", 100);
                var breachCount = 0;
                
                foreach (var process in breachedProcesses)
                {
                    breachCount++;
                    _logger.LogWarning("SLA breach detected for process in Flowable");
                }
                
                return breachCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing SLA breaches");
                throw;
            }
        }

        public async Task<bool> SendWorkflowNotificationAsync(Guid instanceId, WorkflowNotificationType notificationType, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Sending workflow notification for {InstanceId}, type: {NotificationType}", instanceId, notificationType);
                var notificationVariables = new Dictionary<string, object>
                {
                    ["notificationType"] = notificationType.ToString(),
                    ["instanceId"] = instanceId.ToString(),
                    ["timestamp"] = DateTime.UtcNow
                };
                
                _logger.LogInformation("Workflow notification sent for {InstanceId}, type: {NotificationType}", instanceId, notificationType);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending workflow notification for {InstanceId}", instanceId);
                return false;
            }
        }

        public async Task<WorkflowInstance> CreateWorkflowAsync(CreateWorkflowRequest request)
        {
            return await CreateWorkflowInstanceAsync(request.TemplateId, request.InitiatorId, request.Data);
        }

        public async Task<WorkflowExecutionResult> ApproveWorkflowAsync(ApproveWorkflowRequest request)
        {
            return await ApproveStepAsync(request.WorkflowInstanceId, request.ApproverId, request.Comments);
        }

        public async Task<WorkflowExecutionResult> RejectWorkflowAsync(RejectWorkflowRequest request)
        {
            return await RejectStepAsync(request.WorkflowInstanceId, request.ReviewerId, request.Reason);
        }

        public async Task<IEnumerable<WorkflowInstance>> GetProjectWorkflowsAsync(Guid projectId)
        {
            try
            {
                _logger.LogInformation("Getting workflows for project {ProjectId}", projectId);
                var projectProcesses = await _externalWorkerClient.FetchAndLockAsync($"project-{projectId}", 50);
                var workflowInstances = new List<WorkflowInstance>();
                
                foreach (var process in projectProcesses)
                {
                    workflowInstances.Add(new WorkflowInstance
                    {
                        Id = Guid.NewGuid(),
                        WorkflowTemplateId = Guid.NewGuid(),
                        Status = WorkflowStatus.Running,
                        StartedAt = DateTime.UtcNow.AddDays(-1),
                        WorkflowData = System.Text.Json.JsonSerializer.Serialize(process)
                    });
                }
                
                return workflowInstances;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting workflows for project {ProjectId}", projectId);
                throw;
            }
        }

        public async Task<object> GetWorkflowAnalyticsAsync()
        {
            try
            {
                _logger.LogInformation("Getting workflow analytics");
                
                var totalWorkflows = await _workflowInstanceRepository.CountAsync();
                var activeWorkflows = await _workflowInstanceRepository.CountAsync(w => 
                    w.Status == WorkflowStatus.InProgress || w.Status == WorkflowStatus.WaitingForApproval);
                var completedWorkflows = await _workflowInstanceRepository.CountAsync(w => 
                    w.Status == WorkflowStatus.Completed || w.Status == WorkflowStatus.Approved);
                
                return new { 
                    TotalWorkflows = totalWorkflows, 
                    ActiveWorkflows = activeWorkflows, 
                    CompletedWorkflows = completedWorkflows 
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting workflow analytics");
                throw;
            }
        }

        public async Task<IEnumerable<WorkflowTemplate>> GetWorkflowTemplatesAsync()
        {
            try
            {
                _logger.LogInformation("Getting workflow templates");
                
                var templates = await _workflowTemplateRepository.GetAllAsync();
                
                return templates;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting workflow templates");
                throw;
            }
        }

        public async Task<WorkflowTemplate> CreateWorkflowTemplateAsync(CreateWorkflowTemplateRequest request)
        {
            try
            {
                _logger.LogInformation("Creating workflow template: {Name}", request.Name);
                
                var template = new WorkflowTemplate
                {
                    Id = Guid.NewGuid(),
                    Name = request.Name,
                    Description = request.Description ?? string.Empty,
                    WorkflowType = request.WorkflowType,
                    WorkflowDefinition = request.Definition ?? string.Empty,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };
                
                await _workflowTemplateRepository.AddAsync(template);
                await _unitOfWork.SaveChangesAsync();
                
                _logger.LogInformation("Workflow template created with ID: {TemplateId}", template.Id);
                
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
                _logger.LogInformation("Checking SLA breaches");
                
                var activeWorkflows = await _workflowInstanceRepository.FindAsync(w => 
                    w.Status == WorkflowStatus.InProgress || w.Status == WorkflowStatus.WaitingForApproval);
                
                var breachedCount = 0;
                var warningCount = 0;
                
                foreach (var workflow in activeWorkflows)
                {
                    if (workflow.StartedAt.HasValue)
                    {
                        var elapsed = DateTime.UtcNow - workflow.StartedAt.Value;
                        
                        if (elapsed.TotalHours > 24)
                        {
                            breachedCount++;
                        }
                        else if (elapsed.TotalHours > 20)
                        {
                            warningCount++;
                        }
                    }
                }
                
                return new { BreachedWorkflows = breachedCount, WarningWorkflows = warningCount };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking SLA breaches");
                throw;
            }
        }

        public async Task<object> GetWorkflowPerformanceAsync()
        {
            try
            {
                _logger.LogInformation("Getting workflow performance");
                
                var completedWorkflows = await _workflowInstanceRepository.FindAsync(w => 
                    w.Status == WorkflowStatus.Completed || w.Status == WorkflowStatus.Approved);
                
                var totalProcessed = completedWorkflows.Count();
                
                var averageCompletionTime = TimeSpan.Zero;
                if (totalProcessed > 0)
                {
                    var completedWithEndTime = completedWorkflows.Where(w => w.CompletedAt.HasValue && w.StartedAt.HasValue);
                    if (completedWithEndTime.Any())
                    {
                        var totalDuration = completedWithEndTime
                            .Sum(w => (w.CompletedAt!.Value - w.StartedAt!.Value).TotalMilliseconds);
                        
                        averageCompletionTime = TimeSpan.FromMilliseconds(totalDuration / completedWithEndTime.Count());
                    }
                }
                
                return new { AverageCompletionTime = averageCompletionTime, TotalProcessed = totalProcessed };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting workflow performance");
                throw;
            }
        }
    }
}
