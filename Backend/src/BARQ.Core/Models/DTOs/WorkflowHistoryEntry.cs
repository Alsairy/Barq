using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class WorkflowHistoryEntry
{
    /// <summary>
    /// Gets or sets the unique identifier for the workflow history entry.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the workflow instance this history entry belongs to.
    /// </summary>
    public Guid WorkflowInstanceId { get; set; }

    /// <summary>
    /// Gets or sets the action that was performed on the workflow.
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the detailed description of the workflow action or change.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public WorkflowStatus PreviousStatus { get; set; }

    /// <summary>
    /// </summary>
    public WorkflowStatus NewStatus { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who performed the action.
    /// </summary>
    public Guid PerformedByUserId { get; set; }

    /// <summary>
    /// Gets or sets the name of the user who performed the action.
    /// </summary>
    public string PerformedByUserName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the action was performed.
    /// </summary>
    public DateTime PerformedAt { get; set; }

    /// <summary>
    /// </summary>
    public string? Comments { get; set; }

    /// <summary>
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}
