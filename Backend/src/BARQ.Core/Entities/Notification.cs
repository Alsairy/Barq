using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

/// <summary>
/// </summary>
public class Notification : TenantEntity
{
    /// <summary>
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    [Required]
    [MaxLength(2000)]
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public NotificationType Type { get; set; }

    /// <summary>
    /// </summary>
    public NotificationPriority Priority { get; set; } = NotificationPriority.Normal;

    /// <summary>
    /// </summary>
    public NotificationStatus Status { get; set; } = NotificationStatus.Pending;

    /// <summary>
    /// </summary>
    public List<NotificationChannel> Channels { get; set; } = new();

    /// <summary>
    /// </summary>
    public string? Data { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? ReadAt { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? ScheduledAt { get; set; }

    /// <summary>
    /// </summary>
    public bool RequireAcknowledgment { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? AcknowledgedAt { get; set; }

    /// <summary>
    /// </summary>
    public Guid RecipientUserId { get; set; }
    public virtual User RecipientUser { get; set; } = null!;

    /// <summary>
    /// </summary>
    public Guid? SenderUserId { get; set; }
    public virtual User? SenderUser { get; set; }

    /// <summary>
    /// </summary>
    public Guid? ProjectId { get; set; }
    public virtual Project? Project { get; set; }

    /// <summary>
    /// </summary>
    public Guid? WorkflowInstanceId { get; set; }
    public virtual WorkflowInstance? WorkflowInstance { get; set; }

    /// <summary>
    /// </summary>
    public Guid? AITaskId { get; set; }
    public virtual AITask? AITask { get; set; }
}
