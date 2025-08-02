namespace BARQ.Core.Models.Requests;

/// <summary>
/// </summary>
public class UpdateTenantConfigurationRequest
{
    /// <summary>
    /// </summary>
    public Guid TenantId { get; set; }
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public bool MergeWithExisting { get; set; } = true;
    
    /// <summary>
    /// </summary>
    public List<string>? AllowedResources { get; set; }
}
