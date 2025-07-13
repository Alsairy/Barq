using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

public class SubscriptionResponse : BaseResponse
{
    public SubscriptionDto? Subscription { get; set; }
}

public class UsageTrackingResponse : BaseResponse
{
    public decimal CurrentUsage { get; set; }
    public decimal UsageLimit { get; set; }
    public decimal UsagePercentage { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
}

public class BillingCalculationResponse : BaseResponse
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public DateTime BillingPeriodStart { get; set; }
    public DateTime BillingPeriodEnd { get; set; }
    public List<BillingLineItemDto> LineItems { get; set; } = new();
}

public class BillingNotificationResponse : BaseResponse
{
    public bool NotificationSent { get; set; }
    public string NotificationType { get; set; } = string.Empty;
    public DateTime? NextNotificationDate { get; set; }
}

public class BillingLineItemDto
{
    public string Description { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}
