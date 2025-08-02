using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class NotificationResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public string? NotificationId { get; set; }
    
    /// <summary>
    /// </summary>
    public bool NotificationSent { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? SentAt { get; set; }
    
    /// <summary>
    /// </summary>
    public string? DeliveryStatus { get; set; }
}

/// <summary>
/// </summary>
public class NotificationTemplateResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public NotificationTemplateDto? Template { get; set; }
}

/// <summary>
/// </summary>
public class NotificationQueueResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public string? QueueId { get; set; }
    
    /// <summary>
    /// </summary>
    public int QueuePosition { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? EstimatedDelivery { get; set; }
}

/// <summary>
/// </summary>
public class NotificationDeliveryResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public List<NotificationDeliveryDto> DeliveryResults { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public int TotalSent { get; set; }
    
    /// <summary>
    /// </summary>
    public int TotalFailed { get; set; }
}

/// <summary>
/// </summary>
public class NotificationPreferencesResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public NotificationPreferencesDto? Preferences { get; set; }
}

/// <summary>
/// </summary>
public class NotificationTemplateDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string TemplateName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Subject { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string HtmlBody { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string? TextBody { get; set; }
    
    /// <summary>
    /// </summary>
    public List<string> RequiredVariables { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// </summary>
public class NotificationDeliveryDto
{
    /// <summary>
    /// </summary>
    public string RecipientEmail { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public bool DeliverySuccessful { get; set; }
    
    /// <summary>
    /// </summary>
    public string? ErrorMessage { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime AttemptedAt { get; set; }
}

/// <summary>
/// </summary>
public class NotificationPreferencesDto
{
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public bool EmailNotifications { get; set; }
    
    /// <summary>
    /// </summary>
    public bool PushNotifications { get; set; }
    
    /// <summary>
    /// </summary>
    public bool WorkflowNotifications { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, bool> NotificationTypes { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public DateTime LastUpdated { get; set; }
}
