using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

/// <summary>
/// Represents a development sprint in the BARQ platform
/// </summary>
public class Sprint : TenantEntity
{
    /// <summary>
    /// Sprint name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Sprint description
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Sprint number within the project
    /// </summary>
    public int SprintNumber { get; set; }

    /// <summary>
    /// Sprint status
    /// </summary>
    public SprintStatus Status { get; set; } = SprintStatus.Planning;

    /// <summary>
    /// Sprint start date
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Sprint end date
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Sprint goal
    /// </summary>
    [MaxLength(500)]
    public string? Goal { get; set; }

    /// <summary>
    /// Sprint capacity (story points or hours)
    /// </summary>
    public decimal? Capacity { get; set; }

    /// <summary>
    /// Sprint velocity (completed story points)
    /// </summary>
    public decimal? Velocity { get; set; }

    /// <summary>
    /// Sprint budget allocation
    /// </summary>
    public decimal? Budget { get; set; }

    /// <summary>
    /// AI processing cost for this sprint
    /// </summary>
    public decimal? AICost { get; set; }

    /// <summary>
    /// Sprint retrospective notes
    /// </summary>
    public string? RetrospectiveNotes { get; set; }

    /// <summary>
    /// Project this sprint belongs to
    /// </summary>
    public Guid ProjectId { get; set; }
    
    /// <summary>
    /// Gets or sets the project that this sprint belongs to
    /// </summary>
    public virtual Project Project { get; set; } = null!;

    /// <summary>
    /// User stories in this sprint
    /// </summary>
    public virtual ICollection<UserStory> UserStories { get; set; } = new List<UserStory>();

    /// <summary>
    /// AI tasks for this sprint
    /// </summary>
    public virtual ICollection<AITask> AITasks { get; set; } = new List<AITask>();

    /// <summary>
    /// Workflow instances for this sprint
    /// </summary>
    public virtual ICollection<WorkflowInstance> WorkflowInstances { get; set; } = new List<WorkflowInstance>();

    /// <summary>
    /// Cost tracking records for this sprint
    /// </summary>
    public virtual ICollection<CostTracking> CostTrackings { get; set; } = new List<CostTracking>();
}

