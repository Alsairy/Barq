namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class OrganizationMemberDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the organization member.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the user.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier for the organization.
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address of the member.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string Role { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the member joined the organization.
    /// </summary>
    public DateTime JoinedAt { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the member is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// </summary>
    public string? ProfileImageUrl { get; set; }
}
