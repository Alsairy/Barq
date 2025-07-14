namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class LdapConfigurationValidationResponse
{
    /// <summary>
    /// Validation success status
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Validation errors
    /// </summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// Validation warnings
    /// </summary>
    public List<string> Warnings { get; set; } = new();

    /// <summary>
    /// </summary>
    public Dictionary<string, object>? ConfigurationDetails { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? LastValidated { get; set; }
}

/// <summary>
/// </summary>
public class LdapConfigurationResponse
{
    /// <summary>
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public Guid? ConfigurationId { get; set; }

    /// <summary>
    /// </summary>
    public Guid TenantId { get; set; }

    /// <summary>
    /// </summary>
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// </summary>
    public bool UseSsl { get; set; }

    /// <summary>
    /// </summary>
    public bool UseStartTls { get; set; }

    /// <summary>
    /// </summary>
    public string BaseDn { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? BindDn { get; set; }

    /// <summary>
    /// </summary>
    public string UserSearchFilter { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string GroupSearchFilter { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? UserDnPattern { get; set; }

    /// <summary>
    /// </summary>
    public string EmailAttribute { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string FirstNameAttribute { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string LastNameAttribute { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string DisplayNameAttribute { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string GroupMembershipAttribute { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// </summary>
    public bool AutoProvisionUsers { get; set; }

    /// <summary>
    /// </summary>
    public string? DefaultRole { get; set; }

    /// <summary>
    /// </summary>
    public Dictionary<string, string>? GroupRoleMappings { get; set; }

    /// <summary>
    /// </summary>
    public int ConnectionTimeout { get; set; }

    /// <summary>
    /// </summary>
    public int SearchTimeout { get; set; }

    /// <summary>
    /// </summary>
    public bool IsValid { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? LastSuccessfulAuth { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? LastValidation { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? LastSynchronization { get; set; }

    /// <summary>
    /// Error message if failed
    /// </summary>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// </summary>
public class LdapUserSearchResponse
{
    /// <summary>
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public List<LdapUserInfo> Users { get; set; } = new();

    /// <summary>
    /// </summary>
    public int TotalCount { get; set; }

    /// <summary>
    /// </summary>
    public long ExecutionTimeMs { get; set; }

    /// <summary>
    /// Error message if failed
    /// </summary>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// </summary>
public class LdapSynchronizationResponse
{
    /// <summary>
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public int UsersSynchronized { get; set; }

    /// <summary>
    /// </summary>
    public int UsersCreated { get; set; }

    /// <summary>
    /// </summary>
    public int UsersUpdated { get; set; }

    /// <summary>
    /// </summary>
    public int UsersDeactivated { get; set; }

    /// <summary>
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// </summary>
    public TimeSpan Duration => EndTime - StartTime;

    /// <summary>
    /// </summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// </summary>
    public List<string> Warnings { get; set; } = new();

    /// <summary>
    /// Error message if failed
    /// </summary>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// </summary>
public class LdapConnectionTestResponse
{
    /// <summary>
    /// </summary>
    public bool IsConnected { get; set; }

    /// <summary>
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public Dictionary<string, object> TestDetails { get; set; } = new();

    /// <summary>
    /// </summary>
    public long ConnectionTimeMs { get; set; }

    /// <summary>
    /// </summary>
    public string? ServerInfo { get; set; }

    /// <summary>
    /// </summary>
    public List<string> SupportedAuthMethods { get; set; } = new();

    /// <summary>
    /// Error message if failed
    /// </summary>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// </summary>
public class LdapUserInfo
{
    /// <summary>
    /// </summary>
    public string DistinguishedName { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public List<string> Groups { get; set; } = new();

    /// <summary>
    /// </summary>
    public Dictionary<string, object> Attributes { get; set; } = new();

    /// <summary>
    /// </summary>
    public bool IsActive { get; set; } = true;
}
