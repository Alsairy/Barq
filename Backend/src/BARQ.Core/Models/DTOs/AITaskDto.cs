namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class AITaskDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the AI task.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the AI task or provider.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the detailed description of the AI task and its purpose.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of AI task (e.g., TextGeneration, ImageAnalysis, DataProcessing).
    /// </summary>
    public string TaskType { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current status (e.g., Pending, Running, Completed, Failed).
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the priority level of the AI task (e.g., Low, Medium, High, Critical).
    /// </summary>
    public string Priority { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the parameters and configuration settings for the AI task execution.
    /// </summary>
    public Dictionary<string, object> Parameters { get; set; } = new();

    /// <summary>
    /// Gets or sets the result or output of the completed AI task.
    /// </summary>
    public string? Result { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the AI task was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the AI task execution started.
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the AI task was completed.
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who created the AI task.
    /// </summary>
    public Guid CreatedBy { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user assigned to execute the AI task.
    /// </summary>
    public Guid? AssignedTo { get; set; }

    /// <summary>
    /// Gets or sets the cost incurred for executing the AI task.
    /// </summary>
    public decimal? Cost { get; set; }

    /// <summary>
    /// Gets or sets the total time taken to execute the AI task.
    /// </summary>
    public TimeSpan? ExecutionTime { get; set; }
}


/// <summary>
/// </summary>
public class AIProviderDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the AI task.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the AI task or provider.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of AI provider (e.g., OpenAI, Azure, AWS).
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current status (e.g., Pending, Running, Completed, Failed).
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets whether the AI provider is enabled for use.
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// Gets or sets the configuration settings and parameters for the AI provider.
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new();

    /// <summary>
    /// Gets or sets the list of capabilities supported by the AI provider.
    /// </summary>
    public List<string> Capabilities { get; set; } = new();

    /// <summary>
    /// Gets or sets the cost per request for using this AI provider.
    /// </summary>
    public decimal CostPerRequest { get; set; }

    /// <summary>
    /// Gets or sets the average response time for requests to this AI provider.
    /// </summary>
    public TimeSpan AverageResponseTime { get; set; }

    /// <summary>
    /// Gets or sets the date and time of the last health check for this AI provider.
    /// </summary>
    public DateTime LastHealthCheck { get; set; }
}
