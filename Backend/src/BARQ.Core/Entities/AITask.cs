using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

/// <summary>
/// Represents an AI processing task in the BARQ platform
/// </summary>
public class AITask : TenantEntity
{
    /// <summary>
    /// Task name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Task description
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// AI task type
    /// </summary>
    public AITaskType TaskType { get; set; }

    /// <summary>
    /// AI task status
    /// </summary>
    public AITaskStatus Status { get; set; } = AITaskStatus.Pending;

    /// <summary>
    /// AI task priority
    /// </summary>
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;

    /// <summary>
    /// Input data for the AI task as JSON
    /// </summary>
    public string? InputData { get; set; }

    /// <summary>
    /// Output data from the AI task as JSON
    /// </summary>
    public string? OutputData { get; set; }

    /// <summary>
    /// AI provider used for this task
    /// </summary>
    public AIProvider AIProvider { get; set; }

    /// <summary>
    /// AI model used for this task
    /// </summary>
    [MaxLength(100)]
    public string? AIModel { get; set; }

    /// <summary>
    /// Processing cost for this task
    /// </summary>
    public decimal? Cost { get; set; }

    /// <summary>
    /// Processing time in milliseconds
    /// </summary>
    public long? ProcessingTimeMs { get; set; }

    /// <summary>
    /// Token usage for this task
    /// </summary>
    public int? TokensUsed { get; set; }

    /// <summary>
    /// Task started at
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// Task completed at
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Error message if task failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Retry count
    /// </summary>
    public int RetryCount { get; set; } = 0;

    /// <summary>
    /// Maximum retry attempts
    /// </summary>
    public int MaxRetries { get; set; } = 3;

    /// <summary>
    /// Task configuration as JSON
    /// </summary>
    public string? Configuration { get; set; }

    /// <summary>
    /// Quality score (0-100)
    /// </summary>
    public int? QualityScore { get; set; }

    /// <summary>
    /// Human review required
    /// </summary>
    public bool RequiresHumanReview { get; set; } = false;

    /// <summary>
    /// Human review status
    /// </summary>
    public ReviewStatus? ReviewStatus { get; set; }

    /// <summary>
    /// Human review comments
    /// </summary>
    public string? ReviewComments { get; set; }

    /// <summary>
    /// Project this task belongs to
    /// </summary>
    public Guid? ProjectId { get; set; }
    public virtual Project? Project { get; set; }

    /// <summary>
    /// Sprint this task belongs to
    /// </summary>
    public Guid? SprintId { get; set; }
    public virtual Sprint? Sprint { get; set; }

    /// <summary>
    /// User story this task is related to
    /// </summary>
    public Guid? UserStoryId { get; set; }
    public virtual UserStory? UserStory { get; set; }

    /// <summary>
    /// Parent task (for sub-tasks)
    /// </summary>
    public Guid? ParentTaskId { get; set; }
    public virtual AITask? ParentTask { get; set; }

    /// <summary>
    /// Child tasks
    /// </summary>
    public virtual ICollection<AITask> ChildTasks { get; set; } = new List<AITask>();

    /// <summary>
    /// Workflow instance this task belongs to
    /// </summary>
    public Guid? WorkflowInstanceId { get; set; }
    public virtual WorkflowInstance? WorkflowInstance { get; set; }

    /// <summary>
    /// Task assignee (for human review)
    /// </summary>
    public Guid? AssigneeId { get; set; }
    public virtual User? Assignee { get; set; }

    /// <summary>
    /// </summary>
    public Guid? UserId 
    { 
        get => AssigneeId; 
        set => AssigneeId = value; 
    }
}

