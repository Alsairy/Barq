using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class NotificationDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Message { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public NotificationType Type { get; set; }
    
    /// <summary>
    /// </summary>
    public NotificationStatus Status { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? ReadAt { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? SentAt { get; set; }
    
    /// <summary>
    /// </summary>
    public string? ActionUrl { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Metadata { get; set; } = new();
}
