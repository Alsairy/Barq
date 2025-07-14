using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using BARQ.Core.Services.Integration;
using BARQ.Core.Models.DTOs;

namespace BARQ.Infrastructure.Integration.Adapters;

public class RestProtocolAdapter : IProtocolAdapter
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<RestProtocolAdapter> _logger;

    public string Protocol => "REST";

    public RestProtocolAdapter(HttpClient httpClient, ILogger<RestProtocolAdapter> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IntegrationResponse> SendAsync(IntegrationRequest request, IntegrationEndpoint endpoint, CancellationToken cancellationToken = default)
    {
        var startTime = DateTime.UtcNow;

        try
        {
            var url = $"{endpoint.BaseUrl.TrimEnd('/')}/{request.Path.TrimStart('/')}";
            
            if (request.Parameters.Any())
            {
                var queryString = string.Join("&", request.Parameters.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value?.ToString() ?? "")}"));
                url += $"?{queryString}";
            }

            var httpRequest = new HttpRequestMessage(new HttpMethod(request.Method.ToUpper()), url);

            foreach (var header in endpoint.DefaultHeaders)
            {
                httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            foreach (var header in request.Headers)
            {
                httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            await ApplyAuthenticationAsync(httpRequest, endpoint);

            if (!string.IsNullOrEmpty(request.Body) && (request.Method.ToUpper() == "POST" || request.Method.ToUpper() == "PUT" || request.Method.ToUpper() == "PATCH"))
            {
                httpRequest.Content = new StringContent(request.Body, Encoding.UTF8, "application/json");
            }

            httpRequest.Headers.Add("X-Correlation-Id", request.CorrelationId ?? request.Id);
            httpRequest.Headers.Add("X-Request-Id", request.Id);

            _httpClient.Timeout = TimeSpan.FromSeconds(request.TimeoutSeconds);

            var httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken);

            var responseBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);
            var responseHeaders = httpResponse.Headers.ToDictionary(h => h.Key, h => string.Join(", ", h.Value));

            var response = new IntegrationResponse
            {
                RequestId = request.Id,
                Success = httpResponse.IsSuccessStatusCode,
                StatusCode = (int)httpResponse.StatusCode,
                Body = responseBody,
                Headers = responseHeaders,
                ProcessedAt = DateTime.UtcNow,
                ProcessingTimeMs = (DateTime.UtcNow - startTime).Milliseconds,
                EndpointId = endpoint.Id
            };

            if (!httpResponse.IsSuccessStatusCode)
            {
                response.ErrorMessage = $"HTTP {httpResponse.StatusCode}: {httpResponse.ReasonPhrase}";
            }

            _logger.LogInformation("REST request {RequestId} to {Url} completed with status {StatusCode} in {ProcessingTime}ms",
                request.Id, url, response.StatusCode, response.ProcessingTimeMs);

            return response;
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            _logger.LogWarning("REST request {RequestId} timed out after {Timeout}s", request.Id, request.TimeoutSeconds);
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
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "HTTP error in REST request {RequestId}", request.Id);
            return new IntegrationResponse
            {
                RequestId = request.Id,
                Success = false,
                StatusCode = 500,
                ErrorMessage = $"HTTP error: {ex.Message}",
                ProcessedAt = DateTime.UtcNow,
                ProcessingTimeMs = (DateTime.UtcNow - startTime).Milliseconds,
                EndpointId = endpoint.Id
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error in REST request {RequestId}", request.Id);
            return new IntegrationResponse
            {
                RequestId = request.Id,
                Success = false,
                StatusCode = 500,
                ErrorMessage = $"Unexpected error: {ex.Message}",
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
                _logger.LogWarning("REST endpoint validation failed: BaseUrl is required");
                return false;
            }

            if (!Uri.TryCreate(endpoint.BaseUrl, UriKind.Absolute, out var uri))
            {
                _logger.LogWarning("REST endpoint validation failed: Invalid BaseUrl format");
                return false;
            }

            if (uri.Scheme != "http" && uri.Scheme != "https")
            {
                _logger.LogWarning("REST endpoint validation failed: Only HTTP and HTTPS schemes are supported");
                return false;
            }

            return await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating REST endpoint {EndpointId}", endpoint.Id);
            return false;
        }
    }

    public async Task<IntegrationHealthStatus> CheckHealthAsync(IntegrationEndpoint endpoint)
    {
        var startTime = DateTime.UtcNow;

        try
        {
            var healthUrl = $"{endpoint.BaseUrl.TrimEnd('/')}/health";
            var request = new HttpRequestMessage(HttpMethod.Get, healthUrl);

            foreach (var header in endpoint.DefaultHeaders)
            {
                request.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            await ApplyAuthenticationAsync(request, endpoint);

            _httpClient.Timeout = TimeSpan.FromSeconds(endpoint.TimeoutSeconds);

            var response = await _httpClient.SendAsync(request);
            var responseTime = (DateTime.UtcNow - startTime).TotalMilliseconds;

            var healthStatus = new IntegrationHealthStatus
            {
                EndpointId = endpoint.Id,
                IsHealthy = response.IsSuccessStatusCode,
                Status = response.IsSuccessStatusCode ? "Healthy" : "Unhealthy",
                ResponseTimeMs = (long)responseTime,
                CheckedAt = DateTime.UtcNow
            };

            if (!response.IsSuccessStatusCode)
            {
                healthStatus.ErrorMessage = $"HTTP {response.StatusCode}: {response.ReasonPhrase}";
            }

            var responseBody = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(responseBody))
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
            _logger.LogError(ex, "Error checking health for REST endpoint {EndpointId}", endpoint.Id);
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
            _logger.LogError(ex, "Error applying authentication to REST request");
        }
    }
}
