namespace BARQ.Core.Models.DTOs;

/// <summary>
/// </summary>
public class TenantContextDto
{
    /// <summary>
    /// </summary>
    public Guid TenantId { get; set; }
    
    /// <summary>
    /// </summary>
    public string TenantName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public string Domain { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public bool IsActive { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public List<string> AllowedResources { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public DateTime LastAccessed { get; set; }
}
