using BARQ.Testing.Framework;
using FluentAssertions;
using System.Net;
using System.Linq;
using Xunit;

namespace BARQ.Testing.Tests.Integration
{
[Collection("ProjectApiTestCollection")]
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
        
        var projectsResponse = await _factory.GetAsync("/api/projects", authToken);
        projectsResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var projectsContent = await projectsResponse.Content.ReadAsStringAsync();
        var projects = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement[]>(projectsContent);
        var acmeProject = projects?.FirstOrDefault(p => 
        {
            if (p.TryGetProperty("name", out var nameProperty))
            {
                return nameProperty.GetString() == "Acme Project";
            }
            return false;
        });
        acmeProject.Should().NotBeNull();
        
        var projectId = acmeProject.HasValue && acmeProject.Value.TryGetProperty("id", out var idProperty) 
            ? idProperty.GetString() 
            : null;
        
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
        var betaProjects = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement[]>(betaProjectsContent);
        var betaProject = betaProjects?.FirstOrDefault(p => p.GetProperty("name").GetString() == "Beta Project");
        betaProject.Should().NotBeNull();
        
        var betaProjectId = betaProject?.GetProperty("id").GetString();
        
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
        var projects = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement[]>(projectsContent);
        var acmeProject = projects?.FirstOrDefault(p => p.GetProperty("name").GetString() == "Acme Project");
        acmeProject.Should().NotBeNull();
        
        var projectId = acmeProject?.GetProperty("id").GetString();
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

        var createdProjectContent = await createResponse.Content.ReadAsStringAsync();
        var createdProject = System.Text.Json.JsonSerializer.Deserialize<System.Text.Json.JsonElement>(createdProjectContent);
        var projectId = createdProject.GetProperty("id").GetString();

        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
        var deleteResponse = await client.DeleteAsync($"/api/projects/{projectId}");

        deleteResponse.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);
    }
}
}
