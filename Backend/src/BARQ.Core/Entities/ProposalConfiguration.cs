using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

public class ProposalConfiguration : TenantEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public string ProposalTemplates { get; set; } = string.Empty; // JSON
    public string TechnicalSpecifications { get; set; } = string.Empty; // JSON
    public string FinancialModeling { get; set; } = string.Empty; // JSON
    public string RiskAssessment { get; set; } = string.Empty; // JSON
    public string ImplementationPlanning { get; set; } = string.Empty; // JSON
    public string CompetitivePositioning { get; set; } = string.Empty; // JSON
    
    public bool IsActive { get; set; } = true;
    public int Priority { get; set; } = 1;
    
    public virtual Organization Organization { get; set; } = null!;
}
