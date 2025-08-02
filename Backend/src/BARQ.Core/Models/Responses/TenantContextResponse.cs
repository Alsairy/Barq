using BARQ.Core.Models.Responses;
using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class TenantContextResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public TenantContextDto? TenantContext { get; set; }
    
    /// <summary>
    /// </summary>
    public IEnumerable<string> AvailableFeatures { get; set; } = new List<string>();
    
    /// <summary>
    /// </summary>
    public Dictionary<string, object> Configuration { get; set; } = new Dictionary<string, object>();
}
