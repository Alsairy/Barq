using BARQ.Core.Enums;
using BARQ.Core.Interfaces;

namespace BARQ.Core.Models.Requests;

/// <summary>
/// Request model for creating and managing AI tasks
/// </summary>
public class AITaskRequest
{
    /// <summary>
    /// Gets or sets the title of the AI task
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description of the AI task (important-comment)
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the type of AI task to be performed
    /// </summary>
    public AITaskType TaskType { get; set; }
    
    /// <summary>
    /// Gets or sets the priority level of the task
    /// </summary>
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;
    
    /// <summary>
    /// Gets or sets the input data for the AI task
    /// </summary>
    public string InputData { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets additional parameters for task execution
    /// </summary>
    public Dictionary<string, object> Parameters { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the optional project identifier this task belongs to
    /// </summary>
    public Guid? ProjectId { get; set; }
    
    /// <summary>
    /// Gets or sets the optional scheduled execution time for the task
    /// </summary>
    public DateTime? ScheduledAt { get; set; }
    
    /// <summary>
    /// Gets or sets the optional requirements for the AI task
    /// </summary>
    public AITaskRequirements? Requirements { get; set; }
}

/// <summary>
/// Request model for creating a new AI task with assignment
/// </summary>
public class CreateAITaskRequest : AITaskRequest
{
    /// <summary>
    /// Gets or sets the identifier of the user assigned to this task (important-comment)
    /// </summary>
    public Guid AssignedToUserId { get; set; }
}

/// <summary>
/// Request model for updating an existing AI task
/// </summary>
public class UpdateAITaskRequest : AITaskRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the task to update (important-comment)
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the optional status update for the task
    /// </summary>
    public AITaskStatus? Status { get; set; }
    
    /// <summary>
    /// Gets or sets the optional output data from task execution
    /// </summary>
    public string? OutputData { get; set; }
    
    /// <summary>
    /// Gets or sets the optional error message if task execution failed
    /// </summary>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// Request model for configuring AI provider settings
/// </summary>
public class ConfigureAIProviderRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the AI provider (important-comment)
    /// </summary>
    public Guid ProviderId { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the AI provider
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the type of AI provider
    /// </summary>
    public string Type { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the configuration settings for the provider
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new();
    
    /// <summary>
    /// Gets or sets a value indicating whether the provider is enabled (important-comment)
    /// </summary>
    public bool IsEnabled { get; set; } = true;
    
    /// <summary>
    /// Gets or sets the list of capabilities supported by the provider
    /// </summary>
    public List<string> Capabilities { get; set; } = new();
}

/// <summary>
/// Request model for executing multiple AI tasks in a batch
/// </summary>
public class ExecuteBatchAITasksRequest
{
    /// <summary>
    /// Gets or sets the list of AI tasks to execute in the batch
    /// </summary>
    public List<CreateAITaskRequest> Tasks { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the name of the batch execution
    /// </summary>
    public string BatchName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the priority level for the entire batch
    /// </summary>
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;
    
    /// <summary>
    /// Gets or sets the optional scheduled execution time for the batch
    /// </summary>
    public DateTime? ScheduledAt { get; set; }
}
