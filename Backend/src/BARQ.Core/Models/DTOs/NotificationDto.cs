using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Data transfer object for system notifications and alerts
/// </summary>
public class NotificationDto
{
    /// <summary>
    /// Unique identifier for the notification
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Title of the notification message
    /// </summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// Content body of the notification message
    /// </summary>
    public string Message { get; set; } = string.Empty;
    /// <summary>
    /// Type classification of the notification
    /// </summary>
    public NotificationType Type { get; set; }
    /// <summary>
    /// Current status of the notification (important-comment)
    /// </summary>
    public NotificationStatus Status { get; set; }
    /// <summary>
    /// Identifier of the user who should receive the notification
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// Date and time when the notification was created (important-comment)
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Date and time when the notification was read by the user (important-comment)
    /// </summary>
    public DateTime? ReadAt { get; set; }
    /// <summary>
    /// Date and time when the notification was sent to the user (important-comment)
    /// </summary>
    public DateTime? SentAt { get; set; }
    /// <summary>
    /// Optional URL for notification action or redirect
    /// </summary>
    public string? ActionUrl { get; set; }
    /// <summary>
    /// Additional metadata and context information for the notification
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}
