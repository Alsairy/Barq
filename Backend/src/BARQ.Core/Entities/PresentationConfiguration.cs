using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

/// <summary>Configuration for presentation generation including templates, structure, branding, content depth, audience customization, and interactivity. (important-comment)</summary>
public class PresentationConfiguration : TenantEntity
{
    /// <summary>Gets or sets the configuration name. (important-comment)</summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>Gets or sets an optional description. (important-comment)</summary>
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    /// <summary>Gets or sets presentation templates in JSON format. (important-comment)</summary>
    public string PresentationTemplates { get; set; } = string.Empty;
    
    /// <summary>Gets or sets the slide structure definition in JSON format. (important-comment)</summary>
    public string SlideStructure { get; set; } = string.Empty;
    
    /// <summary>Gets or sets design and branding settings in JSON format. (important-comment)</summary>
    public string DesignBranding { get; set; } = string.Empty;
    
    /// <summary>Gets or sets the content depth configuration in JSON format. (important-comment)</summary>
    public string ContentDepth { get; set; } = string.Empty;
    
    /// <summary>Gets or sets audience customization options in JSON format. (important-comment)</summary>
    public string AudienceCustomization { get; set; } = string.Empty;
    
    /// <summary>Gets or sets interactive elements configuration in JSON format. (important-comment)</summary>
    public string InteractiveElements { get; set; } = string.Empty;
    
    /// <summary>Gets or sets a value indicating whether the configuration is active. (important-comment)</summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>Gets or sets the selection priority; lower indicates higher priority. (important-comment)</summary>
    public int Priority { get; set; } = 1;
    
    /// <summary>Gets or sets the owning organization for multi-tenant scoping. (important-comment)</summary>
    public virtual Organization Organization { get; set; } = null!;
}
