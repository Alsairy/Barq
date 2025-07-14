using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

public class WorkflowHistoryEntry
{
    public Guid Id { get; set; }
    public Guid WorkflowInstanceId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public WorkflowStatus PreviousStatus { get; set; }
    public WorkflowStatus NewStatus { get; set; }
    public Guid PerformedByUserId { get; set; }
    public string PerformedByUserName { get; set; } = string.Empty;
    public DateTime PerformedAt { get; set; }
    public string? Comments { get; set; }
    public Dictionary<string, object> Metadata { get; set; } = new();
}
