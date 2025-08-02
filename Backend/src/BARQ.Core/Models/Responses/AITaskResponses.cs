using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class AITaskResponse
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
    public DateTime? CompletedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Result { get; set; }
}

/// <summary>
/// </summary>
public class AITaskExecutionResponse
{
    /// <summary>
    /// </summary>
    public Guid TaskId { get; set; }
    
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
public class AITaskStatusResponse
{
    /// <summary>
    /// </summary>
    public Guid TaskId { get; set; }
    
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public int Progress { get; set; }
    
    /// <summary>
    /// </summary>
    public string? CurrentStep { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime LastUpdated { get; set; }
    
    /// <summary>
    /// </summary>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// </summary>
public class AITaskResultResponse
{
    /// <summary>
    /// </summary>
    public Guid TaskId { get; set; }
    
    /// <summary>
    /// </summary>
    public string Result { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime CompletedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public TimeSpan ExecutionTime { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal Cost { get; set; }
}

/// <summary>
/// </summary>
public class AITaskAnalyticsResponse
{
    /// <summary>
    /// </summary>
    public int TotalTasks { get; set; }
    
    /// <summary>
    /// </summary>
    public int CompletedTasks { get; set; }
    
    /// <summary>
    /// </summary>
    public int FailedTasks { get; set; }
    
    /// <summary>
    /// </summary>
    public int PendingTasks { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal TotalCost { get; set; }
    
    /// <summary>
    /// </summary>
    public TimeSpan AverageExecutionTime { get; set; }
}

/// <summary>
/// </summary>
public class AIProviderHealthResponse
{
    /// <summary>
    /// </summary>
    public Guid ProviderId { get; set; }
    
    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public TimeSpan ResponseTime { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime LastChecked { get; set; }
    
    /// <summary>
    /// </summary>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// </summary>
public class AIProviderConfigurationResponse
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public DateTime ConfiguredAt { get; set; }
}

/// <summary>
/// </summary>
public class AIBatchExecutionResponse
{
    /// <summary>
    /// </summary>
    public string BatchId { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public int TotalTasks { get; set; }
    
    /// <summary>
    /// </summary>
    public int QueuedTasks { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime SubmittedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class AITaskQueueStatusResponse
{
    /// <summary>
    /// </summary>
    public int QueuedTasks { get; set; }
    
    /// <summary>
    /// </summary>
    public int ProcessingTasks { get; set; }
    
    /// <summary>
    /// </summary>
    public int CompletedTasks { get; set; }
    
    /// <summary>
    /// </summary>
    public int FailedTasks { get; set; }
    
    /// <summary>
    /// </summary>
    public TimeSpan AverageWaitTime { get; set; }
}

/// <summary>
/// </summary>
public class AICostAnalysisResponse
{
    /// <summary>
    /// </summary>
    public decimal TotalCost { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal CostThisMonth { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal CostLastMonth { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, decimal> CostByProvider { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public Dictionary<string, decimal> CostByTaskType { get; set; } = new();
}
