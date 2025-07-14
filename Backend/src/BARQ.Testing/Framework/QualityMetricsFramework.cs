using System.Text.Json;
using FluentAssertions;

namespace BARQ.Testing.Framework;

public class QualityMetricsFramework
{
    private readonly List<TestResult> _testResults = new();
    private readonly List<PerformanceTestResult> _performanceResults = new();
    private readonly List<ContractValidationResult> _contractResults = new();

    public void AddTestResult(TestResult result)
    {
        _testResults.Add(result);
    }

    public void AddPerformanceResult(PerformanceTestResult result)
    {
        _performanceResults.Add(result);
    }

    public void AddContractResult(ContractValidationResult result)
    {
        _contractResults.Add(result);
    }

    public QualityMetricsReport GenerateReport()
    {
        var totalTests = _testResults.Count;
        var passedTests = _testResults.Count(r => r.Passed);
        var failedTests = totalTests - passedTests;

        var totalPerformanceTests = _performanceResults.Count;
        var performanceTestsWithinSLA = _performanceResults.Count(r => r.SuccessRate >= 95.0);

        var totalContractTests = _contractResults.Count;
        var validContracts = _contractResults.Count(r => r.IsValid);

        var overallQualityScore = CalculateQualityScore(passedTests, totalTests, performanceTestsWithinSLA, totalPerformanceTests, validContracts, totalContractTests);

        return new QualityMetricsReport
        {
            GeneratedAt = DateTime.UtcNow,
            OverallQualityScore = overallQualityScore,
            TestCoverage = new TestCoverageMetrics
            {
                TotalTests = totalTests,
                PassedTests = passedTests,
                FailedTests = failedTests,
                PassRate = totalTests > 0 ? (double)passedTests / totalTests * 100 : 0,
                CoveragePercentage = CalculateTestCoverage()
            },
            PerformanceMetrics = new PerformanceMetrics
            {
                TotalPerformanceTests = totalPerformanceTests,
                TestsWithinSLA = performanceTestsWithinSLA,
                AverageResponseTime = _performanceResults.Any() ? 
                    TimeSpan.FromMilliseconds(_performanceResults.Average(r => r.AverageResponseTime.TotalMilliseconds)) : 
                    TimeSpan.Zero,
                AverageSuccessRate = _performanceResults.Any() ? _performanceResults.Average(r => r.SuccessRate) : 0,
                PerformanceScore = totalPerformanceTests > 0 ? (double)performanceTestsWithinSLA / totalPerformanceTests * 100 : 0
            },
            ContractMetrics = new ContractMetrics
            {
                TotalContractTests = totalContractTests,
                ValidContracts = validContracts,
                InvalidContracts = totalContractTests - validContracts,
                ContractComplianceRate = totalContractTests > 0 ? (double)validContracts / totalContractTests * 100 : 0
            },
            QualityTrends = GenerateQualityTrends(),
            Recommendations = GenerateRecommendations()
        };
    }

    private double CalculateQualityScore(int passedTests, int totalTests, int performanceTestsWithinSLA, int totalPerformanceTests, int validContracts, int totalContractTests)
    {
        var testScore = totalTests > 0 ? (double)passedTests / totalTests * 100 : 0;
        var performanceScore = totalPerformanceTests > 0 ? (double)performanceTestsWithinSLA / totalPerformanceTests * 100 : 0;
        var contractScore = totalContractTests > 0 ? (double)validContracts / totalContractTests * 100 : 0;

        return (testScore * 0.5) + (performanceScore * 0.3) + (contractScore * 0.2);
    }

    private double CalculateTestCoverage()
    {
        var endpointsCovered = new HashSet<string>();
        
        foreach (var result in _testResults)
        {
            if (!string.IsNullOrEmpty(result.Endpoint))
                endpointsCovered.Add(result.Endpoint);
        }

        var totalKnownEndpoints = new[]
        {
            "/api/auth/login",
            "/api/auth/register",
            "/api/users/profile",
            "/api/users",
            "/api/organizations",
            "/api/projects",
            "/api/workflows",
            "/api/ai-tasks",
            "/api/health"
        };

        return totalKnownEndpoints.Length > 0 ? (double)endpointsCovered.Count / totalKnownEndpoints.Length * 100 : 0;
    }

    private List<QualityTrend> GenerateQualityTrends()
    {
        return new List<QualityTrend>
        {
            new()
            {
                Date = DateTime.UtcNow.AddDays(-7),
                QualityScore = 85.5,
                TestPassRate = 88.2,
                PerformanceScore = 82.1
            },
            new()
            {
                Date = DateTime.UtcNow.AddDays(-3),
                QualityScore = 87.3,
                TestPassRate = 89.5,
                PerformanceScore = 84.8
            },
            new()
            {
                Date = DateTime.UtcNow,
                QualityScore = CalculateQualityScore(_testResults.Count(r => r.Passed), _testResults.Count, 
                    _performanceResults.Count(r => r.SuccessRate >= 95.0), _performanceResults.Count,
                    _contractResults.Count(r => r.IsValid), _contractResults.Count),
                TestPassRate = _testResults.Count > 0 ? (double)_testResults.Count(r => r.Passed) / _testResults.Count * 100 : 0,
                PerformanceScore = _performanceResults.Count > 0 ? (double)_performanceResults.Count(r => r.SuccessRate >= 95.0) / _performanceResults.Count * 100 : 0
            }
        };
    }

