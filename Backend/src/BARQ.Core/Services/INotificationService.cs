using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface INotificationService
{
    /// <summary>
    /// </summary>
    Task<NotificationResponse> SendEmailNotificationAsync(SendEmailNotificationRequest request);

    /// <summary>
    /// </summary>
    Task<NotificationResponse> SendWorkflowNotificationAsync(SendWorkflowNotificationRequest request);

    /// <summary>
    /// </summary>
    Task<NotificationTemplateResponse> CreateEmailTemplateAsync(CreateEmailTemplateRequest request);

    /// <summary>
    /// </summary>
    Task<NotificationTemplateResponse> UpdateEmailTemplateAsync(UpdateEmailTemplateRequest request);

    /// <summary>
    /// </summary>
    Task<NotificationQueueResponse> QueueNotificationAsync(QueueNotificationRequest request);

    /// <summary>
    /// </summary>
    Task<NotificationDeliveryResponse> GetNotificationDeliveryStatusAsync(Guid notificationId);

    /// <summary>
    /// </summary>
    Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(Guid userId, int page = 1, int pageSize = 20);

    /// <summary>
    /// </summary>
    Task<NotificationPreferencesResponse> UpdateNotificationPreferencesAsync(UpdateNotificationPreferencesRequest request);
}
