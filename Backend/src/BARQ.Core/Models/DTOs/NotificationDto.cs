using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class NotificationDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the notification.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the type of notification (e.g., Info, Warning, Error).
    /// </summary>
    public NotificationType Type { get; set; }

    /// <summary>
    /// Gets or sets the current status of the notification (e.g., Unread, Read, Dismissed).
    /// </summary>
    public NotificationStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user who should receive the notification.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the notification was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the notification was read by the user.
    /// </summary>
    public DateTime? ReadAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the notification was sent to the user.
    /// </summary>
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// </summary>
    public string? ActionUrl { get; set; }

    /// <summary>
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}
