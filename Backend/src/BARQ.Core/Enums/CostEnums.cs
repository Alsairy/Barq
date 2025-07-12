namespace BARQ.Core.Enums;

/// <summary>
/// Cost category enumeration
/// </summary>
public enum CostCategory
{
    /// <summary>
    /// AI service costs
    /// </summary>
    AIServices = 0,

    /// <summary>
    /// Infrastructure costs
    /// </summary>
    Infrastructure = 1,

    /// <summary>
    /// Software licensing costs
    /// </summary>
    SoftwareLicensing = 2,

    /// <summary>
    /// Third-party integrations
    /// </summary>
    ThirdPartyIntegrations = 3,

    /// <summary>
    /// Cloud services
    /// </summary>
    CloudServices = 4,

    /// <summary>
    /// Development tools
    /// </summary>
    DevelopmentTools = 5,

    /// <summary>
    /// Security services
    /// </summary>
    SecurityServices = 6,

    /// <summary>
    /// Monitoring and analytics
    /// </summary>
    MonitoringAnalytics = 7,

    /// <summary>
    /// Support and maintenance
    /// </summary>
    SupportMaintenance = 8,

    /// <summary>
    /// Training and certification
    /// </summary>
    TrainingCertification = 9,

    /// <summary>
    /// Consulting services
    /// </summary>
    ConsultingServices = 10,

    /// <summary>
    /// Other miscellaneous costs
    /// </summary>
    Other = 99
}

/// <summary>
/// Cost type enumeration
/// </summary>
public enum CostType
{
    /// <summary>
    /// One-time cost
    /// </summary>
    OneTime = 0,

    /// <summary>
    /// Recurring monthly cost
    /// </summary>
    Monthly = 1,

    /// <summary>
    /// Recurring annual cost
    /// </summary>
    Annual = 2,

    /// <summary>
    /// Usage-based cost
    /// </summary>
    UsageBased = 3,

    /// <summary>
    /// Per-transaction cost
    /// </summary>
    PerTransaction = 4,

    /// <summary>
    /// Per-user cost
    /// </summary>
    PerUser = 5,

    /// <summary>
    /// Per-project cost
    /// </summary>
    PerProject = 6,

    /// <summary>
    /// Variable cost
    /// </summary>
    Variable = 7,

    /// <summary>
    /// Fixed cost
    /// </summary>
    Fixed = 8
}

/// <summary>
/// Cost allocation method enumeration
/// </summary>
public enum CostAllocationMethod
{
    /// <summary>
    /// Direct allocation to specific entity
    /// </summary>
    Direct = 0,

    /// <summary>
    /// Proportional allocation based on usage
    /// </summary>
    Proportional = 1,

    /// <summary>
    /// Equal distribution among entities
    /// </summary>
    EqualDistribution = 2,

    /// <summary>
    /// Weighted allocation based on factors
    /// </summary>
    Weighted = 3,

    /// <summary>
    /// Activity-based costing
    /// </summary>
    ActivityBased = 4,

    /// <summary>
    /// Time-based allocation
    /// </summary>
    TimeBased = 5,

    /// <summary>
    /// Resource-based allocation
    /// </summary>
    ResourceBased = 6,

    /// <summary>
    /// Custom allocation method
    /// </summary>
    Custom = 99
}

/// <summary>
/// Cost summary type enumeration
/// </summary>
public enum CostSummaryType
{
    /// <summary>
    /// Daily cost summary
    /// </summary>
    Daily = 0,

    /// <summary>
    /// Weekly cost summary
    /// </summary>
    Weekly = 1,

    /// <summary>
    /// Monthly cost summary
    /// </summary>
    Monthly = 2,

    /// <summary>
    /// Quarterly cost summary
    /// </summary>
    Quarterly = 3,

    /// <summary>
    /// Annual cost summary
    /// </summary>
    Annual = 4,

    /// <summary>
    /// Project-based summary
    /// </summary>
    Project = 5,

    /// <summary>
    /// Sprint-based summary
    /// </summary>
    Sprint = 6,

    /// <summary>
    /// Team-based summary
    /// </summary>
    Team = 7,

    /// <summary>
    /// Department-based summary
    /// </summary>
    Department = 8,

    /// <summary>
    /// Provider-based summary
    /// </summary>
    Provider = 9,

    /// <summary>
    /// Custom summary type
    /// </summary>
    Custom = 99
}

