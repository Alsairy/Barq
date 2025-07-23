namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Represents user preferences and personalization settings for the application interface and notifications.
/// </summary>
public class UserPreferencesDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the user whose preferences are being configured.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// Gets or sets the user's preferred time zone for displaying dates and times.
    /// </summary>
    public string? TimeZone { get; set; }
    
    /// <summary>
    /// Gets or sets the user's preferred language for the application interface.
    /// </summary>
    public string? Language { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the user wants to receive email notifications.
    /// </summary>
    public bool EmailNotifications { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the user wants to receive push notifications.
    /// </summary>
    public bool PushNotifications { get; set; }
    
    /// <summary>
    /// Gets or sets the user's preferred application theme (e.g., Light, Dark, Auto).
    /// </summary>
    public string? Theme { get; set; }
    
    /// <summary>
    /// Gets or sets the user's preferred date format for displaying dates.
    /// </summary>
    public string? DateFormat { get; set; }
    
    /// <summary>
    /// Gets or sets the user's preferred time format for displaying times (e.g., 12-hour, 24-hour).
    /// </summary>
    public string? TimeFormat { get; set; }
}
