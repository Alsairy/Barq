using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BARQ.Infrastructure.Data;
using System.Diagnostics;
using System.Data.Common;

namespace BARQ.Infrastructure.Performance;

public interface IDatabasePerformanceService
{
    Task<DatabasePerformanceMetrics> GetPerformanceMetricsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<SlowQueryInfo>> GetSlowQueriesAsync(TimeSpan threshold, CancellationToken cancellationToken = default);
    Task<ConnectionPoolMetrics> GetConnectionPoolMetricsAsync(CancellationToken cancellationToken = default);
    Task OptimizeQueryAsync(string query, CancellationToken cancellationToken = default);
    Task<QueryExecutionPlan> GetExecutionPlanAsync(string query, CancellationToken cancellationToken = default);
    Task<IEnumerable<IndexRecommendation>> GetIndexRecommendationsAsync(CancellationToken cancellationToken = default);
    Task RebuildIndexesAsync(CancellationToken cancellationToken = default);
    Task UpdateStatisticsAsync(CancellationToken cancellationToken = default);
}

public class DatabasePerformanceService : IDatabasePerformanceService
{
    private readonly BarqDbContext _context;
    private readonly ILogger<DatabasePerformanceService> _logger;
    private readonly DatabasePerformanceOptions _options;
    private readonly List<SlowQueryInfo> _slowQueries = new();

    public DatabasePerformanceService(
        BarqDbContext context,
        ILogger<DatabasePerformanceService> logger,
        IOptions<DatabasePerformanceOptions> options)
    {
        _context = context;
        _logger = logger;
        _options = options.Value;
    }

    public async Task<DatabasePerformanceMetrics> GetPerformanceMetricsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            
            var connectionTime = await MeasureConnectionTimeAsync(cancellationToken);
            
            var userCount = await _context.Users.CountAsync(cancellationToken);
            var organizationCount = await _context.Organizations.CountAsync(cancellationToken);
            var projectCount = await _context.Projects.CountAsync(cancellationToken);
            
            var databaseSize = await GetDatabaseSizeAsync(cancellationToken);
            
            stopwatch.Stop();
            
