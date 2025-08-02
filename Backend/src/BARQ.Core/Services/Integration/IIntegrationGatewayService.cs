using BARQ.Core.Models.DTOs;
using BARQ.Core.Enums;

namespace BARQ.Core.Services.Integration;

/// <summary>
/// </summary>
public interface IIntegrationGatewayService
{
    /// <summary>
    /// </summary>
    Task<IntegrationResponse> RouteRequestAsync(IntegrationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    Task<bool> RegisterEndpointAsync(IntegrationEndpoint endpoint);

    /// <summary>
    /// </summary>
    Task<bool> UnregisterEndpointAsync(string endpointId);

    /// <summary>
    /// </summary>
    Task<IEnumerable<IntegrationEndpoint>> GetRegisteredEndpointsAsync();

    /// <summary>
    /// </summary>
    Task<IntegrationHealthStatus> CheckEndpointHealthAsync(string endpointId);

    /// <summary>
    /// </summary>
    Task<IEnumerable<IntegrationLog>> GetIntegrationLogsAsync(DateTime? fromDate = null, DateTime? toDate = null);

    /// <summary>
    /// </summary>
    Task<bool> CheckHealthAsync();
}

/// <summary>
/// </summary>
public interface IProtocolAdapter
{
    /// <summary>
    /// </summary>
    string Protocol { get; }

    /// <summary>
    /// </summary>
    Task<IntegrationResponse> SendAsync(IntegrationRequest request, IntegrationEndpoint endpoint, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    Task<bool> ValidateEndpointAsync(IntegrationEndpoint endpoint);

    /// <summary>
    /// </summary>
    Task<IntegrationHealthStatus> CheckHealthAsync(IntegrationEndpoint endpoint);
}

/// <summary>
/// </summary>
public interface IMessageOrchestrationService
{
    /// <summary>
    /// </summary>
    Task<string> EnqueueMessageAsync(IntegrationMessage message, MessagePriority priority = MessagePriority.Normal);

    /// <summary>
    /// </summary>
    Task<IntegrationMessage?> DequeueMessageAsync(string queueName, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    Task<bool> ProcessMessageAsync(IntegrationMessage message);

    /// <summary>
    /// </summary>
    Task<MessageTransformationResult> TransformMessageAsync(IntegrationMessage message, string targetFormat);

    /// <summary>
    /// </summary>
    Task<IEnumerable<QueueStatus>> GetQueueStatusAsync();

    /// <summary>
    /// </summary>
    Task<bool> RetryFailedMessageAsync(string messageId);
}

/// <summary>
/// </summary>
public interface IIntegrationMonitoringService
{
    /// <summary>
    /// </summary>
    Task LogIntegrationEventAsync(IntegrationEvent integrationEvent);

    /// <summary>
    /// </summary>
    Task<IntegrationMetrics> GetMetricsAsync(DateTime fromDate, DateTime toDate);

    /// <summary>
    /// </summary>
    Task<IEnumerable<IntegrationAlert>> GetActiveAlertsAsync();

    /// <summary>
    /// </summary>
    Task<bool> CreateAlertRuleAsync(IntegrationAlertRule rule);

    /// <summary>
    /// </summary>
    Task<IntegrationHealthDashboard> GetHealthDashboardAsync();
}
