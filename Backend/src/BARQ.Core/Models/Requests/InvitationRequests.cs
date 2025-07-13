namespace BARQ.Core.Models.Requests;

public class SendInvitationRequest
{
    public string Email { get; set; } = string.Empty;
    public Guid OrganizationId { get; set; }
    public Guid InvitedBy { get; set; }
    public List<Guid> RoleIds { get; set; } = new();
    public string? PersonalMessage { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

public class BulkInvitationRequest
{
    public List<string> Emails { get; set; } = new();
    public Guid OrganizationId { get; set; }
    public Guid InvitedBy { get; set; }
    public List<Guid> RoleIds { get; set; } = new();
    public string? PersonalMessage { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

public class AcceptInvitationRequest
{
    public string InvitationToken { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool AcceptTerms { get; set; }
}

public class CompleteOnboardingRequest
{
    public Guid UserId { get; set; }
    public string? PhoneNumber { get; set; }
    public string? TimeZone { get; set; }
    public string? Language { get; set; }
    public Dictionary<string, object>? AdditionalData { get; set; }
}
