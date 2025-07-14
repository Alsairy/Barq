using BARQ.Testing.Framework;
using FluentAssertions;
using System.Net;
using Xunit;

namespace BARQ.Testing.Tests.Integration;

public class UserApiTests : IClassFixture<ApiTestFramework>
{
    private readonly ApiTestFramework _factory;

    public UserApiTests(ApiTestFramework factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetUserProfile_WithValidAuth_ReturnsSuccess()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var response = await _factory.GetAsync("/api/users/profile", authToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
        content.Should().Contain("test@acme.com");
    }

    [Fact]
    public async Task GetUserProfile_WithoutAuth_ReturnsUnauthorized()
    {
        var response = await _factory.GetAsync("/api/users/profile");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task UpdateUserProfile_WithValidData_ReturnsSuccess()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var updateRequest = new
        {
            FirstName = "Updated John",
            LastName = "Updated Doe",
            PhoneNumber = "+1234567890"
        };

        var response = await _factory.PostJsonAsync("/api/users/profile", updateRequest, authToken);

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task GetUsers_WithValidAuth_ReturnsSuccess()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var response = await _factory.GetAsync("/api/users", authToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task CreateUser_WithValidData_ReturnsCreated()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var createRequest = new
        {
            Email = "newuser@acme.com",
            FirstName = "New",
            LastName = "User",
            Password = "NewPassword123!"
        };

        var response = await _factory.PostJsonAsync("/api/users", createRequest, authToken);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task CreateUser_WithInvalidEmail_ReturnsBadRequest()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var createRequest = new
        {
            Email = "invalid-email",
            FirstName = "New",
            LastName = "User",
            Password = "NewPassword123!"
        };

        var response = await _factory.PostJsonAsync("/api/users", createRequest, authToken);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task UserTenantIsolation_UserCannotAccessOtherTenantUsers()
    {
        var acmeToken = await _factory.GetAuthTokenAsync("test@acme.com");
        var betaUserId = "44444444-4444-4444-4444-444444444444";
        
        var response = await _factory.GetAsync($"/api/users/{betaUserId}", acmeToken);

        response.StatusCode.Should().BeOneOf(HttpStatusCode.Forbidden, HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ChangePassword_WithValidData_ReturnsSuccess()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var changePasswordRequest = new
        {
            CurrentPassword = "TestPassword123!",
            NewPassword = "NewTestPassword123!",
            ConfirmPassword = "NewTestPassword123!"
        };

        var response = await _factory.PostJsonAsync("/api/users/change-password", changePasswordRequest, authToken);

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);
    }
}
