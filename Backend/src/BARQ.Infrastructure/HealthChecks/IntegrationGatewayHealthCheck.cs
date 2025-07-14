using Microsoft.Extensions.Diagnostics.HealthChecks;
using BARQ.Core.Services.Integration;
using Microsoft.Extensions.Logging;

namespace BARQ.Infrastructure.HealthChecks;

public class IntegrationGatewayHealthCheck : IHealthCheck
{
    private readonly IIntegrationGatewayService _integrationGatewayService;
    private readonly ILogger<IntegrationGatewayHealthCheck> _logger;

    public IntegrationGatewayHealthCheck(
        IIntegrationGatewayService integrationGatewayService,
        ILogger<IntegrationGatewayHealthCheck> logger)
    {
        _integrationGatewayService = integrationGatewayService;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var isHealthy = await _integrationGatewayService.CheckHealthAsync();
            
            if (isHealthy)
            {
                return HealthCheckResult.Healthy("Integration Gateway is operational");
            }
            
            return HealthCheckResult.Degraded("Integration Gateway is experiencing issues");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Integration Gateway health check failed");
            return HealthCheckResult.Unhealthy("Integration Gateway is not responding", ex);
        }
    }
}
