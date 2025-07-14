using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

public class ProjectResponse : BaseResponse
{
    public ProjectDto? Project { get; set; }
}

public class ProjectMemberResponse : BaseResponse
{
    public ProjectMemberDto? Member { get; set; }
}

public class ProjectResourceResponse : BaseResponse
{
    public ProjectResourceDto? Resource { get; set; }
}

public class ProjectBudgetResponse : BaseResponse
{
    public ProjectBudgetDto? Budget { get; set; }
}

public class ProjectRiskResponse : BaseResponse
{
    public ProjectRiskDto? Risk { get; set; }
}

public class ProjectAnalyticsResponse : BaseResponse
{
    public ProjectAnalyticsDto? Analytics { get; set; }
}

public class ProjectTimelineResponse : BaseResponse
{
    public ProjectTimelineDto? Timeline { get; set; }
}

public class ProjectValidationResponse : BaseResponse
{
    public bool IsValid { get; set; }
    public List<string> ValidationErrors { get; set; } = new();
}

public class ProjectHealthCheckResponse : BaseResponse
{
    public ProjectHealthStatus HealthStatus { get; set; }
    public List<ProjectHealthIssueDto> Issues { get; set; } = new();
    public ProjectHealthMetricsDto Metrics { get; set; } = new();
}

public class ProjectComplianceResponse : BaseResponse
{
    public bool IsCompliant { get; set; }
    public List<ComplianceIssueDto> ComplianceIssues { get; set; } = new();
    public ComplianceScoreDto ComplianceScore { get; set; } = new();
}

public class ProjectCostAnalysisResponse : BaseResponse
{
    public ProjectCostAnalysisDto? CostAnalysis { get; set; }
}

public class ProjectAnalyticsDto
{
    public Guid ProjectId { get; set; }
    public int TotalTasks { get; set; }
    public int CompletedTasks { get; set; }
    public int PendingTasks { get; set; }
    public int OverdueTasks { get; set; }
    public decimal CompletionPercentage { get; set; }
    public TimeSpan AverageTaskDuration { get; set; }
    public decimal TeamProductivity { get; set; }
    public decimal BudgetUtilization { get; set; }
    public List<TeamMemberProductivityDto> TeamProductivity { get; set; } = new();
    public List<TaskCompletionTrendDto> CompletionTrends { get; set; } = new();
}

public class ProjectTimelineDto
{
    public Guid ProjectId { get; set; }
    public List<TimelineEventDto> Events { get; set; } = new();
    public List<MilestoneDto> Milestones { get; set; } = new();
    public List<CriticalPathDto> CriticalPath { get; set; } = new();
}

public class TimelineEventDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string EventType { get; set; } = string.Empty;
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
}

public class MilestoneDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsCritical { get; set; }
}

public class CriticalPathDto
{
    public Guid TaskId { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public TimeSpan Duration { get; set; }
    public List<Guid> Dependencies { get; set; } = new();
}

public enum ProjectHealthStatus
{
    Healthy,
    Warning,
    Critical,
    Unknown
}

public class ProjectHealthIssueDto
{
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
}

public class ProjectHealthMetricsDto
{
    public decimal ScheduleHealth { get; set; }
    public decimal BudgetHealth { get; set; }
    public decimal QualityHealth { get; set; }
    public decimal TeamHealth { get; set; }
    public decimal RiskHealth { get; set; }
    public decimal OverallHealth { get; set; }
}

public class ComplianceIssueDto
{
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Requirement { get; set; } = string.Empty;
    public string Recommendation { get; set; } = string.Empty;
}

public class ComplianceScoreDto
{
    public decimal OverallScore { get; set; }
    public decimal SecurityScore { get; set; }
    public decimal QualityScore { get; set; }
    public decimal ProcessScore { get; set; }
    public decimal DocumentationScore { get; set; }
}

public class ProjectCostAnalysisDto
{
    public Guid ProjectId { get; set; }
    public decimal TotalBudget { get; set; }
    public decimal ActualCost { get; set; }
    public decimal ProjectedCost { get; set; }
    public decimal CostVariance { get; set; }
    public decimal CostPerformanceIndex { get; set; }
    public List<CostBreakdownDto> CostBreakdown { get; set; } = new();
    public List<CostTrendDto> CostTrends { get; set; } = new();
}

public class CostBreakdownDto
{
    public string Category { get; set; } = string.Empty;
    public decimal BudgetedAmount { get; set; }
    public decimal ActualAmount { get; set; }
    public decimal Variance { get; set; }
    public decimal PercentageOfTotal { get; set; }
}

public class CostTrendDto
{
    public DateTime Date { get; set; }
    public decimal CumulativeBudget { get; set; }
    public decimal CumulativeActual { get; set; }
    public decimal CumulativeProjected { get; set; }
}

public class TeamMemberProductivityDto
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public int TasksCompleted { get; set; }
    public decimal AverageTaskDuration { get; set; }
    public decimal ProductivityScore { get; set; }
}

public class TaskCompletionTrendDto
{
    public DateTime Date { get; set; }
    public int TasksCompleted { get; set; }
    public int TasksCreated { get; set; }
    public decimal CompletionRate { get; set; }
}
