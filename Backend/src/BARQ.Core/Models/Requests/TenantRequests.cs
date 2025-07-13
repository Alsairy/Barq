namespace BARQ.Core.Models.Requests;

public class UpdateTenantConfigurationRequest
{
    public Guid TenantId { get; set; }
    public Dictionary<string, object> Configuration { get; set; } = new();
    public bool MergeWithExisting { get; set; } = true;
    public List<string>? AllowedResources { get; set; }
}
