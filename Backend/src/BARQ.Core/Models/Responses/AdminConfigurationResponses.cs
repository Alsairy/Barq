using BARQ.Core.Entities;

namespace BARQ.Core.Models.Responses;

public class AdminConfigurationResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }
    public List<string> Errors { get; set; } = new();
}

public class AdminConfigurationListResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<object> Data { get; set; } = new();
    public int TotalCount { get; set; }
    public List<string> Errors { get; set; } = new();
}

public class TemplateManagementResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public object? Data { get; set; }
    public List<string> Errors { get; set; } = new();
}

public class TemplateManagementListResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<object> Data { get; set; } = new();
    public int TotalCount { get; set; }
    public string Category { get; set; } = string.Empty;
    public List<string> Errors { get; set; } = new();
}

public class CodeGenerationConfigDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string TechnologyStack { get; set; } = string.Empty;
    public string ArchitecturalPatterns { get; set; } = string.Empty;
    public string CodingStandards { get; set; } = string.Empty;
    public string QualityRequirements { get; set; } = string.Empty;
    public string VersionRequirements { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class BRDTemplateConfigDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string DocumentStructure { get; set; } = string.Empty;
    public string SprintConfiguration { get; set; } = string.Empty;
    public string UserStoryTemplates { get; set; } = string.Empty;
    public string AcceptanceCriteria { get; set; } = string.Empty;
    public string StakeholderAnalysis { get; set; } = string.Empty;
    public string BusinessProcessModeling { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class ProposalConfigDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string ProposalTemplates { get; set; } = string.Empty;
    public string TechnicalSpecifications { get; set; } = string.Empty;
    public string FinancialModeling { get; set; } = string.Empty;
    public string RiskAssessment { get; set; } = string.Empty;
    public string ImplementationPlanning { get; set; } = string.Empty;
    public string CompetitivePositioning { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class PresentationConfigDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string PresentationTemplates { get; set; } = string.Empty;
    public string SlideStructure { get; set; } = string.Empty;
    public string DesignBranding { get; set; } = string.Empty;
    public string ContentDepth { get; set; } = string.Empty;
    public string AudienceCustomization { get; set; } = string.Empty;
    public string InteractiveElements { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class DesignConfigDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string VisualDesignStandards { get; set; } = string.Empty;
    public string BrandCompliance { get; set; } = string.Empty;
    public string UIUXDesignPatterns { get; set; } = string.Empty;
    public string ImageStyleConfiguration { get; set; } = string.Empty;
    public string DesignSystemIntegration { get; set; } = string.Empty;
    public string AccessibilityCompliance { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class TestingConfigDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string TestingMethodology { get; set; } = string.Empty;
    public string TestCoverage { get; set; } = string.Empty;
    public string PerformanceTesting { get; set; } = string.Empty;
    public string SecurityTesting { get; set; } = string.Empty;
    public string AutomatedTesting { get; set; } = string.Empty;
    public string ComplianceTesting { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public int Priority { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
