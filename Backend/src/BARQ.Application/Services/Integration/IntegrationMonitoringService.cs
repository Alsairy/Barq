using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using BARQ.Core.Services.Integration;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Enums;
using BARQ.Core.Services;

namespace BARQ.Application.Services.Integration;

public class IntegrationMonitoringService : IIntegrationMonitoringService
{
    private readonly ILogger<IntegrationMonitoringService> _logger;
    private readonly ITenantProvider _tenantProvider;
    private readonly ConcurrentQueue<IntegrationEvent> _events;
    private readonly ConcurrentDictionary<string, IntegrationAlert> _activeAlerts;
    private readonly ConcurrentDictionary<string, IntegrationAlertRule> _alertRules;
    private readonly Timer _metricsTimer;
    private readonly Timer _alertProcessingTimer;

    public IntegrationMonitoringService(
        ILogger<IntegrationMonitoringService> logger,
        ITenantProvider tenantProvider)
    {
        _logger = logger;
        _tenantProvider = tenantProvider;
        _events = new ConcurrentQueue<IntegrationEvent>();
        _activeAlerts = new ConcurrentDictionary<string, IntegrationAlert>();
        _alertRules = new ConcurrentDictionary<string, IntegrationAlertRule>();
        
        _metricsTimer = new Timer(ProcessMetrics, null, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
        _alertProcessingTimer = new Timer(ProcessAlerts, null, TimeSpan.FromSeconds(30), TimeSpan.FromSeconds(30));
        
        InitializeDefaultAlertRules();
    }

    public async Task LogIntegrationEventAsync(IntegrationEvent integrationEvent)
    {
        try
        {
            integrationEvent.CreatedAt = DateTime.UtcNow;
            if (string.IsNullOrEmpty(integrationEvent.Id))
            {
                integrationEvent.Id = Guid.NewGuid().ToString();
            }

            _events.Enqueue(integrationEvent);

            while (_events.Count > 10000)
            {
                _events.TryDequeue(out _);
            }

            _logger.LogInformation("Integration event logged: {EventType} for endpoint {EndpointId}", 
                integrationEvent.EventType, integrationEvent.EndpointId);

            await EvaluateAlertRulesAsync(integrationEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging integration event {EventId}", integrationEvent.Id);
        }
    }

    public async Task<IntegrationMetrics> GetMetricsAsync(DateTime fromDate, DateTime toDate)
    {
        try
        {
            var tenantId = _tenantProvider.GetTenantId();
            var tenantEvents = _events.Where(e => e.TenantId == tenantId && 
                                                 e.CreatedAt >= fromDate && 
                                                 e.CreatedAt <= toDate).ToList();

            var totalRequests = tenantEvents.Count(e => e.EventType == "REQUEST_PROCESSED");
            var successfulRequests = tenantEvents.Count(e => e.EventType == "REQUEST_PROCESSED" && 
                                                            e.Level == IntegrationEventLevel.Info);
            var failedRequests = totalRequests - successfulRequests;

            var endpointUsage = tenantEvents
                .Where(e => e.EventType == "REQUEST_PROCESSED")
                .GroupBy(e => e.EndpointId)
                .ToDictionary(g => g.Key, g => g.Count());

            var errorCounts = tenantEvents
                .Where(e => e.Level == IntegrationEventLevel.Error)
                .GroupBy(e => e.EventType)
                .ToDictionary(g => g.Key, g => g.Count());

            var responseTimeEvents = tenantEvents
                .Where(e => e.EventType == "REQUEST_PROCESSED" && e.Data.ContainsKey("ProcessingTimeMs"))
                .ToList();

            var averageResponseTime = responseTimeEvents.Any() 
                ? responseTimeEvents.Average(e => Convert.ToDouble(e.Data["ProcessingTimeMs"])) 
                : 0.0;

            var metrics = new IntegrationMetrics
            {
                TotalRequests = totalRequests,
                SuccessfulRequests = successfulRequests,
                FailedRequests = failedRequests,
                SuccessRate = totalRequests > 0 ? (double)successfulRequests / totalRequests * 100 : 0,
                AverageResponseTime = averageResponseTime,
                EndpointUsage = endpointUsage,
                ErrorCounts = errorCounts,
                FromDate = fromDate,
                ToDate = toDate
            };

            _logger.LogInformation("Generated integration metrics for tenant {TenantId}: {TotalRequests} total requests, {SuccessRate}% success rate", 
                tenantId, totalRequests, metrics.SuccessRate);

            return await Task.FromResult(metrics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating integration metrics");
            return new IntegrationMetrics
            {
                FromDate = fromDate,
                ToDate = toDate
            };
        }
    }

    public async Task<IEnumerable<IntegrationAlert>> GetActiveAlertsAsync()
    {
        try
        {
            var tenantId = _tenantProvider.GetTenantId();
            var tenantAlerts = _activeAlerts.Values
                .Where(a => !a.IsResolved && 
                           _events.Any(e => e.TenantId == tenantId && e.EndpointId == a.EndpointId))
                .OrderByDescending(a => a.CreatedAt)
                .ToList();

            return await Task.FromResult(tenantAlerts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active alerts");
            return new List<IntegrationAlert>();
        }
    }

    public async Task<bool> CreateAlertRuleAsync(IntegrationAlertRule rule)
    {
        try
        {
            if (string.IsNullOrEmpty(rule.Id))
            {
                rule.Id = Guid.NewGuid().ToString();
            }

            rule.CreatedAt = DateTime.UtcNow;
            _alertRules.TryAdd(rule.Id, rule);

            _logger.LogInformation("Alert rule created: {RuleName} with condition {Condition}", 
                rule.Name, rule.Condition);

            return await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating alert rule {RuleName}", rule.Name);
            return false;
        }
    }

    public async Task<IntegrationHealthDashboard> GetHealthDashboardAsync()
    {
        try
        {
            var tenantId = _tenantProvider.GetTenantId();
            var recentEvents = _events.Where(e => e.TenantId == tenantId && 
                                                 e.CreatedAt >= DateTime.UtcNow.AddHours(-1)).ToList();

            var endpointHealthMap = recentEvents
                .Where(e => e.EventType == "REQUEST_PROCESSED")
                .GroupBy(e => e.EndpointId)
                .ToDictionary(g => g.Key, g => new
                {
                    Total = g.Count(),
                    Successful = g.Count(e => e.Level == IntegrationEventLevel.Info),
                    LastActivity = g.Max(e => e.CreatedAt)
                });

            var totalEndpoints = endpointHealthMap.Count;
            var healthyEndpoints = endpointHealthMap.Count(kvp => 
                kvp.Value.Total > 0 && (double)kvp.Value.Successful / kvp.Value.Total >= 0.95);
            var unhealthyEndpoints = totalEndpoints - healthyEndpoints;

            var activeAlerts = await GetActiveAlertsAsync();
            var criticalAlerts = activeAlerts.Where(a => a.Severity == IntegrationAlertSeverity.Critical);

            var recentlyFailedEndpoints = endpointHealthMap
                .Where(kvp => kvp.Value.Total > 0 && (double)kvp.Value.Successful / kvp.Value.Total < 0.5)
                .Select(kvp => new IntegrationEndpoint
                {
                    Id = kvp.Key,
                    Name = kvp.Key,
                    IsHealthy = false,
                    LastHealthCheck = kvp.Value.LastActivity
                })
                .ToList();

            var overallHealthScore = totalEndpoints > 0 ? (double)healthyEndpoints / totalEndpoints * 100 : 100;

            var dashboard = new IntegrationHealthDashboard
            {
                TotalEndpoints = totalEndpoints,
                HealthyEndpoints = healthyEndpoints,
                UnhealthyEndpoints = unhealthyEndpoints,
                ActiveAlerts = activeAlerts.Count(),
                OverallHealthScore = overallHealthScore,
                RecentlyFailedEndpoints = recentlyFailedEndpoints,
                CriticalAlerts = criticalAlerts,
                LastUpdated = DateTime.UtcNow
            };

            _logger.LogInformation("Generated health dashboard: {HealthyEndpoints}/{TotalEndpoints} healthy endpoints, {ActiveAlerts} active alerts", 
                healthyEndpoints, totalEndpoints, activeAlerts.Count());

            return dashboard;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating health dashboard");
            return new IntegrationHealthDashboard
            {
                LastUpdated = DateTime.UtcNow
            };
        }
    }

    private void InitializeDefaultAlertRules()
    {
        var defaultRules = new[]
        {
            new IntegrationAlertRule
            {
                Id = "high-error-rate",
                Name = "High Error Rate",
                Description = "Alert when error rate exceeds 10% in 5 minutes",
                Condition = "error_rate > 0.1 AND time_window = 5m",
                Severity = IntegrationAlertSeverity.High,
                IsActive = true,
                Parameters = new Dictionary<string, object>
                {
                    ["threshold"] = 0.1,
                    ["time_window_minutes"] = 5
                }
            },
            new IntegrationAlertRule
            {
                Id = "slow-response-time",
                Name = "Slow Response Time",
                Description = "Alert when average response time exceeds 5 seconds",
                Condition = "avg_response_time > 5000ms",
                Severity = IntegrationAlertSeverity.Medium,
                IsActive = true,
                Parameters = new Dictionary<string, object>
                {
                    ["threshold_ms"] = 5000
                }
            },
            new IntegrationAlertRule
            {
                Id = "endpoint-down",
                Name = "Endpoint Down",
                Description = "Alert when endpoint is completely unavailable",
                Condition = "success_rate = 0 AND request_count > 0",
                Severity = IntegrationAlertSeverity.Critical,
                IsActive = true,
                Parameters = new Dictionary<string, object>
                {
                    ["min_requests"] = 1
                }
            }
        };

        foreach (var rule in defaultRules)
        {
            _alertRules.TryAdd(rule.Id, rule);
        }
    }

    private async Task EvaluateAlertRulesAsync(IntegrationEvent integrationEvent)
    {
        try
        {
            foreach (var rule in _alertRules.Values.Where(r => r.IsActive))
            {
                var shouldAlert = await EvaluateRuleCondition(rule, integrationEvent);
                
                if (shouldAlert)
                {
                    await CreateAlertAsync(rule, integrationEvent);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error evaluating alert rules for event {EventId}", integrationEvent.Id);
        }
    }

    private async Task<bool> EvaluateRuleCondition(IntegrationAlertRule rule, IntegrationEvent integrationEvent)
    {
        try
        {
            switch (rule.Id)
            {
                case "high-error-rate":
                    return await EvaluateHighErrorRate(rule, integrationEvent);
                case "slow-response-time":
                    return await EvaluateSlowResponseTime(rule, integrationEvent);
                case "endpoint-down":
                    return await EvaluateEndpointDown(rule, integrationEvent);
                default:
                    return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error evaluating rule condition for rule {RuleId}", rule.Id);
            return false;
        }
    }

    private async Task<bool> EvaluateHighErrorRate(IntegrationAlertRule rule, IntegrationEvent integrationEvent)
    {
        if (integrationEvent.Level != IntegrationEventLevel.Error)
            return false;

        var timeWindow = TimeSpan.FromMinutes((int)rule.Parameters["time_window_minutes"]);
        var threshold = (double)rule.Parameters["threshold"];
        var cutoffTime = DateTime.UtcNow.Subtract(timeWindow);

        var recentEvents = _events.Where(e => e.EndpointId == integrationEvent.EndpointId && 
                                             e.CreatedAt >= cutoffTime &&
                                             e.EventType == "REQUEST_PROCESSED").ToList();

        if (recentEvents.Count == 0)
            return false;

        var errorCount = recentEvents.Count(e => e.Level == IntegrationEventLevel.Error);
        var errorRate = (double)errorCount / recentEvents.Count;

        return await Task.FromResult(errorRate > threshold);
    }

    private async Task<bool> EvaluateSlowResponseTime(IntegrationAlertRule rule, IntegrationEvent integrationEvent)
    {
        if (integrationEvent.EventType != "REQUEST_PROCESSED" || 
            !integrationEvent.Data.ContainsKey("ProcessingTimeMs"))
            return false;

        var threshold = (int)rule.Parameters["threshold_ms"];
        var responseTime = Convert.ToDouble(integrationEvent.Data["ProcessingTimeMs"]);

        return await Task.FromResult(responseTime > threshold);
    }

    private async Task<bool> EvaluateEndpointDown(IntegrationAlertRule rule, IntegrationEvent integrationEvent)
    {
        if (integrationEvent.EventType != "REQUEST_PROCESSED")
            return false;

        var minRequests = (int)rule.Parameters["min_requests"];
        var recentEvents = _events.Where(e => e.EndpointId == integrationEvent.EndpointId && 
                                             e.CreatedAt >= DateTime.UtcNow.AddMinutes(-5) &&
                                             e.EventType == "REQUEST_PROCESSED").ToList();

        if (recentEvents.Count < minRequests)
            return false;

        var successfulRequests = recentEvents.Count(e => e.Level == IntegrationEventLevel.Info);
        return await Task.FromResult(successfulRequests == 0);
    }

    private async Task CreateAlertAsync(IntegrationAlertRule rule, IntegrationEvent integrationEvent)
    {
        try
        {
            var alertKey = $"{rule.Id}:{integrationEvent.EndpointId}";
            
            if (_activeAlerts.ContainsKey(alertKey))
                return;

            var alert = new IntegrationAlert
            {
                Id = Guid.NewGuid().ToString(),
                RuleId = rule.Id,
                Title = rule.Name,
                Description = $"{rule.Description} - Endpoint: {integrationEvent.EndpointId}",
                Severity = rule.Severity,
                EndpointId = integrationEvent.EndpointId,
                CreatedAt = DateTime.UtcNow,
                IsResolved = false,
                Data = new Dictionary<string, object>
                {
                    ["TriggerEventId"] = integrationEvent.Id,
                    ["RuleName"] = rule.Name,
                    ["EndpointId"] = integrationEvent.EndpointId
                }
            };

            _activeAlerts.TryAdd(alertKey, alert);

            _logger.LogWarning("Integration alert created: {AlertTitle} for endpoint {EndpointId} with severity {Severity}", 
                alert.Title, alert.EndpointId, alert.Severity);

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating alert for rule {RuleId}", rule.Id);
        }
    }

    private async void ProcessMetrics(object? state)
    {
        try
        {
            var now = DateTime.UtcNow;
            var oneHourAgo = now.AddHours(-1);
            
            var metrics = await GetMetricsAsync(oneHourAgo, now);
            
            _logger.LogInformation("Processed integration metrics: {TotalRequests} requests, {SuccessRate}% success rate", 
                metrics.TotalRequests, metrics.SuccessRate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in background metrics processing");
        }
    }

    private void ProcessAlerts(object? state)
    {
        try
        {
            var expiredAlerts = _activeAlerts.Values
                .Where(a => !a.IsResolved && a.CreatedAt < DateTime.UtcNow.AddHours(-24))
                .ToList();

            foreach (var alert in expiredAlerts)
            {
                alert.IsResolved = true;
                alert.ResolvedAt = DateTime.UtcNow;
                
                _logger.LogInformation("Auto-resolved expired alert: {AlertId}", alert.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in background alert processing");
        }
    }

    public void Dispose()
    {
        _metricsTimer?.Dispose();
        _alertProcessingTimer?.Dispose();
    }
}