            return new DatabasePerformanceMetrics
            {
                ConnectionTime = connectionTime,
                QueryResponseTime = stopwatch.Elapsed,
                TotalRecords = userCount + organizationCount + projectCount,
                DatabaseSize = databaseSize,
                SlowQueryCount = _slowQueries.Count(q => q.Timestamp > DateTime.UtcNow.AddHours(-1)),
                AverageQueryTime = _slowQueries.Any() ? 
                    TimeSpan.FromMilliseconds(_slowQueries.Average(q => q.ExecutionTime.TotalMilliseconds)) : 
                    TimeSpan.Zero,
                ConnectionPoolSize = await GetConnectionPoolSizeAsync(cancellationToken),
                ActiveConnections = await GetActiveConnectionsAsync(cancellationToken),
                Timestamp = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting database performance metrics");
            throw;
        }
    }

    public async Task<IEnumerable<SlowQueryInfo>> GetSlowQueriesAsync(TimeSpan threshold, CancellationToken cancellationToken = default)
    {
        try
        {
            return _slowQueries
                .Where(q => q.ExecutionTime > threshold)
                .OrderByDescending(q => q.ExecutionTime)
                .Take(_options.MaxSlowQueryResults);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting slow queries");
            return Enumerable.Empty<SlowQueryInfo>();
        }
    }

    public async Task<ConnectionPoolMetrics> GetConnectionPoolMetricsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return new ConnectionPoolMetrics
            {
                MaxPoolSize = _options.MaxConnectionPoolSize,
                CurrentPoolSize = await GetConnectionPoolSizeAsync(cancellationToken),
                ActiveConnections = await GetActiveConnectionsAsync(cancellationToken),
                IdleConnections = await GetIdleConnectionsAsync(cancellationToken),
                ConnectionsCreated = await GetConnectionsCreatedAsync(cancellationToken),
                ConnectionsDestroyed = await GetConnectionsDestroyedAsync(cancellationToken),
                Timestamp = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting connection pool metrics");
            throw;
        }
    }

    public async Task OptimizeQueryAsync(string query, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Optimizing query: {Query}", query);
            
            var executionPlan = await GetExecutionPlanAsync(query, cancellationToken);
            
            if (executionPlan.EstimatedCost > _options.HighCostThreshold)
            {
                _logger.LogWarning("High-cost query detected: {Query}, Cost: {Cost}", 
                    query, executionPlan.EstimatedCost);
            }
            
            var indexRecommendations = await GetIndexRecommendationsAsync(cancellationToken);
            foreach (var recommendation in indexRecommendations)
            {
                _logger.LogInformation("Index recommendation: {Table}.{Columns}", 
                    recommendation.TableName, string.Join(", ", recommendation.ColumnNames));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error optimizing query: {Query}", query);
        }
    }

    public async Task<QueryExecutionPlan> GetExecutionPlanAsync(string query, CancellationToken cancellationToken = default)
    {
        try
        {
            var stopwatch = Stopwatch.StartNew();
            
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = $"EXPLAIN QUERY PLAN {query}";
            
            if (_context.Database.GetDbConnection().State != System.Data.ConnectionState.Open)
            {
                await _context.Database.GetDbConnection().OpenAsync(cancellationToken);
            }
            
            using var reader = await command.ExecuteReaderAsync(cancellationToken);
            var planSteps = new List<string>();
            
            while (await reader.ReadAsync(cancellationToken))
            {
                planSteps.Add(reader.GetString(0));
            }
            
            stopwatch.Stop();
            
            return new QueryExecutionPlan
            {
                Query = query,
                PlanSteps = planSteps,
                EstimatedCost = CalculateEstimatedCost(planSteps),
                EstimatedRows = CalculateEstimatedRows(planSteps),
                ExecutionTime = stopwatch.Elapsed,
                Timestamp = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting execution plan for query: {Query}", query);
            return new QueryExecutionPlan
            {
                Query = query,
                PlanSteps = new List<string> { "Error retrieving execution plan" },
                EstimatedCost = 0,
                EstimatedRows = 0,
                ExecutionTime = TimeSpan.Zero,
                Timestamp = DateTime.UtcNow
            };
        }
    }

    public async Task<IEnumerable<IndexRecommendation>> GetIndexRecommendationsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var recommendations = new List<IndexRecommendation>();
            
            var slowQueries = await GetSlowQueriesAsync(TimeSpan.FromSeconds(_options.SlowQueryThresholdSeconds), cancellationToken);
            
            foreach (var slowQuery in slowQueries)
            {
                var recommendation = AnalyzeQueryForIndexes(slowQuery);
                if (recommendation != null)
                {
                    recommendations.Add(recommendation);
                }
            }
            
            return recommendations.DistinctBy(r => $"{r.TableName}_{string.Join("_", r.ColumnNames)}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting index recommendations");
            return Enumerable.Empty<IndexRecommendation>();
        }
    }

    public async Task RebuildIndexesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Starting index rebuild process");
            
            var tables = new[] { "Users", "Organizations", "Projects", "WorkflowInstances" };
            
            foreach (var table in tables)
            {
                try
                {
                    await _context.Database.ExecuteSqlRawAsync($"REINDEX TABLE {table}", cancellationToken);
                    _logger.LogInformation("Rebuilt indexes for table: {Table}", table);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error rebuilding indexes for table: {Table}", table);
                }
            }
            
            _logger.LogInformation("Index rebuild process completed");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during index rebuild process");
        }
    }

    public async Task UpdateStatisticsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Updating database statistics");
            
            await _context.Database.ExecuteSqlRawAsync("ANALYZE", cancellationToken);
            
            _logger.LogInformation("Database statistics updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating database statistics");
        }
    }

    private async Task<TimeSpan> MeasureConnectionTimeAsync(CancellationToken cancellationToken)
    {
        var stopwatch = Stopwatch.StartNew();
        await _context.Database.CanConnectAsync(cancellationToken);
        stopwatch.Stop();
        return stopwatch.Elapsed;
    }

    private async Task<long> GetDatabaseSizeAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var command = _context.Database.GetDbConnection().CreateCommand();
            command.CommandText = "SELECT page_count * page_size as size FROM pragma_page_count(), pragma_page_size()";
            
            if (_context.Database.GetDbConnection().State != System.Data.ConnectionState.Open)
            {
                await _context.Database.GetDbConnection().OpenAsync(cancellationToken);
            }
            
            var result = await command.ExecuteScalarAsync(cancellationToken);
            return result != null ? Convert.ToInt64(result) : 0;
        }
        catch
        {
            return 0;
        }
    }

    private async Task<int> GetConnectionPoolSizeAsync(CancellationToken cancellationToken)
    {
        return _options.MaxConnectionPoolSize;
    }

    private async Task<int> GetActiveConnectionsAsync(CancellationToken cancellationToken)
    {
        return 1; // Simplified for demo
    }

    private async Task<int> GetIdleConnectionsAsync(CancellationToken cancellationToken)
    {
        return 0; // Simplified for demo
    }

    private async Task<long> GetConnectionsCreatedAsync(CancellationToken cancellationToken)
    {
        return 0; // Simplified for demo
    }

    private async Task<long> GetConnectionsDestroyedAsync(CancellationToken cancellationToken)
    {
        return 0; // Simplified for demo
    }

    private double CalculateEstimatedCost(List<string> planSteps)
    {
        return planSteps.Count * 10.0;
    }

    private long CalculateEstimatedRows(List<string> planSteps)
    {
        return 1000;
    }

    private IndexRecommendation? AnalyzeQueryForIndexes(SlowQueryInfo slowQuery)
    {
        if (slowQuery.Query.Contains("WHERE") && slowQuery.ExecutionTime > TimeSpan.FromSeconds(2))
        {
            return new IndexRecommendation
            {
                TableName = "Users", // Simplified
                ColumnNames = new[] { "Email", "TenantId" },
                Reason = "Frequent WHERE clause usage",
                EstimatedImprovement = "50% query time reduction",
                Priority = IndexPriority.High
            };
        }
        
        return null;
    }

    public void TrackSlowQuery(string query, TimeSpan executionTime, string? executionPlan = null)
    {
        if (executionTime > TimeSpan.FromSeconds(_options.SlowQueryThresholdSeconds))
        {
            _slowQueries.Add(new SlowQueryInfo
            {
                Query = query,
                ExecutionTime = executionTime,
                ExecutionPlan = executionPlan,
                Timestamp = DateTime.UtcNow
            });

            var cutoff = DateTime.UtcNow.AddHours(-_options.SlowQueryRetentionHours);
            _slowQueries.RemoveAll(q => q.Timestamp < cutoff);

            _logger.LogWarning("Slow query detected: {Query}, Execution time: {ExecutionTime}ms", 
                query, executionTime.TotalMilliseconds);
        }
    }
}

