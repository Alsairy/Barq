using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Data transfer object for project information and management details
/// </summary>
public class ProjectDto
{
    /// <summary>
    /// Unique identifier for the project
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Name of the project
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Description of the project
    /// </summary>
    public string Description { get; set; } = string.Empty;
    /// <summary>
    /// Current status of the project
    /// </summary>
    public ProjectStatus Status { get; set; }
    /// <summary>
    /// Priority level of the project
    /// </summary>
    public ProjectPriority Priority { get; set; }
    /// <summary>
    /// Type classification of the project
    /// </summary>
    public ProjectType Type { get; set; }
    /// <summary>
    /// Planned start date of the project
    /// </summary>
    public DateTime StartDate { get; set; }
    /// <summary>
    /// Planned end date of the project
    /// </summary>
    public DateTime? EndDate { get; set; }
    /// <summary>
    /// Actual completion date of the project
    /// </summary>
    public DateTime? ActualEndDate { get; set; }
    /// <summary>
    /// Total budget allocated for the project
    /// </summary>
    public decimal Budget { get; set; }
    /// <summary>
    /// Actual cost incurred for the project
    /// </summary>
    public decimal ActualCost { get; set; }
    /// <summary>
    /// Current progress percentage of the project
    /// </summary>
    public int ProgressPercentage { get; set; }
    /// <summary>
    /// Identifier of the organization that owns the project (important-comment)
    /// </summary>
    public Guid OrganizationId { get; set; }
    /// <summary>
    /// Identifier of the project manager
    /// </summary>
    public Guid ProjectManagerId { get; set; }
    /// <summary>
    /// Name of the project manager
    /// </summary>
    public string ProjectManagerName { get; set; } = string.Empty;
    /// <summary>
    /// Date and time when the project was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Date and time when the project was last updated
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    /// <summary>
    /// List of project team members
    /// </summary>
    public List<ProjectMemberDto> Members { get; set; } = new();
    /// <summary>
    /// List of project risks and mitigation plans
    /// </summary>
    public List<ProjectRiskDto> Risks { get; set; } = new();
    /// <summary>
    /// AI configuration settings for the project
    /// </summary>
    public ProjectAIConfigurationDto? AIConfiguration { get; set; }
}

/// <summary>
/// </summary>
public class ProjectMemberDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    /// <summary>
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public ProjectRole Role { get; set; }
    /// <summary>
    /// </summary>
    public DateTime JoinedAt { get; set; }
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; }
    /// <summary>
    /// </summary>
    public decimal AllocationPercentage { get; set; }
}

/// <summary>
/// </summary>
public class ProjectResourceDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    public Guid? ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public string Type { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public decimal Cost { get; set; }
    public int? Allocation { get; set; }
    public DateTime AllocatedAt { get; set; }
    /// <summary>
    /// </summary>
    public DateTime? DeallocatedAt { get; set; }
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; }
}

/// <summary>
/// </summary>
public class ProjectRiskDto
{
    /// <summary>
    /// </summary>
    public Guid Id { get; set; }
    public Guid? ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// Detailed description of the risk
    /// </summary>
    public string Description { get; set; } = string.Empty;
    public RiskLevel Level { get; set; }
    public RiskStatus Status { get; set; }
    /// <summary>
    /// </summary>
    public decimal Impact { get; set; }
    /// <summary>
    /// </summary>
    public decimal Probability { get; set; }
    /// <summary>
    /// </summary>
    public string MitigationPlan { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public Guid AssignedToId { get; set; }
    /// <summary>
    /// </summary>
    public string AssignedToName { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public DateTime IdentifiedAt { get; set; }
    /// <summary>
    /// </summary>
    public DateTime? ResolvedAt { get; set; }
}

/// <summary>
/// </summary>
public class ProjectBudgetDto
{
    /// <summary>
    /// </summary>
    public Guid ProjectId { get; set; }
    /// <summary>
    /// </summary>
    public decimal TotalBudget { get; set; }
    /// <summary>
    /// </summary>
    public decimal AllocatedBudget { get; set; }
    /// <summary>
    /// </summary>
    public decimal SpentBudget { get; set; }
    /// <summary>
    /// </summary>
    public decimal RemainingBudget { get; set; }
    /// <summary>
    /// </summary>
    public List<BudgetCategoryDto> Categories { get; set; } = new();
}

/// <summary>
/// </summary>
public class BudgetCategoryDto
{
    /// <summary>
    /// </summary>
    public string Category { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public decimal AllocatedAmount { get; set; }
    /// <summary>
    /// </summary>
    public decimal SpentAmount { get; set; }
    /// <summary>
    /// </summary>
    public decimal RemainingAmount { get; set; }
}

/// <summary>
/// </summary>
public class ProjectAIConfigurationDto
{
    /// <summary>
    /// </summary>
    public bool IsAIEnabled { get; set; }
    /// <summary>
    /// </summary>
    public List<string> EnabledFeatures { get; set; } = new();
    /// <summary>
    /// </summary>
    public string PreferredProvider { get; set; } = string.Empty;
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Settings { get; set; } = new();
}
