using BARQ.Core.Models.DTOs;
using BARQ.Core.Enums;

namespace BARQ.Core.Services.Integration;

public interface IIntegrationGatewayService
{
    Task<IntegrationResponse> RouteRequestAsync(IntegrationRequest request, CancellationToken cancellationToken = default);
    Task<bool> RegisterEndpointAsync(IntegrationEndpoint endpoint);
    Task<bool> UnregisterEndpointAsync(string endpointId);
    Task<IEnumerable<IntegrationEndpoint>> GetRegisteredEndpointsAsync();
    Task<IntegrationHealthStatus> CheckEndpointHealthAsync(string endpointId);
    Task<IEnumerable<IntegrationLog>> GetIntegrationLogsAsync(DateTime? fromDate = null, DateTime? toDate = null);
    Task<bool> CheckHealthAsync();
}

public interface IProtocolAdapter
{
    string Protocol { get; }
    Task<IntegrationResponse> SendAsync(IntegrationRequest request, IntegrationEndpoint endpoint, CancellationToken cancellationToken = default);
    Task<bool> ValidateEndpointAsync(IntegrationEndpoint endpoint);
    Task<IntegrationHealthStatus> CheckHealthAsync(IntegrationEndpoint endpoint);
}

public interface IMessageOrchestrationService
{
    Task<string> EnqueueMessageAsync(IntegrationMessage message, MessagePriority priority = MessagePriority.Normal);
    Task<IntegrationMessage?> DequeueMessageAsync(string queueName, CancellationToken cancellationToken = default);
    Task<bool> ProcessMessageAsync(IntegrationMessage message);
    Task<MessageTransformationResult> TransformMessageAsync(IntegrationMessage message, string targetFormat);
    Task<IEnumerable<QueueStatus>> GetQueueStatusAsync();
    Task<bool> RetryFailedMessageAsync(string messageId);
}

public interface IIntegrationMonitoringService
{
    Task LogIntegrationEventAsync(IntegrationEvent integrationEvent);
    Task<IntegrationMetrics> GetMetricsAsync(DateTime fromDate, DateTime toDate);
    Task<IEnumerable<IntegrationAlert>> GetActiveAlertsAsync();
    Task<bool> CreateAlertRuleAsync(IntegrationAlertRule rule);
    Task<IntegrationHealthDashboard> GetHealthDashboardAsync();
}
