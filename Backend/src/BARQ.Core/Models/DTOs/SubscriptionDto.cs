using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

public class SubscriptionDto
{
    public Guid Id { get; set; }
    public Guid OrganizationId { get; set; }
    public SubscriptionPlan Plan { get; set; }
    public SubscriptionStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal MonthlyPrice { get; set; }
    public int UserLimit { get; set; }
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