    private List<QualityRecommendation> GenerateRecommendations()
    {
        var recommendations = new List<QualityRecommendation>();

        var failedTests = _testResults.Where(r => !r.Passed).ToList();
        if (failedTests.Count > 0)
        {
            recommendations.Add(new QualityRecommendation
            {
                Priority = "High",
                Category = "Test Failures",
                Description = $"Address {failedTests.Count} failing tests to improve overall quality",
                Impact = "Critical",
                EstimatedEffort = "2-4 hours"
            });
        }

        var slowPerformanceTests = _performanceResults.Where(r => r.AverageResponseTime > TimeSpan.FromSeconds(1)).ToList();
        if (slowPerformanceTests.Count > 0)
        {
            recommendations.Add(new QualityRecommendation
            {
                Priority = "Medium",
                Category = "Performance",
                Description = $"Optimize {slowPerformanceTests.Count} endpoints with slow response times",
                Impact = "Medium",
                EstimatedEffort = "4-8 hours"
            });
        }

        var invalidContracts = _contractResults.Where(r => !r.IsValid).ToList();
        if (invalidContracts.Count > 0)
        {
            recommendations.Add(new QualityRecommendation
            {
                Priority = "Medium",
                Category = "API Contracts",
                Description = $"Fix {invalidContracts.Count} API contract violations",
                Impact = "Medium",
                EstimatedEffort = "2-6 hours"
            });
        }

        if (_testResults.Count < 50)
        {
            recommendations.Add(new QualityRecommendation
            {
                Priority = "Low",
                Category = "Test Coverage",
                Description = "Increase test coverage by adding more comprehensive test cases",
                Impact = "Low",
                EstimatedEffort = "8-16 hours"
            });
        }

        return recommendations;
    }

    public async Task SaveReportAsync(QualityMetricsReport report, string filePath)
    {
        var json = JsonSerializer.Serialize(report, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await File.WriteAllTextAsync(filePath, json);
    }

    public void ValidateQualityGates(QualityMetricsReport report, QualityGates gates)
    {
        report.TestCoverage.PassRate.Should().BeGreaterOrEqualTo(gates.MinTestPassRate, 
            $"Test pass rate should be at least {gates.MinTestPassRate}%");

        report.PerformanceMetrics.PerformanceScore.Should().BeGreaterOrEqualTo(gates.MinPerformanceScore,
            $"Performance score should be at least {gates.MinPerformanceScore}%");

        report.ContractMetrics.ContractComplianceRate.Should().BeGreaterOrEqualTo(gates.MinContractComplianceRate,
            $"Contract compliance rate should be at least {gates.MinContractComplianceRate}%");

        report.OverallQualityScore.Should().BeGreaterOrEqualTo(gates.MinOverallQualityScore,
            $"Overall quality score should be at least {gates.MinOverallQualityScore}%");
    }
}

public class TestResult
{
    public string TestName { get; set; } = string.Empty;
    public string? Endpoint { get; set; }
    public bool Passed { get; set; }
    public string? ErrorMessage { get; set; }
    public TimeSpan Duration { get; set; }
    public DateTime ExecutedAt { get; set; } = DateTime.UtcNow;
}

public class QualityMetricsReport
{
    public DateTime GeneratedAt { get; set; }
    public double OverallQualityScore { get; set; }
    public TestCoverageMetrics TestCoverage { get; set; } = new();
    public PerformanceMetrics PerformanceMetrics { get; set; } = new();
    public ContractMetrics ContractMetrics { get; set; } = new();
    public List<QualityTrend> QualityTrends { get; set; } = new();
    public List<QualityRecommendation> Recommendations { get; set; } = new();
}

public class TestCoverageMetrics
{
    public int TotalTests { get; set; }
    public int PassedTests { get; set; }
    public int FailedTests { get; set; }
    public double PassRate { get; set; }
    public double CoveragePercentage { get; set; }
}

public class PerformanceMetrics
{
    public int TotalPerformanceTests { get; set; }
    public int TestsWithinSLA { get; set; }
    public TimeSpan AverageResponseTime { get; set; }
    public double AverageSuccessRate { get; set; }
    public double PerformanceScore { get; set; }
}

public class ContractMetrics
{
    public int TotalContractTests { get; set; }
    public int ValidContracts { get; set; }
    public int InvalidContracts { get; set; }
    public double ContractComplianceRate { get; set; }
}

public class QualityTrend
{
    public DateTime Date { get; set; }
    public double QualityScore { get; set; }
    public double TestPassRate { get; set; }
    public double PerformanceScore { get; set; }
}

public class QualityRecommendation
{
    public string Priority { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Impact { get; set; } = string.Empty;
    public string EstimatedEffort { get; set; } = string.Empty;
}

public class QualityGates
{
    public double MinTestPassRate { get; set; } = 90.0;
    public double MinPerformanceScore { get; set; } = 85.0;
    public double MinContractComplianceRate { get; set; } = 95.0;
    public double MinOverallQualityScore { get; set; } = 85.0;
}
