using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Entities;

/// <summary>Configuration for Business Requirement Document (BRD) templates, including structure, defaults, and metadata. (important-comment)</summary>
public class BRDTemplateConfiguration : TenantEntity
{
    /// <summary>Gets or sets the human-readable name of the template. (important-comment)</summary>
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    /// <summary>Gets or sets an optional description for the template. (important-comment)</summary>
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    /// <summary>Gets or sets the document sections and ordering definition in JSON format. (important-comment)</summary>
    public string DocumentStructure { get; set; } = string.Empty;
    
    /// <summary>Gets or sets the default sprint planning configuration in JSON format. (important-comment)</summary>
    public string SprintConfiguration { get; set; } = string.Empty;
    
    /// <summary>Gets or sets user story templates in JSON format. (important-comment)</summary>
    public string UserStoryTemplates { get; set; } = string.Empty;
    
    /// <summary>Gets or sets acceptance criteria templates in JSON format. (important-comment)</summary>
    public string AcceptanceCriteria { get; set; } = string.Empty;
    
    /// <summary>Gets or sets stakeholder analysis content in JSON format. (important-comment)</summary>
    public string StakeholderAnalysis { get; set; } = string.Empty;
    
    /// <summary>Gets or sets business process modeling content in JSON format. (important-comment)</summary>
    public string BusinessProcessModeling { get; set; } = string.Empty;
    
    /// <summary>Gets or sets a value indicating whether the template is active. (important-comment)</summary>
    public bool IsActive { get; set; } = true;
    
    /// <summary>Gets or sets the priority used when selecting among templates; lower means higher priority. (important-comment)</summary>
    public int Priority { get; set; } = 1;
    
    /// <summary>Gets or sets the owning organization for multi-tenant scoping. (important-comment)</summary>
    public virtual Organization Organization { get; set; } = null!;
}
