using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class TenantIsolationResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public bool IsIsolated { get; set; }
    
    /// <summary>
    /// </summary>
    public Guid TenantId { get; set; }
    
    /// <summary>
    /// </summary>
    public List<string> AccessibleResources { get; set; } = new();
    
    /// <summary>
    /// </summary>
    public List<string> RestrictedResources { get; set; } = new();
}

/// <summary>
/// </summary>
public class TenantSwitchResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public Guid NewTenantId { get; set; }
    
    /// <summary>
    /// </summary>
    public string TenantName { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public bool SwitchSuccessful { get; set; }
    
    /// <summary>
    /// </summary>
    public string? NewAccessToken { get; set; }
}

/// <summary>
/// </summary>
public class TenantResourceUsageDto
{
    /// <summary>
    /// </summary>
    public Guid TenantId { get; set; }
    
    /// <summary>
    /// </summary>
    public string ResourceType { get; set; } = string.Empty;
    
    /// <summary>
    /// </summary>
    public decimal CurrentUsage { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal Limit { get; set; }
    
    /// <summary>
    /// </summary>
    public decimal UsagePercentage { get; set; }
    
    /// <summary>
    /// </summary>
    public DateTime LastUpdated { get; set; }
}
