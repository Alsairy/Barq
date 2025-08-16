using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Data transfer object for organization information and metadata
/// </summary>
public class OrganizationDto
{
    /// <summary>
    /// Unique identifier for the organization
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Name of the organization
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// Optional description of the organization
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Website URL of the organization
    /// </summary>
    public string? Website { get; set; }
    /// <summary>
    /// URL to the organization's logo image
    /// </summary>
    public string? LogoUrl { get; set; }
    /// <summary>
    /// Physical address of the organization
    /// </summary>
    public string? Address { get; set; }
    /// <summary>
    /// Phone number of the organization
    /// </summary>
    public string? PhoneNumber { get; set; }
    /// <summary>
    /// Current subscription plan of the organization
    /// </summary>
    public SubscriptionPlan SubscriptionPlan { get; set; }
    /// <summary>
    /// Date when the subscription expires
    /// </summary>
    public DateTime? SubscriptionExpiryDate { get; set; }
    /// <summary>
    /// Indicates whether the organization is currently active
    /// </summary>
    public bool IsActive { get; set; }
    /// <summary>
    /// Date and time when the organization was created
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Number of users in the organization
    /// </summary>
    public int UserCount { get; set; }
    /// <summary>
    /// Number of projects in the organization
    /// </summary>
    public int ProjectCount { get; set; }
}
