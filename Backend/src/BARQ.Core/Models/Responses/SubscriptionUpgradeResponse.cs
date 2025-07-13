using BARQ.Core.Models.Responses;

namespace BARQ.Core.Models.Responses;

public class SubscriptionUpgradeResponse : BaseResponse
{
    public Guid SubscriptionId { get; set; }
    public string NewPlan { get; set; } = string.Empty;
    public decimal NewPrice { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime NextBillingDate { get; set; }
    public string? ProrationDetails { get; set; }
}

public class SubscriptionDowngradeResponse : BaseResponse
{
    public Guid SubscriptionId { get; set; }
    public string NewPlan { get; set; } = string.Empty;
    public decimal NewPrice { get; set; }
    public DateTime EffectiveDate { get; set; }
    public DateTime NextBillingDate { get; set; }
    public string? DowngradeReason { get; set; }
}
