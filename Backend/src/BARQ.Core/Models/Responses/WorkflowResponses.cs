using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

public class WorkflowResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Guid CreatedBy { get; set; }
}

public class WorkflowExecutionResponse
{
    public Guid WorkflowId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? ExecutionId { get; set; }
    public DateTime StartedAt { get; set; }
    public string? Message { get; set; }
}

public class WorkflowApprovalResponse
{
    public Guid WorkflowId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime ApprovedAt { get; set; }
    public Guid ApprovedBy { get; set; }
    public string? Comments { get; set; }
}

public class WorkflowRejectionResponse
{
    public Guid WorkflowId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime RejectedAt { get; set; }
    public Guid RejectedBy { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class WorkflowCancellationResponse
{
    public Guid WorkflowId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CancelledAt { get; set; }
    public Guid CancelledBy { get; set; }
    public string? Reason { get; set; }
}

public class WorkflowStatusResponse
{
    public Guid WorkflowId { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? CurrentStep { get; set; }
    public int Progress { get; set; }
    public DateTime LastUpdated { get; set; }
    public string? Message { get; set; }
}

public class WorkflowHistoryResponse
{
    public Guid WorkflowId { get; set; }
    public List<WorkflowHistoryEntry> History { get; set; } = new();
}

public class WorkflowAnalyticsResponse
{
    public int TotalWorkflows { get; set; }
    public int CompletedWorkflows { get; set; }
    public int PendingWorkflows { get; set; }
    public int RejectedWorkflows { get; set; }
    public TimeSpan AverageCompletionTime { get; set; }
    public int SlaBreaches { get; set; }
}

public class WorkflowTemplateResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class WorkflowSlaBreachResponse
{
    public int TotalBreaches { get; set; }
    public int BreachesThisMonth { get; set; }
    public List<WorkflowSlaBreachInfo> RecentBreaches { get; set; } = new();
}

public class WorkflowSlaBreachInfo
{
    public Guid WorkflowId { get; set; }
    public string WorkflowName { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public TimeSpan Delay { get; set; }
}

public class WorkflowPerformanceResponse
{
    public TimeSpan AverageCompletionTime { get; set; }
    public TimeSpan MedianCompletionTime { get; set; }
    public int CompletionRate { get; set; }
    public int SlaComplianceRate { get; set; }
    public Dictionary<string, TimeSpan> CompletionTimeByType { get; set; } = new();
}
