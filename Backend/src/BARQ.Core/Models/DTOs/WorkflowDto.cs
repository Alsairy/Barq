namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Data transfer object representing workflow information
/// </summary>
public class WorkflowDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the workflow (important-comment)
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the workflow name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the workflow description
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the workflow status
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the workflow priority
    /// </summary>
    public string Priority { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the template identifier
    /// </summary>
    public Guid TemplateId { get; set; }
    
    /// <summary>
    /// Gets or sets the template name
    /// </summary>
    public string TemplateName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the workflow data
    /// </summary>
    public Dictionary<string, object> Data { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the creation date and time
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the workflow was started
    /// </summary>
    public DateTime? StartedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the workflow was completed
    /// </summary>
    public DateTime? CompletedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the due date for the workflow
    /// </summary>
    public DateTime? DueDate { get; set; }
    
    /// <summary>
    /// Gets or sets the identifier of the user who created the workflow
    /// </summary>
    public Guid CreatedBy { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the user who created the workflow
    /// </summary>
    public string CreatedByName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the identifier of the user assigned to the workflow
    /// </summary>
    public Guid? AssignedTo { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the user assigned to the workflow
    /// </summary>
    public string? AssignedToName { get; set; }
    
    /// <summary>
    /// Gets or sets the current step of the workflow
    /// </summary>
    public string? CurrentStep { get; set; }
    
    /// <summary>
    /// Gets or sets the progress percentage of the workflow
    /// </summary>
    public int Progress { get; set; }
}

/// <summary>
/// Data transfer object representing workflow approval information
/// </summary>
public class WorkflowApprovalDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the approval
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the workflow identifier
    /// </summary>
    public Guid WorkflowId { get; set; }
    
    /// <summary>
    /// Gets or sets the workflow name
    /// </summary>
    public string WorkflowName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the approval status
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the identifier of the user who requested approval
    /// </summary>
    public Guid RequestedBy { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the user who requested approval
    /// </summary>
    public string RequestedByName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the date and time when approval was requested
    /// </summary>
    public DateTime RequestedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the identifier of the user who approved the request
    /// </summary>
    public Guid? ApprovedBy { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the user who approved the request
    /// </summary>
    public string? ApprovedByName { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the request was approved
    /// </summary>
    public DateTime? ApprovedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the approval comments
    /// </summary>
    public string? Comments { get; set; }
    
    /// <summary>
    /// Gets or sets the reason for the approval decision
    /// </summary>
    public string? Reason { get; set; }
}

/// <summary>
/// Data transfer object representing workflow template information
/// </summary>
public class WorkflowTemplateDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the template
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the template name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the template description
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the template category
    /// </summary>
    public string Category { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets a value indicating whether the template is active
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Gets or sets the list of workflow step templates
    /// </summary>
    public List<WorkflowStepTemplateDto> Steps { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the default data for the template
    /// </summary>
    public Dictionary<string, object> DefaultData { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the service level agreement hours
    /// </summary>
    public int SlaHours { get; set; }
    
    /// <summary>
    /// Gets or sets the creation date and time
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the identifier of the user who created the template
    /// </summary>
    public Guid CreatedBy { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the user who created the template
    /// </summary>
    public string CreatedByName { get; set; } = string.Empty;
}

/// <summary>
/// Data transfer object representing workflow step template information
/// </summary>
public class WorkflowStepTemplateDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the step template
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the step template name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the step template type
    /// </summary>
    public string Type { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the execution order of the step
    /// </summary>
    public int Order { get; set; }
    
    /// <summary>
    /// Gets or sets the configuration data for the step
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new();
    
    /// <summary>
    /// Gets or sets a value indicating whether the step requires approval
    /// </summary>
    public bool RequiresApproval { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the step template is active
    /// </summary>
    public bool IsActive { get; set; }
}
