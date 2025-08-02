using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface ISecurityMonitoringService
{
    /// <summary>
    /// </summary>
    Task<SecurityEventDto> LogSecurityEventAsync(string eventType, string description, string? userId = null, string? ipAddress = null);

    /// <summary>
    /// </summary>
    Task<IEnumerable<SecurityEventDto>> GetSecurityEventsAsync(DateTime? fromDate = null, DateTime? toDate = null, string? eventType = null);

    /// <summary>
    /// </summary>
    Task<ThreatDetectionResultDto> AnalyzeThreatAsync(string eventData, string eventType);

    /// <summary>
    /// </summary>
    Task<bool> IsAnomalousActivityAsync(string userId, string activityType, string? metadata = null);

    /// <summary>
    /// </summary>
    Task<SecurityDashboardDto> GetSecurityDashboardAsync(DateTime? fromDate = null, DateTime? toDate = null);

    /// <summary>
    /// </summary>
    Task<IEnumerable<SecurityAlertDto>> GetActiveAlertsAsync();

    /// <summary>
    /// </summary>
    Task<bool> CreateSecurityAlertAsync(string alertType, string message, string severity, string? userId = null);

    /// <summary>
    /// </summary>
    Task<bool> ResolveSecurityAlertAsync(Guid alertId, string resolution);

    /// <summary>
    /// </summary>
    Task<SecurityMetricsDto> GetSecurityMetricsAsync(DateTime fromDate, DateTime toDate);

    /// <summary>
    /// </summary>
    Task<bool> EnableRealTimeMonitoringAsync();

    /// <summary>
    /// </summary>
    Task<bool> DisableRealTimeMonitoringAsync();
}
