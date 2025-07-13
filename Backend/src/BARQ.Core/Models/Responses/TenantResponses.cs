using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

public class TenantIsolationResponse : BaseResponse
{
    public bool IsIsolated { get; set; }
    public Guid TenantId { get; set; }
    public List<string> AccessibleResources { get; set; } = new();
    public List<string> RestrictedResources { get; set; } = new();
}

public class TenantSwitchResponse : BaseResponse
{
    public Guid NewTenantId { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public bool SwitchSuccessful { get; set; }
    public string? NewAccessToken { get; set; }
}

public class TenantResourceUsageDto
{
    public Guid TenantId { get; set; }
    public string ResourceType { get; set; } = string.Empty;
    public decimal CurrentUsage { get; set; }
    public decimal Limit { get; set; }
    public decimal UsagePercentage { get; set; }
    public DateTime LastUpdated { get; set; }
}
