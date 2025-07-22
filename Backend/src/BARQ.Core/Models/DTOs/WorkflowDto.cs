namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class WorkflowDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the workflow instance.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the workflow instance.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current status of the workflow (e.g., Pending, Running, Completed, Failed).
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the priority level of the workflow (e.g., Low, Medium, High, Critical).
    /// </summary>
    public string Priority { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the workflow template this instance is based on.
    /// </summary>
    public Guid TemplateId { get; set; }

    /// <summary>
    /// Gets or sets the name of the workflow template this instance is based on.
    /// </summary>
    public string TemplateName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the workflow data and variables used during execution.
    /// </summary>
    public Dictionary<string, object> Data { get; set; } = new();

    /// <summary>
    /// Gets or sets the date and time when the workflow was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the workflow execution started.
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the workflow was completed.
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Gets or sets the due date for the workflow completion, if applicable.
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who created the workflow.
    /// </summary>
    public Guid CreatedBy { get; set; }

    /// <summary>
    /// </summary>
    public string CreatedByName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the user assigned to execute the workflow.
    /// </summary>
    public Guid? AssignedTo { get; set; }

    /// <summary>
    /// Gets or sets the name of the user assigned to execute the workflow, if any.
    /// </summary>
    public string? AssignedToName { get; set; }

    /// <summary>
    /// Gets or sets the name of the current step being executed in the workflow.
    /// </summary>
    public string? CurrentStep { get; set; }

    /// <summary>
    /// Gets or sets the completion progress of the workflow as a percentage (0-100).
    /// </summary>
    public int Progress { get; set; }
}

/// <summary>
/// </summary>
public class WorkflowApprovalDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the approval request.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the workflow requiring approval.
    /// </summary>
    public Guid WorkflowId { get; set; }

    /// <summary>
    /// Gets or sets the name of the workflow requiring approval.
    /// </summary>
    public string WorkflowName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current status of the approval request (e.g., Pending, Approved, Rejected).
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the user who requested the approval.
    /// </summary>
    public Guid RequestedBy { get; set; }

    /// <summary>
    /// Gets or sets the name of the user who requested the approval.
    /// </summary>
    public string RequestedByName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the approval was requested.
    /// </summary>
    public DateTime RequestedAt { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who approved or rejected the request.
    /// </summary>
    public Guid? ApprovedBy { get; set; }

    /// <summary>
    /// Gets or sets the name of the user who approved or rejected the request, if applicable.
    /// </summary>
    public string? ApprovedByName { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the approval decision was made.
    /// </summary>
    public DateTime? ApprovedAt { get; set; }

    /// <summary>
    /// Gets or sets additional comments or notes about the approval request.
    /// </summary>
    public string? Comments { get; set; }

    /// <summary>
    /// Gets or sets the reason for the approval or rejection decision.
    /// </summary>
    public string? Reason { get; set; }
}

/// <summary>
/// </summary>
public class WorkflowTemplateDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the workflow template.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the workflow template.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the workflow template is active and available for use.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the collection of step templates that define the workflow structure.
    /// </summary>
    public List<WorkflowStepTemplateDto> Steps { get; set; } = new();

    /// <summary>
    /// </summary>
    public Dictionary<string, object> DefaultData { get; set; } = new();

    /// <summary>
    /// </summary>
    public int SlaHours { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the workflow template was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who created the workflow template.
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
    /// Gets or sets the unique identifier of the workflow step template.
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
    /// Gets or sets the configuration settings and parameters for the workflow step.
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether this step requires manual approval before proceeding.
    /// </summary>
    public bool RequiresApproval { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the workflow step is active and will be executed.
    /// </summary>
    public bool IsActive { get; set; }
}
