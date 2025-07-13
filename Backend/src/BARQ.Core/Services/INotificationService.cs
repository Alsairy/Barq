using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

public interface INotificationService
{
    Task<NotificationResponse> SendEmailNotificationAsync(SendEmailNotificationRequest request);
    Task<NotificationResponse> SendWorkflowNotificationAsync(SendWorkflowNotificationRequest request);
    Task<NotificationTemplateResponse> CreateEmailTemplateAsync(CreateEmailTemplateRequest request);
    Task<NotificationTemplateResponse> UpdateEmailTemplateAsync(UpdateEmailTemplateRequest request);
    Task<NotificationQueueResponse> QueueNotificationAsync(QueueNotificationRequest request);
    Task<NotificationDeliveryResponse> GetNotificationDeliveryStatusAsync(Guid notificationId);
    Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(Guid userId, int page = 1, int pageSize = 20);
    Task<NotificationPreferencesResponse> UpdateNotificationPreferencesAsync(UpdateNotificationPreferencesRequest request);
}
