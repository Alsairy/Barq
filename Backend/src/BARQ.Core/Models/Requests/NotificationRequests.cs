using BARQ.Core.Enums;
using static BARQ.Core.Enums.NotificationEnums;

namespace BARQ.Core.Models.Requests;

public class SendEmailNotificationRequest
{
    public string ToEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string? TemplateName { get; set; }
    public Dictionary<string, object>? TemplateData { get; set; }
    public bool IsHtml { get; set; } = true;
    public List<string>? Attachments { get; set; }
}

public class SendWorkflowNotificationRequest
{
    public Guid WorkflowInstanceId { get; set; }
    public string NotificationType { get; set; } = string.Empty;
    public List<Guid> RecipientUserIds { get; set; } = new();
    public Dictionary<string, object> NotificationData { get; set; } = new();
    public string? CustomMessage { get; set; }
}

public class CreateEmailTemplateRequest
{
    public string TemplateName { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string HtmlBody { get; set; } = string.Empty;
    public string? TextBody { get; set; }
    public List<string> RequiredVariables { get; set; } = new();
    public bool IsActive { get; set; } = true;
}

public class UpdateEmailTemplateRequest
{
    public Guid TemplateId { get; set; }
    public string? TemplateName { get; set; }
    public string? Subject { get; set; }
    public string? HtmlBody { get; set; }
    public string? TextBody { get; set; }
    public List<string>? RequiredVariables { get; set; }
    public bool? IsActive { get; set; }
}

public class QueueNotificationRequest
{
    public string NotificationType { get; set; } = string.Empty;
    public List<Guid> RecipientUserIds { get; set; } = new();
    public Dictionary<string, object> NotificationData { get; set; } = new();
    public DateTime? ScheduledFor { get; set; }
    public int Priority { get; set; } = 5;
}

public class UpdateNotificationPreferencesRequest
{
    public Guid UserId { get; set; }
    public bool EmailNotifications { get; set; }
    public bool PushNotifications { get; set; }
    public bool WorkflowNotifications { get; set; }
    public Dictionary<string, bool> NotificationTypes { get; set; } = new();
}

public class SendNotificationRequest
{
    public List<Guid> RecipientUserIds { get; set; } = new();
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public NotificationPriority Priority { get; set; } = NotificationPriority.Normal;
    public List<NotificationChannel> Channels { get; set; } = new();
    public string? Data { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public bool RequireAcknowledgment { get; set; }
    public Guid? ProjectId { get; set; }
    public Guid? WorkflowInstanceId { get; set; }
    public Guid? AITaskId { get; set; }
}

public class SendBulkNotificationRequest
{
    public List<SendNotificationRequest> Notifications { get; set; } = new();
    public bool ProcessInBatches { get; set; } = true;
    public int BatchSize { get; set; } = 100;
    public TimeSpan? DelayBetweenBatches { get; set; }
}
