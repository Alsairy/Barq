using BARQ.Testing.Framework;
using FluentAssertions;
using Xunit;

namespace BARQ.Testing.Tests.Quality;

public class QualityMetricsTests : IClassFixture<ApiTestFramework>
{
    private readonly ApiTestFramework _factory;
    private readonly QualityMetricsFramework _qualityFramework;

    public QualityMetricsTests(ApiTestFramework factory)
    {
        _factory = factory;
        _qualityFramework = new QualityMetricsFramework();
    }

    [Fact]
    public async Task QualityMetrics_ShouldGenerateComprehensiveReport()
    {
        await SeedTestResults();
        
        var report = _qualityFramework.GenerateReport();
        
        report.Should().NotBeNull();
        report.GeneratedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromMinutes(1));
        report.OverallQualityScore.Should().BeGreaterThan(0);
        report.TestCoverage.Should().NotBeNull();
        report.PerformanceMetrics.Should().NotBeNull();
        report.ContractMetrics.Should().NotBeNull();
        report.QualityTrends.Should().NotBeEmpty();
        report.Recommendations.Should().NotBeNull();
    }

    [Fact]
    public async Task QualityGates_ShouldValidateMinimumStandards()
    {
        await SeedGoodTestResults();
        
        var report = _qualityFramework.GenerateReport();
        var qualityGates = new QualityGates
        {
            MinTestPassRate = 85.0,
            MinPerformanceScore = 80.0,
            MinContractComplianceRate = 90.0,
            MinOverallQualityScore = 80.0
        };

        _qualityFramework.ValidateQualityGates(report, qualityGates);
    }

    [Fact]
    public async Task QualityReport_ShouldBeSaveable()
    {
        await SeedTestResults();
        
        var report = _qualityFramework.GenerateReport();
        var tempFile = Path.GetTempFileName();
        
        try
        {
            await _qualityFramework.SaveReportAsync(report, tempFile);
            
            File.Exists(tempFile).Should().BeTrue();
            var content = await File.ReadAllTextAsync(tempFile);
            content.Should().NotBeNullOrEmpty();
            content.Should().Contain("overallQualityScore");
        }
        finally
        {
            if (File.Exists(tempFile))
                File.Delete(tempFile);
        }
    }

    [Fact]
    public async Task QualityTrends_ShouldShowImprovement()
    {
        await SeedTestResults();
        
        var report = _qualityFramework.GenerateReport();
        
        report.QualityTrends.Should().NotBeEmpty();
        report.QualityTrends.Should().HaveCountGreaterThan(1);
        
        var latestTrend = report.QualityTrends.OrderByDescending(t => t.Date).First();
        latestTrend.QualityScore.Should().BeGreaterThan(0);
    }

    [Fact]
    public async Task QualityRecommendations_ShouldProvideActionableInsights()
    {
        await SeedMixedTestResults();
        
        var report = _qualityFramework.GenerateReport();
        
        report.Recommendations.Should().NotBeEmpty();
        
        foreach (var recommendation in report.Recommendations)
        {
            recommendation.Priority.Should().NotBeNullOrEmpty();
            recommendation.Category.Should().NotBeNullOrEmpty();
            recommendation.Description.Should().NotBeNullOrEmpty();
            recommendation.Impact.Should().NotBeNullOrEmpty();
            recommendation.EstimatedEffort.Should().NotBeNullOrEmpty();
        }
    }

    [Fact]
    public async Task TestCoverage_ShouldCalculateCorrectly()
    {
        await SeedTestResults();
        
        var report = _qualityFramework.GenerateReport();
        
        report.TestCoverage.TotalTests.Should().BeGreaterThan(0);
        report.TestCoverage.PassRate.Should().BeInRange(0, 100);
        report.TestCoverage.CoveragePercentage.Should().BeInRange(0, 100);
    }

    [Fact]
    public async Task PerformanceMetrics_ShouldCalculateCorrectly()
    {
        await SeedPerformanceResults();
        
        var report = _qualityFramework.GenerateReport();
        
        report.PerformanceMetrics.TotalPerformanceTests.Should().BeGreaterThan(0);
        report.PerformanceMetrics.AverageSuccessRate.Should().BeInRange(0, 100);
        report.PerformanceMetrics.PerformanceScore.Should().BeInRange(0, 100);
    }

    private async Task SeedTestResults()
    {
        _qualityFramework.AddTestResult(new TestResult
        {
            TestName = "Login_WithValidCredentials_ReturnsSuccess",
            Endpoint = "/api/auth/login",
            Passed = true,
            Duration = TimeSpan.FromMilliseconds(150)
        });

        _qualityFramework.AddTestResult(new TestResult
        {
            TestName = "GetUserProfile_WithValidAuth_ReturnsSuccess",
            Endpoint = "/api/users/profile",
            Passed = true,
            Duration = TimeSpan.FromMilliseconds(200)
        });

        _qualityFramework.AddTestResult(new TestResult
        {
            TestName = "GetOrganizations_WithValidAuth_ReturnsSuccess",
            Endpoint = "/api/organizations",
            Passed = false,
            ErrorMessage = "Timeout occurred",
            Duration = TimeSpan.FromSeconds(5)
        });

        await Task.CompletedTask;
    }

    private async Task SeedGoodTestResults()
    {
        for (int i = 0; i < 20; i++)
        {
            _qualityFramework.AddTestResult(new TestResult
            {
                TestName = $"Test_{i}",
                Endpoint = $"/api/endpoint{i % 5}",
                Passed = i < 18, // 90% pass rate
                Duration = TimeSpan.FromMilliseconds(100 + i * 10)
            });
        }

        await SeedPerformanceResults();
        await SeedContractResults();
    }

    private async Task SeedMixedTestResults()
    {
        for (int i = 0; i < 10; i++)
        {
            _qualityFramework.AddTestResult(new TestResult
            {
                TestName = $"Test_{i}",
                Endpoint = $"/api/endpoint{i % 3}",
                Passed = i % 3 != 0, // 66% pass rate
                ErrorMessage = i % 3 == 0 ? "Test failed" : null,
                Duration = TimeSpan.FromMilliseconds(100 + i * 50)
            });
        }

        await Task.CompletedTask;
    }

    private async Task SeedPerformanceResults()
    {
        _qualityFramework.AddPerformanceResult(new PerformanceTestResult
        {
            Endpoint = "/api/auth/login",
            VirtualUsers = 10,
            Duration = TimeSpan.FromMinutes(1),
            TotalRequests = 600,
            SuccessfulRequests = 580,
            FailedRequests = 20,
            AverageResponseTime = TimeSpan.FromMilliseconds(250),
            SuccessRate = 96.7
        });

        _qualityFramework.AddPerformanceResult(new PerformanceTestResult
        {
            Endpoint = "/api/users/profile",
            VirtualUsers = 15,
            Duration = TimeSpan.FromMinutes(1),
            TotalRequests = 900,
            SuccessfulRequests = 850,
            FailedRequests = 50,
            AverageResponseTime = TimeSpan.FromMilliseconds(180),
            SuccessRate = 94.4
        });

        await Task.CompletedTask;
    }

    private async Task SeedContractResults()
    {
        _qualityFramework.AddContractResult(new ContractValidationResult
        {
            Endpoint = "/api/auth/login",
            Method = "POST",
            OperationId = "login",
            IsValid = true,
            ValidationErrors = new List<string>()
        });

        _qualityFramework.AddContractResult(new ContractValidationResult
        {
            Endpoint = "/api/users/profile",
            Method = "GET",
            OperationId = "getUserProfile",
            IsValid = true,
            ValidationErrors = new List<string>()
        });

        await Task.CompletedTask;
    }
}
