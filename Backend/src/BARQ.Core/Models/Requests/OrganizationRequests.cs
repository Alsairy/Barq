namespace BARQ.Core.Models.Requests;

public class CreateOrganizationRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Domain { get; set; }
    public string SubscriptionPlan { get; set; } = "Free";
    public Dictionary<string, object>? Settings { get; set; }
}

public class UpdateOrganizationRequest
{
    public Guid OrganizationId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Domain { get; set; }
    public bool? IsActive { get; set; }
}

public class UpdateOrganizationSettingsRequest
{
    public Guid OrganizationId { get; set; }
    public Dictionary<string, object> Settings { get; set; } = new();
    public bool MergeWithExisting { get; set; } = true;
}

public class UpdateOrganizationBrandingRequest
{
    public Guid OrganizationId { get; set; }
    public string? LogoUrl { get; set; }
    public string? PrimaryColor { get; set; }
    public string? SecondaryColor { get; set; }
    public string? FontFamily { get; set; }
    public Dictionary<string, object>? CustomBranding { get; set; }
}

public class SendUserInvitationRequest
{
    public string Email { get; set; } = string.Empty;
    public Guid OrganizationId { get; set; }
    public Guid InvitedBy { get; set; }
    public List<Guid> RoleIds { get; set; } = new();
    public string? PersonalMessage { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

public class BulkUserInvitationRequest
{
    public List<string> Emails { get; set; } = new();
    public Guid OrganizationId { get; set; }
    public Guid InvitedBy { get; set; }
    public List<Guid> RoleIds { get; set; } = new();
    public string? PersonalMessage { get; set; }
    public DateTime? ExpiresAt { get; set; }
}

public class UpgradeSubscriptionRequest
{
    public Guid OrganizationId { get; set; }
    public string NewPlanName { get; set; } = string.Empty;
    public string BillingCycle { get; set; } = "monthly";
    public bool ProrateBilling { get; set; } = true;
    public DateTime? EffectiveDate { get; set; }
}

public class DowngradeSubscriptionRequest
{
    public Guid OrganizationId { get; set; }
    public string NewPlanName { get; set; } = string.Empty;
    public string BillingCycle { get; set; } = "monthly";
    public bool ImmediateDowngrade { get; set; } = false;
    public DateTime? EffectiveDate { get; set; }
}
