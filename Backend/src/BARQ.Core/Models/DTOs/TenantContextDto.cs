namespace BARQ.Core.Models.DTOs;

public class TenantContextDto
{
    public Guid TenantId { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public string Domain { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Dictionary<string, object> Configuration { get; set; } = new();
    public List<string> AllowedResources { get; set; } = new();
    public DateTime LastAccessed { get; set; }
}
