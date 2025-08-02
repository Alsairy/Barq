using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class SubscriptionResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public SubscriptionDto? Subscription { get; set; }
}

/// <summary>
/// </summary>
public class UsageTrackingResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public decimal CurrentUsage { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal UsageLimit { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal UsagePercentage { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime PeriodStart { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime PeriodEnd { get; set; }
}

/// <summary>
/// </summary>
public class BillingCalculationResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public decimal Amount { get; set; }
    
    /// <summary>
    /// </summary>
    public string Currency { get; set; } = "USD";
    
    /// <summary>
    /// </summary>
    public DateTime BillingPeriodStart { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime BillingPeriodEnd { get; set; }
    
    /// <summary>
    /// </summary>
    public List<BillingLineItemDto> LineItems { get; set; } = new();
}

/// <summary>
/// </summary>
public class BillingNotificationResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public bool NotificationSent { get; set; }
    
    /// <summary>
    /// </summary>
    public string NotificationType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime? NextNotificationDate { get; set; }
}

/// <summary>
/// </summary>
public class BillingLineItemDto
{
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public decimal Quantity { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal UnitPrice { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal TotalPrice { get; set; }
}
