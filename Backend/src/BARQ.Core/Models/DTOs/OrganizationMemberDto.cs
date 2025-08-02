namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class OrganizationMemberDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Role { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime JoinedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// </summary>
    public string? ProfileImageUrl { get; set; }
}
