namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Data transfer object representing tenant context information
/// </summary>
public class TenantContextDto
{
    /// <summary>
    /// Gets or sets the tenant identifier
    /// </summary>
    public Guid TenantId { get; set; }
    
    /// <summary>
    /// Gets or sets the tenant name
    /// </summary>
    public string TenantName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the tenant domain
    /// </summary>
    public string Domain { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets a value indicating whether the tenant is active
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Gets or sets the tenant configuration settings
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the list of allowed resources for the tenant
    /// </summary>
    public List<string> AllowedResources { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the last accessed date for the tenant
    /// </summary>
    public DateTime LastAccessed { get; set; }
}
