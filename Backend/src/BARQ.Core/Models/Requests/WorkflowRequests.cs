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

public class CreateWorkflowRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid TemplateId { get; set; }
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;
    public Dictionary<string, object> Data { get; set; } = new();
    public Guid? ProjectId { get; set; }
    public DateTime? DueDate { get; set; }
    public List<Guid> ApproverUserIds { get; set; } = new();
    public Guid InitiatorId { get; set; }
    public Guid? AssigneeId { get; set; }
}

public class ApproveWorkflowRequest
{
    public Guid WorkflowId { get; set; }
    public Guid WorkflowInstanceId { get; set; }
    public Guid ApproverId { get; set; }
    public string? Comments { get; set; }
    public Dictionary<string, object>? AdditionalData { get; set; }
}

public class RejectWorkflowRequest
{
    public Guid WorkflowId { get; set; }
    public Guid WorkflowInstanceId { get; set; }
    public Guid ReviewerId { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string? Comments { get; set; }
}

public class CreateWorkflowTemplateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public WorkflowType WorkflowType { get; set; }
    public List<WorkflowStepTemplateRequest> Steps { get; set; } = new();
    public Dictionary<string, object> DefaultData { get; set; } = new();
    public int SlaHours { get; set; } = 24;
}

public class WorkflowStepTemplateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int Order { get; set; }
    public Dictionary<string, object> Configuration { get; set; } = new();
    public bool RequiresApproval { get; set; }
}
