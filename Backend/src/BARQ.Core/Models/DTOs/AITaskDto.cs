namespace BARQ.Core.Models.DTOs;

public class AITaskDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string TaskType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public Dictionary<string, object> Parameters { get; set; } = new();
    public string? Result { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public Guid? AssignedTo { get; set; }
    public decimal? Cost { get; set; }
    public TimeSpan? ExecutionTime { get; set; }
}

public class AIProviderDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool IsEnabled { get; set; }
    public Dictionary<string, object> Configuration { get; set; } = new();
    public List<string> Capabilities { get; set; } = new();
    public decimal CostPerRequest { get; set; }
    public TimeSpan AverageResponseTime { get; set; }
    public DateTime LastHealthCheck { get; set; }
}
