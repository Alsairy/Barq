using Microsoft.Extensions.Diagnostics.HealthChecks;
using BARQ.Core.Services;

namespace BARQ.Infrastructure.HealthChecks;

public class SecurityHealthCheck : IHealthCheck
{
    private readonly ISecurityMonitoringService _securityMonitoringService;
    private readonly IThreatDetectionService _threatDetectionService;
    private readonly IEncryptionService _encryptionService;

    public SecurityHealthCheck(
        ISecurityMonitoringService securityMonitoringService,
        IThreatDetectionService threatDetectionService,
        IEncryptionService encryptionService)
    {
        _securityMonitoringService = securityMonitoringService;
        _threatDetectionService = threatDetectionService;
        _encryptionService = encryptionService;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var issues = new List<string>();
            var data = new Dictionary<string, object>();

            try
            {
                var monitoringStatus = await _securityMonitoringService.GetSecurityStatusAsync();
                data["SecurityMonitoring"] = "Healthy";
                data["ActiveThreats"] = monitoringStatus.ActiveThreats?.Count ?? 0;
                data["SecurityScore"] = monitoringStatus.SecurityScore;
            }
            catch (Exception ex)
            {
                issues.Add($"Security monitoring: {ex.Message}");
                data["SecurityMonitoring"] = "Failed";
            }

            try
            {
                var threatStatus = await _threatDetectionService.GetThreatStatusAsync();
                data["ThreatDetection"] = "Healthy";
                data["ThreatLevel"] = threatStatus.CurrentThreatLevel.ToString();
                data["LastScan"] = threatStatus.LastScanTime;
            }
            catch (Exception ex)
            {
                issues.Add($"Threat detection: {ex.Message}");
                data["ThreatDetection"] = "Failed";
            }

            try
            {
                var testData = "health_check_test";
                var encrypted = await _encryptionService.EncryptAsync(testData);
                var decrypted = await _encryptionService.DecryptAsync(encrypted);
                
                if (testData != decrypted)
                {
                    issues.Add("Encryption service: Data integrity check failed");
                    data["Encryption"] = "Failed";
                }
                else
                {
                    data["Encryption"] = "Healthy";
                }
            }
            catch (Exception ex)
            {
                issues.Add($"Encryption service: {ex.Message}");
                data["Encryption"] = "Failed";
            }

            stopwatch.Stop();
            data["ResponseTime"] = stopwatch.ElapsedMilliseconds;
            data["IssuesFound"] = issues.Count;

            if (issues.Count > 2)
            {
                return HealthCheckResult.Unhealthy(
                    $"Multiple security services are failing: {string.Join(", ", issues)}",
                    data: data);
            }
            else if (issues.Count > 0)
            {
                return HealthCheckResult.Degraded(
                    $"Some security services have issues: {string.Join(", ", issues)}",
                    data: data);
            }

            return HealthCheckResult.Healthy(
                "All security services are healthy",
                data: data);
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                $"Security health check failed: {ex.Message}",
                ex,
                new Dictionary<string, object>
                {
                    ["Exception"] = ex.GetType().Name,
                    ["Message"] = ex.Message
                });
        }
    }
}
