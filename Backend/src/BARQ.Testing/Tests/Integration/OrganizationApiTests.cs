using BARQ.Testing.Framework;
using FluentAssertions;
using System.Net;
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
        var organizationId = "11111111-1111-1111-1111-111111111111";
        
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
        var organizationId = "11111111-1111-1111-1111-111111111111";
        var updateRequest = new
        {
            Name = "Updated Acme Corporation",
            Domain = "acme-updated.com"
        };

        var response = await _factory.PostJsonAsync($"/api/organizations/{organizationId}", updateRequest, authToken);

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task TenantIsolation_UserCannotAccessOtherTenantData()
    {
        var acmeToken = await _factory.GetAuthTokenAsync("test@acme.com");
        var betaOrganizationId = "22222222-2222-2222-2222-222222222222";
        
        var response = await _factory.GetAsync($"/api/organizations/{betaOrganizationId}", acmeToken);

        response.StatusCode.Should().BeOneOf(HttpStatusCode.Forbidden, HttpStatusCode.NotFound);
    }
}
