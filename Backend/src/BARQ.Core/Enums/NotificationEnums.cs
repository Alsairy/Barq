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
