namespace BARQ.Core.Models.Requests;

/// <summary>
/// </summary>
public class SendInvitationRequest
{
    /// <summary>
    /// </summary>
    public string Email { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid InvitedBy { get; set; }
    
    /// <summary>
    /// </summary>
    public List<Guid> RoleIds { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public string? PersonalMessage { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
}

/// <summary>
/// </summary>
public class BulkInvitationRequest
{
    /// <summary>
    /// </summary>
    public List<string> Emails { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid InvitedBy { get; set; }
    
    /// <summary>
    /// </summary>
    public List<Guid> RoleIds { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public string? PersonalMessage { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? ExpiresAt { get; set; }
}

/// <summary>
/// </summary>
public class AcceptInvitationRequest
{
    /// <summary>
    /// </summary>
    public string InvitationToken { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string FirstName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string LastName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Password { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public bool AcceptTerms { get; set; }
}

/// <summary>
/// </summary>
public class CompleteOnboardingRequest
{
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    /// </summary>
    public string? TimeZone { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Language { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object>? AdditionalData { get; set; }
}
