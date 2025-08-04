using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

public class PresentationConfiguration : TenantEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public string PresentationTemplates { get; set; } = string.Empty; // JSON
    public string SlideStructure { get; set; } = string.Empty; // JSON
    public string DesignBranding { get; set; } = string.Empty; // JSON
    public string ContentDepth { get; set; } = string.Empty; // JSON
    public string AudienceCustomization { get; set; } = string.Empty; // JSON
    public string InteractiveElements { get; set; } = string.Empty; // JSON
    
    public bool IsActive { get; set; } = true;
    public int Priority { get; set; } = 1;
    
    public virtual Organization Organization { get; set; } = null!;
}
