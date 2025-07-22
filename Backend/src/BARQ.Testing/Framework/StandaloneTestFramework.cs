using Microsoft.Extensions.DependencyInjection;
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
using Microsoft.Extensions.Logging;
using System.Net;

namespace BARQ.Testing.Framework;

public class StandaloneTestFramework : IAsyncLifetime
{
    private readonly string DatabaseName = $"TestDb_{Guid.NewGuid()}";
    private readonly object DatabaseLock = new object();
    private bool DatabaseSeeded = false;
    private ServiceProvider _serviceProvider = null!;
    
    private void ConfigureServices()
    {
        var services = new ServiceCollection();
        
        services.AddLogging(builder => builder.AddConsole());
        
        services.AddDbContext<BarqDbContext>(options =>
        {
            options.UseInMemoryDatabase(DatabaseName);
            options.EnableSensitiveDataLogging();
            options.EnableDetailedErrors();
        });

        services.AddScoped<ITenantProvider, TestTenantProvider>();
        services.AddScoped<ITestDataSeeder, TestDataSeeder>();
        
        services.AddScoped<BARQ.Core.Services.IAuthenticationService, BARQ.Application.Services.Authentication.AuthenticationService>();
        services.AddScoped<BARQ.Core.Services.IPasswordService, BARQ.Application.Services.Authentication.PasswordService>();
        services.AddScoped<BARQ.Core.Services.IMultiFactorAuthService, BARQ.Application.Services.Authentication.MultiFactorAuthService>();
        services.AddScoped<BARQ.Core.Services.IUserRoleService, BARQ.Application.Services.Users.UserRoleService>();
        services.AddScoped<BARQ.Core.Repositories.IUnitOfWork, BARQ.Infrastructure.Repositories.UnitOfWork>();
        
        services.AddScoped(typeof(BARQ.Core.Repositories.IRepository<>), typeof(BARQ.Infrastructure.Repositories.GenericRepository<>));
        
        _serviceProvider = services.BuildServiceProvider();
    }

    public async Task InitializeAsync()
    {
        Console.WriteLine($"[INIT] Starting standalone test framework initialization at {DateTime.UtcNow} for database {DatabaseName}");
        
        lock (DatabaseLock)
        {
            if (DatabaseSeeded)
            {
                Console.WriteLine($"[INIT] Database already seeded for {DatabaseName}, skipping initialization");
                return;
            }
        }
        
        ConfigureServices();
        
        using var scope = _serviceProvider.CreateScope();
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

    public async Task DisposeAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<BarqDbContext>();
        await context.Database.EnsureDeletedAsync();
        
        _serviceProvider?.Dispose();
    }

    public async Task<T?> GetServiceAsync<T>() where T : class
    {
        using var scope = _serviceProvider.CreateScope();
        return scope.ServiceProvider.GetService<T>();
    }

    public async Task<BarqDbContext> GetDbContextAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        return scope.ServiceProvider.GetRequiredService<BarqDbContext>();
    }

    public HttpClient CreateClient()
    {
        return new HttpClient { BaseAddress = new Uri("http://localhost:5000") };
    }

    public async Task<HttpResponseMessage> PostJsonAsync(string endpoint, object data, string? authToken = null)
    {
        var client = CreateClient();
        
        if (!string.IsNullOrEmpty(authToken))
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
        }

        var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        return await client.PostAsync(endpoint, content);
    }

    public async Task<HttpResponseMessage> GetAsync(string endpoint, string? authToken = null)
    {
        var client = CreateClient();
        
        if (!string.IsNullOrEmpty(authToken))
        {
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
        }
        
        return await client.GetAsync(endpoint);
    }

    public async Task<T?> DeserializeResponseAsync<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(content))
            return default(T);
            
        return JsonSerializer.Deserialize<T>(content, new JsonSerializerOptions 
        { 
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true
        });
    }

    public async Task<string> GetAuthTokenAsync(string email = "test@acme.com", string password = "TestPassword123!")
    {
        var loginRequest = new
        {
            Request = new
            {
                Email = email,
                Password = password
            }
        };

        var response = await PostJsonAsync("/api/auth/login", loginRequest);
        
        if (response.StatusCode == HttpStatusCode.OK)
        {
            var authResponse = await DeserializeResponseAsync<ApiResponse<AuthenticationResponse>>(response);
            return authResponse?.Data?.AccessToken ?? "mock-token";
        }
        
        return "mock-token";
    }
}
