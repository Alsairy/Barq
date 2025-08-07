namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Data transfer object representing user profile information (important-comment)
/// </summary>
public class UserProfileDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the user (important-comment)
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the user's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the user's first name
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the user's last name
    /// </summary>
    public string LastName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the user's phone number
    /// </summary>
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    /// Gets or sets the URL to the user's profile image
    /// </summary>
    public string? ProfileImageUrl { get; set; }
    
    /// <summary>
    /// Gets or sets the user's date of birth
    /// </summary>
    public DateTime? DateOfBirth { get; set; }
    
    /// <summary>
    /// Gets or sets the user's preferred time zone
    /// </summary>
    public string? TimeZone { get; set; }
    
    /// <summary>
    /// Gets or sets the user's preferred language
    /// </summary>
    public string? Language { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the user is active (important-comment)
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether the user's email is verified (important-comment)
    /// </summary>
    public bool EmailVerified { get; set; }
    
    /// <summary>
    /// Gets or sets a value indicating whether multi-factor authentication is enabled (important-comment)
    /// </summary>
    public bool MfaEnabled { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time when the user was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// Gets or sets the date and time of the user's last login
    /// </summary>
    public DateTime? LastLoginAt { get; set; }
    
    /// <summary>
    /// Gets or sets the tenant identifier for the user (important-comment)
    /// </summary>
    public Guid TenantId { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the tenant
    /// </summary>
    public string TenantName { get; set; } = string.Empty;
}
