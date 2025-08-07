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
    
    /// <summary>
    /// Gets or sets whether the subscription is currently active
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Gets or sets the next billing date for the subscription
    /// </summary>
    public DateTime? NextBillingDate { get; set; }
    
    /// <summary>
    /// Gets or sets the usage tracking information for the subscription
    /// </summary>
    public UsageTrackingDto Usage { get; set; } = new();
}

/// <summary>
/// Data transfer object for subscription plan information (important-comment)
/// </summary>
public class SubscriptionPlanDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the subscription plan (important-comment)
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the name of the subscription plan
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the description of the subscription plan
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the monthly price for the subscription plan (important-comment)
    /// </summary>
    public decimal MonthlyPrice { get; set; }
    
    /// <summary>
    /// Gets or sets the yearly price for the subscription plan
    /// </summary>
    public decimal YearlyPrice { get; set; }
    
    /// <summary>
    /// Gets or sets the maximum number of users allowed in this plan (important-comment)
    /// </summary>
    public int UserLimit { get; set; }
    
    /// <summary>
    /// Gets or sets the maximum number of projects allowed in this plan (important-comment)
    /// </summary>
    public int ProjectLimit { get; set; }
    
    /// <summary>
    /// Gets or sets the list of features included in this plan
    /// </summary>
    public ICollection<string> Features { get; set; } = new List<string>();
}

/// <summary>
/// Data transfer object for usage tracking information
/// </summary>
public class UsageTrackingDto
{
    /// <summary>
    /// Gets or sets the current number of users in the subscription
    /// </summary>
    public int CurrentUsers { get; set; }
    
    /// <summary>
    /// Gets or sets the current number of projects in the subscription
    /// </summary>
    public int CurrentProjects { get; set; }
    
    /// <summary>
    /// Gets or sets the storage used in megabytes
    /// </summary>
    public int StorageUsedMB { get; set; }
    
    /// <summary>
    /// Gets or sets the number of API calls made this month
    /// </summary>
    public int ApiCallsThisMonth { get; set; }
    
    /// <summary>
    /// Gets or sets the timestamp when usage was last updated
    /// </summary>
    public DateTime LastUpdated { get; set; }
}
