using Microsoft.Extensions.Logging;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using BARQ.Core.Services.Integration;
using BARQ.Core.Models.DTOs;

namespace BARQ.Infrastructure.Integration.Adapters;

public class SoapProtocolAdapter : IProtocolAdapter
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<SoapProtocolAdapter> _logger;

    public string Protocol => "SOAP";

    public SoapProtocolAdapter(HttpClient httpClient, ILogger<SoapProtocolAdapter> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IntegrationResponse> SendAsync(IntegrationRequest request, IntegrationEndpoint endpoint, CancellationToken cancellationToken = default)
    {
        var startTime = DateTime.UtcNow;

        try
        {
            var soapEnvelope = CreateSoapEnvelope(request);
            var content = new StringContent(soapEnvelope, Encoding.UTF8, "text/xml");

            var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpoint.BaseUrl)
            {
                Content = content
            };

            httpRequest.Headers.Add("SOAPAction", request.Headers.GetValueOrDefault("SOAPAction", ""));

            foreach (var header in endpoint.DefaultHeaders)
            {
                httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            foreach (var header in request.Headers.Where(h => h.Key != "SOAPAction"))
            {
                httpRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            await ApplyAuthenticationAsync(httpRequest, endpoint);

            httpRequest.Headers.Add("X-Correlation-Id", request.CorrelationId ?? request.Id);
            httpRequest.Headers.Add("X-Request-Id", request.Id);

            _httpClient.Timeout = TimeSpan.FromSeconds(request.TimeoutSeconds);

            var httpResponse = await _httpClient.SendAsync(httpRequest, cancellationToken);
            var responseBody = await httpResponse.Content.ReadAsStringAsync(cancellationToken);

            var response = new IntegrationResponse
            {
                RequestId = request.Id,
                Success = httpResponse.IsSuccessStatusCode && !IsSoapFault(responseBody),
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
            else if (IsSoapFault(responseBody))
            {
                response.ErrorMessage = ExtractSoapFaultMessage(responseBody);
                response.Success = false;
            }

            _logger.LogInformation("SOAP request {RequestId} to {Url} completed with status {StatusCode} in {ProcessingTime}ms",
                request.Id, endpoint.BaseUrl, response.StatusCode, response.ProcessingTimeMs);

            return response;
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            _logger.LogWarning("SOAP request {RequestId} timed out after {Timeout}s", request.Id, request.TimeoutSeconds);
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
            _logger.LogError(ex, "Error in SOAP request {RequestId}", request.Id);
            return new IntegrationResponse
            {
                RequestId = request.Id,
                Success = false,
                StatusCode = 500,
                ErrorMessage = $"SOAP error: {ex.Message}",
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
                _logger.LogWarning("SOAP endpoint validation failed: BaseUrl is required");
                return false;
            }

            if (!Uri.TryCreate(endpoint.BaseUrl, UriKind.Absolute, out var uri))
            {
                _logger.LogWarning("SOAP endpoint validation failed: Invalid BaseUrl format");
                return false;
            }

            if (uri.Scheme != "http" && uri.Scheme != "https")
            {
                _logger.LogWarning("SOAP endpoint validation failed: Only HTTP and HTTPS schemes are supported");
                return false;
            }

            try
            {
                var wsdlUrl = endpoint.BaseUrl.EndsWith("?wsdl") ? endpoint.BaseUrl : $"{endpoint.BaseUrl}?wsdl";
                var request = new HttpRequestMessage(HttpMethod.Get, wsdlUrl);
                
                _httpClient.Timeout = TimeSpan.FromSeconds(endpoint.TimeoutSeconds);
                var response = await _httpClient.SendAsync(request);
                
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("SOAP endpoint validation failed: WSDL not accessible");
                    return false;
                }

                var wsdlContent = await response.Content.ReadAsStringAsync();
                if (!wsdlContent.Contains("definitions") || !wsdlContent.Contains("wsdl"))
                {
                    _logger.LogWarning("SOAP endpoint validation failed: Invalid WSDL format");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "SOAP endpoint validation warning: Could not validate WSDL");
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating SOAP endpoint {EndpointId}", endpoint.Id);
            return false;
        }
    }

    public async Task<IntegrationHealthStatus> CheckHealthAsync(IntegrationEndpoint endpoint)
    {
        var startTime = DateTime.UtcNow;

        try
        {
            var wsdlUrl = endpoint.BaseUrl.EndsWith("?wsdl") ? endpoint.BaseUrl : $"{endpoint.BaseUrl}?wsdl";
            var request = new HttpRequestMessage(HttpMethod.Get, wsdlUrl);

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
            else
            {
                var wsdlContent = await response.Content.ReadAsStringAsync();
                healthStatus.AdditionalInfo["WsdlSize"] = wsdlContent.Length;
                healthStatus.AdditionalInfo["HasDefinitions"] = wsdlContent.Contains("definitions");
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
            _logger.LogError(ex, "Error checking health for SOAP endpoint {EndpointId}", endpoint.Id);
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

    private string CreateSoapEnvelope(IntegrationRequest request)
    {
        var envelope = new XDocument(
            new XDeclaration("1.0", "utf-8", null),
            new XElement(XName.Get("Envelope", "http://schemas.xmlsoap.org/soap/envelope/"),
                new XAttribute(XNamespace.Xmlns + "soap", "http://schemas.xmlsoap.org/soap/envelope/"),
                new XElement(XName.Get("Header", "http://schemas.xmlsoap.org/soap/envelope/")),
                new XElement(XName.Get("Body", "http://schemas.xmlsoap.org/soap/envelope/"))
            )
        );

        var body = envelope.Root?.Element(XName.Get("Body", "http://schemas.xmlsoap.org/soap/envelope/"));
        
        if (!string.IsNullOrEmpty(request.Body))
        {
            try
            {
                var bodyContent = XElement.Parse(request.Body);
                body?.Add(bodyContent);
            }
            catch (XmlException)
            {
                var methodName = request.Parameters.GetValueOrDefault("MethodName", "DefaultMethod").ToString();
                var methodElement = new XElement(methodName);
                
                foreach (var param in request.Parameters.Where(p => p.Key != "MethodName"))
                {
                    methodElement.Add(new XElement(param.Key, param.Value));
                }
                
                body?.Add(methodElement);
            }
        }

        return envelope.ToString();
    }

    private bool IsSoapFault(string responseBody)
    {
        try
        {
            if (string.IsNullOrEmpty(responseBody))
                return false;

            return responseBody.Contains("soap:Fault") || 
                   responseBody.Contains("Fault") && responseBody.Contains("faultcode");
        }
        catch
        {
            return false;
        }
    }

    private string ExtractSoapFaultMessage(string responseBody)
    {
        try
        {
            var doc = XDocument.Parse(responseBody);
            var fault = doc.Descendants().FirstOrDefault(e => e.Name.LocalName == "Fault");
            
            if (fault != null)
            {
                var faultString = fault.Descendants().FirstOrDefault(e => e.Name.LocalName == "faultstring")?.Value;
                var faultCode = fault.Descendants().FirstOrDefault(e => e.Name.LocalName == "faultcode")?.Value;
                
                return $"SOAP Fault - Code: {faultCode}, Message: {faultString}";
            }
        }
        catch (Exception ex)
        {
            return $"SOAP Fault (could not parse details): {ex.Message}";
        }

        return "SOAP Fault occurred";
    }

    private async Task ApplyAuthenticationAsync(HttpRequestMessage request, IntegrationEndpoint endpoint)
    {
        if (string.IsNullOrEmpty(endpoint.AuthenticationType))
            return;

        try
        {
            switch (endpoint.AuthenticationType.ToUpper())
            {
                case "BASIC":
                    if (endpoint.AuthenticationConfig.TryGetValue("Username", out var username) &&
                        endpoint.AuthenticationConfig.TryGetValue("Password", out var password))
                    {
                        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{username}:{password}"));
                        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", credentials);
                    }
                    break;

                case "BEARER":
                    if (endpoint.AuthenticationConfig.TryGetValue("Token", out var token))
                    {
                        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    }
                    break;
            }

            await Task.CompletedTask;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error applying authentication to SOAP request");
        }
    }
}
