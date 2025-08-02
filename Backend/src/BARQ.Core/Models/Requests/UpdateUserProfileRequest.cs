
namespace BARQ.Core.Models.Requests;

/// <summary>
/// </summary>
public class UpdateUserProfileRequest
{
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? FirstName { get; set; }
    
    /// <summary>
    /// </summary>
    public string? LastName { get; set; }
    
    /// <summary>
    /// </summary>
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? DateOfBirth { get; set; }
    
    /// <summary>
    /// </summary>
    public string? TimeZone { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Language { get; set; }
}

/// <summary>
/// </summary>
public class UploadProfileImageRequest
{
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public byte[] ImageData { get; set; } = Array.Empty<byte>();
    
    /// <summary>
    /// </summary>
    public string ImageFileName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string ImageContentType { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class UpdateUserPreferencesRequest
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
    public bool? EmailNotifications { get; set; }
    
    /// <summary>
    /// </summary>
    public bool? PushNotifications { get; set; }
    
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
