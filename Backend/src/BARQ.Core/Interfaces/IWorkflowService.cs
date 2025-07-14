using BARQ.Core.Entities;
using BARQ.Core.Enums;
using BARQ.Core.Models.Requests;

namespace BARQ.Core.Interfaces;

/// <summary>
/// Interface for workflow management service
/// </summary>
public interface IWorkflowService
{
    /// <summary>
    /// Create a new workflow instance from template
    /// </summary>
    /// <param name="templateId">Workflow template ID</param>
    /// <param name="initiatorId">User who initiated the workflow</param>
    /// <param name="workflowData">Initial workflow data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Created workflow instance</returns>
    Task<WorkflowInstance> CreateWorkflowInstanceAsync(Guid templateId, Guid initiatorId, object? workflowData = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Start a workflow instance
    /// </summary>
    /// <param name="instanceId">Workflow instance ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Start result</returns>
    Task<WorkflowExecutionResult> StartWorkflowAsync(Guid instanceId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Approve current workflow step
    /// </summary>
    /// <param name="instanceId">Workflow instance ID</param>
    /// <param name="approverId">User who is approving</param>
    /// <param name="comments">Approval comments</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Approval result</returns>
    Task<WorkflowExecutionResult> ApproveStepAsync(Guid instanceId, Guid approverId, string? comments = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Reject current workflow step
    /// </summary>
    /// <param name="instanceId">Workflow instance ID</param>
    /// <param name="approverId">User who is rejecting</param>
    /// <param name="reason">Rejection reason</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Rejection result</returns>
    Task<WorkflowExecutionResult> RejectStepAsync(Guid instanceId, Guid approverId, string reason, CancellationToken cancellationToken = default);

    /// <summary>
    /// Request changes for current workflow step
    /// </summary>
    /// <param name="instanceId">Workflow instance ID</param>
    /// <param name="reviewerId">User requesting changes</param>
    /// <param name="changeRequests">Requested changes</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Change request result</returns>
    Task<WorkflowExecutionResult> RequestChangesAsync(Guid instanceId, Guid reviewerId, string changeRequests, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cancel a workflow instance
    /// </summary>
    /// <param name="instanceId">Workflow instance ID</param>
    /// <param name="cancellerId">User who is cancelling</param>
    /// <param name="reason">Cancellation reason</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Cancellation result</returns>
    Task<WorkflowExecutionResult> CancelWorkflowAsync(Guid instanceId, Guid cancellerId, string? reason = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Escalate a workflow step due to SLA breach
    /// </summary>
    /// <param name="instanceId">Workflow instance ID</param>
    /// <param name="escalationReason">Escalation reason</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Escalation result</returns>
    Task<WorkflowExecutionResult> EscalateWorkflowAsync(Guid instanceId, string escalationReason, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get workflow instance status
    /// </summary>
    /// <param name="instanceId">Workflow instance ID</param>
    /// <returns>Workflow status</returns>
    Task<WorkflowInstanceStatus> GetWorkflowStatusAsync(Guid instanceId);

    /// <summary>
    /// Get pending approvals for a user
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="workflowType">Optional workflow type filter</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Pending approvals</returns>
    Task<IEnumerable<WorkflowInstance>> GetPendingApprovalsAsync(Guid userId, WorkflowType? workflowType = null, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get workflow history for an instance
    /// </summary>
    /// <param name="instanceId">Workflow instance ID</param>
    /// <returns>Workflow history</returns>
    Task<IEnumerable<WorkflowHistoryEntry>> GetWorkflowHistoryAsync(Guid instanceId);

    /// <summary>
    /// Update workflow data
    /// </summary>
    /// <param name="instanceId">Workflow instance ID</param>
    /// <param name="workflowData">Updated workflow data</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Update result</returns>
    Task<bool> UpdateWorkflowDataAsync(Guid instanceId, object workflowData, CancellationToken cancellationToken = default);

    /// <summary>
    /// Check for SLA breaches and trigger escalations
    /// </summary>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Number of escalations triggered</returns>
    Task<int> ProcessSLABreachesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Send workflow notifications
    /// </summary>
    /// <param name="instanceId">Workflow instance ID</param>
    /// <param name="notificationType">Type of notification</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Notification result</returns>
    Task<bool> SendWorkflowNotificationAsync(Guid instanceId, WorkflowNotificationType notificationType, CancellationToken cancellationToken = default);

    Task<WorkflowInstance> CreateWorkflowAsync(CreateWorkflowRequest request);
    Task<WorkflowExecutionResult> ApproveWorkflowAsync(ApproveWorkflowRequest request);
    Task<WorkflowExecutionResult> RejectWorkflowAsync(RejectWorkflowRequest request);
    Task<IEnumerable<WorkflowInstance>> GetProjectWorkflowsAsync(Guid projectId);
    Task<object> GetWorkflowAnalyticsAsync();
    Task<IEnumerable<WorkflowTemplate>> GetWorkflowTemplatesAsync();
    Task<WorkflowTemplate> CreateWorkflowTemplateAsync(CreateWorkflowTemplateRequest request);
    Task<object> CheckSlaBreachesAsync();
    Task<object> GetWorkflowPerformanceAsync();
}

/// <summary>
/// Workflow execution result
/// </summary>
public class WorkflowExecutionResult
{
    /// <summary>
    /// Execution success status
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Result message
    /// </summary>
    public string? Message { get; set; }

    /// <summary>
    /// New workflow status
    /// </summary>
    public WorkflowStatus NewStatus { get; set; }

    /// <summary>
    /// Next step index
    /// </summary>
    public int? NextStepIndex { get; set; }

    /// <summary>
    /// Next assignee
    /// </summary>
    public Guid? NextAssigneeId { get; set; }

    /// <summary>
    /// Workflow is completed
    /// </summary>
    public bool IsCompleted { get; set; }

    /// <summary>
    /// Error details if failed
    /// </summary>
    public string? ErrorDetails { get; set; }

    /// <summary>
    /// Additional data
    /// </summary>
    public Dictionary<string, object>? AdditionalData { get; set; }
}

/// <summary>
/// Workflow instance status
/// </summary>
public class WorkflowInstanceStatus
{
    /// <summary>
    /// Instance ID
    /// </summary>
    public Guid InstanceId { get; set; }

    /// <summary>
    /// Workflow instance ID (alias for InstanceId)
    /// </summary>
    public Guid WorkflowInstanceId 
    { 
        get => InstanceId; 
        set => InstanceId = value; 
    }

    /// <summary>
    /// Current status
    /// </summary>
    public WorkflowStatus Status { get; set; }

    /// <summary>
    /// Current step index
    /// </summary>
    public int CurrentStepIndex { get; set; }

    /// <summary>
    /// Current step name
    /// </summary>
    public string? CurrentStepName { get; set; }

    /// <summary>
    /// Current step (alias for CurrentStepName)
    /// </summary>
    public string? CurrentStep 
    { 
        get => CurrentStepName; 
        set => CurrentStepName = value; 
    }

    /// <summary>
    /// Current assignee
    /// </summary>
    public Guid? CurrentAssigneeId { get; set; }

    /// <summary>
    /// Current assignee name
    /// </summary>
    public string? CurrentAssigneeName { get; set; }

    /// <summary>
    /// Due date
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Is overdue
    /// </summary>
    public bool IsOverdue { get; set; }

    /// <summary>
    /// Progress percentage
    /// </summary>
    public decimal ProgressPercentage { get; set; }

    /// <summary>
    /// Progress (alias for ProgressPercentage)
    /// </summary>
    public decimal Progress 
    { 
        get => ProgressPercentage; 
        set => ProgressPercentage = value; 
    }

    /// <summary>
    /// Total steps
    /// </summary>
    public int TotalSteps { get; set; }

    /// <summary>
    /// Completed steps
    /// </summary>
    public int CompletedSteps { get; set; }

    /// <summary>
    /// Created at timestamp
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// Completed at timestamp
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// </summary>
    public DateTime LastUpdated { get; set; }

    /// <summary>
    /// Error message if any
    /// </summary>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Workflow history entry
/// </summary>
public class WorkflowHistoryEntry
{
    /// <summary>
    /// Entry ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Workflow instance ID
    /// </summary>
    public Guid WorkflowInstanceId { get; set; }

    /// <summary>
    /// Timestamp
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Action performed
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Description of the action
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// User who performed the action
    /// </summary>
    public Guid? UserId { get; set; }

    /// <summary>
    /// User name
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// Step index
    /// </summary>
    public int? StepIndex { get; set; }

    /// <summary>
    /// Step name
    /// </summary>
    public string? StepName { get; set; }

    /// <summary>
    /// Comments or notes
    /// </summary>
    public string? Comments { get; set; }

    /// <summary>
    /// Previous status
    /// </summary>
    public WorkflowStatus? PreviousStatus { get; set; }

    /// <summary>
    /// New status
    /// </summary>
    public WorkflowStatus? NewStatus { get; set; }

    /// <summary>
    /// Additional data
    /// </summary>
    public string? AdditionalData { get; set; }

    /// <summary>
    /// Additional details
    /// </summary>
    public string? Details { get; set; }
}

/// <summary>
/// Workflow notification type
/// </summary>
public enum WorkflowNotificationType
{
    /// <summary>
    /// Workflow started notification
    /// </summary>
    WorkflowStarted = 0,

    /// <summary>
    /// Approval required notification
    /// </summary>
    ApprovalRequired = 1,

    /// <summary>
    /// Workflow approved notification
    /// </summary>
    WorkflowApproved = 2,

    /// <summary>
    /// Workflow rejected notification
    /// </summary>
    WorkflowRejected = 3,

    /// <summary>
    /// Changes requested notification
    /// </summary>
    ChangesRequested = 4,

    /// <summary>
    /// Workflow completed notification
    /// </summary>
    WorkflowCompleted = 5,

    /// <summary>
    /// SLA breach warning
    /// </summary>
    SLAWarning = 6,

    /// <summary>
    /// Workflow escalated notification
    /// </summary>
    WorkflowEscalated = 7,

    /// <summary>
    /// Workflow cancelled notification
    /// </summary>
    WorkflowCancelled = 8
}

