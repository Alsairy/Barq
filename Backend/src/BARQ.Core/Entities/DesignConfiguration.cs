using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

/// <summary>Configuration for application design standards, patterns, and compliance requirements. (important-comment)</summary>
public class DesignConfiguration : TenantEntity
{
    /// <summary>Gets or sets the configuration name. (important-comment)</summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>Gets or sets an optional description. (important-comment)</summary>
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    /// <summary>Gets or sets visual design standards in JSON format. (important-comment)</summary>
    public string VisualDesignStandards { get; set; } = string.Empty;
    
    /// <summary>Gets or sets brand compliance settings in JSON format. (important-comment)</summary>
    public string BrandCompliance { get; set; } = string.Empty;
    
    /// <summary>Gets or sets UI/UX design patterns in JSON format. (important-comment)</summary>
    public string UIUXDesignPatterns { get; set; } = string.Empty;
    
    /// <summary>Gets or sets image style configuration in JSON format. (important-comment)</summary>
    public string ImageStyleConfiguration { get; set; } = string.Empty;
    
    /// <summary>Gets or sets design system integration configuration in JSON format. (important-comment)</summary>
    public string DesignSystemIntegration { get; set; } = string.Empty;
    
    /// <summary>Gets or sets accessibility compliance requirements in JSON format. (important-comment)</summary>
    public string AccessibilityCompliance { get; set; } = string.Empty;
    
    /// <summary>Gets or sets a value indicating whether the configuration is active. (important-comment)</summary>
    public bool IsActive { get; set; } = true;

    /// <summary>Gets or sets the selection priority; lower indicates higher priority. (important-comment)</summary>
    public int Priority { get; set; } = 1;
    
    /// <summary>Gets or sets the owning organization for multi-tenant scoping. (important-comment)</summary>
    public virtual Organization Organization { get; set; } = null!;
}
