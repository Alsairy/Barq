using System.ComponentModel.DataAnnotations;
using BARQ.Core.Enums;
using BARQ.Core.Attributes;

namespace BARQ.Core.Entities;

/// <summary>
/// Represents an organization (tenant) in the BARQ platform
/// </summary>
public class Organization : BaseEntity
{
    /// <summary>
    /// Organization name
    /// </summary>
    [Required]
    [MaxLength(200)]
    [SearchableEncrypted]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Organization display name
    /// </summary>
    [MaxLength(200)]
    public string? DisplayName { get; set; }

    /// <summary>
    /// Organization description
    /// </summary>
    [MaxLength(1000)]
    public string? Description { get; set; }

    /// <summary>
    /// Organization website URL
    /// </summary>
    [MaxLength(500)]
    public string? Website { get; set; }

    /// <summary>
    /// Organization logo URL
    /// </summary>
    [MaxLength(500)]
    public string? LogoUrl { get; set; }

    /// <summary>
    /// Organization subscription plan
    /// </summary>
    public SubscriptionPlan SubscriptionPlan { get; set; } = SubscriptionPlan.Free;

    /// <summary>
    /// Organization status
    /// </summary>
    public OrganizationStatus Status { get; set; } = OrganizationStatus.Active;

    /// <summary>
    /// Maximum number of users allowed
    /// </summary>
    public int MaxUsers { get; set; } = 5;

    /// <summary>
    /// Maximum number of projects allowed
    /// </summary>
    public int MaxProjects { get; set; } = 3;

    /// <summary>
    /// Organization settings as JSON
    /// </summary>
    public string? Settings { get; set; }

    /// <summary>
    /// Subscription expiry date
    /// </summary>
    public DateTime? SubscriptionExpiryDate { get; set; }

    /// <summary>
    /// Users in this organization
    /// </summary>
    public virtual ICollection<User> Users { get; set; } = new List<User>();

    /// <summary>
    /// Projects in this organization
    /// </summary>
    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    /// <summary>
    /// AI provider configurations for this organization
    /// </summary>
    public virtual ICollection<AIProviderConfiguration> AIProviderConfigurations { get; set; } = new List<AIProviderConfiguration>();

    /// <summary>
    /// Workflow templates for this organization
    /// </summary>
    public virtual ICollection<WorkflowTemplate> WorkflowTemplates { get; set; } = new List<WorkflowTemplate>();
}

