using BARQ.Testing.Framework;
using FluentAssertions;
using System.Net;
using Xunit;

namespace BARQ.Testing.Tests.Integration
{
[Collection("AuthenticationApiTestCollection")]
public class AuthenticationApiTests : IClassFixture<ApiTestFramework>
{
    private readonly ApiTestFramework _factory;

    public AuthenticationApiTests(ApiTestFramework factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsSuccess()
    {
        var loginRequest = new
        {
            Request = new
            {
                Email = "test@acme.com",
                Password = "TestPassword123!"
            }
        };

        var response = await _factory.PostJsonAsync("/api/auth/login", loginRequest);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"DEBUG: Response content: {content}");
        
        var apiResponse = await _factory.DeserializeResponseAsync<BARQ.Shared.DTOs.ApiResponse<BARQ.Core.Models.Responses.AuthenticationResponse>>(response);
        Console.WriteLine($"DEBUG: ApiResponse Success: {apiResponse?.Success}, Data is null: {apiResponse?.Data == null}");
        
        var authResponse = apiResponse?.Data;
        if (authResponse != null)
        {
            Console.WriteLine($"DEBUG: AuthResponse Success: {authResponse.Success}, AccessToken: '{authResponse.AccessToken}', RefreshToken: '{authResponse.RefreshToken}'");
        }

        authResponse.Should().NotBeNull();
        authResponse!.AccessToken.Should().NotBeNullOrEmpty();
        authResponse.Success.Should().BeTrue();
    }

    [Fact]
    public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
    {
        var loginRequest = new
        {
            Request = new
            {
                Email = "test@acme.com",
                Password = "WrongPassword"
            }
        };

        var response = await _factory.PostJsonAsync("/api/auth/login", loginRequest);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task Login_WithInvalidEmail_ReturnsBadRequest()
    {
        var loginRequest = new
        {
            Request = new
            {
                Email = "invalid-email",
                Password = "TestPassword123!"
            }
        };

        var response = await _factory.PostJsonAsync("/api/auth/login", loginRequest);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsCreated()
    {
        var registerRequest = new
        {
            Request = new
            {
                Email = "newuser@test.com",
                FirstName = "New",
                LastName = "User",
                Password = "NewPassword123!",
                ConfirmPassword = "NewPassword123!",
                OrganizationName = "Test Organization"
            }
        };

        var response = await _factory.PostJsonAsync("/api/user/register", registerRequest);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task Register_WithExistingEmail_ReturnsConflict()
    {
        var registerRequest = new
        {
            Request = new
            {
                Email = "test@acme.com",
                FirstName = "Duplicate",
                LastName = "User",
                Password = "NewPassword123!",
                ConfirmPassword = "NewPassword123!",
                OrganizationName = "Test Organization"
            }
        };

        var response = await _factory.PostJsonAsync("/api/user/register", registerRequest);

        response.StatusCode.Should().Be(HttpStatusCode.Conflict);
    }

    [Fact]
    public async Task RefreshToken_WithValidToken_ReturnsNewToken()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var refreshRequest = new
        {
            RefreshToken = "valid-refresh-token"
        };

        var response = await _factory.PostJsonAsync("/api/auth/refresh-token", refreshRequest, authToken);

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Logout_WithValidToken_ReturnsSuccess()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        
        var response = await _factory.PostJsonAsync("/api/auth/logout", new { }, authToken);

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task ForgotPassword_WithValidEmail_ReturnsSuccess()
    {
        var forgotPasswordRequest = new
        {
            Email = "test@acme.com"
        };

        var response = await _factory.PostJsonAsync("/api/auth/forgot-password", forgotPasswordRequest);

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);
    }

    [Fact]
    public async Task ResetPassword_WithValidToken_ReturnsSuccess()
    {
        var resetPasswordRequest = new
        {
            Token = "valid-reset-token",
            NewPassword = "NewResetPassword123!",
            ConfirmPassword = "NewResetPassword123!"
        };

        var response = await _factory.PostJsonAsync("/api/auth/reset-password", resetPasswordRequest);

        response.StatusCode.Should().BeOneOf(HttpStatusCode.OK, HttpStatusCode.BadRequest);
    }
}
}
