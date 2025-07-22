using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using BARQ.Infrastructure.Data;
using BARQ.Core.Entities;
using BARQ.Core.Services;
using BARQ.Core.Models.Responses;
using BARQ.Shared.DTOs;
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
    private readonly string DatabaseName = $"TestDb_{Guid.NewGuid()}";
    private readonly object DatabaseLock = new object();
    private bool DatabaseSeeded = false;
    
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

            services.AddDbContext<BarqDbContext>(options =>
            {
                options.UseInMemoryDatabase(DatabaseName);
                options.EnableSensitiveDataLogging();
                options.EnableDetailedErrors();
            }, ServiceLifetime.Scoped);

            services.RemoveAll<ITenantProvider>();
            services.AddScoped<ITenantProvider, TestTenantProvider>();
            services.AddScoped<ITestDataSeeder, TestDataSeeder>();
            
            services.AddScoped<BARQ.Core.Services.IAuthenticationService, BARQ.Application.Services.Authentication.AuthenticationService>();
            services.AddScoped<BARQ.Core.Services.IPasswordService, BARQ.Application.Services.Authentication.PasswordService>();
            services.AddScoped<BARQ.Core.Services.IMultiFactorAuthService, BARQ.Application.Services.Authentication.MultiFactorAuthService>();
            services.AddScoped<BARQ.Core.Services.IUserRoleService, BARQ.Application.Services.Users.UserRoleService>();
            services.AddScoped<BARQ.Core.Repositories.IUnitOfWork, BARQ.Infrastructure.Repositories.UnitOfWork>();
            
            services.AddScoped(typeof(BARQ.Core.Repositories.IRepository<>), typeof(BARQ.Infrastructure.Repositories.GenericRepository<>));
        });
    }

    public async Task InitializeAsync()
    {
        Console.WriteLine($"[INIT] Starting test framework initialization at {DateTime.UtcNow} for database {DatabaseName}");
        
        lock (DatabaseLock)
        {
            if (DatabaseSeeded)
            {
                Console.WriteLine($"[INIT] Database already seeded for {DatabaseName}, skipping initialization");
                return;
            }
        }
        
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BarqDbContext>();
        await context.Database.EnsureCreatedAsync();
        Console.WriteLine($"[INIT] Database {DatabaseName} created successfully");
        
        var seeder = scope.ServiceProvider.GetRequiredService<ITestDataSeeder>();
        await seeder.SeedTestDataAsync();
        
        var userCount = await context.Users.CountAsync();
        Console.WriteLine($"[INIT] Initialization completed for {DatabaseName}. Users in DB: {userCount}");
        
        lock (DatabaseLock)
        {
            DatabaseSeeded = true;
        }
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
        Console.WriteLine($"[AUTH TOKEN] Attempting to get auth token for {email}");
        
        using var scope = Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BarqDbContext>();
        var userExists = await context.Users.AnyAsync(u => u.Email == email);
        var userCount = await context.Users.CountAsync();
        Console.WriteLine($"[AUTH TOKEN] User exists: {userExists}, Total users in DB: {userCount}");
        
        var loginRequest = new { Request = new { Email = email, Password = password } };
        var response = await PostJsonAsync("/api/auth/login", loginRequest);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"[AUTH TOKEN] Login failed with status {response.StatusCode}: {errorContent}");
        }
        
        response.Should().BeSuccessful();
        var apiResponse = await DeserializeResponseAsync<ApiResponse<AuthenticationResponse>>(response);
        return apiResponse?.Data?.AccessToken ?? throw new InvalidOperationException("Failed to get auth token");
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
        Console.WriteLine($"[TEST SEEDING] Starting test data seeding at {DateTime.UtcNow}");
        
        try
        {
            if (await _context.Users.AnyAsync())
            {
                _context.Users.RemoveRange(await _context.Users.ToListAsync());
            }
            if (await _context.Organizations.AnyAsync())
            {
                _context.Organizations.RemoveRange(await _context.Organizations.ToListAsync());
            }
            if (await _context.Projects.AnyAsync())
            {
                _context.Projects.RemoveRange(await _context.Projects.ToListAsync());
            }
            await _context.SaveChangesAsync();
            Console.WriteLine($"[TEST SEEDING] Cleared existing data successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[TEST SEEDING] Error clearing existing data: {ex.Message}");
        }
        

        var acmeOrgId = new Guid("11111111-1111-1111-1111-111111111111");
        var betaOrgId = new Guid("22222222-2222-2222-2222-222222222222");
        var acmeUserId = Guid.NewGuid();
        var betaUserId = Guid.NewGuid();
        var acmeProjectId = Guid.NewGuid();
        var betaProjectId = Guid.NewGuid();
        
        Console.WriteLine($"[TEST SEEDING] Generated IDs - AcmeOrg: {acmeOrgId}, AcmeUser: {acmeUserId}");

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
            TwoFactorEnabled = false,
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
            TwoFactorEnabled = false,
            CreatedAt = DateTime.UtcNow,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("TestPassword123!")
        };

        _context.Users.AddRange(acmeUser, betaUser);
        Console.WriteLine($"[TEST SEEDING] Added users to context. AcmeUser email: {acmeUser.Email}, password hash length: {acmeUser.PasswordHash?.Length}");

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

        try
        {
            await _context.SaveChangesAsync();
            
            var userCount = await _context.Users.CountAsync();
            var orgCount = await _context.Organizations.CountAsync();
            Console.WriteLine($"[TEST SEEDING] Seeding completed. Users: {userCount}, Organizations: {orgCount}");
            
            var testUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == "test@acme.com");
            Console.WriteLine($"[TEST SEEDING] Test user found: {testUser != null}, Email: {testUser?.Email}, TenantId: {testUser?.TenantId}");
            
            if (testUser == null)
            {
                Console.WriteLine($"[TEST SEEDING] ERROR: Test user not found after seeding!");
                var allUsers = await _context.Users.ToListAsync();
                Console.WriteLine($"[TEST SEEDING] All users in DB: {string.Join(", ", allUsers.Select(u => u.Email))}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[TEST SEEDING] Error during final save: {ex.Message}");
            Console.WriteLine($"[TEST SEEDING] Stack trace: {ex.StackTrace}");
            throw;
        }
    }
}

public class TestTenantProvider : ITenantProvider
{
    private Guid _tenantId;
    private string _tenantName = "Test Tenant";
    private Guid _currentUserId;
    
    // Use a static tenant ID that matches the seeded organization
    private static readonly Guid AcmeOrgId = new Guid("11111111-1111-1111-1111-111111111111");

    public TestTenantProvider()
    {
        _tenantId = AcmeOrgId;
        _currentUserId = Guid.NewGuid();
    }

    public Guid GetTenantId()
    {
        return _tenantId;
    }

    public void SetTenantId(Guid tenantId)
    {
        _tenantId = tenantId;
        Console.WriteLine($"[TEST TENANT] Tenant ID set to: {tenantId}");
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
