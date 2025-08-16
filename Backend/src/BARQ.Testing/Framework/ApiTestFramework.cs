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
using System.Collections.Generic;

using Microsoft.Extensions.Configuration;
using BARQ.Core.Models.Responses;
using BARQ.Shared.DTOs;

namespace BARQ.Testing.Framework;

public class ApiTestFramework : WebApplicationFactory<Program>, IAsyncLifetime
{
    private Guid _testTenantId;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        Environment.SetEnvironmentVariable("ASPNETCORE_ENVIRONMENT", "Testing");
        builder.UseEnvironment("Testing");

        builder.ConfigureAppConfiguration((context, config) =>
        {
            var dict = new Dictionary<string, string?>
            {
                ["Jwt:Secret"] = "test-secret-32-characters-minimum-0123456789",
                ["Jwt:Issuer"] = "https://api.test",
                ["Jwt:Audience"] = "https://test.app",
                ["Auth:Cookie:Enabled"] = "false"
            };
            config.AddInMemoryCollection(dict);
        });
        
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
            services.AddSingleton<ITenantProvider, TestTenantProvider>();
            services.AddScoped<ITestDataSeeder, TestDataSeeder>();
        });
    }

    public async Task InitializeAsync()
    {
        using var scope = Services.CreateScope();
        var tenantProvider = scope.ServiceProvider.GetRequiredService<ITenantProvider>();
        _testTenantId = Guid.NewGuid();
        tenantProvider.SetTenantId(_testTenantId);

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
        if (_testTenantId != Guid.Empty)
        {
            client.DefaultRequestHeaders.Remove("X-Tenant-ID");
            client.DefaultRequestHeaders.Add("X-Tenant-ID", _testTenantId.ToString());
        }

        var json = JsonSerializer.Serialize(data);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        return await client.PostAsync(endpoint, content);
    }

    public async Task<HttpResponseMessage> GetAsync(string endpoint, string? authToken = null)
    {
        var client = CreateClient();
        if (!string.IsNullOrEmpty(authToken))
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
        if (_testTenantId != Guid.Empty)
        {
            client.DefaultRequestHeaders.Remove("X-Tenant-ID");
            client.DefaultRequestHeaders.Add("X-Tenant-ID", _testTenantId.ToString());
        }

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
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Login failed with status {(int)response.StatusCode}: {content}");
        }
        var apiResponse = await DeserializeResponseAsync<ApiResponse<AuthenticationResponse>>(response);
        return apiResponse?.Data?.AccessToken ?? throw new InvalidOperationException("Failed to get auth token");
    }

    public async Task ResetDatabaseAsync()
    {
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BarqDbContext>();
        context.Organizations.RemoveRange(context.Organizations);
        context.Users.RemoveRange(context.Users);
        context.Projects.RemoveRange(context.Projects);
        await context.SaveChangesAsync();
        var seeder = scope.ServiceProvider.GetRequiredService<ITestDataSeeder>();
        await seeder.SeedTestDataAsync();
    }
}

public interface ITestDataSeeder
{
    Task SeedTestDataAsync();
}

public class TestDataSeeder : ITestDataSeeder
{
    private readonly BarqDbContext _context;
    private readonly ITenantProvider _tenantProvider;

    public TestDataSeeder(BarqDbContext context, ITenantProvider tenantProvider)
    {
        _context = context;
        _tenantProvider = tenantProvider;
    }

    public async Task SeedTestDataAsync()
    {
        var acmeDomain = "acme.com";
        var betaDomain = "beta.com";
        var acmeEmail = "test@acme.com";
        var betaEmail = "test@beta.com";

        var acmeOrg = _context.Organizations.FirstOrDefault(o => o.Domain == acmeDomain);
        if (acmeOrg == null)
        {
            acmeOrg = new Organization
            {
                Id = _tenantProvider.GetTenantId() != Guid.Empty ? _tenantProvider.GetTenantId() : Guid.NewGuid(),
                Name = "Acme Corporation",
                Domain = acmeDomain,
                SubscriptionPlan = Core.Enums.SubscriptionPlan.Professional,
                Status = BARQ.Core.Enums.OrganizationStatus.Active,
                CreatedAt = DateTime.UtcNow
            };
            _context.Organizations.Add(acmeOrg);
        }

        var betaOrg = _context.Organizations.FirstOrDefault(o => o.Domain == betaDomain);
        if (betaOrg == null)
        {
            betaOrg = new Organization
            {
                Id = Guid.NewGuid(),
                Name = "Beta Industries",
                Domain = betaDomain,
                SubscriptionPlan = Core.Enums.SubscriptionPlan.Enterprise,
                Status = BARQ.Core.Enums.OrganizationStatus.Active,
                CreatedAt = DateTime.UtcNow
            };
            _context.Organizations.Add(betaOrg);
        }

        _tenantProvider.SetTenantId(acmeOrg.Id);

        var acmeUser = _context.Users.FirstOrDefault(u => u.Email == acmeEmail);
        if (acmeUser == null)
        {
            acmeUser = new User
            {
                Id = Guid.NewGuid(),
                Email = acmeEmail,
                FirstName = "John",
                LastName = "Doe",
                TenantId = acmeOrg.Id,
                Status = BARQ.Core.Enums.UserStatus.Active,
                EmailVerified = true,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("TestPassword123!")
            };
            _context.Users.Add(acmeUser);
        }

        var betaUser = _context.Users.FirstOrDefault(u => u.Email == betaEmail);
        if (betaUser == null)
        {
            betaUser = new User
            {
                Id = Guid.NewGuid(),
                Email = betaEmail,
                FirstName = "Jane",
                LastName = "Smith",
                TenantId = betaOrg.Id,
                Status = BARQ.Core.Enums.UserStatus.Active,
                EmailVerified = true,
                EmailConfirmed = true,
                CreatedAt = DateTime.UtcNow,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("TestPassword123!")
            };
            _context.Users.Add(betaUser);
        }

        var acmeProject = _context.Projects.FirstOrDefault(p => p.Name == "Acme Project" && p.TenantId == acmeOrg.Id);
        if (acmeProject == null)
        {
            acmeProject = new Project
            {
                Id = Guid.NewGuid(),
                Name = "Acme Project",
                Description = "Test project for Acme",
                TenantId = acmeOrg.Id,
                CreatedById = acmeUser.Id,
                Status = Core.Enums.ProjectStatus.Active,
                CreatedAt = DateTime.UtcNow
            };
            _context.Projects.Add(acmeProject);
        }

        var betaProject = _context.Projects.FirstOrDefault(p => p.Name == "Beta Project" && p.TenantId == betaOrg.Id);
        if (betaProject == null)
        {
            betaProject = new Project
            {
                Id = Guid.NewGuid(),
                Name = "Beta Project",
                Description = "Test project for Beta",
                TenantId = betaOrg.Id,
                CreatedById = betaUser.Id,
                Status = Core.Enums.ProjectStatus.Active,
                CreatedAt = DateTime.UtcNow
            };
            _context.Projects.Add(betaProject);
        }

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
