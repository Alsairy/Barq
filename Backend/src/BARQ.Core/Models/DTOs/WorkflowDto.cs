namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class WorkflowDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Priority { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Guid TemplateId { get; set; }
    
    /// <summary>
    /// </summary>
    public string TemplateName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Data { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? StartedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? CompletedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? DueDate { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid CreatedBy { get; set; }
    
    /// <summary>
    /// </summary>
    public string CreatedByName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Guid? AssignedTo { get; set; }
    
    /// <summary>
    /// </summary>
    public string? AssignedToName { get; set; }
    
    /// <summary>
    /// </summary>
    public string? CurrentStep { get; set; }
    
    /// <summary>
    /// </summary>
    public int Progress { get; set; }
}

/// <summary>
/// </summary>
public class WorkflowApprovalDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid WorkflowId { get; set; }
    
    /// <summary>
    /// </summary>
    public string WorkflowName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Guid RequestedBy { get; set; }
    
    /// <summary>
    /// </summary>
    public string RequestedByName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime RequestedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid? ApprovedBy { get; set; }
    
    /// <summary>
    /// </summary>
    public string? ApprovedByName { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? ApprovedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Comments { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Reason { get; set; }
}

/// <summary>
/// </summary>
public class WorkflowTemplateDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
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
    public bool IsActive { get; set; }
    
    /// <summary>
    /// </summary>
    public List<WorkflowStepTemplateDto> Steps { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> DefaultData { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public int SlaHours { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid CreatedBy { get; set; }
    
    /// <summary>
    /// </summary>
    public string CreatedByName { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class WorkflowStepTemplateDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
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
    
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; }
}
