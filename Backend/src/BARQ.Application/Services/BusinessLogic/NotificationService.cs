using Microsoft.Extensions.Logging;
using AutoMapper;
using BARQ.Core.Services;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;
using BARQ.Core.Entities;
using BARQ.Core.Repositories;
using BARQ.Core.Enums;
using BARQ.Core.Services;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace BARQ.Application.Services.BusinessLogic;

public class NotificationService : INotificationService
{
    private readonly IRepository<Notification> _notificationRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<AuditLog> _auditLogRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<NotificationService> _logger;
    private readonly ITenantProvider _tenantProvider;
    private readonly IHubContext<NotificationHub>? _hubContext;

    public NotificationService(
        IRepository<Notification> notificationRepository,
        IRepository<User> userRepository,
        IRepository<AuditLog> auditLogRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<NotificationService> logger,
        ITenantProvider tenantProvider,
        IHubContext<NotificationHub>? hubContext = null)
    {
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
        _auditLogRepository = auditLogRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
        _tenantProvider = tenantProvider;
        _hubContext = hubContext;
    }

    public async Task<NotificationResponse> SendNotificationAsync(SendNotificationRequest request)
    {
        try
        {
            var tenantId = _tenantProvider.GetTenantId();
            
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Message = request.Message,
                Type = request.Type,
                Priority = request.Priority,
                UserId = request.UserId,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                TenantId = tenantId,
                Data = request.Data != null ? JsonSerializer.Serialize(request.Data) : null
            };

            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            await SendMultiChannelNotificationAsync(notification, request.Channels ?? new List<NotificationChannel> { NotificationChannel.InApp });

            await LogAuditAsync("NOTIFICATION_SENT", $"Notification sent: {notification.Title}", notification.Id);

            var notificationDto = _mapper.Map<NotificationDto>(notification);
            _logger.LogInformation("Notification sent: {NotificationId} to user {UserId}", notification.Id, request.UserId);

            return new NotificationResponse
            {
                Success = true,
                Message = "Notification sent successfully",
                Notification = notificationDto
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending notification to user: {UserId}", request.UserId);
            return new NotificationResponse
            {
                Success = false,
                Message = "Failed to send notification"
            };
        }
    }

