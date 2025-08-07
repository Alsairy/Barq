namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Data transfer object for user preferences and personalization settings
/// </summary>
public class UserPreferencesDto
{
    /// <summary>
    /// Identifier of the user these preferences belong to
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// User's preferred time zone
    /// </summary>
    public string? TimeZone { get; set; }
    /// <summary>
    /// User's preferred language for the interface
    /// </summary>
    public string? Language { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive email notifications
    /// </summary>
    public bool EmailNotifications { get; set; }
    /// <summary>
    /// Indicates whether the user wants to receive push notifications
    /// </summary>
    public bool PushNotifications { get; set; }
    /// <summary>
    /// User's preferred UI theme
    /// </summary>
    public string? Theme { get; set; }
    /// <summary>
    /// User's preferred date format
    /// </summary>
    public string? DateFormat { get; set; }
    /// <summary>
    /// User's preferred time format
    /// </summary>
    public string? TimeFormat { get; set; }
}
