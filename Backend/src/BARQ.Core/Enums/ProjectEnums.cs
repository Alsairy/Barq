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
/// Priority enumeration
/// </summary>
public enum Priority
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

