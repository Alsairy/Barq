using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Models.Requests;

/// <summary>
/// </summary>
public class LdapAuthenticationRequest
{
    /// <summary>
    /// </summary>
    [Required]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    [Required]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    [Required]
    public string TenantIdentifier { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? Domain { get; set; }
}

/// <summary>
/// </summary>
public class LdapUserSearchRequest
{
    /// <summary>
    /// </summary>
    [Required]
    public Guid TenantId { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    public string SearchQuery { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public List<string>? Attributes { get; set; }

    /// <summary>
    /// </summary>
    public int MaxResults { get; set; } = 100;

    /// <summary>
    /// </summary>
    public int TimeoutSeconds { get; set; } = 30;
}

/// <summary>
/// </summary>
public class UpdateLdapConfigurationRequest
{
    /// <summary>
    /// </summary>
    [Required]
    public Guid TenantId { get; set; }

    /// <summary>
    /// </summary>
    [Required]
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
    [Required]
    public string BaseDn { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? BindDn { get; set; }

    /// <summary>
    /// </summary>
    public string? BindPassword { get; set; }

    /// <summary>
    /// </summary>
    public string UserSearchFilter { get; set; } = "(&(objectClass=user)(sAMAccountName={0}))";

    /// <summary>
    /// </summary>
    public string GroupSearchFilter { get; set; } = "(&(objectClass=group)(member={0}))";

    /// <summary>
    /// </summary>
    public string? UserDnPattern { get; set; }

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
    public Dictionary<string, string>? GroupRoleMappings { get; set; }

    /// <summary>
    /// </summary>
    public int ConnectionTimeout { get; set; } = 30;

    /// <summary>
    /// </summary>
    public int SearchTimeout { get; set; } = 30;
}
