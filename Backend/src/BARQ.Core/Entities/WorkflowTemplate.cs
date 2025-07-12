using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

/// <summary>
/// Represents a workflow template for approval processes
/// </summary>
public class WorkflowTemplate : TenantEntity
{
    /// <summary>
    /// Template name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Template description
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Workflow type
    /// </summary>
    public WorkflowType WorkflowType { get; set; }

    /// <summary>
    /// Template version
    /// </summary>
    [MaxLength(20)]
    public string Version { get; set; } = "1.0";

    /// <summary>
    /// Workflow definition as JSON
    /// </summary>
    public string WorkflowDefinition { get; set; } = string.Empty;

    /// <summary>
    /// Approval steps as JSON array
    /// </summary>
    public string ApprovalSteps { get; set; } = string.Empty;

    /// <summary>
    /// SLA configuration as JSON
    /// </summary>
    public string? SLAConfiguration { get; set; }

    /// <summary>
    /// Escalation rules as JSON
    /// </summary>
    public string? EscalationRules { get; set; }

    /// <summary>
    /// Notification settings as JSON
    /// </summary>
    public string? NotificationSettings { get; set; }

    /// <summary>
    /// Template is active
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Template is default for this workflow type
    /// </summary>
    public bool IsDefault { get; set; } = false;

    /// <summary>
    /// Organization this template belongs to
    /// </summary>
    public virtual Organization Organization { get; set; } = null!;

    /// <summary>
    /// Workflow instances created from this template
    /// </summary>
    public virtual ICollection<WorkflowInstance> WorkflowInstances { get; set; } = new List<WorkflowInstance>();
}

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
    public Priority Priority { get; set; } = Priority.Medium;

    /// <summary>
    /// Workflow template used
    /// </summary>
    public Guid WorkflowTemplateId { get; set; }
    public virtual WorkflowTemplate WorkflowTemplate { get; set; } = null!;

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
    /// AI tasks related to this workflow
    /// </summary>
    public virtual ICollection<AITask> AITasks { get; set; } = new List<AITask>();
}

