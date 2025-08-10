using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Data transfer object representing workflow history entry information
/// </summary>
public class WorkflowHistoryEntry
{
    /// <summary>
    /// Gets or sets the unique identifier for the history entry
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the workflow instance identifier
    /// </summary>
    public Guid WorkflowInstanceId { get; set; }
    
    /// <summary>
    /// Gets or sets the action performed
    /// </summary>
    public string Action { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description of the action
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the previous status before the action
    /// </summary>
    public WorkflowStatus PreviousStatus { get; set; }
    
    /// <summary>
    /// Gets or sets the new status after the action
    /// </summary>
    public WorkflowStatus NewStatus { get; set; }
    
    /// <summary>
    /// Gets or sets the identifier of the user who performed the action
    /// </summary>
    public Guid PerformedByUserId { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the user who performed the action
    /// </summary>
    public string PerformedByUserName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the date and time when the action was performed
    /// </summary>
    public DateTime PerformedAt { get; set; }
    
    /// <summary>
    /// Gets or sets optional comments about the action
    /// </summary>
    public string? Comments { get; set; }
    
    /// <summary>
    /// Gets or sets additional metadata associated with the action
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}
