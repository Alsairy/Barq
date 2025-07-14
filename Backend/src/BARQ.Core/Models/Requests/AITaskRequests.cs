using BARQ.Core.Enums;
using BARQ.Core.Interfaces;

namespace BARQ.Core.Models.Requests;

public class AITaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public AITaskType TaskType { get; set; }
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;
    public string InputData { get; set; } = string.Empty;
    public Dictionary<string, object> Parameters { get; set; } = new();
    public Guid? ProjectId { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public AITaskRequirements? Requirements { get; set; }
}

public class CreateAITaskRequest : AITaskRequest
{
    public Guid AssignedToUserId { get; set; }
}

public class UpdateAITaskRequest : AITaskRequest
{
    public Guid Id { get; set; }
    public AITaskStatus? Status { get; set; }
    public string? OutputData { get; set; }
    public string? ErrorMessage { get; set; }
}

public class ConfigureAIProviderRequest
{
    public Guid ProviderId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public Dictionary<string, object> Configuration { get; set; } = new();
    public bool IsEnabled { get; set; } = true;
    public List<string> Capabilities { get; set; } = new();
}

public class ExecuteBatchAITasksRequest
{
    public List<CreateAITaskRequest> Tasks { get; set; } = new();
    public string BatchName { get; set; } = string.Empty;
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;
    public DateTime? ScheduledAt { get; set; }
}
