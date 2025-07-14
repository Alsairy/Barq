using Microsoft.Extensions.Logging;
using BARQ.Core.Services.Integration;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Enums;
using BARQ.Core.Repositories;
using BARQ.Core.Services;
using System.Collections.Concurrent;

namespace BARQ.Application.Services.Integration;

public class IntegrationGatewayService : IIntegrationGatewayService
{
    private readonly ILogger<IntegrationGatewayService> _logger;
    private readonly IEnumerable<IProtocolAdapter> _protocolAdapters;
    private readonly IIntegrationMonitoringService _monitoringService;
    private readonly ITenantProvider _tenantProvider;
    private readonly ConcurrentDictionary<string, IntegrationEndpoint> _endpoints;
    private readonly ConcurrentQueue<IntegrationLog> _integrationLogs;

    public IntegrationGatewayService(
        ILogger<IntegrationGatewayService> logger,
        IEnumerable<IProtocolAdapter> protocolAdapters,
        IIntegrationMonitoringService monitoringService,
        ITenantProvider tenantProvider)
    {
        _logger = logger;
        _protocolAdapters = protocolAdapters;
        _monitoringService = monitoringService;
        _tenantProvider = tenantProvider;
        _endpoints = new ConcurrentDictionary<string, IntegrationEndpoint>();
        _integrationLogs = new ConcurrentQueue<IntegrationLog>();
    }

    public async Task<IntegrationResponse> RouteRequestAsync(IntegrationRequest request, CancellationToken cancellationToken = default)
    {
        var startTime = DateTime.UtcNow;
        var tenantId = _tenantProvider.GetTenantId();
        request.TenantId = tenantId;

        try
        {
            _logger.LogInformation("Processing integration request {RequestId} for endpoint {EndpointId}", 
                request.Id, request.EndpointId);

            if (!_endpoints.TryGetValue(request.EndpointId, out var endpoint))
            {
                var errorResponse = new IntegrationResponse
                {
                    RequestId = request.Id,
                    Success = false,
                    StatusCode = 404,
                    ErrorMessage = $"Endpoint {request.EndpointId} not found",
                    ProcessedAt = DateTime.UtcNow,
                    ProcessingTimeMs = (DateTime.UtcNow - startTime).Milliseconds
                };

                await LogIntegrationRequestAsync(request, errorResponse);
                return errorResponse;
            }

            if (endpoint.TenantId != tenantId)
            {
                var errorResponse = new IntegrationResponse
                {
                    RequestId = request.Id,
                    Success = false,
                    StatusCode = 403,
                    ErrorMessage = "Access denied to endpoint",
                    ProcessedAt = DateTime.UtcNow,
                    ProcessingTimeMs = (DateTime.UtcNow - startTime).Milliseconds
                };

                await LogIntegrationRequestAsync(request, errorResponse);
                return errorResponse;
            }

            if (!endpoint.IsActive)
            {
                var errorResponse = new IntegrationResponse
                {
                    RequestId = request.Id,
                    Success = false,
                    StatusCode = 503,
                    ErrorMessage = "Endpoint is currently inactive",
                    ProcessedAt = DateTime.UtcNow,
                    ProcessingTimeMs = (DateTime.UtcNow - startTime).Milliseconds
                };

                await LogIntegrationRequestAsync(request, errorResponse);
                return errorResponse;
            }

            var adapter = _protocolAdapters.FirstOrDefault(a => a.Protocol == endpoint.Protocol.ToString());
            if (adapter == null)
            {
                var errorResponse = new IntegrationResponse
                {
                    RequestId = request.Id,
                    Success = false,
                    StatusCode = 501,
                    ErrorMessage = $"Protocol {endpoint.Protocol} not supported",
                    ProcessedAt = DateTime.UtcNow,
                    ProcessingTimeMs = (DateTime.UtcNow - startTime).Milliseconds
                };

                await LogIntegrationRequestAsync(request, errorResponse);
                return errorResponse;
            }

            var response = await adapter.SendAsync(request, endpoint, cancellationToken);
            response.ProcessingTimeMs = (DateTime.UtcNow - startTime).Milliseconds;

            await LogIntegrationRequestAsync(request, response);

            await _monitoringService.LogIntegrationEventAsync(new IntegrationEvent
            {
                EventType = "REQUEST_PROCESSED",
                EndpointId = request.EndpointId,
                Description = $"Request {request.Id} processed with status {response.StatusCode}",
                Level = response.Success ? IntegrationEventLevel.Info : IntegrationEventLevel.Error,
                Data = new Dictionary<string, object>
                {
                    ["RequestId"] = request.Id,
                    ["StatusCode"] = response.StatusCode,
                    ["ProcessingTimeMs"] = response.ProcessingTimeMs
                },
                TenantId = tenantId
            });

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing integration request {RequestId}", request.Id);

            var errorResponse = new IntegrationResponse
            {
                RequestId = request.Id,
                Success = false,
                StatusCode = 500,
                ErrorMessage = "Internal server error",
                ProcessedAt = DateTime.UtcNow,
                ProcessingTimeMs = (DateTime.UtcNow - startTime).Milliseconds
            };

            await LogIntegrationRequestAsync(request, errorResponse);
            return errorResponse;
        }
    }

