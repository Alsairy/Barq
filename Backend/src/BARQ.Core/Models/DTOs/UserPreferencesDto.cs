namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class UserPreferencesDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the user whose preferences are being configured.
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? TimeZone { get; set; }
    
    /// <summary>
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
    /// </summary>
    public string? Theme { get; set; }
    
    /// <summary>
    /// </summary>
    public string? DateFormat { get; set; }
    
    /// <summary>
    /// </summary>
    public string? TimeFormat { get; set; }
}
