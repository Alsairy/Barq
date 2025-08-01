namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class UserProfileDto
{
    /// <summary>
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
    /// </summary>
    public string? TimeZone { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Language { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// </summary>
    public bool EmailVerified { get; set; }
    
    /// <summary>
    /// </summary>
    public bool MfaEnabled { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? LastLoginAt { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid TenantId { get; set; }
    
    /// <summary>
    /// </summary>
    public string TenantName { get; set; } = string.Empty;
}
