using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using BARQ.Infrastructure.Data;
using BARQ.Core.Entities;
using BARQ.Core.Services;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using FluentAssertions;
using Xunit;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace BARQ.Testing.Framework;

public class ApiTestFramework : WebApplicationFactory<Program>, IAsyncLifetime
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
        builder.UseEnvironment("Testing");
        
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BarqDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(BarqDbContext));
            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            var dbName = Guid.NewGuid().ToString();
            services.AddDbContext<BarqDbContext>(options =>
            {
                options.UseInMemoryDatabase(dbName);
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            });

            services.RemoveAll<ITenantProvider>();
            services.AddScoped<ITenantProvider, TestTenantProvider>();
            services.AddScoped<ITestDataSeeder, TestDataSeeder>();
        });
    }

    public async Task InitializeAsync()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BarqDbContext>();
        await context.Database.EnsureCreatedAsync();
        
        var seeder = scope.ServiceProvider.GetRequiredService<ITestDataSeeder>();
        await seeder.SeedTestDataAsync();
    }

    public new async Task DisposeAsync()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BarqDbContext>();
        await context.Database.EnsureDeletedAsync();
        await base.DisposeAsync();
    }

    public async Task<HttpResponseMessage> PostJsonAsync<T>(string endpoint, T data, string? authToken = null)
    {
        var client = CreateClient();
        if (!string.IsNullOrEmpty(authToken))
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);

        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        return await client.PostAsync(endpoint, content);
    }

    public async Task<HttpResponseMessage> GetAsync(string endpoint, string? authToken = null)
    {
        var client = CreateClient();
        if (!string.IsNullOrEmpty(authToken))
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);

        return await client.GetAsync(endpoint);
    }

    public async Task<T?> DeserializeResponseAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<string> GetAuthTokenAsync(string email = "test@acme.com", string password = "TestPassword123!")
    {
        var loginRequest = new { Request = new { Email = email, Password = password } };
        var response = await PostJsonAsync("/api/auth/login", loginRequest);
        
        response.Should().BeSuccessful();
        var authResponse = await DeserializeResponseAsync<AuthenticationResponse>(response);
        return authResponse?.AccessToken ?? throw new InvalidOperationException("Failed to get auth token");
    }
}

public interface ITestDataSeeder
{
    Task SeedTestDataAsync();
}

public class TestDataSeeder : ITestDataSeeder
{
    private readonly BarqDbContext _context;

    public TestDataSeeder(BarqDbContext context)
    {
        _context = context;
    }

    public async Task SeedTestDataAsync()
    {
        if (_context.Organizations.Any())
        {
            return;
        }

        var acmeOrgId = Guid.NewGuid();
        var betaOrgId = Guid.NewGuid();
        var acmeUserId = Guid.NewGuid();
        var betaUserId = Guid.NewGuid();
        var acmeProjectId = Guid.NewGuid();
        var betaProjectId = Guid.NewGuid();

        var acmeOrg = new Organization
        {
            Id = acmeOrgId,
            Name = "Acme Corporation",
            Domain = "acme.com",
            SubscriptionPlan = Core.Enums.SubscriptionPlan.Professional,
            Status = BARQ.Core.Enums.OrganizationStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        var betaOrg = new Organization
        {
            Id = betaOrgId,
            Name = "Beta Industries",
            Domain = "beta.com",
            SubscriptionPlan = Core.Enums.SubscriptionPlan.Enterprise,
            Status = BARQ.Core.Enums.OrganizationStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        _context.Organizations.AddRange(acmeOrg, betaOrg);

        var acmeUser = new User
        {
            Id = acmeUserId,
            Email = "test@acme.com",
            FirstName = "John",
            LastName = "Doe",
            TenantId = acmeOrgId,
            Status = BARQ.Core.Enums.UserStatus.Active,
            EmailVerified = true,
                EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("TestPassword123!")
        };
        

        var betaUser = new User
        {
            Id = betaUserId,
            Email = "test@beta.com",
            FirstName = "Jane",
            LastName = "Smith",
            TenantId = betaOrgId,
            Status = BARQ.Core.Enums.UserStatus.Active,
            EmailVerified = true,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("TestPassword123!")
        };

        _context.Users.AddRange(acmeUser, betaUser);

        var acmeProject = new Project
        {
            Id = acmeProjectId,
            Name = "Acme Project",
            Description = "Test project for Acme",
            TenantId = acmeOrgId,
            CreatedById = acmeUserId,
            Status = Core.Enums.ProjectStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        var betaProject = new Project
        {
            Id = betaProjectId,
            Name = "Beta Project",
            Description = "Test project for Beta",
            TenantId = betaOrgId,
            CreatedById = betaUserId,
            Status = Core.Enums.ProjectStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.AddRange(acmeProject, betaProject);

        await _context.SaveChangesAsync();
    }
}

public class TestTenantProvider : ITenantProvider
{
    private Guid _tenantId;
    private string _tenantName = "Test Tenant";
    private Guid _currentUserId;

    public TestTenantProvider()
    {
        _tenantId = Guid.NewGuid();
        _currentUserId = Guid.NewGuid();
    }

    public Guid GetTenantId()
    {
        return _tenantId;
    }

    public void SetTenantId(Guid tenantId)
    {
        _tenantId = tenantId;
    }

    public string GetTenantName()
    {
        return _tenantName;
    }

    public void SetTenantName(string tenantName)
    {
        _tenantName = tenantName;
    }

    public bool IsMultiTenant()
    {
        return true;
    }

    public void ClearTenantContext()
    {
        _tenantId = Guid.Empty;
        _tenantName = string.Empty;
        _currentUserId = Guid.Empty;
    }

    public Guid GetCurrentUserId()
    {
        return _currentUserId;
    }
}


public class TestAuthenticationHandler : Microsoft.AspNetCore.Authentication.AuthenticationHandler<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions>
{
    public TestAuthenticationHandler(Microsoft.Extensions.Options.IOptionsMonitor<Microsoft.AspNetCore.Authentication.AuthenticationSchemeOptions> options,
        Microsoft.Extensions.Logging.ILoggerFactory logger, System.Text.Encodings.Web.UrlEncoder encoder, Microsoft.AspNetCore.Authentication.ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<Microsoft.AspNetCore.Authentication.AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, "TestUser"),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, Guid.NewGuid().ToString())
        };

        var identity = new System.Security.Claims.ClaimsIdentity(claims, "Test");
        var principal = new System.Security.Claims.ClaimsPrincipal(identity);
        var ticket = new Microsoft.AspNetCore.Authentication.AuthenticationTicket(principal, "Test");

        return Task.FromResult(Microsoft.AspNetCore.Authentication.AuthenticateResult.Success(ticket));
    }
}

public class AuthenticationResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
}
