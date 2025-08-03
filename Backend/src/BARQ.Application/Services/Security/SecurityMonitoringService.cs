using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using BARQ.Core.Services;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Repositories;
using BARQ.Core.Entities;

namespace BARQ.Application.Services.Security;

public class SecurityMonitoringService : ISecurityMonitoringService
{
    private readonly IRepository<AuditLog> _auditRepository;
    private readonly IThreatDetectionService _threatDetectionService;
    private readonly ISiemIntegrationService _siemIntegrationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SecurityMonitoringService> _logger;
    private readonly List<SecurityEventDto> _recentEvents;
    private readonly List<SecurityAlertDto> _activeAlerts;
    private bool _realTimeMonitoringEnabled;

    public SecurityMonitoringService(
        IRepository<AuditLog> auditRepository,
        IThreatDetectionService threatDetectionService,
        ISiemIntegrationService siemIntegrationService,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        ILogger<SecurityMonitoringService> logger)
    {
        _auditRepository = auditRepository;
        _threatDetectionService = threatDetectionService;
        _siemIntegrationService = siemIntegrationService;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
        _recentEvents = new List<SecurityEventDto>();
        _activeAlerts = new List<SecurityAlertDto>();
        _realTimeMonitoringEnabled = bool.Parse(configuration["Security:RealTimeMonitoring"] ?? "true");
    }

    public async Task<SecurityEventDto> LogSecurityEventAsync(string eventType, string description, string? userId = null, string? ipAddress = null)
    {
        try
        {
            var securityEvent = new SecurityEventDto
            {
                Id = Guid.NewGuid(),
                EventType = eventType,
                Description = description,
                Severity = DetermineSeverity(eventType),
                Timestamp = DateTime.UtcNow,
                UserId = userId,
                IPAddress = ipAddress,
                IsResolved = false
            };

            var auditEntry = new AuditLog
            {
                Id = securityEvent.Id,
                EntityName = "SecurityEvent",
                EntityId = securityEvent.Id,
                Action = eventType,
                OldValues = null,
                NewValues = description,
                IPAddress = ipAddress ?? "Unknown",
                UserAgent = "SecurityMonitoringService",
                UserId = string.IsNullOrEmpty(userId) ? Guid.Empty : Guid.Parse(userId),
                CreatedAt = securityEvent.Timestamp,
                TenantId = Guid.Empty
            };

            await _auditRepository.AddAsync(auditEntry);
            await _unitOfWork.SaveChangesAsync();

            _recentEvents.Add(securityEvent);
            if (_recentEvents.Count > 1000)
            {
                _recentEvents.RemoveAt(0);
            }

            var threatResult = await _threatDetectionService.AssessThreatLevelAsync(description, eventType);
            if (threatResult.RiskScore > 0.7)
            {
                await CreateSecurityAlertAsync(eventType, $"High-risk security event detected: {description}", "High", userId);
            }

            if (_realTimeMonitoringEnabled)
            {
                var siemEvent = new SiemEventDto
                {
                    Id = securityEvent.Id,
                    EventType = eventType,
                    Source = "BARQ Security Monitoring",
                    Severity = securityEvent.Severity,
                    Timestamp = securityEvent.Timestamp,
                    Message = description,
                    UserId = userId,
                    IPAddress = ipAddress
                };

                await _siemIntegrationService.SendEventToSiemAsync(siemEvent);
            }

            _logger.LogInformation("Security event logged: {EventType} - {Description}", eventType, description);
            return securityEvent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to log security event: {EventType}", eventType);
            throw;
        }
    }

