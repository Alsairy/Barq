using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class ProjectDto
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
    public ProjectStatus Status { get; set; }
    
    /// <summary>
    /// </summary>
    public ProjectPriority Priority { get; set; }
    
    /// <summary>
    /// </summary>
    public ProjectType Type { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? EndDate { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? ActualEndDate { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal Budget { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal ActualCost { get; set; }
    
    /// <summary>
    /// </summary>
    public int ProgressPercentage { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid ProjectManagerId { get; set; }
    
    /// <summary>
    /// </summary>
    public string ProjectManagerName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public DateTime CreatedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime UpdatedAt { get; set; }
    
    /// <summary>
    /// </summary>
    public List<ProjectMemberDto> Members { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public List<ProjectRiskDto> Risks { get; set; } = new();
    
    /// <summary>
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
    
    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Type { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public decimal Cost { get; set; }
    
    /// <summary>
    /// </summary>
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
    
    /// <summary>
    /// </summary>
    public string Title { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public RiskLevel Level { get; set; }
    
    /// <summary>
    /// </summary>
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
