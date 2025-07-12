namespace BARQ.Core.Enums;

/// <summary>
/// AI task type enumeration
/// </summary>
public enum AITaskType
{
    /// <summary>
    /// Business requirements analysis
    /// </summary>
    RequirementsAnalysis = 0,

    /// <summary>
    /// User story generation
    /// </summary>
    UserStoryGeneration = 1,

    /// <summary>
    /// Code generation
    /// </summary>
    CodeGeneration = 2,

    /// <summary>
    /// Code review and analysis
    /// </summary>
    CodeReview = 3,

    /// <summary>
    /// Test case generation
    /// </summary>
    TestCaseGeneration = 4,

    /// <summary>
    /// UI/UX design generation
    /// </summary>
    UIDesignGeneration = 5,

    /// <summary>
    /// Logo design generation
    /// </summary>
    LogoGeneration = 6,

    /// <summary>
    /// Product name generation
    /// </summary>
    NameGeneration = 7,

    /// <summary>
    /// Security analysis
    /// </summary>
    SecurityAnalysis = 8,

    /// <summary>
    /// Performance analysis
    /// </summary>
    PerformanceAnalysis = 9,

    /// <summary>
    /// Documentation generation
    /// </summary>
    DocumentationGeneration = 10,

    /// <summary>
    /// UX research and analysis
    /// </summary>
    UXResearch = 11,

    /// <summary>
    /// API documentation generation
    /// </summary>
    APIDocumentation = 12,

    /// <summary>
    /// Database schema generation
    /// </summary>
    DatabaseSchemaGeneration = 13,

    /// <summary>
    /// Deployment script generation
    /// </summary>
    DeploymentScriptGeneration = 14,

    /// <summary>
    /// Quality assurance analysis
    /// </summary>
    QualityAssurance = 15
}

/// <summary>
/// AI task status enumeration
/// </summary>
public enum AITaskStatus
{
    /// <summary>
    /// Task is pending execution
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Task is currently running
    /// </summary>
    Running = 1,

    /// <summary>
    /// Task completed successfully
    /// </summary>
    Completed = 2,

    /// <summary>
    /// Task failed with error
    /// </summary>
    Failed = 3,

    /// <summary>
    /// Task was cancelled
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// Task is waiting for human review
    /// </summary>
    PendingReview = 5,

    /// <summary>
    /// Task is approved after review
    /// </summary>
    Approved = 6,

    /// <summary>
    /// Task is rejected after review
    /// </summary>
    Rejected = 7,

    /// <summary>
    /// Task is paused
    /// </summary>
    Paused = 8
}

/// <summary>
/// AI provider enumeration
/// </summary>
public enum AIProvider
{
    /// <summary>
    /// OpenAI (GPT, DALL-E, Codex)
    /// </summary>
    OpenAI = 0,

    /// <summary>
    /// Anthropic (Claude)
    /// </summary>
    Anthropic = 1,

    /// <summary>
    /// Google (Gemini, Bard)
    /// </summary>
    Google = 2,

    /// <summary>
    /// Microsoft Azure OpenAI
    /// </summary>
    AzureOpenAI = 3,

    /// <summary>
    /// GitHub Copilot
    /// </summary>
    GitHubCopilot = 4,

    /// <summary>
    /// Devin AI
    /// </summary>
    DevinAI = 5,

    /// <summary>
    /// Manus AI
    /// </summary>
    ManusAI = 6,

    /// <summary>
    /// Grok (X.AI)
    /// </summary>
    Grok = 7,

    /// <summary>
    /// Stability AI
    /// </summary>
    StabilityAI = 8,

    /// <summary>
    /// Midjourney
    /// </summary>
    Midjourney = 9,

    /// <summary>
    /// Custom AI provider
    /// </summary>
    Custom = 10
}

/// <summary>
/// Review status enumeration
/// </summary>
public enum ReviewStatus
{
    /// <summary>
    /// Pending review
    /// </summary>
    Pending = 0,

    /// <summary>
    /// Under review
    /// </summary>
    InReview = 1,

    /// <summary>
    /// Approved
    /// </summary>
    Approved = 2,

    /// <summary>
    /// Rejected
    /// </summary>
    Rejected = 3,

    /// <summary>
    /// Requires changes
    /// </summary>
    RequiresChanges = 4
}

