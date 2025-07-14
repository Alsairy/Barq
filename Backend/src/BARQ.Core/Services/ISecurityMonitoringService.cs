using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Services;

public interface ISecurityMonitoringService
{
    Task<SecurityEventDto> LogSecurityEventAsync(string eventType, string description, string? userId = null, string? ipAddress = null);
    Task<IEnumerable<SecurityEventDto>> GetSecurityEventsAsync(DateTime? fromDate = null, DateTime? toDate = null, string? eventType = null);
    Task<ThreatDetectionResultDto> AnalyzeThreatAsync(string eventData, string eventType);
    Task<bool> IsAnomalousActivityAsync(string userId, string activityType, string? metadata = null);
    Task<SecurityDashboardDto> GetSecurityDashboardAsync(DateTime? fromDate = null, DateTime? toDate = null);
    Task<IEnumerable<SecurityAlertDto>> GetActiveAlertsAsync();
    Task<bool> CreateSecurityAlertAsync(string alertType, string message, string severity, string? userId = null);
    Task<bool> ResolveSecurityAlertAsync(Guid alertId, string resolution);
    Task<SecurityMetricsDto> GetSecurityMetricsAsync(DateTime fromDate, DateTime toDate);
    Task<bool> EnableRealTimeMonitoringAsync();
    Task<bool> DisableRealTimeMonitoringAsync();
}
