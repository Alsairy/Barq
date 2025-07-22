namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class TenantDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the tenant.
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
    public string Domain { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the tenant is active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the date and time when the tenant was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// </summary>
    public int UserCount { get; set; }

    /// <summary>
    /// </summary>
    public int ProjectCount { get; set; }

    /// <summary>
    /// </summary>
    public string SubscriptionPlan { get; set; } = string.Empty;
}
