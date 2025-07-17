namespace BARQ.Core.Enums;

/// <summary>
/// </summary>
public enum NotificationType
{
    /// <summary>
    /// </summary>
    Email = 1,
    /// <summary>
    /// </summary>
    InApp = 2,
    /// <summary>
    /// </summary>
    Push = 3,
    /// <summary>
    /// </summary>
    SMS = 4,
    /// <summary>
    /// </summary>
    Workflow = 5
}

/// <summary>
/// </summary>
public enum NotificationStatus
{
    /// <summary>
    /// </summary>
    Pending = 1,
    /// <summary>
    /// </summary>
    Sent = 2,
    /// <summary>
    /// </summary>
    Delivered = 3,
    /// <summary>
    /// </summary>
    Failed = 4,
    /// <summary>
    /// </summary>
    Read = 5
}

/// <summary>
/// </summary>
public enum NotificationPriority
{
    /// <summary>
    /// </summary>
    Low = 1,
    /// <summary>
    /// </summary>
    Normal = 2,
    /// <summary>
    /// </summary>
    High = 3,
    /// <summary>
    /// </summary>
    Critical = 4,
    /// <summary>
    /// </summary>
    Urgent = 5
}

/// <summary>
/// </summary>
public enum NotificationChannel
{
    /// <summary>
    /// </summary>
    Email = 1,
    /// <summary>
    /// </summary>
    SMS = 2,
    /// <summary>
    /// </summary>
    Push = 3,
    /// <summary>
    /// </summary>
    InApp = 4,
    /// <summary>
    /// </summary>
    Slack = 5,
    /// <summary>
    /// </summary>
    Teams = 6,
    /// <summary>
    /// </summary>
    Webhook = 7,
    /// <summary>
    /// </summary>
    Dashboard = 8,
    /// <summary>
    /// </summary>
    RealTime = 9
}
