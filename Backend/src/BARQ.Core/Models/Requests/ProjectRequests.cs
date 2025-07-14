using BARQ.Core.Enums;

namespace BARQ.Core.Models.Requests;

public class CreateProjectRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ProjectType Type { get; set; }
    public ProjectPriority Priority { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal Budget { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid ProjectManagerId { get; set; }
    public List<Guid> InitialMemberIds { get; set; } = new();
    public ProjectAIConfigurationRequest? AIConfiguration { get; set; }
}

public class UpdateProjectRequest
{
    public Guid ProjectId { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public ProjectStatus? Status { get; set; }
    public ProjectPriority? Priority { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public decimal? Budget { get; set; }
    public int? ProgressPercentage { get; set; }
    public Guid? ProjectManagerId { get; set; }
    public ProjectAIConfigurationRequest? AIConfiguration { get; set; }
}

public class AddProjectMemberRequest
{
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
    public ProjectRole Role { get; set; }
    public decimal AllocationPercentage { get; set; } = 100;
}

public class UpdateProjectMemberRequest
{
    public Guid ProjectId { get; set; }
    public Guid UserId { get; set; }
    public ProjectRole? Role { get; set; }
    public decimal? AllocationPercentage { get; set; }
    public bool? IsActive { get; set; }
}

public class AllocateResourceRequest
{
    public Guid ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class UpdateProjectBudgetRequest
{
    public Guid ProjectId { get; set; }
    public decimal TotalBudget { get; set; }
    public List<BudgetCategoryRequest> Categories { get; set; } = new();
}

public class BudgetCategoryRequest
{
    public string Category { get; set; } = string.Empty;
    public decimal AllocatedAmount { get; set; }
}

public class AddProjectRiskRequest
{
    public Guid ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public RiskLevel Level { get; set; }
    public decimal Impact { get; set; }
    public decimal Probability { get; set; }
    public string MitigationPlan { get; set; } = string.Empty;
    public Guid AssignedToId { get; set; }
}

public class UpdateProjectRiskRequest
{
    public Guid RiskId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public RiskLevel? Level { get; set; }
    public RiskStatus? Status { get; set; }
    public decimal? Impact { get; set; }
    public decimal? Probability { get; set; }
    public string? MitigationPlan { get; set; }
    public Guid? AssignedToId { get; set; }
}

public class ProjectAIConfigurationRequest
{
    public bool IsAIEnabled { get; set; }
    public List<string> EnabledFeatures { get; set; } = new();
    public string PreferredProvider { get; set; } = string.Empty;
    public Dictionary<string, object> Settings { get; set; } = new();
}
