namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Data transfer object for user invitation information and status
/// </summary>
public class UserInvitationDto
{
    /// <summary>
    /// Unique identifier for the user invitation
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Email address of the invited user
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Identifier of the organization the user is invited to (important-comment)
    /// </summary>
    public Guid OrganizationId { get; set; }
    /// <summary>
    /// Name of the organization the user is invited to
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;
    /// <summary>
    /// Identifier of the user who sent the invitation
    /// </summary>
    public Guid InvitedBy { get; set; }
    /// <summary>
    /// Name of the user who sent the invitation
    /// </summary>
    public string InvitedByName { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the invitation was sent
    /// </summary>
    public DateTime InvitedAt { get; set; }
    /// <summary>
    /// Date and time when the invitation expires
    /// </summary>
    public DateTime ExpiresAt { get; set; }
    /// <summary>
    /// Indicates whether the invitation has been accepted
    /// </summary>
    public bool IsAccepted { get; set; }
    /// <summary>
    /// Date and time when the invitation was accepted
    /// </summary>
    public DateTime? AcceptedAt { get; set; }
    /// <summary>
    /// Current status of the invitation
    /// </summary>
    public string Status { get; set; } = string.Empty;
    /// <summary>
    /// List of roles assigned to the invited user
    /// </summary>
    public List<string> AssignedRoles { get; set; } = new();
}
