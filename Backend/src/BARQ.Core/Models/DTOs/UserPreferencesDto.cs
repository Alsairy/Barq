namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class UserPreferencesDto
{
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? TimeZone { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Language { get; set; }
    
    /// <summary>
    /// </summary>
    public bool EmailNotifications { get; set; }
    
    /// <summary>
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
