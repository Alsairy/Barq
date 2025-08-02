using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class TenantConfigurationResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public List<string> AllowedResources { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public DateTime LastUpdated { get; set; }
}

/// <summary>
/// </summary>
public class TenantValidationResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public bool IsValid { get; set; }
    
    /// <summary>
    /// </summary>
    public List<string> ValidationMessages { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public Guid TenantId { get; set; }
    
    /// <summary>
    /// </summary>
    public string TenantName { get; set; } = string.Empty;
}
