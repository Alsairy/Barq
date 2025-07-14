namespace BARQ.Core.Enums;

public enum NotificationType
{
    Email = 1,
    InApp = 2,
    Push = 3,
    SMS = 4,
    Workflow = 5
}

public enum NotificationStatus
{
    Pending = 1,
    Sent = 2,
    Delivered = 3,
    Failed = 4,
    Read = 5
}

public enum NotificationPriority
{
    Low = 1,
    Normal = 2,
    High = 3,
    Critical = 4,
    Urgent = 5
}

public enum NotificationChannel
{
    Email = 1,
    SMS = 2,
    Push = 3,
    InApp = 4,
    Slack = 5,
    Teams = 6,
    Webhook = 7,
    Dashboard = 8
}
