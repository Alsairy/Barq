using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using BARQ.Core.Services.Integration;
using BARQ.Core.Models.DTOs;

namespace BARQ.Infrastructure.Integration.Adapters;

public class GraphQLProtocolAdapter : IProtocolAdapter
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<GraphQLProtocolAdapter> _logger;

    public string Protocol => "GraphQL";

    public GraphQLProtocolAdapter(HttpClient httpClient, ILogger<GraphQLProtocolAdapter> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IntegrationResponse> SendAsync(IntegrationRequest request, IntegrationEndpoint endpoint, CancellationToken cancellationToken = default)
    {
        var startTime = DateTime.UtcNow;

        try
        {
            var graphqlRequest = CreateGraphQLRequest(request);
            var jsonContent = JsonSerializer.Serialize(graphqlRequest);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpoint.BaseUrl)
            {
                Content = content
            };

            foreach (var header in endpoint.DefaultHeaders)
            {
                httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            foreach (var header in request.Headers)
            {
                httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            await ApplyAuthenticationAsync(httpRequest, endpoint);

            httpRequest.Headers.Add("X-Correlation-Id", request.CorrelationId ?? request.Id);
            httpRequest.Headers.Add("X-Request-Id", request.Id);

            _httpClient.Timeout = TimeSpan.FromSeconds(request.TimeoutSeconds);

            var httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken);
            var responseBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

            var hasGraphQLErrors = HasGraphQLErrors(responseBody);

            var response = new IntegrationResponse
            {
                RequestId = request.Id,
                Success = httpResponse.IsSuccessStatusCode && !hasGraphQLErrors,
                StatusCode = (int)httpResponse.StatusCode,
                Body = responseBody,
                Headers = httpResponse.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value)),
                ProcessedAt = DateTime.UtcNow,
                ProcessingTimeMs = (DateTime.UtcNow - startTime).Milliseconds,
                EndpointId = endpoint.Id
            };

            if (!httpResponse.IsSuccessStatusCode)
            {
                response.ErrorMessage = $"HTTP {httpResponse.StatusCode}: {httpResponse.ReasonPhrase}";
            }
            else if (hasGraphQLErrors)
            {
                response.ErrorMessage = ExtractGraphQLErrors(responseBody);
            }

            _logger.LogInformation("GraphQL request {RequestId} to {Url} completed with status {StatusCode} in {ProcessingTime}ms",
                request.Id, endpoint.BaseUrl, response.StatusCode, response.ProcessingTimeMs);

            return response;
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            _logger.LogWarning("GraphQL request {RequestId} timed out after {Timeout}s", request.Id, request.TimeoutSeconds);
            return new IntegrationResponse
            {
                RequestId = request.Id,
                Success = false,
                StatusCode = 408,
                ErrorMessage = "Request timeout",
                ProcessedAt = DateTime.UtcNow,
                ProcessingTimeMs = (DateTime.UtcNow - startTime).Milliseconds,
                EndpointId = endpoint.Id
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GraphQL request {RequestId}", request.Id);
            return new IntegrationResponse
            {
                RequestId = request.Id,
                Success = false,
                StatusCode = 500,
                ErrorMessage = $"GraphQL error: {ex.Message}",
                ProcessedAt = DateTime.UtcNow,
                ProcessingTimeMs = (DateTime.UtcNow - startTime).Milliseconds,
                EndpointId = endpoint.Id
            };
        }
    }

    public async Task<bool> ValidateEndpointAsync(IntegrationEndpoint endpoint)
    {
        try
        {
            if (string.IsNullOrEmpty(endpoint.BaseUrl))
            {
                _logger.LogWarning("GraphQL endpoint validation failed: BaseUrl is required");
                return false;
            }

            if (!Uri.TryCreate(endpoint.BaseUrl, UriKind.Absolute, out var uri))
            {
                _logger.LogWarning("GraphQL endpoint validation failed: Invalid BaseUrl format");
                return false;
            }

            if (uri.Scheme != "http" && uri.Scheme != "https")
            {
                _logger.LogWarning("GraphQL endpoint validation failed: Only HTTP and HTTPS schemes are supported");
                return false;
            }

            try
            {
                var introspectionQuery = new
                {
                    query = "query IntrospectionQuery { __schema { queryType { name } } }"
                };

                var jsonContent = JsonSerializer.Serialize(introspectionQuery);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var request = new HttpRequestMessage(HttpMethod.Post, endpoint.BaseUrl) { Content = content };

                _httpClient.Timeout = TimeSpan.FromSeconds(endpoint.TimeoutSeconds);
                var response = await _httpClient.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("GraphQL endpoint validation failed: Introspection query failed");
                    return false;
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                if (HasGraphQLErrors(responseBody))
                {
                    _logger.LogWarning("GraphQL endpoint validation failed: Introspection returned errors");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "GraphQL endpoint validation warning: Could not validate with introspection");
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating GraphQL endpoint {EndpointId}", endpoint.Id);
            return false;
        }
    }

    public async Task<IntegrationHealthStatus> CheckHealthAsync(IntegrationEndpoint endpoint)
    {
        var startTime = DateTime.UtcNow;

        try
        {
            var healthQuery = new
            {
                query = "query HealthCheck { __schema { queryType { name } } }"
            };

            var jsonContent = JsonSerializer.Serialize(healthQuery);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var request = new HttpRequestMessage(HttpMethod.Post, endpoint.BaseUrl) { Content = content };

            foreach (var header in endpoint.DefaultHeaders)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            await ApplyAuthenticationAsync(request, endpoint);

            _httpClient.Timeout = TimeSpan.FromSeconds(endpoint.TimeoutSeconds);

            var response = await _httpClient.SendAsync(request);
            var responseTime = (DateTime.UtcNow - startTime).TotalMilliseconds;
            var responseBody = await response.Content.ReadAsStringAsync();

            var hasErrors = HasGraphQLErrors(responseBody);

            var healthStatus = new IntegrationHealthStatus
            {
                EndpointId = endpoint.Id,
                IsHealthy = response.IsSuccessStatusCode && !hasErrors,
                Status = response.IsSuccessStatusCode && !hasErrors ? "Healthy" : "Unhealthy",
                ResponseTimeMs = (long)responseTime,
                CheckedAt = DateTime.UtcNow
            };

            if (!response.IsSuccessStatusCode)
            {
                healthStatus.ErrorMessage = $"HTTP {response.StatusCode}: {response.ReasonPhrase}";
            }
            else if (hasErrors)
            {
                healthStatus.ErrorMessage = ExtractGraphQLErrors(responseBody);
            }
            else
            {
                try
                {
                    var healthData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
                    if (healthData != null)
                    {
                        healthStatus.AdditionalInfo = healthData;
                    }
                }
                catch
                {
                    healthStatus.AdditionalInfo["RawResponse"] = responseBody;
                }
            }

            return healthStatus;
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return new IntegrationHealthStatus
            {
                EndpointId = endpoint.Id,
                IsHealthy = false,
                Status = "Timeout",
                ErrorMessage = "Health check timed out",
                ResponseTimeMs = (long)(DateTime.UtcNow - startTime).TotalMilliseconds,
                CheckedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking health for GraphQL endpoint {EndpointId}", endpoint.Id);
            return new IntegrationHealthStatus
            {
                EndpointId = endpoint.Id,
                IsHealthy = false,
                Status = "Error",
                ErrorMessage = ex.Message,
                ResponseTimeMs = (long)(DateTime.UtcNow - startTime).TotalMilliseconds,
                CheckedAt = DateTime.UtcNow
            };
        }
    }

    private object CreateGraphQLRequest(IntegrationRequest request)
    {
        var graphqlRequest = new Dictionary<string, object>();

        if (!string.IsNullOrEmpty(request.Body))
        {
            try
            {
                var bodyObject = JsonSerializer.Deserialize<Dictionary<string, object>>(request.Body);
                if (bodyObject != null)
                {
                    return bodyObject;
                }
            }
            catch (JsonException)
            {
                graphqlRequest["query"] = request.Body;
            }
        }
        else
        {
            var query = request.Parameters.GetValueOrDefault("query", "")?.ToString();
            if (!string.IsNullOrEmpty(query))
            {
                graphqlRequest["query"] = query;
            }

            var variables = request.Parameters.GetValueOrDefault("variables");
            if (variables != null)
            {
                graphqlRequest["variables"] = variables;
            }

            var operationName = request.Parameters.GetValueOrDefault("operationName")?.ToString();
            if (!string.IsNullOrEmpty(operationName))
            {
                graphqlRequest["operationName"] = operationName;
            }
        }

        return graphqlRequest;
    }

    private bool HasGraphQLErrors(string responseBody)
    {
        try
        {
            if (string.IsNullOrEmpty(responseBody))
                return false;

            var response = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
            return response != null && response.ContainsKey("errors");
        }
        catch
        {
            return false;
        }
    }

    private string ExtractGraphQLErrors(string responseBody)
    {
        try
        {
            var response = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
            if (response != null && response.TryGetValue("errors", out var errorsObj))
            {
                var errors = JsonSerializer.Serialize(errorsObj);
                return $"GraphQL Errors: {errors}";
            }
        }
        catch (Exception ex)
        {
            return $"GraphQL Errors (could not parse): {ex.Message}";
        }

        return "GraphQL errors occurred";
    }

    private async Task ApplyAuthenticationAsync(HttpRequestMessage request, IntegrationEndpoint endpoint)
    {
        if (string.IsNullOrEmpty(endpoint.AuthenticationType))
            return;

        try
        {
            switch (endpoint.AuthenticationType.ToUpper())
            {
                case "APIKEY":
                    if (endpoint.AuthenticationConfig.TryGetValue("ApiKey", out var apiKey) &&
                        endpoint.AuthenticationConfig.TryGetValue("HeaderName", out var headerName))
                    {
                        request.Headers.TryAddWithoutValidation(headerName, apiKey);
                    }
                    break;

                case "BEARER":
                    if (endpoint.AuthenticationConfig.TryGetValue("Token", out var token))
                    {
                        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    }
                    break;

                case "BASIC":
                    if (endpoint.AuthenticationConfig.TryGetValue("Username", out var username) &&
                        endpoint.AuthenticationConfig.TryGetValue("Password", out var password))
                    {
                        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
                        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
                    }
                    break;
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error applying authentication to GraphQL request");
        }
    }
}
