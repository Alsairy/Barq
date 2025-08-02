using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class WorkflowResponse
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
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? StartedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? CompletedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid CreatedBy { get; set; }
}

/// <summary>
/// </summary>
public class WorkflowExecutionResponse
{
    /// <summary>
    /// </summary>
    public Guid WorkflowId { get; set; }
    
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string? ExecutionId { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime StartedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Message { get; set; }
}

/// <summary>
/// </summary>
public class WorkflowApprovalResponse
{
    /// <summary>
    /// </summary>
    public Guid WorkflowId { get; set; }
    
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime ApprovedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid ApprovedBy { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Comments { get; set; }
}

/// <summary>
/// </summary>
public class WorkflowRejectionResponse
{
    /// <summary>
    /// </summary>
    public Guid WorkflowId { get; set; }
    
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime RejectedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid RejectedBy { get; set; }
    
    /// <summary>
    /// </summary>
    public string Reason { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class WorkflowCancellationResponse
{
    /// <summary>
    /// </summary>
    public Guid WorkflowId { get; set; }
    
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime CancelledAt { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid CancelledBy { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Reason { get; set; }
}

/// <summary>
/// </summary>
public class WorkflowStatusResponse
{
    /// <summary>
    /// </summary>
    public Guid WorkflowId { get; set; }
    
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string? CurrentStep { get; set; }
    
    /// <summary>
    /// </summary>
    public int Progress { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime LastUpdated { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Message { get; set; }
}

/// <summary>
/// </summary>
public class WorkflowHistoryResponse
{
    /// <summary>
    /// </summary>
    public Guid WorkflowId { get; set; }
    
    /// <summary>
    /// </summary>
    public List<WorkflowHistoryEntry> History { get; set; } = new();
}

/// <summary>
/// </summary>
public class WorkflowAnalyticsResponse
{
    /// <summary>
    /// </summary>
    public int TotalWorkflows { get; set; }
    
    /// <summary>
    /// </summary>
    public int CompletedWorkflows { get; set; }
    
    /// <summary>
    /// </summary>
    public int PendingWorkflows { get; set; }
    
    /// <summary>
    /// </summary>
    public int RejectedWorkflows { get; set; }
    
    /// <summary>
    /// </summary>
    public TimeSpan AverageCompletionTime { get; set; }
    
    /// <summary>
    /// </summary>
    public int SlaBreaches { get; set; }
}

/// <summary>
/// </summary>
public class WorkflowTemplateResponse
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
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// </summary>
public class WorkflowSlaBreachResponse
{
    /// <summary>
    /// </summary>
    public int TotalBreaches { get; set; }
    
    /// <summary>
    /// </summary>
    public int BreachesThisMonth { get; set; }
    
    /// <summary>
    /// </summary>
    public List<WorkflowSlaBreachInfo> RecentBreaches { get; set; } = new();
}

/// <summary>
/// </summary>
public class WorkflowSlaBreachInfo
{
    /// <summary>
    /// </summary>
    public Guid WorkflowId { get; set; }
    
    /// <summary>
    /// </summary>
    public string WorkflowName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime DueDate { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? CompletedDate { get; set; }
    
    /// <summary>
    /// </summary>
    public TimeSpan Delay { get; set; }
}

/// <summary>
/// </summary>
public class WorkflowPerformanceResponse
{
    /// <summary>
    /// </summary>
    public TimeSpan AverageCompletionTime { get; set; }
    
    /// <summary>
    /// </summary>
    public TimeSpan MedianCompletionTime { get; set; }
    
    /// <summary>
    /// </summary>
    public int CompletionRate { get; set; }
    
    /// <summary>
    /// </summary>
    public int SlaComplianceRate { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, TimeSpan> CompletionTimeByType { get; set; } = new();
}
