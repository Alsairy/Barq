namespace BARQ.Core.Models.Requests;

public class CreateSubscriptionRequest
{
    public Guid OrganizationId { get; set; }
    public string PlanName { get; set; } = string.Empty;
    public string BillingCycle { get; set; } = "monthly";
    public DateTime? StartDate { get; set; }
    public Dictionary<string, object>? CustomLimits { get; set; }
}

public class UpdateSubscriptionRequest
{
    public Guid SubscriptionId { get; set; }
    public string? NewPlanName { get; set; }
    public string? BillingCycle { get; set; }
    public Dictionary<string, object>? CustomLimits { get; set; }
    public bool ProrateBilling { get; set; } = true;
}

public class TrackUsageRequest
{
    public Guid OrganizationId { get; set; }
    public string ResourceType { get; set; } = string.Empty;
    public decimal UsageAmount { get; set; }
    public DateTime? UsageDate { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
}

public class CalculateBillingRequest
{
    public Guid OrganizationId { get; set; }
    public DateTime BillingPeriodStart { get; set; }
    public DateTime BillingPeriodEnd { get; set; }
    public bool IncludeUsageCharges { get; set; } = true;
    public bool IncludeOverageCharges { get; set; } = true;
}
