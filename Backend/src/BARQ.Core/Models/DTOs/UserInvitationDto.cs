namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class UserInvitationDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Guid InvitedBy { get; set; }
    
    /// <summary>
    /// </summary>
    public string InvitedByName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime InvitedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime ExpiresAt { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsAccepted { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? AcceptedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public string Status { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public List<string> AssignedRoles { get; set; } = new();
}
