using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

/// <summary>
/// Represents a user story in the BARQ platform
/// </summary>
public class UserStory : TenantEntity
{
    /// <summary>
    /// User story title
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// User story description
    /// </summary>
    [MaxLength(2000)]
    public string? Description { get; set; }

    /// <summary>
    /// User story as a user statement
    /// </summary>
    [MaxLength(1000)]
    public string? AsAUser { get; set; }

    /// <summary>
    /// User story want statement
    /// </summary>
    [MaxLength(1000)]
    public string? IWant { get; set; }

    /// <summary>
    /// User story so that statement
    /// </summary>
    [MaxLength(1000)]
    public string? SoThat { get; set; }

    /// <summary>
    /// Acceptance criteria as JSON array
    /// </summary>
    public string? AcceptanceCriteria { get; set; }

    /// <summary>
    /// Business rules as JSON array
    /// </summary>
    public string? BusinessRules { get; set; }

    /// <summary>
    /// User story status
    /// </summary>
    public UserStoryStatus Status { get; set; } = UserStoryStatus.New;

    /// <summary>
    /// User story priority
    /// </summary>
    public Priority Priority { get; set; } = Priority.Medium;

    /// <summary>
    /// Story points estimation
    /// </summary>
    public int? StoryPoints { get; set; }

    /// <summary>
    /// Effort estimation in hours
    /// </summary>
    public decimal? EffortHours { get; set; }

    /// <summary>
    /// User story epic or theme
    /// </summary>
    [MaxLength(200)]
    public string? Epic { get; set; }

    /// <summary>
    /// User story labels/tags as JSON array
    /// </summary>
    public string? Labels { get; set; }

    /// <summary>
    /// AI generated flag
    /// </summary>
    public bool IsAIGenerated { get; set; } = false;

    /// <summary>
    /// AI generation metadata
    /// </summary>
    public string? AIGenerationMetadata { get; set; }

    /// <summary>
    /// Project this user story belongs to
    /// </summary>
    public Guid ProjectId { get; set; }
    public virtual Project Project { get; set; } = null!;

    /// <summary>
    /// Sprint this user story is assigned to
    /// </summary>
    public Guid? SprintId { get; set; }
    public virtual Sprint? Sprint { get; set; }

    /// <summary>
    /// User story assignee
    /// </summary>
    public Guid? AssigneeId { get; set; }
    public virtual User? Assignee { get; set; }


    /// <summary>
    /// AI tasks related to this user story
    /// </summary>
    public virtual ICollection<AITask> AITasks { get; set; } = new List<AITask>();

    /// <summary>
    /// Workflow instances for this user story
    /// </summary>
    public virtual ICollection<WorkflowInstance> WorkflowInstances { get; set; } = new List<WorkflowInstance>();
}

