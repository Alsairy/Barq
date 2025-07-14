using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

public class ProjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ProjectStatus Status { get; set; }
    public ProjectPriority Priority { get; set; }
    public ProjectType Type { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? ActualEndDate { get; set; }
    public decimal Budget { get; set; }
    public decimal ActualCost { get; set; }
    public int ProgressPercentage { get; set; }
    public Guid OrganizationId { get; set; }
    public Guid ProjectManagerId { get; set; }
    public string ProjectManagerName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<ProjectMemberDto> Members { get; set; } = new();
    public List<ProjectRiskDto> Risks { get; set; } = new();
    public ProjectAIConfigurationDto? AIConfiguration { get; set; }
}

public class ProjectMemberDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public ProjectRole Role { get; set; }
    public DateTime JoinedAt { get; set; }
    public bool IsActive { get; set; }
    public decimal AllocationPercentage { get; set; }
}

public class ProjectResourceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public decimal Cost { get; set; }
    public DateTime AllocatedAt { get; set; }
    public DateTime? DeallocatedAt { get; set; }
    public bool IsActive { get; set; }
}

public class ProjectRiskDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public RiskLevel Level { get; set; }
    public RiskStatus Status { get; set; }
    public decimal Impact { get; set; }
    public decimal Probability { get; set; }
    public string MitigationPlan { get; set; } = string.Empty;
    public Guid AssignedToId { get; set; }
    public string AssignedToName { get; set; } = string.Empty;
    public DateTime IdentifiedAt { get; set; }
    public DateTime? ResolvedAt { get; set; }
}

public class ProjectBudgetDto
{
    public Guid ProjectId { get; set; }
    public decimal TotalBudget { get; set; }
    public decimal AllocatedBudget { get; set; }
    public decimal SpentBudget { get; set; }
    public decimal RemainingBudget { get; set; }
    public List<BudgetCategoryDto> Categories { get; set; } = new();
}

public class BudgetCategoryDto
{
    public string Category { get; set; } = string.Empty;
    public decimal AllocatedAmount { get; set; }
    public decimal SpentAmount { get; set; }
    public decimal RemainingAmount { get; set; }
}

public class ProjectAIConfigurationDto
{
    public bool IsAIEnabled { get; set; }
    public List<string> EnabledFeatures { get; set; } = new();
    public string PreferredProvider { get; set; } = string.Empty;
    public Dictionary<string, object> Settings { get; set; } = new();
}
