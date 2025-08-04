using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Models.Requests;

public class CreateCodeGenerationConfigRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public string TechnologyStack { get; set; } = string.Empty;
    public string ArchitecturalPatterns { get; set; } = string.Empty;
    public string CodingStandards { get; set; } = string.Empty;
    public string QualityRequirements { get; set; } = string.Empty;
    public string VersionRequirements { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int Priority { get; set; } = 1;
}

public class UpdateCodeGenerationConfigRequest : CreateCodeGenerationConfigRequest
{
    public Guid Id { get; set; }
}

public class CreateBRDTemplateConfigRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public string DocumentStructure { get; set; } = string.Empty;
    public string SprintConfiguration { get; set; } = string.Empty;
    public string UserStoryTemplates { get; set; } = string.Empty;
    public string AcceptanceCriteria { get; set; } = string.Empty;
    public string StakeholderAnalysis { get; set; } = string.Empty;
    public string BusinessProcessModeling { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int Priority { get; set; } = 1;
}

public class UpdateBRDTemplateConfigRequest : CreateBRDTemplateConfigRequest
{
    public Guid Id { get; set; }
}

public class CreateProposalConfigRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public string ProposalTemplates { get; set; } = string.Empty;
    public string TechnicalSpecifications { get; set; } = string.Empty;
    public string FinancialModeling { get; set; } = string.Empty;
    public string RiskAssessment { get; set; } = string.Empty;
    public string ImplementationPlanning { get; set; } = string.Empty;
    public string CompetitivePositioning { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int Priority { get; set; } = 1;
}

public class UpdateProposalConfigRequest : CreateProposalConfigRequest
{
    public Guid Id { get; set; }
}

public class CreatePresentationConfigRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public string PresentationTemplates { get; set; } = string.Empty;
    public string SlideStructure { get; set; } = string.Empty;
    public string DesignBranding { get; set; } = string.Empty;
    public string ContentDepth { get; set; } = string.Empty;
    public string AudienceCustomization { get; set; } = string.Empty;
    public string InteractiveElements { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int Priority { get; set; } = 1;
}

public class UpdatePresentationConfigRequest : CreatePresentationConfigRequest
{
    public Guid Id { get; set; }
}

public class CreateDesignConfigRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public string VisualDesignStandards { get; set; } = string.Empty;
    public string BrandCompliance { get; set; } = string.Empty;
    public string UIUXDesignPatterns { get; set; } = string.Empty;
    public string ImageStyleConfiguration { get; set; } = string.Empty;
    public string DesignSystemIntegration { get; set; } = string.Empty;
    public string AccessibilityCompliance { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int Priority { get; set; } = 1;
}

public class UpdateDesignConfigRequest : CreateDesignConfigRequest
{
    public Guid Id { get; set; }
}

public class CreateTestingConfigRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public string TestingMethodology { get; set; } = string.Empty;
    public string TestCoverage { get; set; } = string.Empty;
    public string PerformanceTesting { get; set; } = string.Empty;
    public string SecurityTesting { get; set; } = string.Empty;
    public string AutomatedTesting { get; set; } = string.Empty;
    public string ComplianceTesting { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public int Priority { get; set; } = 1;
}

public class UpdateTestingConfigRequest : CreateTestingConfigRequest
{
    public Guid Id { get; set; }
}

public class UpdateConstraintRulesRequest
{
    public Guid ConfigurationId { get; set; }
    public string ConfigurationType { get; set; } = string.Empty;
    public string ConstraintRules { get; set; } = string.Empty;
}

public class CreateTemplateRequest
{
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;
    
    [MaxLength(1000)]
    public string? Description { get; set; }
    
    public string Category { get; set; } = string.Empty;
    public string TemplateContent { get; set; } = string.Empty;
    public string Variables { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

public class UpdateTemplateRequest : CreateTemplateRequest
{
    public Guid Id { get; set; }
}
