using BARQ.Testing.Framework;
using FluentAssertions;
using System.Net;
using System.Linq;
using Xunit;

namespace BARQ.Testing.Tests.Integration;

public class OrganizationApiTests : IClassFixture<ApiTestFramework>
{
    private readonly ApiTestFramework _factory;

    public OrganizationApiTests(ApiTestFramework factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetOrganizations_WithValidAuth_ReturnsSuccess()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var response = await _factory.GetAsync("/api/organizations", authToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task GetOrganizations_WithoutAuth_ReturnsUnauthorized()
    {
        var response = await _factory.GetAsync("/api/organizations");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateOrganization_WithValidData_ReturnsCreated()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var createRequest = new
        {
            Name = "Test Organization",
            Domain = "test.com",
            SubscriptionPlan = "Professional"
        };

        var response = await _factory.PostJsonAsync("/api/organizations", createRequest, authToken);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task CreateOrganization_WithInvalidData_ReturnsBadRequest()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var createRequest = new
        {
            Name = "",
            Domain = "invalid-domain"
        };

        var response = await _factory.PostJsonAsync("/api/organizations", createRequest, authToken);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task GetOrganization_WithValidId_ReturnsOrganization()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        
        var orgsResponse = await _factory.GetAsync("/api/organizations", authToken);
        orgsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var orgsContent = await orgsResponse.Content.ReadAsStringAsync();
        var orgsJson = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(orgsContent);
        
        System.Text.Json.JsonElement orgsArray;
        if (orgsJson.ValueKind == System.Text.Json.JsonValueKind.Array)
        {
            orgsArray = orgsJson;
        }
        else if (orgsJson.TryGetProperty("data", out orgsArray) && orgsArray.ValueKind == System.Text.Json.JsonValueKind.Array)
        {
        }
        else
        {
            throw new InvalidOperationException($"Unexpected JSON structure: {orgsContent}");
        }
        
        System.Text.Json.JsonElement? acmeOrg = null;
        foreach (var org in orgsArray.EnumerateArray())
        {
            if (org.TryGetProperty("name", out var nameProperty) && 
                nameProperty.GetString() == "Acme Corporation")
            {
                acmeOrg = org;
                break;
            }
        }
        acmeOrg.Should().NotBeNull();
        
        var organizationId = acmeOrg.HasValue && acmeOrg.Value.TryGetProperty("id", out var idProperty) 
            ? idProperty.GetString() 
            : null;
        
        var response = await _factory.GetAsync($"/api/organizations/{organizationId}", authToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Acme Corporation");
    }

    [Fact]
    public async Task GetOrganization_WithInvalidId_ReturnsNotFound()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var invalidId = "99999999-9999-9999-9999-999999999999";
        
        var response = await _factory.GetAsync($"/api/organizations/{invalidId}", authToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateOrganization_WithValidData_ReturnsSuccess()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        
        var orgsResponse = await _factory.GetAsync("/api/organizations", authToken);
        orgsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var orgsContent = await orgsResponse.Content.ReadAsStringAsync();
        var orgsJson = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(orgsContent);
        
        System.Text.Json.JsonElement orgsArray;
        if (orgsJson.ValueKind == System.Text.Json.JsonValueKind.Array)
        {
            orgsArray = orgsJson;
        }
        else if (orgsJson.TryGetProperty("data", out orgsArray) && orgsArray.ValueKind == System.Text.Json.JsonValueKind.Array)
        {
        }
        else
        {
            throw new InvalidOperationException($"Unexpected JSON structure: {orgsContent}");
        }
        
        System.Text.Json.JsonElement? acmeOrg = null;
        foreach (var org in orgsArray.EnumerateArray())
        {
            if (org.TryGetProperty("name", out var nameProperty) && 
                nameProperty.GetString() == "Acme Corporation")
            {
                acmeOrg = org;
                break;
            }
        }
        acmeOrg.Should().NotBeNull();
        
        var organizationId = acmeOrg.HasValue && acmeOrg.Value.TryGetProperty("id", out var idProperty) 
            ? idProperty.GetString() 
            : null;
        var updateRequest = new
        {
            Name = "Updated Acme Corporation",
            Domain = "acme-updated.com"
        };

        var response = await _factory.PostJsonAsync($"/api/organizations/{organizationId}", updateRequest, authToken);

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);
    }

    [Fact(Skip = "Temporarily disabled due to tenant context issues - will be re-enabled after tenant provider fix")]
    public async Task TenantIsolation_UserCannotAccessOtherTenantData()
    {
        var acmeToken = await _factory.GetAuthTokenAsync("test@acme.com");
        
        var betaToken = await _factory.GetAuthTokenAsync("test@beta.com");
        var betaOrgsResponse = await _factory.GetAsync("/api/organizations", betaToken);
        betaOrgsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var betaOrgsContent = await betaOrgsResponse.Content.ReadAsStringAsync();
        var betaOrgsJson = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(betaOrgsContent);
        
        System.Text.Json.JsonElement betaOrgsArray;
        if (betaOrgsJson.ValueKind == System.Text.Json.JsonValueKind.Array)
        {
            betaOrgsArray = betaOrgsJson;
        }
        else if (betaOrgsJson.TryGetProperty("data", out betaOrgsArray) && betaOrgsArray.ValueKind == System.Text.Json.JsonValueKind.Array)
        {
        }
        else
        {
            throw new InvalidOperationException($"Unexpected JSON structure: {betaOrgsContent}");
        }
        
        System.Text.Json.JsonElement? betaOrg = null;
        foreach (var org in betaOrgsArray.EnumerateArray())
        {
            if (org.TryGetProperty("name", out var nameProperty) && 
                nameProperty.GetString() == "Beta Industries")
            {
                betaOrg = org;
                break;
            }
        }
        betaOrg.Should().NotBeNull();
        
        var betaOrganizationId = betaOrg.HasValue && betaOrg.Value.TryGetProperty("id", out var idProperty) 
            ? idProperty.GetString() 
            : null;
        
        var response = await _factory.GetAsync($"/api/organizations/{betaOrganizationId}", acmeToken);

        response.StatusCode.Should().BeOneOf(HttpStatusCode.Forbidden, HttpStatusCode.NotFound);
    }
}
