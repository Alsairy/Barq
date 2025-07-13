using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

public interface ISubscriptionService
{
    Task<SubscriptionDto> GetSubscriptionAsync(Guid organizationId);
    Task<SubscriptionResponse> CreateSubscriptionAsync(CreateSubscriptionRequest request);
    Task<SubscriptionResponse> UpgradeSubscriptionAsync(UpgradeSubscriptionRequest request);
    Task<SubscriptionResponse> DowngradeSubscriptionAsync(DowngradeSubscriptionRequest request);
    Task<SubscriptionResponse> CancelSubscriptionAsync(Guid organizationId);
    Task<UsageTrackingResponse> TrackUsageAsync(TrackUsageRequest request);
    Task<BillingCalculationResponse> CalculateBillingAsync(Guid organizationId);
    Task<IEnumerable<SubscriptionPlanDto>> GetAvailablePlansAsync();
    Task<BillingNotificationResponse> SendBillingNotificationAsync(Guid organizationId);
}
