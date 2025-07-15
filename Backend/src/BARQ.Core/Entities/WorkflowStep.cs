using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

/// <summary>
/// </summary>
public class WorkflowStep : TenantEntity
{
    /// <summary>
    /// Step name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// </summary>
    public WorkflowStepType StepType { get; set; }

    /// <summary>
    /// </summary>
    public int Order { get; set; }

    /// <summary>
    /// </summary>
    public string Configuration { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? InputSchema { get; set; }

    /// <summary>
    /// </summary>
    public string? OutputSchema { get; set; }

    /// <summary>
    /// </summary>
    public string? ValidationRules { get; set; }

    /// <summary>
    /// </summary>
    public int? TimeoutMinutes { get; set; }

    /// <summary>
    /// </summary>
    public string? RetryConfiguration { get; set; }

    /// <summary>
    /// </summary>
    public string? ErrorHandling { get; set; }

    /// <summary>
    /// </summary>
    public string? ExecutionConditions { get; set; }

    /// <summary>
    /// </summary>
    public bool RequiresApproval { get; set; } = false;

    /// <summary>
    /// </summary>
    public bool AllowParallelExecution { get; set; } = false;

    /// <summary>
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// </summary>
    public Guid WorkflowTemplateId { get; set; }
    public virtual WorkflowTemplate WorkflowTemplate { get; set; } = null!;

    /// <summary>
    /// </summary>
    public Guid? ParentStepId { get; set; }
    public virtual WorkflowStep? ParentStep { get; set; }

    /// <summary>
    /// </summary>
    public virtual ICollection<WorkflowStep> ChildSteps { get; set; } = new List<WorkflowStep>();

    /// <summary>
    /// </summary>
    public virtual ICollection<WorkflowStepExecution> StepExecutions { get; set; } = new List<WorkflowStepExecution>();
}
