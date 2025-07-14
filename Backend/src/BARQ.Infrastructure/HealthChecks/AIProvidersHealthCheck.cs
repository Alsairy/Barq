using Microsoft.Extensions.Diagnostics.HealthChecks;
using BARQ.Core.Services;

namespace BARQ.Infrastructure.HealthChecks;

public class AIProvidersHealthCheck : IHealthCheck
{
    private readonly IAIOrchestrationService _aiOrchestrationService;

    public AIProvidersHealthCheck(IAIOrchestrationService aiOrchestrationService)
    {
        _aiOrchestrationService = aiOrchestrationService;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            var data = new Dictionary<string, object>();
            var issues = new List<string>();

            var providers = await _aiOrchestrationService.GetAvailableProvidersAsync();
            data["TotalProviders"] = providers.Count();

            var healthyProviders = 0;
            var degradedProviders = 0;
            var unhealthyProviders = 0;

            foreach (var provider in providers)
            {
                try
                {
                    var healthResult = await _aiOrchestrationService.CheckProviderHealthAsync(provider.Id);
                    
                    switch (healthResult.Status)
                    {
                        case "Healthy":
                            healthyProviders++;
                            break;
                        case "Degraded":
                            degradedProviders++;
                            issues.Add($"{provider.Name}: Degraded performance");
                            break;
                        case "Unhealthy":
                            unhealthyProviders++;
                            issues.Add($"{provider.Name}: Unhealthy");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    unhealthyProviders++;
                    issues.Add($"{provider.Name}: {ex.Message}");
                }
            }

            stopwatch.Stop();

            data["HealthyProviders"] = healthyProviders;
            data["DegradedProviders"] = degradedProviders;
            data["UnhealthyProviders"] = unhealthyProviders;
            data["ResponseTime"] = stopwatch.ElapsedMilliseconds;
            data["IssuesFound"] = issues.Count;

            if (healthyProviders == 0)
            {
                return HealthCheckResult.Unhealthy(
                    "No AI providers are healthy",
                    data: data);
            }
            else if (unhealthyProviders > healthyProviders)
            {
                return HealthCheckResult.Degraded(
                    $"More providers are unhealthy ({unhealthyProviders}) than healthy ({healthyProviders}). Issues: {string.Join(", ", issues)}",
                    data: data);
            }
            else if (issues.Count > 0)
            {
                return HealthCheckResult.Degraded(
                    $"Some AI providers have issues: {string.Join(", ", issues)}",
                    data: data);
            }

            return HealthCheckResult.Healthy(
                $"AI providers are healthy ({healthyProviders} healthy, {degradedProviders} degraded, {unhealthyProviders} unhealthy)",
                data: data);
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy(
                $"AI providers health check failed: {ex.Message}",
                ex,
                new Dictionary<string, object>
                {
                    ["Exception"] = ex.GetType().Name,
                    ["Message"] = ex.Message
                });
        }
    }
}
