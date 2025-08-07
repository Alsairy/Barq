namespace BARQ.Core.Models.DTOs;

/// <summary>
/// Data transfer object representing tenant context information (important-comment)
/// </summary>
public class TenantContextDto
{
    /// <summary>
    /// Gets or sets the tenant identifier (important-comment)
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
    /// Gets or sets a value indicating whether the tenant is active (important-comment)
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// Gets or sets the tenant configuration settings (important-comment)
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the list of allowed resources for the tenant (important-comment)
    /// </summary>
    public List<string> AllowedResources { get; set; } = new();
    
    /// <summary>
    /// Gets or sets the last accessed date for the tenant (important-comment)
    /// </summary>
    public DateTime LastAccessed { get; set; }
}
