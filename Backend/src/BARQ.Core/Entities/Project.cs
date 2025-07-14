using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;

namespace BARQ.Core.Entities;

/// <summary>
/// Represents a development project in the BARQ platform
/// </summary>
public class Project : TenantEntity
{
    /// <summary>
    /// Project name
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Project description
    /// </summary>
    [MaxLength(2000)]
    public string? Description { get; set; }

    /// <summary>
    /// Project key/code (unique within organization)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string ProjectKey { get; set; } = string.Empty;

    /// <summary>
    /// Project type (new product or existing platform)
    /// </summary>
    public ProjectType ProjectType { get; set; } = ProjectType.NewProduct;

    /// <summary>
    /// Project status
    /// </summary>
    public ProjectStatus Status { get; set; } = ProjectStatus.Planning;

    /// <summary>
    /// Project priority
    /// </summary>
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;

    /// <summary>
    /// Project start date
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Project target end date
    /// </summary>
    public DateTime? TargetEndDate { get; set; }

    /// <summary>
    /// Project actual end date
    /// </summary>
    public DateTime? ActualEndDate { get; set; }

    /// <summary>
    /// Project budget
    /// </summary>
    public decimal? Budget { get; set; }

    /// <summary>
    /// </summary>
    public decimal? ActualCost { get; set; }

    /// <summary>
    /// Project progress percentage (0-100)
    /// </summary>
    public decimal? ProgressPercentage { get; set; }

    /// <summary>
    /// Project end date (alias for TargetEndDate for compatibility)
    /// </summary>
    public DateTime? EndDate 
    { 
        get => TargetEndDate; 
        set => TargetEndDate = value; 
    }

    /// <summary>
    /// Technology stack configuration as JSON
    /// </summary>
    public string? TechnologyStack { get; set; }

    /// <summary>
    /// Repository URL for existing projects
    /// </summary>
    [MaxLength(500)]
    public string? RepositoryUrl { get; set; }

    /// <summary>
    /// Repository branch for integration
    /// </summary>
    [MaxLength(100)]
    public string? RepositoryBranch { get; set; } = "main";

    /// <summary>
    /// Project logo URL
    /// </summary>
    [MaxLength(500)]
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Project settings as JSON
    /// </summary>
    public string? Settings { get; set; }

    /// <summary>
    /// AI configuration for this project
    /// </summary>
    public string? AIConfiguration { get; set; }

    /// <summary>
    /// Design system URL or configuration for existing projects
    /// </summary>
    [MaxLength(500)]
    public string? DesignSystemUrl { get; set; }

    /// <summary>
    /// Project owner
    /// </summary>
    public Guid ProjectOwnerId { get; set; }
    public virtual User ProjectOwner { get; set; } = null!;

    /// <summary>
    /// Project manager ID (alias for ProjectOwnerId for compatibility)
    /// </summary>
    public Guid ProjectManagerId 
    { 
        get => ProjectOwnerId; 
        set => ProjectOwnerId = value; 
    }

    /// <summary>
    /// Organization ID (from TenantEntity.TenantId for compatibility)
    /// </summary>
    public Guid OrganizationId 
    { 
        get => TenantId; 
        set => TenantId = value; 
    }

    /// <summary>
    /// Organization this project belongs to
    /// </summary>
    public virtual Organization Organization { get; set; } = null!;

    /// <summary>
    /// Project members
    /// </summary>
    public virtual ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();

    /// <summary>
    /// Sprints in this project
    /// </summary>
    public virtual ICollection<Sprint> Sprints { get; set; } = new List<Sprint>();

    /// <summary>
    /// Business requirements documents for this project
    /// </summary>
    public virtual ICollection<BusinessRequirementDocument> BusinessRequirements { get; set; } = new List<BusinessRequirementDocument>();

    /// <summary>
    /// User stories for this project
    /// </summary>
    public virtual ICollection<UserStory> UserStories { get; set; } = new List<UserStory>();

    /// <summary>
    /// AI processing tasks for this project
    /// </summary>
    public virtual ICollection<AITask> AITasks { get; set; } = new List<AITask>();

    /// <summary>
    /// Cost tracking records for this project
    /// </summary>
    public virtual ICollection<CostTracking> CostTrackings { get; set; } = new List<CostTracking>();

    /// <summary>
    /// ITSM tickets related to this project
    /// </summary>
    public virtual ICollection<ITSMTicket> ITSMTickets { get; set; } = new List<ITSMTicket>();
}