public class DatabasePerformanceMetrics
{
    public TimeSpan ConnectionTime { get; set; }
    public TimeSpan QueryResponseTime { get; set; }
    public long TotalRecords { get; set; }
    public long DatabaseSize { get; set; }
    public int SlowQueryCount { get; set; }
    public TimeSpan AverageQueryTime { get; set; }
    public int ConnectionPoolSize { get; set; }
    public int ActiveConnections { get; set; }
    public DateTime Timestamp { get; set; }
}

public class SlowQueryInfo
{
    public string Query { get; set; } = string.Empty;
    public TimeSpan ExecutionTime { get; set; }
    public string? ExecutionPlan { get; set; }
    public DateTime Timestamp { get; set; }
}

public class ConnectionPoolMetrics
{
    public int MaxPoolSize { get; set; }
    public int CurrentPoolSize { get; set; }
    public int ActiveConnections { get; set; }
    public int IdleConnections { get; set; }
    public long ConnectionsCreated { get; set; }
    public long ConnectionsDestroyed { get; set; }
    public DateTime Timestamp { get; set; }
}

public class QueryExecutionPlan
{
    public string Query { get; set; } = string.Empty;
    public List<string> PlanSteps { get; set; } = new();
    public double EstimatedCost { get; set; }
    public long EstimatedRows { get; set; }
    public TimeSpan ExecutionTime { get; set; }
    public DateTime Timestamp { get; set; }
}

public class IndexRecommendation
{
    public string TableName { get; set; } = string.Empty;
    public string[] ColumnNames { get; set; } = Array.Empty<string>();
    public string Reason { get; set; } = string.Empty;
    public string EstimatedImprovement { get; set; } = string.Empty;
    public IndexPriority Priority { get; set; }
}

public enum IndexPriority
{
    Low,
    Medium,
    High,
    Critical
}

public class DatabasePerformanceOptions
{
    public int MaxConnectionPoolSize { get; set; } = 100;
    public double SlowQueryThresholdSeconds { get; set; } = 1.0;
    public int SlowQueryRetentionHours { get; set; } = 24;
    public int MaxSlowQueryResults { get; set; } = 100;
    public double HighCostThreshold { get; set; } = 100.0;
}
