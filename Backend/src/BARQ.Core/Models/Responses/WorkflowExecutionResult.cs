using BARQ.Core.Enums;

namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class WorkflowExecutionResult
{
    /// <summary>
    /// </summary>
    public bool IsSuccess { get; set; }
    
    /// <summary>
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public WorkflowStepStatus Status { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object>? OutputData { get; set; }
    
    /// <summary>
    /// </summary>
    public string? ErrorDetails { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// </summary>
    public TimeSpan ExecutionDuration { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid? NextStepId { get; set; }
    
    /// <summary>
    /// </summary>
    public bool RequiresApproval { get; set; }
    
    /// <summary>
    /// </summary>
    public List<string> ValidationErrors { get; set; } = new();
}
