using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

public class TestingConfiguration : TenantEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public string TestingMethodology { get; set; } = string.Empty; // JSON
    public string TestCoverage { get; set; } = string.Empty; // JSON
    public string PerformanceTesting { get; set; } = string.Empty; // JSON
    public string SecurityTesting { get; set; } = string.Empty; // JSON
    public string AutomatedTesting { get; set; } = string.Empty; // JSON
    public string ComplianceTesting { get; set; } = string.Empty; // JSON
    
    public bool IsActive { get; set; } = true;
    public int Priority { get; set; } = 1;
    
    public virtual Organization Organization { get; set; } = null!;
}
