namespace BARQ.Core.Models.DTOs;

public class UserInvitationDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public Guid OrganizationId { get; set; }
    public string OrganizationName { get; set; } = string.Empty;
    public Guid InvitedBy { get; set; }
    public string InvitedByName { get; set; } = string.Empty;
    public DateTime InvitedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsAccepted { get; set; }
    public DateTime? AcceptedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<string> AssignedRoles { get; set; } = new();
}
