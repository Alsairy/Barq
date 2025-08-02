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
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public AITaskType TaskType { get; set; }
    
    /// <summary>
    /// </summary>
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;
    
    /// <summary>
    /// </summary>
    public string InputData { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Parameters { get; set; } = new();
    
    /// <summary>
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
    /// </summary>
    public Guid AssignedToUserId { get; set; }
}

/// <summary>
/// </summary>
public class UpdateAITaskRequest : AITaskRequest
{
    /// <summary>
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
    /// </summary>
    public Guid ProviderId { get; set; }
    
    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Type { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public bool IsEnabled { get; set; } = true;
    
    /// <summary>
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
    /// </summary>
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;
    
    /// <summary>
    /// </summary>
    public DateTime? ScheduledAt { get; set; }
}
