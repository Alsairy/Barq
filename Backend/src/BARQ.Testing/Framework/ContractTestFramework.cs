using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using System.Net.Http;
using System.Text.Json;
using FluentAssertions;
using Xunit;

namespace BARQ.Testing.Framework;

public class ContractTestFramework
{
    private readonly HttpClient _httpClient;
    private OpenApiDocument? _openApiDocument;

    public ContractTestFramework(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task LoadOpenApiSpecificationAsync()
    {
        var response = await _httpClient.GetAsync("/swagger/v1/swagger.json");
        response.Should().BeSuccessful("OpenAPI specification should be accessible");

        var content = await response.Content.ReadAsStringAsync();
        var reader = new OpenApiStringReader();
        _openApiDocument = reader.Read(content, out var diagnostic);

        _openApiDocument.Should().NotBeNull("OpenAPI specification should be valid");
        diagnostic.Errors.Should().BeEmpty("OpenAPI specification should have no errors");
    }

    public async Task ValidateEndpointExistsAsync(string path, OperationType operationType)
    {
        if (_openApiDocument == null)
            await LoadOpenApiSpecificationAsync();

        _openApiDocument!.Paths.Should().ContainKey(path, $"Path {path} should exist in OpenAPI specification");
        
        var pathItem = _openApiDocument.Paths[path];
        pathItem.Operations.Should().ContainKey(operationType, $"Operation {operationType} should exist for path {path}");
    }

    public async Task ValidateResponseSchemaAsync<T>(string endpoint, HttpMethod method, T? expectedResponse = default)
    {
        var request = new HttpRequestMessage(method, endpoint);
        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(content) && typeof(T) != typeof(string))
            {
                var deserializedResponse = JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                deserializedResponse.Should().NotBeNull($"Response from {method} {endpoint} should be deserializable to {typeof(T).Name}");
            }
        }
    }

    public async Task ValidateAllEndpointsAsync()
    {
        if (_openApiDocument == null)
            await LoadOpenApiSpecificationAsync();

        var validationTasks = new List<Task>();

        foreach (var path in _openApiDocument!.Paths)
        {
            foreach (var operation in path.Value.Operations)
            {
                var endpoint = path.Key;
                var method = GetHttpMethod(operation.Key);
                
                validationTasks.Add(ValidateEndpointAccessibilityAsync(endpoint, method));
            }
        }

        await Task.WhenAll(validationTasks);
    }

    private async Task ValidateEndpointAccessibilityAsync(string endpoint, HttpMethod method)
    {
        try
        {
            var request = new HttpRequestMessage(method, endpoint);
            var response = await _httpClient.SendAsync(request);
            
            response.StatusCode.Should().NotBe(System.Net.HttpStatusCode.NotFound, 
                $"Endpoint {method} {endpoint} should exist and be accessible");
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Failed to validate endpoint {method} {endpoint}: {ex.Message}", ex);
        }
    }

    private static HttpMethod GetHttpMethod(OperationType operationType)
    {
        return operationType switch
        {
            OperationType.Get => HttpMethod.Get,
            OperationType.Post => HttpMethod.Post,
            OperationType.Put => HttpMethod.Put,
            OperationType.Delete => HttpMethod.Delete,
            OperationType.Patch => HttpMethod.Patch,
            OperationType.Head => HttpMethod.Head,
            OperationType.Options => HttpMethod.Options,
            OperationType.Trace => HttpMethod.Trace,
            _ => throw new ArgumentOutOfRangeException(nameof(operationType), operationType, null)
        };
    }

    public async Task<List<ContractValidationResult>> ValidateContractComplianceAsync()
    {
        var results = new List<ContractValidationResult>();

        if (_openApiDocument == null)
            await LoadOpenApiSpecificationAsync();

        foreach (var path in _openApiDocument!.Paths)
        {
            foreach (var operation in path.Value.Operations)
            {
                var endpoint = path.Key;
                var method = GetHttpMethod(operation.Key);
                var operationInfo = operation.Value;

                var result = new ContractValidationResult
                {
                    Endpoint = endpoint,
                    Method = method.Method,
                    OperationId = operationInfo.OperationId,
                    IsValid = true,
                    ValidationErrors = new List<string>()
                };

                try
                {
                    await ValidateOperationAsync(endpoint, method, operationInfo, result);
                }
                catch (Exception ex)
                {
                    result.IsValid = false;
                    result.ValidationErrors.Add($"Validation failed: {ex.Message}");
                }

                results.Add(result);
            }
        }

        return results;
    }

    private async Task ValidateOperationAsync(string endpoint, HttpMethod method, OpenApiOperation operation, ContractValidationResult result)
    {
        if (operation.Responses == null || !operation.Responses.Any())
        {
            result.ValidationErrors.Add("No responses defined in OpenAPI specification");
            result.IsValid = false;
            return;
        }

        var request = new HttpRequestMessage(method, endpoint);
        var response = await _httpClient.SendAsync(request);

        var responseCode = ((int)response.StatusCode).ToString();
        if (!operation.Responses.ContainsKey(responseCode) && !operation.Responses.ContainsKey("default"))
        {
            result.ValidationErrors.Add($"Response code {responseCode} not documented in OpenAPI specification");
            result.IsValid = false;
        }

        if (response.Content.Headers.ContentType?.MediaType == "application/json")
        {
            var content = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(content))
            {
                try
                {
                    JsonDocument.Parse(content);
                }
                catch (JsonException)
                {
                    result.ValidationErrors.Add("Response is not valid JSON");
                    result.IsValid = false;
                }
            }
        }
    }
}

public class ContractValidationResult
{
    public string Endpoint { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string? OperationId { get; set; }
    public bool IsValid { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
}
