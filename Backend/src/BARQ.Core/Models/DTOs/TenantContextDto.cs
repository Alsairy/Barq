namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class TenantContextDto
{
    /// <summary>
    /// Gets or sets the unique identifier for the tenant.
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// </summary>
    public string TenantName { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string Domain { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets a value indicating whether the tenant is currently active.
    /// </summary>
    public bool IsActive { get; set; }

    /// <summary>
    /// Gets or sets the configuration settings for the tenant.
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new();

    /// <summary>
    /// </summary>
    public List<string> AllowedResources { get; set; } = new();

    /// <summary>
    /// Gets or sets the date and time when the tenant was last accessed.
    /// </summary>
    public DateTime LastAccessed { get; set; }
}
