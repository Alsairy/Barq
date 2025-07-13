using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

public class NotificationResponse : BaseResponse
{
    public string? NotificationId { get; set; }
    public bool NotificationSent { get; set; }
    public DateTime? SentAt { get; set; }
    public string? DeliveryStatus { get; set; }
}

public class NotificationTemplateResponse : BaseResponse
{
    public NotificationTemplateDto? Template { get; set; }
}

public class NotificationQueueResponse : BaseResponse
{
    public string? QueueId { get; set; }
    public int QueuePosition { get; set; }
    public DateTime? EstimatedDelivery { get; set; }
}

public class NotificationDeliveryResponse : BaseResponse
{
    public List<NotificationDeliveryDto> DeliveryResults { get; set; } = new();
    public int TotalSent { get; set; }
    public int TotalFailed { get; set; }
}

public class NotificationPreferencesResponse : BaseResponse
{
    public NotificationPreferencesDto? Preferences { get; set; }
}

public class NotificationTemplateDto
{
    public Guid Id { get; set; }
    public string TemplateName { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string HtmlBody { get; set; } = string.Empty;
    public string? TextBody { get; set; }
    public List<string> RequiredVariables { get; set; } = new();
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class NotificationDeliveryDto
{
    public string RecipientEmail { get; set; } = string.Empty;
    public bool DeliverySuccessful { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime AttemptedAt { get; set; }
}

public class NotificationPreferencesDto
{
    public Guid UserId { get; set; }
    public bool EmailNotifications { get; set; }
    public bool PushNotifications { get; set; }
    public bool WorkflowNotifications { get; set; }
    public Dictionary<string, bool> NotificationTypes { get; set; } = new();
    public DateTime LastUpdated { get; set; }
}
