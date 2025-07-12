namespace BARQ.Core.Enums;

/// <summary>
/// Subscription plans available in the BARQ platform
/// </summary>
public enum SubscriptionPlan
{
    /// <summary>
    /// Free plan with limited features
    /// </summary>
    Free = 0,

    /// <summary>
    /// Professional plan for small to medium teams
    /// </summary>
    Professional = 1,

    /// <summary>
    /// Enterprise plan for large organizations
    /// </summary>
    Enterprise = 2,

    /// <summary>
    /// Custom plan with tailored features
    /// </summary>
    Custom = 3
}

/// <summary>
/// Organization status
/// </summary>
public enum OrganizationStatus
{
    /// <summary>
    /// Organization is active and operational
    /// </summary>
    Active = 0,

    /// <summary>
    /// Organization is suspended
    /// </summary>
    Suspended = 1,

    /// <summary>
    /// Organization is inactive
    /// </summary>
    Inactive = 2,

    /// <summary>
    /// Organization is pending activation
    /// </summary>
    Pending = 3
}

