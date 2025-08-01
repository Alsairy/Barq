using BARQ.Core.Enums;

namespace BARQ.Core.Models.Requests;

/// <summary>
/// </summary>
public class SendEmailNotificationRequest
{
    /// <summary>
    /// </summary>
    public string ToEmail { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Subject { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Body { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string? TemplateName { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object>? TemplateData { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsHtml { get; set; } = true;
    
    /// <summary>
    /// </summary>
    public List<string>? Attachments { get; set; }
}

/// <summary>
/// </summary>
public class SendWorkflowNotificationRequest
{
    /// <summary>
    /// </summary>
    public Guid WorkflowInstanceId { get; set; }
    
    /// <summary>
    /// </summary>
    public string NotificationType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public List<Guid> RecipientUserIds { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> NotificationData { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public string? CustomMessage { get; set; }
}

/// <summary>
/// </summary>
public class CreateEmailTemplateRequest
{
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
    public bool IsActive { get; set; } = true;
}

/// <summary>
/// </summary>
public class UpdateEmailTemplateRequest
{
    /// <summary>
    /// </summary>
    public Guid TemplateId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? TemplateName { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Subject { get; set; }
    
    /// <summary>
    /// </summary>
    public string? HtmlBody { get; set; }
    
    /// <summary>
    /// </summary>
    public string? TextBody { get; set; }
    
    /// <summary>
    /// </summary>
    public List<string>? RequiredVariables { get; set; }
    
    /// <summary>
    /// </summary>
    public bool? IsActive { get; set; }
}

/// <summary>
/// </summary>
public class QueueNotificationRequest
{
    /// <summary>
    /// </summary>
    public string NotificationType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public List<Guid> RecipientUserIds { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> NotificationData { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public DateTime? ScheduledFor { get; set; }
    
    /// <summary>
    /// </summary>
    public int Priority { get; set; } = 5;
}

/// <summary>
/// </summary>
public class UpdateNotificationPreferencesRequest
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
}

/// <summary>
/// </summary>
public class SendNotificationRequest
{
    /// <summary>
    /// </summary>
    public List<Guid> RecipientUserIds { get; set; } = new();
    
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
    public NotificationPriority Priority { get; set; } = NotificationPriority.Normal;
    
    /// <summary>
    /// </summary>
    public List<NotificationChannel> Channels { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public string? Data { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? ScheduledAt { get; set; }
    
    /// <summary>
    /// </summary>
    public bool RequireAcknowledgment { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid? ProjectId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid? WorkflowInstanceId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid? AITaskId { get; set; }
}

/// <summary>
/// </summary>
public class SendBulkNotificationRequest
{
    /// <summary>
    /// </summary>
    public List<SendNotificationRequest> Notifications { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public bool ProcessInBatches { get; set; } = true;
    
    /// <summary>
    /// </summary>
    public int BatchSize { get; set; } = 100;
    
    /// <summary>
    /// </summary>
    public TimeSpan? DelayBetweenBatches { get; set; }
}
