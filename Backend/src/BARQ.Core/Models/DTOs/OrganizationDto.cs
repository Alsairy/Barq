using BARQ.Core.Enums;

namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class OrganizationDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the organization.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the name of the organization.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// </summary>
    public string? Website { get; set; }

    /// <summary>
    /// </summary>
    public string? LogoUrl { get; set; }

    /// <summary>
    /// </summary>
    public string? Address { get; set; }

    /// <summary>
    /// </summary>
    public string? PhoneNumber { get; set; }

    /// <summary>
    /// </summary>
    public SubscriptionPlan SubscriptionPlan { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? SubscriptionExpiryDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the organization is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the organization was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// </summary>
    public int UserCount { get; set; }

    /// <summary>
    /// </summary>
    public int ProjectCount { get; set; }
}
