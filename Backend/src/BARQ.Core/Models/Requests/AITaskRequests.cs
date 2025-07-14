using BARQ.Core.Enums;

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

public class AITaskRequirements
{
    public int MaxTokens { get; set; } = 1000;
    public double Temperature { get; set; } = 0.7;
    public string Model { get; set; } = string.Empty;
    public List<string> RequiredCapabilities { get; set; } = new();
    public Dictionary<string, object> AdditionalSettings { get; set; } = new();
}
