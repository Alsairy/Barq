using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

public class TenantConfigurationResponse : BaseResponse
{
    public Dictionary<string, object> Configuration { get; set; } = new();
    public List<string> AllowedResources { get; set; } = new();
    public DateTime LastUpdated { get; set; }
}

public class TenantValidationResponse : BaseResponse
{
    public bool IsValid { get; set; }
    public List<string> ValidationMessages { get; set; } = new();
    public Guid TenantId { get; set; }
    public string TenantName { get; set; } = string.Empty;
}
