using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface ISubscriptionService
{
    /// <summary>
    /// </summary>
    Task<SubscriptionDto> GetSubscriptionAsync(Guid organizationId);

    /// <summary>
    /// </summary>
    Task<SubscriptionResponse> CreateSubscriptionAsync(CreateSubscriptionRequest request);

    /// <summary>
    /// </summary>
    Task<SubscriptionResponse> UpgradeSubscriptionAsync(UpgradeSubscriptionRequest request);

    /// <summary>
    /// </summary>
    Task<SubscriptionResponse> DowngradeSubscriptionAsync(DowngradeSubscriptionRequest request);

    /// <summary>
    /// </summary>
    Task<SubscriptionResponse> CancelSubscriptionAsync(Guid organizationId);

    /// <summary>
    /// </summary>
    Task<UsageTrackingResponse> TrackUsageAsync(TrackUsageRequest request);

    /// <summary>
    /// </summary>
    Task<BillingCalculationResponse> CalculateBillingAsync(Guid organizationId);

    /// <summary>
    /// </summary>
    Task<IEnumerable<SubscriptionPlanDto>> GetAvailablePlansAsync();

    /// <summary>
    /// </summary>
    Task<BillingNotificationResponse> SendBillingNotificationAsync(Guid organizationId);
}