    public async Task<bool> RegisterEndpointAsync(IntegrationEndpoint endpoint)
    {
        try
        {
            var tenantId = _tenantProvider.GetTenantId();
            endpoint.TenantId = tenantId;
            endpoint.CreatedAt = DateTime.UtcNow;

            var adapter = _protocolAdapters.FirstOrDefault(a => a.Protocol == endpoint.Protocol.ToString());
            if (adapter == null)
            {
                _logger.LogWarning("Cannot register endpoint {EndpointId}: Protocol {Protocol} not supported", 
                    endpoint.Id, endpoint.Protocol);
                return false;
            }

            var isValid = await adapter.ValidateEndpointAsync(endpoint);
            if (!isValid)
            {
                _logger.LogWarning("Cannot register endpoint {EndpointId}: Validation failed", endpoint.Id);
                return false;
            }

            _endpoints.TryAdd(endpoint.Id, endpoint);

            await _monitoringService.LogIntegrationEventAsync(new IntegrationEvent
            {
                EventType = "ENDPOINT_REGISTERED",
                EndpointId = endpoint.Id,
                Description = $"Endpoint {endpoint.Name} registered successfully",
                Level = IntegrationEventLevel.Info,
                Data = new Dictionary<string, object>
                {
                    ["EndpointName"] = endpoint.Name,
                    ["Protocol"] = endpoint.Protocol.ToString(),
                    ["BaseUrl"] = endpoint.BaseUrl
                },
                TenantId = tenantId
            });

            _logger.LogInformation("Endpoint {EndpointId} registered successfully", endpoint.Id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering endpoint {EndpointId}", endpoint.Id);
            return false;
        }
    }

    public async Task<bool> UnregisterEndpointAsync(string endpointId)
    {
        try
        {
            var tenantId = _tenantProvider.GetTenantId();

            if (!_endpoints.TryGetValue(endpointId, out var endpoint))
            {
                _logger.LogWarning("Cannot unregister endpoint {EndpointId}: Not found", endpointId);
                return false;
            }

            if (endpoint.TenantId != tenantId)
            {
                _logger.LogWarning("Cannot unregister endpoint {EndpointId}: Access denied", endpointId);
                return false;
            }

            _endpoints.TryRemove(endpointId, out _);

            await _monitoringService.LogIntegrationEventAsync(new IntegrationEvent
            {
                EventType = "ENDPOINT_UNREGISTERED",
                EndpointId = endpointId,
                Description = $"Endpoint {endpoint.Name} unregistered",
                Level = IntegrationEventLevel.Info,
                Data = new Dictionary<string, object>
                {
                    ["EndpointName"] = endpoint.Name
                },
                TenantId = tenantId
            });

            _logger.LogInformation("Endpoint {EndpointId} unregistered successfully", endpointId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error unregistering endpoint {EndpointId}", endpointId);
            return false;
        }
    }

    public async Task<IEnumerable<IntegrationEndpoint>> GetRegisteredEndpointsAsync()
    {
        try
        {
            var tenantId = _tenantProvider.GetTenantId();
            var tenantEndpoints = _endpoints.Values.Where(e => e.TenantId == tenantId).ToList();

            _logger.LogDebug("Retrieved {Count} endpoints for tenant {TenantId}", 
                tenantEndpoints.Count, tenantId);

            return await Task.FromResult(tenantEndpoints);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving registered endpoints");
            return new List<IntegrationEndpoint>();
        }
    }

    public async Task<IntegrationHealthStatus> CheckEndpointHealthAsync(string endpointId)
    {
        try
        {
            var tenantId = _tenantProvider.GetTenantId();

            if (!_endpoints.TryGetValue(endpointId, out var endpoint))
            {
                return new IntegrationHealthStatus
                {
                    EndpointId = endpointId,
                    IsHealthy = false,
                    Status = "Not Found",
                    ErrorMessage = "Endpoint not found"
                };
            }

            if (endpoint.TenantId != tenantId)
            {
                return new IntegrationHealthStatus
                {
                    EndpointId = endpointId,
                    IsHealthy = false,
                    Status = "Access Denied",
                    ErrorMessage = "Access denied to endpoint"
                };
            }

            var adapter = _protocolAdapters.FirstOrDefault(a => a.Protocol == endpoint.Protocol.ToString());
            if (adapter == null)
            {
                return new IntegrationHealthStatus
                {
                    EndpointId = endpointId,
                    IsHealthy = false,
                    Status = "Protocol Not Supported",
                    ErrorMessage = $"Protocol {endpoint.Protocol} not supported"
                };
            }

            var healthStatus = await adapter.CheckHealthAsync(endpoint);
            endpoint.LastHealthCheck = DateTime.UtcNow;
            endpoint.IsHealthy = healthStatus.IsHealthy;

            return healthStatus;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking endpoint health {EndpointId}", endpointId);
            return new IntegrationHealthStatus
            {
                EndpointId = endpointId,
                IsHealthy = false,
                Status = "Health Check Failed",
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<IEnumerable<IntegrationLog>> GetIntegrationLogsAsync(DateTime? fromDate = null, DateTime? toDate = null)
    {
        try
        {
            var tenantId = _tenantProvider.GetTenantId();
            var logs = _integrationLogs.Where(l => l.TenantId == tenantId);

            if (fromDate.HasValue)
                logs = logs.Where(l => l.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                logs = logs.Where(l => l.CreatedAt <= toDate.Value);

            return await Task.FromResult(logs.OrderByDescending(l => l.CreatedAt).Take(1000).ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving integration logs");
            return new List<IntegrationLog>();
        }
    }

    private async Task LogIntegrationRequestAsync(IntegrationRequest request, IntegrationResponse response)
    {
        try
        {
            var log = new IntegrationLog
            {
                RequestId = request.Id,
                EndpointId = request.EndpointId,
                Method = request.Method,
                Path = request.Path,
                StatusCode = response.StatusCode,
                Success = response.Success,
                ProcessingTimeMs = response.ProcessingTimeMs,
                ErrorMessage = response.ErrorMessage,
                TenantId = request.TenantId
            };

            _integrationLogs.Enqueue(log);

            while (_integrationLogs.Count > 10000)
            {
                _integrationLogs.TryDequeue(out _);
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error logging integration request");
        }
    }
}
