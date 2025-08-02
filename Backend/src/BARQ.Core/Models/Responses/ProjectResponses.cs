using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class ProjectResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public ProjectDto? Project { get; set; }
}

/// <summary>
/// </summary>
public class ProjectMemberResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public ProjectMemberDto? Member { get; set; }
}

/// <summary>
/// </summary>
public class ProjectResourceResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public ProjectResourceDto? Resource { get; set; }
}

/// <summary>
/// </summary>
public class ProjectBudgetResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public ProjectBudgetDto? Budget { get; set; }
}

/// <summary>
/// </summary>
public class ProjectRiskResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public ProjectRiskDto? Risk { get; set; }
}

/// <summary>
/// </summary>
public class ProjectAnalyticsResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public ProjectAnalyticsDto? Analytics { get; set; }
}

/// <summary>
/// </summary>
public class ProjectTimelineResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public ProjectTimelineDto? Timeline { get; set; }
}

/// <summary>
/// </summary>
public class ProjectValidationResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// </summary>
    public List<string> ValidationErrors { get; set; } = new();
}

/// <summary>
/// </summary>
public class ProjectHealthCheckResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public ProjectHealthStatus HealthStatus { get; set; }
    
    /// <summary>
    /// </summary>
    public List<ProjectHealthIssueDto> Issues { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public ProjectHealthMetricsDto Metrics { get; set; } = new();
}

/// <summary>
/// </summary>
public class ProjectComplianceResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public bool IsCompliant { get; set; }
    
    /// <summary>
    /// </summary>
    public List<ComplianceIssueDto> ComplianceIssues { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public ComplianceScoreDto ComplianceScore { get; set; } = new();
}

/// <summary>
/// </summary>
public class ProjectCostAnalysisResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public ProjectCostAnalysisDto? CostAnalysis { get; set; }
}

/// <summary>
/// </summary>
public class ProjectAnalyticsDto
{
    /// <summary>
    /// </summary>
    public Guid ProjectId { get; set; }
    
    /// <summary>
    /// </summary>
    public int TotalTasks { get; set; }
    
    /// <summary>
    /// </summary>
    public int CompletedTasks { get; set; }
    
    /// <summary>
    /// </summary>
    public int PendingTasks { get; set; }
    
    /// <summary>
    /// </summary>
    public int OverdueTasks { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal CompletionPercentage { get; set; }
    
    /// <summary>
    /// </summary>
    public TimeSpan AverageTaskDuration { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal TeamProductivityScore { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal BudgetUtilization { get; set; }
    
    /// <summary>
    /// </summary>
    public List<TeamMemberProductivityDto> TeamProductivity { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public List<TaskCompletionTrendDto> CompletionTrends { get; set; } = new();
}

/// <summary>
/// </summary>
public class ProjectTimelineDto
{
    /// <summary>
    /// </summary>
    public Guid ProjectId { get; set; }
    
    /// <summary>
    /// </summary>
    public List<TimelineEventDto> Events { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public List<MilestoneDto> Milestones { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public List<CriticalPathDto> CriticalPath { get; set; } = new();
}

/// <summary>
/// </summary>
public class TimelineEventDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime EventDate { get; set; }
    
    /// <summary>
    /// </summary>
    public string EventType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public Guid? UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? UserName { get; set; }
}

/// <summary>
/// </summary>
public class MilestoneDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime DueDate { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? CompletedDate { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsCompleted { get; set; }
    
    /// <summary>
    /// </summary>
    public bool IsCritical { get; set; }
}

/// <summary>
/// </summary>
public class CriticalPathDto
{
    /// <summary>
    /// </summary>
    public Guid TaskId { get; set; }
    
    /// <summary>
    /// </summary>
    public string TaskName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime EndDate { get; set; }
    
    /// <summary>
    /// </summary>
    public TimeSpan Duration { get; set; }
    
    /// <summary>
    /// </summary>
    public List<Guid> Dependencies { get; set; } = new();
}

/// <summary>
/// </summary>
public enum ProjectHealthStatus
{
    /// <summary>
    /// </summary>
    Healthy,
    
    /// <summary>
    /// </summary>
    Warning,
    
    /// <summary>
    /// </summary>
    Critical,
    
    /// <summary>
    /// </summary>
    Unknown
}

/// <summary>
/// </summary>
public class ProjectHealthIssueDto
{
    /// <summary>
    /// </summary>
    public string Category { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Recommendation { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class ProjectHealthMetricsDto
{
    /// <summary>
    /// </summary>
    public decimal ScheduleHealth { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal BudgetHealth { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal QualityHealth { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal TeamHealth { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal RiskHealth { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal OverallHealth { get; set; }
}

/// <summary>
/// </summary>
public class ComplianceIssueDto
{
    /// <summary>
    /// </summary>
    public string Category { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Severity { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Requirement { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Recommendation { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class ComplianceScoreDto
{
    /// <summary>
    /// </summary>
    public decimal OverallScore { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal SecurityScore { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal QualityScore { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal ProcessScore { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal DocumentationScore { get; set; }
}

/// <summary>
/// </summary>
public class ProjectCostAnalysisDto
{
    /// <summary>
    /// </summary>
    public Guid ProjectId { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal TotalBudget { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal ActualCost { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal ProjectedCost { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal CostVariance { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal CostPerformanceIndex { get; set; }
    
    /// <summary>
    /// </summary>
    public List<CostBreakdownDto> CostBreakdown { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public List<CostTrendDto> CostTrends { get; set; } = new();
}

/// <summary>
/// </summary>
public class CostBreakdownDto
{
    /// <summary>
    /// </summary>
    public string Category { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public decimal BudgetedAmount { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal ActualAmount { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal Variance { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal PercentageOfTotal { get; set; }
}

/// <summary>
/// </summary>
public class CostTrendDto
{
    /// <summary>
    /// </summary>
    public DateTime Date { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal CumulativeBudget { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal CumulativeActual { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal CumulativeProjected { get; set; }
}

/// <summary>
/// </summary>
public class TeamMemberProductivityDto
{
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public int TasksCompleted { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal AverageTaskDuration { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal ProductivityScore { get; set; }
}

/// <summary>
/// </summary>
public class TaskCompletionTrendDto
{
    /// <summary>
    /// </summary>
    public DateTime Date { get; set; }
    
    /// <summary>
    /// </summary>
    public int TasksCompleted { get; set; }
    
    /// <summary>
    /// </summary>
    public int TasksCreated { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal CompletionRate { get; set; }
}
