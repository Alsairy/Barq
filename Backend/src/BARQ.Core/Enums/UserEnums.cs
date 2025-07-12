namespace BARQ.Core.Enums;

/// <summary>
/// User status enumeration
/// </summary>
public enum UserStatus
{
    /// <summary>
    /// User is active and can access the system
    /// </summary>
    Active = 0,

    /// <summary>
    /// User is inactive and cannot access the system
    /// </summary>
    Inactive = 1,

    /// <summary>
    /// User is suspended temporarily
    /// </summary>
    Suspended = 2,

    /// <summary>
    /// User account is pending activation
    /// </summary>
    Pending = 3,

    /// <summary>
    /// User account is locked due to security reasons
    /// </summary>
    Locked = 4
}

/// <summary>
/// Authentication type enumeration
/// </summary>
public enum AuthenticationType
{
    /// <summary>
    /// Local authentication using email and password
    /// </summary>
    Local = 0,

    /// <summary>
    /// Azure Active Directory authentication
    /// </summary>
    AzureAD = 1,

    /// <summary>
    /// LDAP authentication
    /// </summary>
    LDAP = 2,

    /// <summary>
    /// SAML authentication
    /// </summary>
    SAML = 3,

    /// <summary>
    /// OAuth authentication
    /// </summary>
    OAuth = 4,

    /// <summary>
    /// OpenID Connect authentication
    /// </summary>
    OpenIDConnect = 5
}

