using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

/// <summary>
/// </summary>
public class WorkflowStepExecution : TenantEntity
{
    /// <summary>
    /// </summary>
    public WorkflowStepStatus Status { get; set; } = WorkflowStepStatus.Pending;

    /// <summary>
    /// </summary>
    public string? InputData { get; set; }

    /// <summary>
    /// </summary>
    public string? OutputData { get; set; }

    /// <summary>
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// </summary>
    public string? ErrorDetails { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// </summary>
    public long? DurationMs { get; set; }

    /// <summary>
    /// </summary>
    public int RetryCount { get; set; } = 0;

    /// <summary>
    /// </summary>
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// </summary>
    public DateTime? NextRetryAt { get; set; }

    /// <summary>
    /// </summary>
    public string? ExecutionContext { get; set; }

    /// <summary>
    /// </summary>
    public string? ExecutionLogs { get; set; }

    /// <summary>
    /// </summary>
    public string? PerformanceMetrics { get; set; }

    /// <summary>
    /// </summary>
    public Guid WorkflowInstanceId { get; set; }
    /// <summary>
    /// </summary>
    public virtual WorkflowInstance WorkflowInstance { get; set; } = null!;

    /// <summary>
    /// </summary>
    public Guid WorkflowStepId { get; set; }
    /// <summary>
    /// </summary>
    public virtual WorkflowStep WorkflowStep { get; set; } = null!;

    /// <summary>
    /// </summary>
    public Guid? ExecutedById { get; set; }
    /// <summary>
    /// </summary>
    public virtual User? ExecutedBy { get; set; }

    /// <summary>
    /// </summary>
    public Guid? AssignedToId { get; set; }
    /// <summary>
    /// </summary>
    public virtual User? AssignedTo { get; set; }
}
