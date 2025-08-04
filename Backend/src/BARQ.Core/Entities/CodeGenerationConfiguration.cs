using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

public class CodeGenerationConfiguration : TenantEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public string TechnologyStack { get; set; } = string.Empty; // JSON
    public string ArchitecturalPatterns { get; set; } = string.Empty; // JSON
    public string CodingStandards { get; set; } = string.Empty; // JSON
    public string QualityRequirements { get; set; } = string.Empty; // JSON
    public string VersionRequirements { get; set; } = string.Empty; // JSON
    
    public bool IsActive { get; set; } = true;
    public int Priority { get; set; } = 1;
    
    public virtual Organization Organization { get; set; } = null!;
}
