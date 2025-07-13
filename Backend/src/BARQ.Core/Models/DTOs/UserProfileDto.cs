namespace BARQ.Core.Models.DTOs;

public class UserProfileDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public string? ProfileImageUrl { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? TimeZone { get; set; }
    public string? Language { get; set; }
    public bool IsActive { get; set; }
    public bool EmailVerified { get; set; }
    public bool MfaEnabled { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? LastLoginAt { get; set; }
    public Guid TenantId { get; set; }
    public string TenantName { get; set; } = string.Empty;
}
