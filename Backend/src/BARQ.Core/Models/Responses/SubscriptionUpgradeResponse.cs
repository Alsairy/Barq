using BARQ.Core.Models.Responses;

namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class SubscriptionUpgradeResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public Guid SubscriptionId { get; set; }
    
    /// <summary>
    /// </summary>
    public string NewPlan { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public decimal NewPrice { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime EffectiveDate { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime NextBillingDate { get; set; }
    
    /// <summary>
    /// </summary>
    public string? ProrationDetails { get; set; }
}

/// <summary>
/// </summary>
public class SubscriptionDowngradeResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public Guid SubscriptionId { get; set; }
    
    /// <summary>
    /// </summary>
    public string NewPlan { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public decimal NewPrice { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime EffectiveDate { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime NextBillingDate { get; set; }
    
    /// <summary>
    /// </summary>
    public string? DowngradeReason { get; set; }
}
