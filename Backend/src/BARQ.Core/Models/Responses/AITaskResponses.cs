using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

public class AITaskResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? Result { get; set; }
}

public class AITaskExecutionResponse
{
    public Guid TaskId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ExecutionId { get; set; }
    public DateTime StartedAt { get; set; }
    public string? Message { get; set; }
}

public class AITaskStatusResponse
{
    public Guid TaskId { get; set; }
    public string Status { get; set; } = string.Empty;
    public int Progress { get; set; }
    public string? CurrentStep { get; set; }
    public DateTime LastUpdated { get; set; }
    public string? ErrorMessage { get; set; }
}

public class AITaskResultResponse
{
    public Guid TaskId { get; set; }
    public string Result { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CompletedAt { get; set; }
    public TimeSpan ExecutionTime { get; set; }
    public decimal Cost { get; set; }
}

public class AITaskAnalyticsResponse
{
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int FailedTasks { get; set; }
    public int PendingTasks { get; set; }
    public decimal TotalCost { get; set; }
    public TimeSpan AverageExecutionTime { get; set; }
}

public class AIProviderHealthResponse
{
    public Guid ProviderId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public TimeSpan ResponseTime { get; set; }
    public DateTime LastChecked { get; set; }
    public string? ErrorMessage { get; set; }
}

public class AIProviderConfigurationResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public Dictionary<string, object> Configuration { get; set; } = new();
    public DateTime ConfiguredAt { get; set; }
}

public class AIBatchExecutionResponse
{
    public string BatchId { get; set; } = string.Empty;
    public int TotalTasks { get; set; }
    public int QueuedTasks { get; set; }
    public DateTime SubmittedAt { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class AITaskQueueStatusResponse
{
    public int QueuedTasks { get; set; }
    public int ProcessingTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int FailedTasks { get; set; }
    public TimeSpan AverageWaitTime { get; set; }
}

public class AICostAnalysisResponse
{
    public decimal TotalCost { get; set; }
    public decimal CostThisMonth { get; set; }
    public decimal CostLastMonth { get; set; }
    public Dictionary<string, decimal> CostByProvider { get; set; } = new();
    public Dictionary<string, decimal> CostByTaskType { get; set; } = new();
}
