using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

public class BRDTemplateConfiguration : TenantEntity
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public string DocumentStructure { get; set; } = string.Empty; // JSON
    public string SprintConfiguration { get; set; } = string.Empty; // JSON
    public string UserStoryTemplates { get; set; } = string.Empty; // JSON
    public string AcceptanceCriteria { get; set; } = string.Empty; // JSON
    public string StakeholderAnalysis { get; set; } = string.Empty; // JSON
    public string BusinessProcessModeling { get; set; } = string.Empty; // JSON
    
    public bool IsActive { get; set; } = true;
    public int Priority { get; set; } = 1;
    
    public virtual Organization Organization { get; set; } = null!;
}
