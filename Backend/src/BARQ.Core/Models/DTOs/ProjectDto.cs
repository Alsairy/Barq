using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class ProjectDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the project.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the project.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the current status of the project (e.g., Active, Completed, On Hold).
    /// </summary>
    public ProjectStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the priority level of the project (e.g., Low, Medium, High, Critical).
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
    /// Gets or sets the unique identifier of the organization that owns the project.
    /// </summary>
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the project manager.
    /// </summary>
    public Guid ProjectManagerId { get; set; }

    /// <summary>
    /// Gets or sets the name of the project manager.
    /// </summary>
    public string ProjectManagerName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the project was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the project was last updated.
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
    /// Gets or sets the unique identifier of the project member assignment.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the user assigned to the project.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the email address of the user assigned to the project.
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public ProjectRole Role { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the user joined the project.
    /// </summary>
    public DateTime JoinedAt { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the member is currently active on the project.
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
    /// Gets or sets the unique identifier of the project resource.
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
    /// Gets or sets the date and time when the resource was allocated to the project.
    /// </summary>
    public DateTime AllocatedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the resource was deallocated from the project.
    /// </summary>
    public DateTime? DeallocatedAt { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the resource is currently active for the project.
    /// </summary>
    public bool IsActive { get; set; }
}

/// <summary>
/// </summary>
public class ProjectRiskDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the project risk.
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
    /// Gets or sets the current status of the risk (e.g., Open, Mitigated, Closed).
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
    /// Gets or sets the unique identifier of the user assigned to manage the risk.
    /// </summary>
    public Guid AssignedToId { get; set; }

    /// <summary>
    /// </summary>
    public string AssignedToName { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the date and time when the risk was identified.
    /// </summary>
    public DateTime IdentifiedAt { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the risk was resolved.
    /// </summary>
    public DateTime? ResolvedAt { get; set; }
}

/// <summary>
/// </summary>
public class ProjectBudgetDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the project.
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
    /// Gets or sets a value indicating whether AI features are enabled for the project.
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
