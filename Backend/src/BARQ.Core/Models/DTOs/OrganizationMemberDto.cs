namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Data transfer object for organization member information and role details (important-comment)
/// </summary>
public class OrganizationMemberDto
{
    /// <summary>
    /// Unique identifier for the organization member (important-comment)
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Identifier of the user who is a member of the organization
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// Identifier of the organization the user belongs to
    /// </summary>
    public Guid OrganizationId { get; set; }
    /// <summary>
    /// Display name of the organization member
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// Email address of the organization member
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Role of the member within the organization
    /// </summary>
    public string Role { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the user joined the organization
    /// </summary>
    public DateTime JoinedAt { get; set; }
    /// <summary>
    /// Indicates whether the member is currently active in the organization
    /// </summary>
    public bool IsActive { get; set; }
    /// <summary>
    /// Optional URL to the member's profile image
    /// </summary>
    public string? ProfileImageUrl { get; set; }
}