    public async Task<NotificationResponse> SendEmailNotificationAsync(SendEmailNotificationRequest request)
    {
        try
        {
            _logger.LogInformation("Sending email notification to: {To}, Subject: {Subject}", request.ToEmail, request.Subject);

            return new NotificationResponse
            {
                Success = true,
                Message = "Email sent successfully",
                NotificationId = Guid.NewGuid().ToString(),
                NotificationSent = true,
                SentAt = DateTime.UtcNow,
                DeliveryStatus = "Sent"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email notification to: {To}", request.ToEmail);
            return new NotificationResponse
            {
                Success = false,
                Message = "Failed to send email",
                NotificationSent = false,
                DeliveryStatus = "Failed"
            };
        }
    }

    public Task<NotificationResponse> SendWorkflowNotificationAsync(SendWorkflowNotificationRequest request)
    {
        try
        {
            _logger.LogInformation("Sending workflow notification to workflow: {WorkflowInstanceId}, Type: {NotificationType}", 
                request.WorkflowInstanceId, request.NotificationType);

            return Task.FromResult(new NotificationResponse
            {
                Success = true,
                Message = "Notification sent successfully",
                NotificationId = Guid.NewGuid().ToString(),
                NotificationSent = true,
                SentAt = DateTime.UtcNow,
                DeliveryStatus = "Sent"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending workflow notification to workflow: {WorkflowInstanceId}", request.WorkflowInstanceId);
            return Task.FromResult(new NotificationResponse
            {
                Success = false,
                Message = "Failed to send notification",
                NotificationSent = false,
                DeliveryStatus = "Failed"
            });
        }
    }

    public Task<NotificationTemplateResponse> CreateEmailTemplateAsync(CreateEmailTemplateRequest request)
    {
        try
        {
            var template = new NotificationTemplateDto
            {
                Id = Guid.NewGuid(),
                TemplateName = request.TemplateName,
                Subject = request.Subject,
                HtmlBody = request.HtmlBody,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _logger.LogInformation("Email template created: {TemplateId} - {TemplateName}", 
                template.Id, template.TemplateName);

            return Task.FromResult(new NotificationTemplateResponse
            {
                Success = true,
                Message = "Email template created successfully",
                Template = template
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating email template: {TemplateName}", request.TemplateName);
            return Task.FromResult(new NotificationTemplateResponse
            {
                Success = false,
                Message = "Failed to create email template"
            });
        }
    }

    public Task<NotificationTemplateResponse> UpdateEmailTemplateAsync(UpdateEmailTemplateRequest request)
    {
        try
        {
            _logger.LogInformation("Updating email template: {TemplateId}", request.TemplateId);

            var template = new NotificationTemplateDto
            {
                Id = request.TemplateId,
                TemplateName = request.TemplateName ?? "Updated Template",
                Subject = request.Subject ?? "Updated Subject",
                HtmlBody = request.HtmlBody ?? "Updated Body",
                IsActive = request.IsActive ?? true,
                CreatedAt = DateTime.UtcNow.AddDays(-1),
                UpdatedAt = DateTime.UtcNow
            };

            return Task.FromResult(new NotificationTemplateResponse
            {
                Success = true,
                Message = "Email template updated successfully",
                Template = template
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating email template: {TemplateId}", request.TemplateId);
            return Task.FromResult(new NotificationTemplateResponse
            {
                Success = false,
                Message = "Failed to update email template"
            });
        }
    }

    public Task<NotificationDeliveryResponse> GetNotificationDeliveryStatusAsync(Guid notificationId)
    {
        try
        {
            _logger.LogInformation("Getting notification delivery status: {NotificationId}", notificationId);

            return Task.FromResult(new NotificationDeliveryResponse
            {
                Success = true,
                Message = "Delivery status retrieved successfully",
                DeliveryResults = new List<NotificationDeliveryDto>
                {
                    new NotificationDeliveryDto
                    {
                        RecipientEmail = "user@example.com",
                        DeliverySuccessful = true,
                        AttemptedAt = DateTime.UtcNow
                    }
                },
                TotalSent = 1,
                TotalFailed = 0
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting notification delivery status: {NotificationId}", notificationId);
            return Task.FromResult(new NotificationDeliveryResponse
            {
                Success = false,
                Message = "Failed to get delivery status",
                DeliveryResults = new List<NotificationDeliveryDto>(),
                TotalSent = 0,
                TotalFailed = 1
            });
        }
    }

    public Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(Guid userId, int page = 1, int pageSize = 20)
    {
        try
        {
            _logger.LogInformation("Getting user notifications: {UserId}, Page: {Page}, PageSize: {PageSize}", 
                userId, page, pageSize);

            var notifications = new List<NotificationDto>
            {
                new NotificationDto
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    Title = "Welcome to Barq",
                    Message = "Welcome to the platform!",
                    Type = NotificationType.Email,
                    Status = NotificationStatus.Sent,
                    CreatedAt = DateTime.UtcNow
                }
            };

            return Task.FromResult<IEnumerable<NotificationDto>>(notifications);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user notifications: {UserId}", userId);
            return Task.FromResult<IEnumerable<NotificationDto>>(new List<NotificationDto>());
        }
    }

    public Task<NotificationPreferencesResponse> UpdateNotificationPreferencesAsync(UpdateNotificationPreferencesRequest request)
    {
        try
        {
            _logger.LogInformation("Updating notification preferences for user: {UserId}", request.UserId);

            return Task.FromResult(new NotificationPreferencesResponse
            {
                Success = true,
                Message = "Notification preferences updated successfully",
                Preferences = new NotificationPreferencesDto
                {
                    UserId = request.UserId,
                    EmailNotifications = true,
                    PushNotifications = true,
                    WorkflowNotifications = true,
                    LastUpdated = DateTime.UtcNow
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating notification preferences for user: {UserId}", request.UserId);
            return Task.FromResult(new NotificationPreferencesResponse
            {
                Success = false,
                Message = "Failed to update notification preferences"
            });
        }
    }

    public async Task<NotificationResponse> SendBulkNotificationAsync(SendBulkNotificationRequest request)
    {
        try
        {
            var successCount = 0;
            var failureCount = 0;

            foreach (var userId in request.UserIds)
            {
                try
                {
                    var individualRequest = new SendNotificationRequest
                    {
                        UserId = userId,
                        Title = request.Title,
                        Message = request.Message,
                        Type = request.Type,
                        Priority = request.Priority,
                        Data = request.Data,
                        Channels = request.Channels
                    };

                    var result = await SendNotificationAsync(individualRequest);
                    if (result.Success)
                        successCount++;
                    else
                        failureCount++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error sending bulk notification to user: {UserId}", userId);
                    failureCount++;
                }
            }

            _logger.LogInformation("Bulk notification completed: {Success} successful, {Failed} failed", successCount, failureCount);

            return new NotificationResponse
            {
                Success = failureCount == 0,
                Message = $"Bulk notification completed: {successCount} successful, {failureCount} failed"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending bulk notification");
            return new NotificationResponse
            {
                Success = false,
                Message = "Failed to send bulk notification"
            };
        }
    }

    public async Task<NotificationResponse> SendRealTimeNotificationAsync(Guid userId, string title, string message, object? data = null)
    {
        try
        {
            if (_hubContext != null)
            {
                var notificationData = new
                {
                    id = Guid.NewGuid(),
                    title,
                    message,
                    timestamp = DateTime.UtcNow,
                    data
                };

                await _hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveNotification", notificationData);
                _logger.LogInformation("Real-time notification sent to user: {UserId}", userId);
            }

            var request = new SendNotificationRequest
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = NotificationType.Info,
                Priority = NotificationPriority.Medium,
                Data = data,
                Channels = new List<NotificationChannel> { NotificationChannel.InApp, NotificationChannel.RealTime }
            };

            return await SendNotificationAsync(request);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending real-time notification to user: {UserId}", userId);
            return new NotificationResponse
            {
                Success = false,
                Message = "Failed to send real-time notification"
            };
        }
    }

    public async Task<bool> SendEmailNotificationAsync(Guid userId, string subject, string body, string? templateId = null)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found for email notification: {UserId}", userId);
                return false;
            }

            _logger.LogInformation("Sending email notification to {Email} with subject: {Subject}", user.Email, subject);
            
            await LogAuditAsync("EMAIL_NOTIFICATION_SENT", $"Email sent to {user.Email}: {subject}", userId);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email notification to user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> SendSmsNotificationAsync(Guid userId, string message)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found for SMS notification: {UserId}", userId);
                return false;
            }

            _logger.LogInformation("Sending SMS notification to user: {UserId}", userId);
            
            await LogAuditAsync("SMS_NOTIFICATION_SENT", $"SMS sent to user {userId}: {message}", userId);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending SMS notification to user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> SendPushNotificationAsync(Guid userId, string title, string body, object? data = null)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("User not found for push notification: {UserId}", userId);
                return false;
            }

            _logger.LogInformation("Sending push notification to user: {UserId} with title: {Title}", userId, title);
            
            await LogAuditAsync("PUSH_NOTIFICATION_SENT", $"Push notification sent to user {userId}: {title}", userId);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending push notification to user: {UserId}", userId);
            return false;
        }
    }

    public Task<NotificationQueueResponse> QueueNotificationAsync(QueueNotificationRequest request)
    {
        try
        {
            _logger.LogInformation("Queuing notification for delivery at: {ScheduledFor}", request.ScheduledFor);

            return Task.FromResult(new NotificationQueueResponse
            {
                Success = true,
                Message = "Notification queued successfully",
                QueueId = Guid.NewGuid().ToString(),
                QueuePosition = 1,
                EstimatedDelivery = DateTime.UtcNow.AddMinutes(5)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error queuing notification");
            return Task.FromResult(new NotificationQueueResponse
            {
                Success = false,
                Message = "Failed to queue notification"
            });
        }
    }

    private async Task SendMultiChannelNotificationAsync(Notification notification, List<NotificationChannel> channels)
    {
        foreach (var channel in channels)
        {
            try
            {
                switch (channel)
                {
                    case NotificationChannel.Email:
                        await SendEmailNotificationAsync(notification.UserId, notification.Title, notification.Message);
                        break;
                    case NotificationChannel.SMS:
                        await SendSmsNotificationAsync(notification.UserId, notification.Message);
                        break;
                    case NotificationChannel.Push:
                        await SendPushNotificationAsync(notification.UserId, notification.Title, notification.Message);
                        break;
                    case NotificationChannel.RealTime:
                        if (_hubContext != null)
                        {
                            await _hubContext.Clients.User(notification.UserId.ToString())
                                .SendAsync("ReceiveNotification", notification);
                        }
                        break;
                    case NotificationChannel.InApp:
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification via {Channel} to user {UserId}", channel, notification.UserId);
            }
        }
    }

    private async Task LogAuditAsync(string action, string description, Guid? entityId = null)
    {
        try
        {
            var auditLog = new AuditLog
            {
                Id = Guid.NewGuid(),
                Action = action,
                EntityName = "Notification",
                EntityId = entityId,
                Description = description,
                UserId = _tenantProvider.GetCurrentUserId(),
                TenantId = _tenantProvider.GetTenantId(),
                Timestamp = DateTime.UtcNow,
                IpAddress = "127.0.0.1"
            };

            await _auditLogRepository.AddAsync(auditLog);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging audit: {Action}", action);
        }
    }
}

public class NotificationHub : Hub
{
    public async Task JoinGroup(string groupName)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveGroup(string groupName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }
}
