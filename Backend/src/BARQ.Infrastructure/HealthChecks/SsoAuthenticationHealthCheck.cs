using Microsoft.Extensions.Diagnostics.HealthChecks;
using BARQ.Core.Services;
using Microsoft.Extensions.Logging;

namespace BARQ.Infrastructure.HealthChecks;

public class SsoAuthenticationHealthCheck : IHealthCheck
{
    private readonly ISsoAuthenticationService _ssoAuthenticationService;
    private readonly ILogger<SsoAuthenticationHealthCheck> _logger;

    public SsoAuthenticationHealthCheck(
        ISsoAuthenticationService ssoAuthenticationService,
        ILogger<SsoAuthenticationHealthCheck> logger)
    {
        _ssoAuthenticationService = ssoAuthenticationService;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var isHealthy = await _ssoAuthenticationService.CheckHealthAsync();
            
            if (isHealthy)
            {
                return HealthCheckResult.Healthy("SSO Authentication service is operational");
            }
            
            return HealthCheckResult.Degraded("SSO Authentication service is experiencing issues");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "SSO Authentication health check failed");
            return HealthCheckResult.Unhealthy("SSO Authentication service is not responding", ex);
        }
    }
}
