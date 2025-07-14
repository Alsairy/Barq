using BARQ.Testing.Framework;
using FluentAssertions;
using Microsoft.OpenApi.Models;
using Xunit;

namespace BARQ.Testing.Tests.Contract;

public class OpenApiContractTests : IClassFixture<ApiTestFramework>
{
    private readonly ApiTestFramework _factory;
    private readonly ContractTestFramework _contractFramework;

    public OpenApiContractTests(ApiTestFramework factory)
    {
        _factory = factory;
        _contractFramework = new ContractTestFramework(_factory.CreateClient());
    }

    [Fact]
    public async Task OpenApiSpecification_ShouldBeAccessible()
    {
        await _contractFramework.LoadOpenApiSpecificationAsync();
    }

    [Fact]
    public async Task AllEndpoints_ShouldExistInOpenApiSpec()
    {
        var criticalEndpoints = new[]
        {
            "/api/auth/login",
            "/api/auth/register",
            "/api/users/profile",
            "/api/organizations",
            "/api/projects",
            "/api/workflows",
            "/api/ai-tasks"
        };

        foreach (var endpoint in criticalEndpoints)
        {
            await _contractFramework.ValidateEndpointExistsAsync(endpoint, OperationType.Get);
        }
    }

    [Fact]
    public async Task AuthenticationEndpoints_ShouldHaveCorrectSchemas()
    {
        await _contractFramework.ValidateResponseSchemaAsync<AuthenticationResponse>("/api/auth/login", HttpMethod.Post);
    }

    [Fact]
    public async Task UserEndpoints_ShouldHaveCorrectSchemas()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
        
        var contractFramework = new ContractTestFramework(client);
        await contractFramework.ValidateResponseSchemaAsync<object>("/api/users/profile", HttpMethod.Get);
    }

    [Fact]
    public async Task OrganizationEndpoints_ShouldHaveCorrectSchemas()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
        
        var contractFramework = new ContractTestFramework(client);
        await contractFramework.ValidateResponseSchemaAsync<object>("/api/organizations", HttpMethod.Get);
    }

    [Fact]
    public async Task ProjectEndpoints_ShouldHaveCorrectSchemas()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
        
        var contractFramework = new ContractTestFramework(client);
        await contractFramework.ValidateResponseSchemaAsync<object>("/api/projects", HttpMethod.Get);
    }

    [Fact]
    public async Task AllEndpoints_ShouldBeAccessible()
    {
        await _contractFramework.ValidateAllEndpointsAsync();
    }

    [Fact]
    public async Task ContractCompliance_ShouldPassValidation()
    {
        var results = await _contractFramework.ValidateContractComplianceAsync();
        
        results.Should().NotBeEmpty();
        
        var failedValidations = results.Where(r => !r.IsValid).ToList();
        if (failedValidations.Any())
        {
            var errorMessages = failedValidations.SelectMany(r => r.ValidationErrors).ToList();
            throw new InvalidOperationException($"Contract validation failed: {string.Join(", ", errorMessages)}");
        }
    }

    [Fact]
    public async Task SecurityEndpoints_ShouldRequireAuthentication()
    {
        var secureEndpoints = new[]
        {
            "/api/users/profile",
            "/api/organizations",
            "/api/projects",
            "/api/workflows",
            "/api/ai-tasks"
        };

        foreach (var endpoint in secureEndpoints)
        {
            var response = await _factory.GetAsync(endpoint);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized, 
                $"Endpoint {endpoint} should require authentication");
        }
    }
}
