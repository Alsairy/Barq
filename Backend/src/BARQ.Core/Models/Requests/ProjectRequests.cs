using BARQ.Core.Enums;

namespace BARQ.Core.Models.Requests;

/// <summary>
/// </summary>
public class CreateProjectRequest
{
    /// <summary>
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public ProjectType Type { get; set; }
    
    /// <summary>
    /// </summary>
    public ProjectPriority Priority { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime StartDate { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? EndDate { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal Budget { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid OrganizationId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid ProjectManagerId { get; set; }
    
    /// <summary>
    /// </summary>
    public List<Guid> InitialMemberIds { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public ProjectAIConfigurationRequest? AIConfiguration { get; set; }
}

/// <summary>
/// </summary>
public class UpdateProjectRequest
{
    /// <summary>
    /// </summary>
    public Guid ProjectId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Name { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// </summary>
    public ProjectStatus? Status { get; set; }
    
    /// <summary>
    /// </summary>
    public ProjectPriority? Priority { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? StartDate { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? EndDate { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime? ActualEndDate { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal? Budget { get; set; }
    
    /// <summary>
    /// </summary>
    public int? ProgressPercentage { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid? ProjectManagerId { get; set; }
    
    /// <summary>
    /// </summary>
    public ProjectAIConfigurationRequest? AIConfiguration { get; set; }
}

/// <summary>
/// </summary>
public class AddProjectMemberRequest
{
    /// <summary>
    /// </summary>
    public Guid ProjectId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public ProjectRole Role { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal AllocationPercentage { get; set; } = 100;
}

/// <summary>
/// </summary>
public class UpdateProjectMemberRequest
{
    /// <summary>
    /// </summary>
    public Guid ProjectId { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid UserId { get; set; }
    
    /// <summary>
    /// </summary>
    public ProjectRole? Role { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal? AllocationPercentage { get; set; }
    
    /// <summary>
    /// </summary>
    public bool? IsActive { get; set; }
}

/// <summary>
/// </summary>
public class AllocateResourceRequest
{
    /// <summary>
    /// </summary>
    public Guid ProjectId { get; set; }
    
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
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// </summary>
public class UpdateProjectBudgetRequest
{
    /// <summary>
    /// </summary>
    public Guid ProjectId { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal TotalBudget { get; set; }
    
    /// <summary>
    /// </summary>
    public List<BudgetCategoryRequest> Categories { get; set; } = new();
}

/// <summary>
/// </summary>
public class BudgetCategoryRequest
{
    /// <summary>
    /// </summary>
    public string Category { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public decimal AllocatedAmount { get; set; }
}

/// <summary>
/// </summary>
public class AddProjectRiskRequest
{
    /// <summary>
    /// </summary>
    public Guid ProjectId { get; set; }
    
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
}

/// <summary>
/// </summary>
public class UpdateProjectRiskRequest
{
    /// <summary>
    /// </summary>
    public Guid RiskId { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Title { get; set; }
    
    /// <summary>
    /// </summary>
    public string? Description { get; set; }
    
    /// <summary>
    /// </summary>
    public RiskLevel? Level { get; set; }
    
    /// <summary>
    /// </summary>
    public RiskStatus? Status { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal? Impact { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal? Probability { get; set; }
    
    /// <summary>
    /// </summary>
    public string? MitigationPlan { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid? AssignedToId { get; set; }
}

/// <summary>
/// </summary>
public class ProjectAIConfigurationRequest
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
