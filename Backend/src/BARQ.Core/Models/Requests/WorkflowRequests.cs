using BARQ.Core.Enums;

namespace BARQ.Core.Models.Requests;

/// <summary>
/// </summary>
public class CreateWorkflowInstanceRequest
{
    /// <summary>
    /// </summary>
    public Guid WorkflowTemplateId { get; set; }
    
    /// <summary>
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;
    
    /// <summary>
    /// </summary>
    public Guid InitiatedByUserId { get; set; }
    
    /// <summary>
    /// </summary>
    public object? WorkflowData { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? ScheduledStartDate { get; set; }
    
    /// <summary>
    /// </summary>
    public List<Guid> ApproverUserIds { get; set; } = new();
}

/// <summary>
/// </summary>
public class UpdateWorkflowInstanceRequest
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Title { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// </summary>
    public ProjectPriority? Priority { get; set; }
    
    /// <summary>
    /// </summary>
    public WorkflowStatus? Status { get; set; }
    
    /// <summary>
    /// </summary>
    public object? WorkflowData { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Comments { get; set; }
}

/// <summary>
/// </summary>
public class WorkflowApprovalRequest
{
    /// <summary>
    /// </summary>
    public Guid WorkflowInstanceId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid StepId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid ApproverId { get; set; }
    
    /// <summary>
    /// </summary>
    public WorkflowApprovalAction Action { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Comments { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Reason { get; set; }
}

/// <summary>
/// </summary>
public enum WorkflowApprovalAction
{
    /// <summary>
    /// </summary>
    Approve,
    
    /// <summary>
    /// </summary>
    Reject,
    
    /// <summary>
    /// </summary>
    RequestChanges,
    
    /// <summary>
    /// </summary>
    Escalate
}

/// <summary>
/// </summary>
public class CreateWorkflowRequest
{
    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Guid TemplateId { get; set; }
    
    /// <summary>
    /// </summary>
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Data { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public Guid? ProjectId { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? DueDate { get; set; }
    
    /// <summary>
    /// </summary>
    public List<Guid> ApproverUserIds { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public Guid InitiatorId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid? AssigneeId { get; set; }
}

/// <summary>
/// </summary>
public class ApproveWorkflowRequest
{
    /// <summary>
    /// </summary>
    public Guid WorkflowId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid WorkflowInstanceId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid ApproverId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Comments { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object>? AdditionalData { get; set; }
}

/// <summary>
/// </summary>
public class RejectWorkflowRequest
{
    /// <summary>
    /// </summary>
    public Guid WorkflowId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid WorkflowInstanceId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid ReviewerId { get; set; }
    
    /// <summary>
    /// </summary>
    public string Reason { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string? Comments { get; set; }
}

/// <summary>
/// </summary>
public class CreateWorkflowTemplateRequest
{
    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Category { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public WorkflowType WorkflowType { get; set; }
    
    /// <summary>
    /// </summary>
    public List<WorkflowStepTemplateRequest> Steps { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> DefaultData { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public int SlaHours { get; set; } = 24;
}

/// <summary>
/// </summary>
public class WorkflowStepTemplateRequest
{
    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Type { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public int Order { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public bool RequiresApproval { get; set; }
}
