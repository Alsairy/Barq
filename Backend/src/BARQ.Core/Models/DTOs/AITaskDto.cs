namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class AITaskDto
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
    public string TaskType { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string Priority { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public Dictionary<string, object> Parameters { get; set; } = new();

    /// <summary>
    /// </summary>
    public string? Result { get; set; }

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

    /// <summary>
    /// </summary>
    public Guid? AssignedTo { get; set; }

    /// <summary>
    /// </summary>
    public decimal? Cost { get; set; }

    /// <summary>
    /// </summary>
    public TimeSpan? ExecutionTime { get; set; }
}


/// <summary>
/// </summary>
public class AIProviderDto
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
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new();

    /// <summary>
    /// </summary>
    public List<string> Capabilities { get; set; } = new();

    /// <summary>
    /// </summary>
    public decimal CostPerRequest { get; set; }

    /// <summary>
    /// </summary>
    public TimeSpan AverageResponseTime { get; set; }

    /// <summary>
    /// </summary>
    public DateTime LastHealthCheck { get; set; }
}
