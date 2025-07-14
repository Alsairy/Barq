using BARQ.Core.Enums;

namespace BARQ.Core.Models.Responses;

public class WorkflowExecutionResult
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; } = string.Empty;
    public WorkflowStepStatus Status { get; set; }
    public Dictionary<string, object>? OutputData { get; set; }
    public string? ErrorDetails { get; set; }
    public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
    public TimeSpan ExecutionDuration { get; set; }
    public Guid? NextStepId { get; set; }
    public bool RequiresApproval { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
}
