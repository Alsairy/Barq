using BARQ.Core.Enums;
using BARQ.Core.Interfaces;

namespace BARQ.Core.Models.Requests;

/// <summary>
/// </summary>
public class AITaskRequest
{
    /// <summary>
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the detailed description of the AI task.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of AI task to be performed.
    /// </summary>
    public AITaskType TaskType { get; set; }

    /// <summary>
    /// Gets or sets the priority level of the AI task.
    /// </summary>
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;

    /// <summary>
    /// </summary>
    public string InputData { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public Dictionary<string, object> Parameters { get; set; } = new();

    /// <summary>
    /// Gets or sets the unique identifier of the project this task belongs to, if applicable.
    /// </summary>
    public Guid? ProjectId { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? ScheduledAt { get; set; }

    /// <summary>
    /// </summary>
    public AITaskRequirements? Requirements { get; set; }
}

/// <summary>
/// </summary>
public class CreateAITaskRequest : AITaskRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the user to whom the task is assigned.
    /// </summary>
    public Guid AssignedToUserId { get; set; }
}

/// <summary>
/// Represents a request to update an existing AI task with new information.
/// </summary>
public class UpdateAITaskRequest : AITaskRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the AI task to update.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// </summary>
    public AITaskStatus? Status { get; set; }

    /// <summary>
    /// </summary>
    public string? OutputData { get; set; }

    /// <summary>
    /// </summary>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// </summary>
public class ConfigureAIProviderRequest
{
    /// <summary>
    /// Gets or sets the unique identifier of the AI provider.
    /// </summary>
    public Guid ProviderId { get; set; }

    /// <summary>
    /// Gets or sets the name of the AI provider.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the configuration settings for the AI provider.
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new();

    /// <summary>
    /// Gets or sets a value indicating whether the AI provider is enabled.
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// Gets or sets the list of capabilities supported by the AI provider.
    /// </summary>
    public List<string> Capabilities { get; set; } = new();
}

/// <summary>
/// </summary>
public class ExecuteBatchAITasksRequest
{
    /// <summary>
    /// </summary>
    public List<CreateAITaskRequest> Tasks { get; set; } = new();

    /// <summary>
    /// </summary>
    public string BatchName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the priority level for the entire batch operation.
    /// </summary>
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;

    /// <summary>
    /// </summary>
    public DateTime? ScheduledAt { get; set; }
}
