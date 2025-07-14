using NBomber.Contracts;
using NBomber.CSharp;
using System.Net.Http;
using System.Text.Json;
using System.Text;
using FluentAssertions;

namespace BARQ.Testing.Framework;

public class PerformanceTestFramework
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public PerformanceTestFramework(HttpClient httpClient, string baseUrl)
    {
        _httpClient = httpClient;
        _baseUrl = baseUrl;
    }

    public async Task<PerformanceTestResult> RunLoadTestAsync(string endpoint, int virtualUsers = 10, TimeSpan? duration = null)
    {
        duration ??= TimeSpan.FromMinutes(1);

        var scenario = Scenario.Create($"load_test_{endpoint.Replace("/", "_")}", async context =>
        {
            var response = await _httpClient.GetAsync(endpoint);
            
            return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
        })
        .WithLoadSimulations(
            Simulation.Inject(rate: virtualUsers, interval: TimeSpan.FromSeconds(1), during: duration.Value)
        );

        var stats = NBomberRunner
            .RegisterScenarios(scenario)
            .Run();

        var sceneStats = stats.ScenarioStats.First();
        
        return new PerformanceTestResult
        {
            Endpoint = endpoint,
            VirtualUsers = virtualUsers,
            Duration = duration.Value,
            TotalRequests = sceneStats.Ok.Request.Count + sceneStats.Fail.Request.Count,
            SuccessfulRequests = sceneStats.Ok.Request.Count,
            FailedRequests = sceneStats.Fail.Request.Count,
            AverageResponseTime = TimeSpan.FromMilliseconds(sceneStats.Ok.Latency.Percent50),
            MinResponseTime = TimeSpan.FromMilliseconds(sceneStats.Ok.Latency.MinMs),
            MaxResponseTime = TimeSpan.FromMilliseconds(sceneStats.Ok.Latency.MaxMs),
            RequestsPerSecond = sceneStats.Ok.Request.Count / duration.Value.TotalSeconds,
            SuccessRate = (double)sceneStats.Ok.Request.Count / (sceneStats.Ok.Request.Count + sceneStats.Fail.Request.Count) * 100
        };
    }

    public async Task<PerformanceTestResult> RunStressTestAsync(string endpoint, int maxUsers = 100, TimeSpan? rampUpTime = null)
    {
        rampUpTime ??= TimeSpan.FromMinutes(2);

        var scenario = Scenario.Create($"stress_test_{endpoint.Replace("/", "_")}", async context =>
        {
            var response = await _httpClient.GetAsync(endpoint);
            return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
        })
        .WithLoadSimulations(
            Simulation.RampingInject(rate: maxUsers, interval: TimeSpan.FromSeconds(1), during: rampUpTime.Value)
        );

        var stats = NBomberRunner
            .RegisterScenarios(scenario)
            .Run();

        var sceneStats = stats.ScenarioStats.First();

        return new PerformanceTestResult
        {
            Endpoint = endpoint,
            VirtualUsers = maxUsers,
            Duration = rampUpTime.Value,
            TotalRequests = sceneStats.Ok.Request.Count + sceneStats.Fail.Request.Count,
            SuccessfulRequests = sceneStats.Ok.Request.Count,
            FailedRequests = sceneStats.Fail.Request.Count,
            AverageResponseTime = TimeSpan.FromMilliseconds(sceneStats.Ok.Latency.Percent50),
            MinResponseTime = TimeSpan.FromMilliseconds(sceneStats.Ok.Latency.MinMs),
            MaxResponseTime = TimeSpan.FromMilliseconds(sceneStats.Ok.Latency.MaxMs),
            RequestsPerSecond = sceneStats.Ok.Request.Count / rampUpTime.Value.TotalSeconds,
            SuccessRate = (double)sceneStats.Ok.Request.Count / (sceneStats.Ok.Request.Count + sceneStats.Fail.Request.Count) * 100
        };
    }

    public async Task<List<PerformanceTestResult>> RunEndpointPerformanceSuiteAsync(List<string> endpoints)
    {
        var results = new List<PerformanceTestResult>();

        foreach (var endpoint in endpoints)
        {
            var loadTestResult = await RunLoadTestAsync(endpoint, virtualUsers: 5, duration: TimeSpan.FromSeconds(30));
            results.Add(loadTestResult);

            await Task.Delay(TimeSpan.FromSeconds(5));
        }

        return results;
    }

    public async Task<PerformanceTestResult> RunConcurrencyTestAsync<T>(string endpoint, T requestData, int concurrentRequests = 50)
    {
        var startTime = DateTime.UtcNow;
        var tasks = new List<Task<(bool Success, TimeSpan ResponseTime)>>();

        for (int i = 0; i < concurrentRequests; i++)
        {
            tasks.Add(ExecuteRequestAsync(endpoint, requestData));
        }

        var results = await Task.WhenAll(tasks);
        var endTime = DateTime.UtcNow;

        var successfulRequests = results.Count(r => r.Success);
        var responseTimes = results.Where(r => r.Success).Select(r => r.ResponseTime).ToList();

        return new PerformanceTestResult
        {
            Endpoint = endpoint,
            VirtualUsers = concurrentRequests,
            Duration = endTime - startTime,
            TotalRequests = concurrentRequests,
            SuccessfulRequests = successfulRequests,
            FailedRequests = concurrentRequests - successfulRequests,
            AverageResponseTime = responseTimes.Any() ? TimeSpan.FromMilliseconds(responseTimes.Average(t => t.TotalMilliseconds)) : TimeSpan.Zero,
            MinResponseTime = responseTimes.Any() ? responseTimes.Min() : TimeSpan.Zero,
            MaxResponseTime = responseTimes.Any() ? responseTimes.Max() : TimeSpan.Zero,
            RequestsPerSecond = concurrentRequests / (endTime - startTime).TotalSeconds,
            SuccessRate = (double)successfulRequests / concurrentRequests * 100
        };
    }

    private async Task<(bool Success, TimeSpan ResponseTime)> ExecuteRequestAsync<T>(string endpoint, T requestData)
    {
        var startTime = DateTime.UtcNow;
        
        try
        {
            var json = JsonSerializer.Serialize(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(endpoint, content);
            
            var endTime = DateTime.UtcNow;
            return (response.IsSuccessStatusCode, endTime - startTime);
        }
        catch
        {
            var endTime = DateTime.UtcNow;
            return (false, endTime - startTime);
        }
    }

    public void ValidatePerformanceRequirements(PerformanceTestResult result, PerformanceRequirements requirements)
    {
        result.AverageResponseTime.Should().BeLessOrEqualTo(requirements.MaxAverageResponseTime, 
            $"Average response time for {result.Endpoint} should be under {requirements.MaxAverageResponseTime.TotalMilliseconds}ms");

        result.MaxResponseTime.Should().BeLessOrEqualTo(requirements.MaxResponseTime,
            $"Maximum response time for {result.Endpoint} should be under {requirements.MaxResponseTime.TotalMilliseconds}ms");

        result.SuccessRate.Should().BeGreaterOrEqualTo(requirements.MinSuccessRate,
            $"Success rate for {result.Endpoint} should be at least {requirements.MinSuccessRate}%");

        result.RequestsPerSecond.Should().BeGreaterOrEqualTo(requirements.MinRequestsPerSecond,
            $"Requests per second for {result.Endpoint} should be at least {requirements.MinRequestsPerSecond}");
    }
}

public class PerformanceTestResult
{
    public string Endpoint { get; set; } = string.Empty;
    public int VirtualUsers { get; set; }
    public TimeSpan Duration { get; set; }
    public int TotalRequests { get; set; }
    public int SuccessfulRequests { get; set; }
    public int FailedRequests { get; set; }
    public TimeSpan AverageResponseTime { get; set; }
    public TimeSpan MinResponseTime { get; set; }
    public TimeSpan MaxResponseTime { get; set; }
    public double RequestsPerSecond { get; set; }
    public double SuccessRate { get; set; }
}

public class PerformanceRequirements
{
    public TimeSpan MaxAverageResponseTime { get; set; } = TimeSpan.FromMilliseconds(500);
    public TimeSpan MaxResponseTime { get; set; } = TimeSpan.FromSeconds(2);
    public double MinSuccessRate { get; set; } = 95.0;
    public double MinRequestsPerSecond { get; set; } = 10.0;
}
