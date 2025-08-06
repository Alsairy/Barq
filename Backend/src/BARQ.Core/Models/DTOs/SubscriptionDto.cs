using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Data transfer object for subscription information
/// </summary>
public class SubscriptionDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the subscription
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the organization identifier associated with this subscription
    /// </summary>
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// Gets or sets the subscription plan type
    /// </summary>
    public SubscriptionPlan Plan { get; set; }
    
    /// <summary>
    /// Gets or sets the current status of the subscription
    /// </summary>
    public SubscriptionStatus Status { get; set; }
    
    /// <summary>
    /// Gets or sets the subscription start date
    /// </summary>
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// Gets or sets the subscription end date (null for active subscriptions)
    /// </summary>
    public DateTime? EndDate { get; set; }
    
    /// <summary>
    /// Gets or sets the monthly price for the subscription
    /// </summary>
    public decimal MonthlyPrice { get; set; }
    
    /// <summary>
    /// Gets or sets the maximum number of users allowed in this subscription
    /// </summary>
    public int UserLimit { get; set; }
    
    /// <summary>
    /// Gets or sets the maximum number of projects allowed in this subscription
    /// </summary>
    public int ProjectLimit { get; set; }
    public bool IsActive { get; set; }
    public DateTime? NextBillingDate { get; set; }
    public UsageTrackingDto Usage { get; set; } = new();
}

public class SubscriptionPlanDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal MonthlyPrice { get; set; }
    public decimal YearlyPrice { get; set; }
    public int UserLimit { get; set; }
    public int ProjectLimit { get; set; }
    public ICollection<string> Features { get; set; } = new List<string>();
}

public class UsageTrackingDto
{
    public int CurrentUsers { get; set; }
    public int CurrentProjects { get; set; }
    public int StorageUsedMB { get; set; }
    public int ApiCallsThisMonth { get; set; }
    public DateTime LastUpdated { get; set; }
}
