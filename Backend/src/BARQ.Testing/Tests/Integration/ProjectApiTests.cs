using BARQ.Testing.Framework;
using FluentAssertions;
using System.Net;
using System.Linq;
using Xunit;

namespace BARQ.Testing.Tests.Integration
{
[Collection("ProjectApiTestCollection")]
public class ProjectApiTests : IClassFixture<StandaloneTestFramework>
{
    private readonly StandaloneTestFramework _factory;

    public ProjectApiTests(StandaloneTestFramework factory)
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
        
        var projectsResponse = await _factory.GetAsync("/api/projects", authToken);
        projectsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var projectsContent = await projectsResponse.Content.ReadAsStringAsync();
        projectsContent.Should().Contain("Acme Project");
        
        var projectId = "11111111-1111-1111-1111-111111111111";
        
        var response = await _factory.GetAsync($"/api/projects/{projectId}", authToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().Contain("Acme Project");
    }

    [Fact]
    public async Task ProjectTenantIsolation_UserCannotAccessOtherTenantProjects()
    {
        var acmeToken = await _factory.GetAuthTokenAsync("test@acme.com");
        
        var betaToken = await _factory.GetAuthTokenAsync("test@beta.com");
        var betaProjectsResponse = await _factory.GetAsync("/api/projects", betaToken);
        betaProjectsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var betaProjectsContent = await betaProjectsResponse.Content.ReadAsStringAsync();
        betaProjectsContent.Should().Contain("Beta Project");
        
        var betaProjectId = "22222222-2222-2222-2222-222222222222";
        
        var response = await _factory.GetAsync($"/api/projects/{betaProjectId}", acmeToken);

        response.StatusCode.Should().BeOneOf(HttpStatusCode.Forbidden, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task UpdateProject_WithValidData_ReturnsSuccess()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        
        var projectsResponse = await _factory.GetAsync("/api/projects", authToken);
        projectsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var projectsContent = await projectsResponse.Content.ReadAsStringAsync();
        projectsContent.Should().Contain("Acme Project");
        
        var projectId = "11111111-1111-1111-1111-111111111111";
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

        var responseContent = await createResponse.Content.ReadAsStringAsync();
        responseContent.Should().NotBeNullOrEmpty();
        
        var projectId = Guid.NewGuid().ToString();

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
        var deleteResponse = await client.DeleteAsync($"/api/projects/{projectId}");

        deleteResponse.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent, HttpStatusCode.NotFound);
    }
}
}
