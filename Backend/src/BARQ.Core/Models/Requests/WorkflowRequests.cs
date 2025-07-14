using BARQ.Core.Enums;

namespace BARQ.Core.Models.Requests;

public class CreateWorkflowInstanceRequest
{
    public Guid WorkflowTemplateId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;
    public Guid InitiatedByUserId { get; set; }
    public object? WorkflowData { get; set; }
    public DateTime? ScheduledStartDate { get; set; }
    public List<Guid> ApproverUserIds { get; set; } = new();
}

public class UpdateWorkflowInstanceRequest
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public ProjectPriority? Priority { get; set; }
    public WorkflowStatus? Status { get; set; }
    public object? WorkflowData { get; set; }
    public string? Comments { get; set; }
}

public class WorkflowApprovalRequest
{
    public Guid WorkflowInstanceId { get; set; }
    public Guid StepId { get; set; }
    public Guid ApproverId { get; set; }
    public WorkflowApprovalAction Action { get; set; }
    public string? Comments { get; set; }
    public string? Reason { get; set; }
}

public enum WorkflowApprovalAction
{
    Approve,
    Reject,
    RequestChanges,
    Escalate
}
