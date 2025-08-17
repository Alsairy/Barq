using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

/// <summary>Configuration for code generation including tech stack, patterns, standards, and requirements. (important-comment)</summary>
public class CodeGenerationConfiguration : TenantEntity
{
    /// <summary>Gets or sets the configuration name. (important-comment)</summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>Gets or sets an optional description. (important-comment)</summary>
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    /// <summary>Gets or sets the target technology stack in JSON format. (important-comment)</summary>
    public string TechnologyStack { get; set; } = string.Empty;
    
    /// <summary>Gets or sets architectural patterns in JSON format. (important-comment)</summary>
    public string ArchitecturalPatterns { get; set; } = string.Empty;
    
    /// <summary>Gets or sets coding standards in JSON format. (important-comment)</summary>
    public string CodingStandards { get; set; } = string.Empty;
    
    /// <summary>Gets or sets quality requirements in JSON format. (important-comment)</summary>
    public string QualityRequirements { get; set; } = string.Empty;
    
    /// <summary>Gets or sets version requirements in JSON format. (important-comment)</summary>
    public string VersionRequirements { get; set; } = string.Empty;
    
    /// <summary>Gets or sets a value indicating whether the configuration is active. (important-comment)</summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>Gets or sets selection priority; lower indicates higher priority. (important-comment)</summary>
    public int Priority { get; set; } = 1;
    
    /// <summary>Gets or sets the owning organization for multi-tenant scoping. (important-comment)</summary>
    public virtual Organization Organization { get; set; } = null!;
}
