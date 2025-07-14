using BARQ.Testing.Framework;
using FluentAssertions;
using System.Net;
using Xunit;

namespace BARQ.Testing.Tests.Integration;

public class ProjectApiTests : IClassFixture<ApiTestFramework>
{
    private readonly ApiTestFramework _factory;

    public ProjectApiTests(ApiTestFramework factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetProjects_WithValidAuth_ReturnsSuccess()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var response = await _factory.GetAsync("/api/projects", authToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task CreateProject_WithValidData_ReturnsCreated()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var createRequest = new
        {
            Name = "Test Project",
            Description = "A test project for API testing",
            Priority = "High"
        };

        var response = await _factory.PostJsonAsync("/api/projects", createRequest, authToken);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task GetProject_WithValidId_ReturnsProject()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var projectId = "55555555-5555-5555-5555-555555555555";
        
        var response = await _factory.GetAsync($"/api/projects/{projectId}", authToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Acme Project");
    }

    [Fact]
    public async Task ProjectTenantIsolation_UserCannotAccessOtherTenantProjects()
    {
        var acmeToken = await _factory.GetAuthTokenAsync("test@acme.com");
        var betaProjectId = "66666666-6666-6666-6666-666666666666";
        
        var response = await _factory.GetAsync($"/api/projects/{betaProjectId}", acmeToken);

        response.StatusCode.Should().BeOneOf(HttpStatusCode.Forbidden, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateProject_WithValidData_ReturnsSuccess()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var projectId = "55555555-5555-5555-5555-555555555555";
        var updateRequest = new
        {
            Name = "Updated Acme Project",
            Description = "Updated description",
            Priority = "Medium"
        };

        var response = await _factory.PostJsonAsync($"/api/projects/{projectId}", updateRequest, authToken);

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task DeleteProject_WithValidId_ReturnsSuccess()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        
        var createRequest = new
        {
            Name = "Project to Delete",
            Description = "This project will be deleted",
            Priority = "Low"
        };

        var createResponse = await _factory.PostJsonAsync("/api/projects", createRequest, authToken);
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var createdProject = await _factory.DeserializeResponseAsync<dynamic>(createResponse);
        var projectId = createdProject?.id?.ToString();

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
        var deleteResponse = await client.DeleteAsync($"/api/projects/{projectId}");

        deleteResponse.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);
    }
}
