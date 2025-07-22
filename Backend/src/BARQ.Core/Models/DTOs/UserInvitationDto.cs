namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class UserInvitationDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the invitation.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the email address of the invited user.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the organization the user is invited to.
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Gets or sets the name of the organization the user is invited to.
    /// </summary>
    public string OrganizationName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the unique identifier of the user who sent the invitation.
    /// </summary>
    public Guid InvitedBy { get; set; }

    /// <summary>
    /// </summary>
    public string InvitedByName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the invitation was sent.
    /// </summary>
    public DateTime InvitedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the invitation expires.
    /// </summary>
    public DateTime ExpiresAt { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the invitation has been accepted.
    /// </summary>
    public bool IsAccepted { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the invitation was accepted.
    /// </summary>
    public DateTime? AcceptedAt { get; set; }

    /// <summary>
    /// Gets or sets the current status of the invitation (e.g., Pending, Accepted, Expired).
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public List<string> AssignedRoles { get; set; } = new();
}
