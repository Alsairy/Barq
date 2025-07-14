using BARQ.Testing.Framework;
using FluentAssertions;
using Xunit;

namespace BARQ.Testing.Tests.Performance;

public class ApiPerformanceTests : IClassFixture<ApiTestFramework>
{
    private readonly ApiTestFramework _factory;
    private readonly PerformanceTestFramework _performanceFramework;

    public ApiPerformanceTests(ApiTestFramework factory)
    {
        _factory = factory;
        _performanceFramework = new PerformanceTestFramework(_factory.CreateClient(), "http://localhost");
    }

    [Fact]
    public async Task AuthenticationEndpoint_ShouldMeetPerformanceRequirements()
    {
        var loginRequest = new
        {
            Email = "test@acme.com",
            Password = "TestPassword123!"
        };

        var result = await _performanceFramework.RunConcurrencyTestAsync("/api/auth/login", loginRequest, 20);
        
        var requirements = new PerformanceRequirements
        {
            MaxAverageResponseTime = TimeSpan.FromMilliseconds(1000),
            MaxResponseTime = TimeSpan.FromSeconds(3),
            MinSuccessRate = 90.0,
            MinRequestsPerSecond = 5.0
        };

        _performanceFramework.ValidatePerformanceRequirements(result, requirements);
    }

    [Fact]
    public async Task UserProfileEndpoint_ShouldMeetPerformanceRequirements()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
        
        var performanceFramework = new PerformanceTestFramework(client, "http://localhost");
        var result = await performanceFramework.RunLoadTestAsync("/api/users/profile", virtualUsers: 10, duration: TimeSpan.FromSeconds(30));
        
        var requirements = new PerformanceRequirements
        {
            MaxAverageResponseTime = TimeSpan.FromMilliseconds(500),
            MaxResponseTime = TimeSpan.FromSeconds(2),
            MinSuccessRate = 95.0,
            MinRequestsPerSecond = 10.0
        };

        performanceFramework.ValidatePerformanceRequirements(result, requirements);
    }

    [Fact]
    public async Task OrganizationEndpoint_ShouldMeetPerformanceRequirements()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
        
        var performanceFramework = new PerformanceTestFramework(client, "http://localhost");
        var result = await performanceFramework.RunLoadTestAsync("/api/organizations", virtualUsers: 15, duration: TimeSpan.FromSeconds(45));
        
        var requirements = new PerformanceRequirements
        {
            MaxAverageResponseTime = TimeSpan.FromMilliseconds(800),
            MaxResponseTime = TimeSpan.FromSeconds(3),
            MinSuccessRate = 92.0,
            MinRequestsPerSecond = 8.0
        };

        performanceFramework.ValidatePerformanceRequirements(result, requirements);
    }

    [Fact]
    public async Task ProjectEndpoint_ShouldMeetPerformanceRequirements()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
        
        var performanceFramework = new PerformanceTestFramework(client, "http://localhost");
        var result = await performanceFramework.RunLoadTestAsync("/api/projects", virtualUsers: 12, duration: TimeSpan.FromSeconds(40));
        
        var requirements = new PerformanceRequirements
        {
            MaxAverageResponseTime = TimeSpan.FromMilliseconds(600),
            MaxResponseTime = TimeSpan.FromSeconds(2.5),
            MinSuccessRate = 93.0,
            MinRequestsPerSecond = 9.0
        };

        performanceFramework.ValidatePerformanceRequirements(result, requirements);
    }

    [Fact]
    public async Task HealthCheckEndpoint_ShouldMeetPerformanceRequirements()
    {
        var result = await _performanceFramework.RunLoadTestAsync("/api/health", virtualUsers: 25, duration: TimeSpan.FromSeconds(20));
        
        var requirements = new PerformanceRequirements
        {
            MaxAverageResponseTime = TimeSpan.FromMilliseconds(200),
            MaxResponseTime = TimeSpan.FromMilliseconds(500),
            MinSuccessRate = 99.0,
            MinRequestsPerSecond = 20.0
        };

        _performanceFramework.ValidatePerformanceRequirements(result, requirements);
    }

    [Fact]
    public async Task CriticalEndpoints_ShouldPassPerformanceSuite()
    {
        var authToken = await _factory.GetAuthTokenAsync();
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", authToken);
        
        var performanceFramework = new PerformanceTestFramework(client, "http://localhost");
        
        var endpoints = new List<string>
        {
            "/api/users/profile",
            "/api/organizations",
            "/api/projects",
            "/api/health"
        };

        var results = await performanceFramework.RunEndpointPerformanceSuiteAsync(endpoints);
        
        results.Should().NotBeEmpty();
        results.Should().HaveCount(endpoints.Count);
        
        foreach (var result in results)
        {
            result.SuccessRate.Should().BeGreaterThan(85.0, $"Endpoint {result.Endpoint} should have success rate > 85%");
            result.AverageResponseTime.Should().BeLessThan(TimeSpan.FromSeconds(2), $"Endpoint {result.Endpoint} should respond within 2 seconds on average");
        }
    }

    [Fact]
    public async Task StressTest_ShouldHandleHighLoad()
    {
        var result = await _performanceFramework.RunStressTestAsync("/api/health", maxUsers: 50, rampUpTime: TimeSpan.FromMinutes(1));
        
        result.SuccessRate.Should().BeGreaterThan(80.0, "System should maintain > 80% success rate under stress");
        result.AverageResponseTime.Should().BeLessThan(TimeSpan.FromSeconds(5), "Average response time should be < 5 seconds under stress");
    }
}
