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
    public virtual WorkflowTemplate WorkflowTemplate { get; set; } = null!;

    /// <summary>
    /// Workflow type (from template for compatibility)
    /// </summary>
    public string? WorkflowType => WorkflowTemplate?.Name;

    /// <summary>
    /// Project this workflow belongs to
    /// </summary>
    public Guid? ProjectId { get; set; }
    public virtual Project? Project { get; set; }

    /// <summary>
    /// Sprint this workflow belongs to
    /// </summary>
    public Guid? SprintId { get; set; }
    public virtual Sprint? Sprint { get; set; }

    /// <summary>
    /// User story this workflow is related to
    /// </summary>
    public Guid? UserStoryId { get; set; }
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
}
