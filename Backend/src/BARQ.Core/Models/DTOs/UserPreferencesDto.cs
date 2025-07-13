namespace BARQ.Core.Models.DTOs;

public class UserPreferencesDto
{
    public Guid UserId { get; set; }
    public string? TimeZone { get; set; }
    public string? Language { get; set; }
    public bool EmailNotifications { get; set; }
    public bool PushNotifications { get; set; }
    public string? Theme { get; set; }
    public string? DateFormat { get; set; }
    public string? TimeFormat { get; set; }
}
