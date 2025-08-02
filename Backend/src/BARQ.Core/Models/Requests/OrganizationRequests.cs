namespace BARQ.Core.Models.Requests;

/// <summary>
/// </summary>
public class CreateOrganizationRequest
{
    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string? Domain { get; set; }
    
    /// <summary>
    /// </summary>
    public string SubscriptionPlan { get; set; } = "Free";
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object>? Settings { get; set; }
}

/// <summary>
/// </summary>
public class UpdateOrganizationRequest
{
    /// <summary>
    /// </summary>
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Domain { get; set; }
    
    /// <summary>
    /// </summary>
    public bool? IsActive { get; set; }
}

/// <summary>
/// </summary>
public class UpdateOrganizationSettingsRequest
{
    /// <summary>
    /// </summary>
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Settings { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public bool MergeWithExisting { get; set; } = true;
}

/// <summary>
/// </summary>
public class UpdateOrganizationBrandingRequest
{
    /// <summary>
    /// </summary>
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? LogoUrl { get; set; }
    
    /// <summary>
    /// </summary>
    public string? PrimaryColor { get; set; }
    
    /// <summary>
    /// </summary>
    public string? SecondaryColor { get; set; }
    
    /// <summary>
    /// </summary>
    public string? FontFamily { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object>? CustomBranding { get; set; }
}

/// <summary>
/// </summary>
public class SendUserInvitationRequest
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
public class BulkUserInvitationRequest
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
public class UpgradeSubscriptionRequest
{
    /// <summary>
    /// </summary>
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// </summary>
    public string NewPlanName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string BillingCycle { get; set; } = "monthly";
    
    /// <summary>
    /// </summary>
    public bool ProrateBilling { get; set; } = true;
    
    /// <summary>
    /// </summary>
    public DateTime? EffectiveDate { get; set; }
}

/// <summary>
/// </summary>
public class DowngradeSubscriptionRequest
{
    /// <summary>
    /// </summary>
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// </summary>
    public string NewPlanName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string BillingCycle { get; set; } = "monthly";
    
    /// <summary>
    /// </summary>
    public bool ImmediateDowngrade { get; set; } = false;
    
    /// <summary>
    /// </summary>
    public DateTime? EffectiveDate { get; set; }
}
