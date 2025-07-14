using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BARQ.Infrastructure.Data;
using BARQ.Core.Entities;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using FluentAssertions;
using Xunit;
using Microsoft.AspNetCore.Hosting;

namespace BARQ.Testing.Framework;

public class ApiTestFramework : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly string _connectionString = "Server=localhost,1433;Database=BarqTestDb;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=true;MultipleActiveResultSets=true;Encrypt=false;";
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<BarqDbContext>));
            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<BarqDbContext>(options =>
                options.UseInMemoryDatabase("TestDatabase"));

            services.AddScoped<ITestDataSeeder, TestDataSeeder>();
        });

        builder.UseEnvironment("Testing");
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
        var loginRequest = new { Email = email, Password = password };
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
        if (await _context.Organizations.AnyAsync())
            return;

        var acmeOrg = new Organization
        {
            Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
            Name = "Acme Corporation",
            Domain = "acme.com",
            SubscriptionPlan = Core.Enums.SubscriptionPlan.Professional,
            Status = BARQ.Core.Enums.OrganizationStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        var betaOrg = new Organization
        {
            Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
            Name = "Beta Industries",
            Domain = "beta.com",
            SubscriptionPlan = Core.Enums.SubscriptionPlan.Enterprise,
            Status = BARQ.Core.Enums.OrganizationStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        _context.Organizations.AddRange(acmeOrg, betaOrg);

        var acmeUser = new User
        {
            Id = Guid.Parse("33333333-3333-3333-3333-333333333333"),
            Email = "test@acme.com",
            FirstName = "John",
            LastName = "Doe",
            TenantId = acmeOrg.Id,
            Status = BARQ.Core.Enums.UserStatus.Active,
            EmailVerified = true,
            CreatedAt = DateTime.UtcNow
        };

        var betaUser = new User
        {
            Id = Guid.Parse("44444444-4444-4444-4444-444444444444"),
            Email = "test@beta.com",
            FirstName = "Jane",
            LastName = "Smith",
            TenantId = betaOrg.Id,
            Status = BARQ.Core.Enums.UserStatus.Active,
            EmailVerified = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.AddRange(acmeUser, betaUser);

        var acmeProject = new Project
        {
            Id = Guid.Parse("55555555-5555-5555-5555-555555555555"),
            Name = "Acme Project",
            Description = "Test project for Acme",
            TenantId = acmeOrg.Id,
            CreatedById = acmeUser.Id,
            Status = Core.Enums.ProjectStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        var betaProject = new Project
        {
            Id = Guid.Parse("66666666-6666-6666-6666-666666666666"),
            Name = "Beta Project",
            Description = "Test project for Beta",
            TenantId = betaOrg.Id,
            CreatedById = betaUser.Id,
            Status = Core.Enums.ProjectStatus.Active,
            CreatedAt = DateTime.UtcNow
        };

        _context.Projects.AddRange(acmeProject, betaProject);

        await _context.SaveChangesAsync();
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
