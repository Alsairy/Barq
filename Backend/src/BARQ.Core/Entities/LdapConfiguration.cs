namespace BARQ.Core.Entities;

/// <summary>
/// </summary>
public class LdapConfiguration : TenantEntity
{

    /// <summary>
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public int Port { get; set; } = 389;

    /// <summary>
    /// </summary>
    public bool UseSsl { get; set; } = false;

    /// <summary>
    /// </summary>
    public bool UseStartTls { get; set; } = false;

    /// <summary>
    /// </summary>
    public string BaseDn { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string BindDn { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string BindPassword { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string UserSearchFilter { get; set; } = "(&(objectClass=user)(sAMAccountName={0}))";

    /// <summary>
    /// </summary>
    public string GroupSearchFilter { get; set; } = "(&(objectClass=group)(member={0}))";

    /// <summary>
    /// </summary>
    public string UserDnPattern { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string EmailAttribute { get; set; } = "mail";

    /// <summary>
    /// </summary>
    public string FirstNameAttribute { get; set; } = "givenName";

    /// <summary>
    /// </summary>
    public string LastNameAttribute { get; set; } = "sn";

    /// <summary>
    /// </summary>
    public string DisplayNameAttribute { get; set; } = "displayName";

    /// <summary>
    /// </summary>
    public string GroupMembershipAttribute { get; set; } = "memberOf";

    /// <summary>
    /// </summary>
    public bool IsEnabled { get; set; } = false;

    /// <summary>
    /// </summary>
    public bool IsRequired { get; set; } = false;

    /// <summary>
    /// </summary>
    public bool AutoProvisionUsers { get; set; } = true;

    /// <summary>
    /// </summary>
    public string? DefaultRole { get; set; }

    /// <summary>
    /// </summary>
    public string? GroupRoleMappings { get; set; }

    /// <summary>
    /// </summary>
    public int ConnectionTimeout { get; set; } = 30;

    /// <summary>
    /// </summary>
    public int SearchTimeout { get; set; } = 30;

    /// <summary>
    /// </summary>
    public DateTime? LastSuccessfulAuth { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? LastValidation { get; set; }

    /// <summary>
    /// </summary>
    public bool IsValid { get; set; } = false;

    /// <summary>
    /// </summary>
    public string? ValidationError { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? LastSynchronization { get; set; }

    /// <summary>
    /// </summary>
    public virtual Organization? Tenant { get; set; }
}
