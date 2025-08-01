using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class WorkflowHistoryEntry
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid WorkflowInstanceId { get; set; }
    
    /// <summary>
    /// </summary>
    public string Action { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public WorkflowStatus PreviousStatus { get; set; }
    
    /// <summary>
    /// </summary>
    public WorkflowStatus NewStatus { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid PerformedByUserId { get; set; }
    
    /// <summary>
    /// </summary>
    public string PerformedByUserName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime PerformedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Comments { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}
