using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

/// <summary>
/// Represents a workflow instance (active workflow)
/// </summary>
public class WorkflowInstance : TenantEntity
{
    /// <summary>
    /// Instance name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Instance description
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Workflow status
    /// </summary>
    public WorkflowStatus Status { get; set; } = WorkflowStatus.Pending;

    /// <summary>
    /// Current step index
    /// </summary>
    public int CurrentStepIndex { get; set; } = 0;

    /// <summary>
    /// Workflow data as JSON
    /// </summary>
    public string? WorkflowData { get; set; }

    /// <summary>
    /// Workflow data (alias for WorkflowData for compatibility)
    /// </summary>
    public string? Data 
    { 
        get => WorkflowData; 
        set => WorkflowData = value; 
    }

    /// <summary>
    /// Approval history as JSON array
    /// </summary>
    public string? ApprovalHistory { get; set; }

    /// <summary>
    /// Workflow started at
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// Workflow completed at
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Due date for completion
    /// </summary>
    public DateTime? DueDate { get; set; }

    /// <summary>
    /// Priority level
    /// </summary>
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;

    /// <summary>
    /// Workflow template used
    /// </summary>
    public Guid WorkflowTemplateId { get; set; }
    /// <summary>
    /// </summary>
    public virtual WorkflowTemplate WorkflowTemplate { get; set; } = null!;

    /// <summary>
    /// Workflow type (from template for compatibility)
    /// </summary>
    public string? WorkflowType => WorkflowTemplate?.Name;

    /// <summary>
    /// Project this workflow belongs to
    /// </summary>
    public Guid? ProjectId { get; set; }
    /// <summary>
    /// The project this workflow instance belongs to
    /// </summary>
    public virtual Project? Project { get; set; }

    /// <summary>
    /// Sprint this workflow belongs to
    /// </summary>
    public Guid? SprintId { get; set; }
    /// <summary>
    /// The sprint this workflow instance belongs to
    /// </summary>
    public virtual Sprint? Sprint { get; set; }

    /// <summary>
    /// User story this workflow is related to
    /// </summary>
    public Guid? UserStoryId { get; set; }
    /// <summary>
    /// The user story this workflow instance is related to
    /// </summary>
    public virtual UserStory? UserStory { get; set; }

    /// <summary>
    /// Current assignee
    /// </summary>
    public Guid? CurrentAssigneeId { get; set; }
    public virtual User? CurrentAssignee { get; set; }

    /// <summary>
    /// Workflow initiator
    /// </summary>
    public Guid InitiatorId { get; set; }
    public virtual User Initiator { get; set; } = null!;

    /// <summary>
    /// </summary>
    public virtual User? CreatedBy => Initiator;

    /// <summary>
    /// </summary>
    public virtual User? StartedBy => Initiator;

    /// <summary>
    /// AI tasks related to this workflow
    /// </summary>
    public virtual ICollection<AITask> AITasks { get; set; } = new List<AITask>();

    /// <summary>
    /// </summary>
    public string? ExecutionContext { get; set; }

    /// <summary>
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// </summary>
    public string? ErrorDetails { get; set; }

    /// <summary>
    /// </summary>
    public string? PerformanceMetrics { get; set; }
}
