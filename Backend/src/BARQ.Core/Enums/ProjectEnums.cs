namespace BARQ.Core.Enums;

/// <summary>
/// Project type enumeration
/// </summary>
public enum ProjectType
{
    /// <summary>
    /// New product development from scratch
    /// </summary>
    NewProduct = 0,

    /// <summary>
    /// Enhancement to existing platform/product
    /// </summary>
    ExistingPlatform = 1,

    /// <summary>
    /// Maintenance and bug fixes
    /// </summary>
    Maintenance = 2,

    /// <summary>
    /// Research and development
    /// </summary>
    Research = 3
}

/// <summary>
/// Project status enumeration
/// </summary>
public enum ProjectStatus
{
    /// <summary>
    /// Project is in planning phase
    /// </summary>
    Planning = 0,

    /// <summary>
    /// Project is active and in development
    /// </summary>
    Active = 1,

    /// <summary>
    /// Project is on hold
    /// </summary>
    OnHold = 2,

    /// <summary>
    /// Project is completed
    /// </summary>
    Completed = 3,

    /// <summary>
    /// Project is cancelled
    /// </summary>
    Cancelled = 4,

    /// <summary>
    /// Project is archived
    /// </summary>
    Archived = 5
}

/// <summary>
/// Project priority enumeration
/// </summary>
public enum ProjectPriority
{
    /// <summary>
    /// Low priority
    /// </summary>
    Low = 0,

    /// <summary>
    /// Medium priority
    /// </summary>
    Medium = 1,

    /// <summary>
    /// High priority
    /// </summary>
    High = 2,

    /// <summary>
    /// Critical priority
    /// </summary>
    Critical = 3
}

/// <summary>
/// Project role enumeration
/// </summary>
public enum ProjectRole
{
    /// <summary>
    /// Project manager
    /// </summary>
    ProjectManager = 0,

    /// <summary>
    /// </summary>
    Developer = 1,

    /// <summary>
    /// </summary>
    Designer = 2,

    /// <summary>
    /// </summary>
    QualityAssurance = 3,

    /// <summary>
    /// </summary>
    BusinessAnalyst = 4,

    /// <summary>
    /// </summary>
    DevOpsEngineer = 5,

    /// <summary>
    /// </summary>
    Stakeholder = 6
}

/// <summary>
/// Risk level enumeration
/// </summary>
public enum RiskLevel
{
    /// <summary>
    /// </summary>
    Low = 0,

    /// <summary>
    /// Medium risk
    /// </summary>
    Medium = 1,

    /// <summary>
    /// </summary>
    High = 2,

    /// <summary>
    /// Critical risk
    /// </summary>
    Critical = 3
}

/// <summary>
/// Risk status enumeration
/// </summary>
public enum RiskStatus
{
    /// <summary>
    /// </summary>
    Identified = 0,

    /// <summary>
    /// </summary>
    Analyzing = 1,

    /// <summary>
    /// </summary>
    Mitigating = 2,

    /// <summary>
    /// </summary>
    Resolved = 3,

    /// <summary>
    /// </summary>
    Accepted = 4,

    /// <summary>
    /// </summary>
    Escalated = 5
}

