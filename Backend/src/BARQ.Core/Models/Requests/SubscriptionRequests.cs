namespace BARQ.Core.Models.Requests;

/// <summary>
/// </summary>
public class CreateSubscriptionRequest
{
    /// <summary>
    /// </summary>
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// </summary>
    public string PlanName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string BillingCycle { get; set; } = "monthly";
    
    /// <summary>
    /// </summary>
    public DateTime? StartDate { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object>? CustomLimits { get; set; }
}

/// <summary>
/// </summary>
public class UpdateSubscriptionRequest
{
    /// <summary>
    /// </summary>
    public Guid SubscriptionId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? NewPlanName { get; set; }
    
    /// <summary>
    /// </summary>
    public string? BillingCycle { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object>? CustomLimits { get; set; }
    
    /// <summary>
    /// </summary>
    public bool ProrateBilling { get; set; } = true;
}

/// <summary>
/// </summary>
public class TrackUsageRequest
{
    /// <summary>
    /// </summary>
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// </summary>
    public string ResourceType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public decimal UsageAmount { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? UsageDate { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object>? Metadata { get; set; }
}

/// <summary>
/// </summary>
public class CalculateBillingRequest
{
    /// <summary>
    /// </summary>
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime BillingPeriodStart { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime BillingPeriodEnd { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IncludeUsageCharges { get; set; } = true;
    
    /// <summary>
    /// </summary>
    public bool IncludeOverageCharges { get; set; } = true;
}
