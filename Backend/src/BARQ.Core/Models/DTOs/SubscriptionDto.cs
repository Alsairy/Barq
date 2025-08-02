using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class SubscriptionDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// </summary>
    public SubscriptionPlan Plan { get; set; }
    
    /// <summary>
    /// </summary>
    public SubscriptionStatus Status { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? EndDate { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal MonthlyPrice { get; set; }
    
    /// <summary>
    /// </summary>
    public int UserLimit { get; set; }
    
    /// <summary>
    /// </summary>
    public int ProjectLimit { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? NextBillingDate { get; set; }
    
    /// <summary>
    /// </summary>
    public UsageTrackingDto Usage { get; set; } = new();
}

/// <summary>
/// </summary>
public class SubscriptionPlanDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public decimal MonthlyPrice { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal YearlyPrice { get; set; }
    
    /// <summary>
    /// </summary>
    public int UserLimit { get; set; }
    
    /// <summary>
    /// </summary>
    public int ProjectLimit { get; set; }
    
    /// <summary>
    /// </summary>
    public ICollection<string> Features { get; set; } = new List<string>();
}

/// <summary>
/// </summary>
public class UsageTrackingDto
{
    /// <summary>
    /// </summary>
    public int CurrentUsers { get; set; }
    
    /// <summary>
    /// </summary>
    public int CurrentProjects { get; set; }
    
    /// <summary>
    /// </summary>
    public int StorageUsedMB { get; set; }
    
    /// <summary>
    /// </summary>
    public int ApiCallsThisMonth { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime LastUpdated { get; set; }
}
