using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

public class DesignConfiguration : TenantEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public string VisualDesignStandards { get; set; } = string.Empty; // JSON
    public string BrandCompliance { get; set; } = string.Empty; // JSON
    public string UIUXDesignPatterns { get; set; } = string.Empty; // JSON
    public string ImageStyleConfiguration { get; set; } = string.Empty; // JSON
    public string DesignSystemIntegration { get; set; } = string.Empty; // JSON
    public string AccessibilityCompliance { get; set; } = string.Empty; // JSON
    
    public bool IsActive { get; set; } = true;
    public int Priority { get; set; } = 1;
    
    public virtual Organization Organization { get; set; } = null!;
}
