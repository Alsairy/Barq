using BARQ.Core.Models.Responses;
using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

public class TenantContextResponse : BaseResponse
{
    public TenantContextDto? TenantContext { get; set; }
    public IEnumerable<string> AvailableFeatures { get; set; } = new List<string>();
    public Dictionary<string, object> Configuration { get; set; } = new Dictionary<string, object>();
}
