
namespace BARQ.Core.Models.Requests;

public class UpdateUserProfileRequest
{
    public Guid UserId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? TimeZone { get; set; }
    public string? Language { get; set; }
}

public class UploadProfileImageRequest
{
    public Guid UserId { get; set; }
    public byte[] ImageData { get; set; } = Array.Empty<byte>();
    public string ImageFileName { get; set; } = string.Empty;
    public string ImageContentType { get; set; } = string.Empty;
}

public class UpdateUserPreferencesRequest
{
    public Guid UserId { get; set; }
    public string? TimeZone { get; set; }
    public string? Language { get; set; }
    public bool? EmailNotifications { get; set; }
    public bool? PushNotifications { get; set; }
    public string? Theme { get; set; }
    public string? DateFormat { get; set; }
    public string? TimeFormat { get; set; }
}