    public Task<IEnumerable<SecurityEventDto>> GetSecurityEventsAsync(DateTime? fromDate = null, DateTime? toDate = null, string? eventType = null)
    {
        try
        {
            var query = _recentEvents.AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(e => e.Timestamp >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(e => e.Timestamp <= toDate.Value);

            if (!string.IsNullOrEmpty(eventType))
                query = query.Where(e => e.EventType.Equals(eventType, StringComparison.OrdinalIgnoreCase));

            return Task.FromResult<IEnumerable<SecurityEventDto>>(query.OrderByDescending(e => e.Timestamp).ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve security events");
            throw;
        }
    }

    public async Task<ThreatDetectionResultDto> AnalyzeThreatAsync(string eventData, string eventType)
    {
        try
        {
            var threatAssessment = await _threatDetectionService.AssessThreatLevelAsync(eventData, eventType);
            
            return new ThreatDetectionResultDto
            {
                IsThreat = threatAssessment.RiskScore > 0.5,
                ThreatLevel = threatAssessment.ThreatLevel,
                ConfidenceScore = threatAssessment.RiskScore,
                ThreatType = threatAssessment.ThreatCategory,
                Description = $"Threat analysis for {eventType}: {string.Join(", ", threatAssessment.RiskFactors)}",
                Indicators = threatAssessment.RiskFactors,
                RecommendedAction = threatAssessment.RecommendedAction,
                DetectedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to analyze threat for event type: {EventType}", eventType);
            throw;
        }
    }

    public async Task<bool> IsAnomalousActivityAsync(string userId, string activityType, string? metadata = null)
    {
        try
        {
            var behavioralAnalysis = await _threatDetectionService.AnalyzeUserBehaviorAsync(userId, TimeSpan.FromHours(24));
            
            if (behavioralAnalysis.IsAnomalous)
            {
                await LogSecurityEventAsync("ANOMALOUS_ACTIVITY", 
                    $"Anomalous activity detected for user {userId}: {activityType}. Patterns: {string.Join(", ", behavioralAnalysis.AnomalousPatterns)}", 
                    userId);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to check anomalous activity for user: {UserId}", userId);
            return false;
        }
    }

    public async Task<SecurityDashboardDto> GetSecurityDashboardAsync(DateTime? fromDate = null, DateTime? toDate = null)
    {
        try
        {
            fromDate ??= DateTime.UtcNow.AddDays(-7);
            toDate ??= DateTime.UtcNow;

            var events = await GetSecurityEventsAsync(fromDate, toDate);
            var alerts = _activeAlerts.Where(a => a.CreatedAt >= fromDate && a.CreatedAt <= toDate);

            var dashboard = new SecurityDashboardDto
            {
                TotalEvents = events.Count(),
                CriticalAlerts = alerts.Count(a => a.Severity == "Critical"),
                HighAlerts = alerts.Count(a => a.Severity == "High"),
                MediumAlerts = alerts.Count(a => a.Severity == "Medium"),
                LowAlerts = alerts.Count(a => a.Severity == "Low"),
                ResolvedAlerts = alerts.Count(a => !a.IsActive),
                ActiveThreats = alerts.Count(a => a.IsActive && (a.Severity == "Critical" || a.Severity == "High")),
                ThreatScore = CalculateThreatScore(events, alerts),
                RecentEvents = events.Take(10),
                ThreatTrends = GenerateThreatTrends(events),
                GeneratedAt = DateTime.UtcNow
            };

            return dashboard;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate security dashboard");
            throw;
        }
    }

    public Task<IEnumerable<SecurityAlertDto>> GetActiveAlertsAsync()
    {
        try
        {
            return Task.FromResult<IEnumerable<SecurityAlertDto>>(_activeAlerts.Where(a => a.IsActive).OrderByDescending(a => a.CreatedAt));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve active alerts");
            throw;
        }
    }

    public async Task<bool> CreateSecurityAlertAsync(string alertType, string message, string severity, string? userId = null)
    {
        try
        {
            var alert = new SecurityAlertDto
            {
                Id = Guid.NewGuid(),
                AlertType = alertType,
                Message = message,
                Severity = severity,
                CreatedAt = DateTime.UtcNow,
                UserId = userId,
                IsActive = true
            };

            _activeAlerts.Add(alert);

            await LogSecurityEventAsync("SECURITY_ALERT_CREATED", 
                $"Security alert created: {alertType} - {message}", userId);

            _logger.LogWarning("Security alert created: {AlertType} - {Message}", alertType, message);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create security alert: {AlertType}", alertType);
            return false;
        }
    }

    public async Task<bool> ResolveSecurityAlertAsync(Guid alertId, string resolution)
    {
        try
        {
            var alert = _activeAlerts.FirstOrDefault(a => a.Id == alertId);
            if (alert == null)
                return false;

            alert.IsActive = false;
            alert.Resolution = resolution;
            alert.ResolvedAt = DateTime.UtcNow;

            await LogSecurityEventAsync("SECURITY_ALERT_RESOLVED", 
                $"Security alert resolved: {alert.AlertType} - {resolution}");

            _logger.LogInformation("Security alert resolved: {AlertId} - {Resolution}", alertId, resolution);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to resolve security alert: {AlertId}", alertId);
            return false;
        }
    }

    public async Task<SecurityMetricsDto> GetSecurityMetricsAsync(DateTime fromDate, DateTime toDate)
    {
        try
        {
            var events = await GetSecurityEventsAsync(fromDate, toDate);
            
            return new SecurityMetricsDto
            {
                TotalSecurityEvents = events.Count(),
                FailedLoginAttempts = events.Count(e => e.EventType == "FAILED_LOGIN"),
                SuccessfulLogins = events.Count(e => e.EventType == "SUCCESSFUL_LOGIN"),
                BruteForceAttempts = events.Count(e => e.EventType == "BRUTE_FORCE_ATTEMPT"),
                SuspiciousActivities = events.Count(e => e.EventType == "SUSPICIOUS_ACTIVITY"),
                DataAccessViolations = events.Count(e => e.EventType == "DATA_ACCESS_VIOLATION"),
                PrivilegeEscalationAttempts = events.Count(e => e.EventType == "PRIVILEGE_ESCALATION"),
                AverageResponseTime = CalculateAverageResponseTime(events),
                FromDate = fromDate,
                ToDate = toDate,
                Trends = GenerateSecurityTrends(events)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to generate security metrics");
            throw;
        }
    }

    public async Task<bool> EnableRealTimeMonitoringAsync()
    {
        try
        {
            _realTimeMonitoringEnabled = true;
            await LogSecurityEventAsync("REAL_TIME_MONITORING_ENABLED", "Real-time security monitoring has been enabled");
            _logger.LogInformation("Real-time security monitoring enabled");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to enable real-time monitoring");
            return false;
        }
    }

    public async Task<bool> DisableRealTimeMonitoringAsync()
    {
        try
        {
            _realTimeMonitoringEnabled = false;
            await LogSecurityEventAsync("REAL_TIME_MONITORING_DISABLED", "Real-time security monitoring has been disabled");
            _logger.LogInformation("Real-time security monitoring disabled");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to disable real-time monitoring");
            return false;
        }
    }

    private string DetermineSeverity(string eventType)
    {
        return eventType.ToUpperInvariant() switch
        {
            "BRUTE_FORCE_ATTEMPT" => "High",
            "DATA_BREACH" => "Critical",
            "PRIVILEGE_ESCALATION" => "High",
            "MALICIOUS_FILE_UPLOAD" => "High",
            "SUSPICIOUS_LOGIN" => "Medium",
            "FAILED_LOGIN" => "Low",
            "ANOMALOUS_ACTIVITY" => "Medium",
            _ => "Low"
        };
    }

    private double CalculateThreatScore(IEnumerable<SecurityEventDto> events, IEnumerable<SecurityAlertDto> alerts)
    {
        var criticalEvents = events.Count(e => e.Severity == "Critical");
        var highEvents = events.Count(e => e.Severity == "High");
        var activeAlerts = alerts.Count(a => a.IsActive);

        return Math.Min(100, (criticalEvents * 10 + highEvents * 5 + activeAlerts * 3) / 10.0);
    }

    private IEnumerable<ThreatTrendDto> GenerateThreatTrends(IEnumerable<SecurityEventDto> events)
    {
        return events
            .GroupBy(e => new { Date = e.Timestamp.Date, e.EventType, e.Severity })
            .Select(g => new ThreatTrendDto
            {
                Date = g.Key.Date,
                ThreatType = g.Key.EventType,
                Count = g.Count(),
                Severity = g.Key.Severity
            })
            .OrderBy(t => t.Date);
    }

    private double CalculateAverageResponseTime(IEnumerable<SecurityEventDto> events)
    {
        var resolvedEvents = events.Where(e => e.IsResolved && e.ResolvedAt.HasValue);
        if (!resolvedEvents.Any())
            return 0;

        return resolvedEvents.Average(e => (e.ResolvedAt!.Value - e.Timestamp).TotalMinutes);
    }

    private IEnumerable<SecurityTrendDto> GenerateSecurityTrends(IEnumerable<SecurityEventDto> events)
    {
        return events
            .GroupBy(e => new { Date = e.Timestamp.Date, MetricType = e.EventType })
            .Select(g => new SecurityTrendDto
            {
                Date = g.Key.Date,
                MetricType = g.Key.MetricType,
                Value = g.Count()
            })
            .OrderBy(t => t.Date);
    }
}
