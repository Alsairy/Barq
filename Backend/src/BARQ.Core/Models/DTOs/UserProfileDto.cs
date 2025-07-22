namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class UserProfileDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// </summary>
    public string? ProfileImageUrl { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the user's preferred time zone, if available.
    /// </summary>
    public string? TimeZone { get; set; }

    /// <summary>
    /// Gets or sets the user's preferred language, if available.
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the user's email is verified.
    /// </summary>
    public bool EmailVerified { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether multi-factor authentication is enabled for the user.
    /// </summary>
    public bool MfaEnabled { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user last logged in, if available.
    /// </summary>
    public DateTime? LastLoginAt { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the tenant the user belongs to.
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// </summary>
    public string TenantName { get; set; } = string.Empty;
}
