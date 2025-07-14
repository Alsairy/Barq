namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class SamlAuthenticationResponse
{
    /// <summary>
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? AuthenticationUrl { get; set; }

    /// <summary>
    /// </summary>
    public string? RelayState { get; set; }

    /// <summary>
    /// Error message if failed
    /// </summary>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// </summary>
public class OAuthAuthenticationResponse
{
    /// <summary>
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? AuthorizationUrl { get; set; }

    /// <summary>
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// Error message if failed
    /// </summary>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// </summary>
public class OpenIdConnectAuthenticationResponse
{
    /// <summary>
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? AuthorizationUrl { get; set; }

    /// <summary>
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// </summary>
    public string? Nonce { get; set; }

    /// <summary>
    /// Error message if failed
    /// </summary>
    public string? ErrorMessage { get; set; }
}

/// <summary>
/// </summary>
public class SsoConfigurationValidationResponse
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
public class SsoConfigurationResponse
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
    public string Provider { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string ProviderName { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// </summary>
    public bool IsRequired { get; set; }

    /// <summary>
    /// </summary>
    public string? EntityId { get; set; }

    /// <summary>
    /// </summary>
    public string? SsoUrl { get; set; }

    /// <summary>
    /// </summary>
    public string? LogoutUrl { get; set; }

    /// <summary>
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// </summary>
    public string? Scopes { get; set; }

    /// <summary>
    /// </summary>
    public string? Authority { get; set; }

    /// <summary>
    /// </summary>
    public string? CallbackUrl { get; set; }

    /// <summary>
    /// </summary>
    public Dictionary<string, string>? AttributeMappings { get; set; }

    /// <summary>
    /// </summary>
    public string? DefaultRole { get; set; }

    /// <summary>
    /// </summary>
    public bool AutoProvisionUsers { get; set; }

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
    /// Error message if failed
    /// </summary>
    public string? ErrorMessage { get; set; }
}
