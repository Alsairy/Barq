using System.ComponentModel.DataAnnotations;

namespace BARQ.Core.Models.Requests;

/// <summary>
/// </summary>
public class SamlAuthenticationRequest
{
    /// <summary>
    /// </summary>
    [Required]
    public string TenantIdentifier { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? RelayState { get; set; }

    /// <summary>
    /// </summary>
    public string? ReturnUrl { get; set; }
}

/// <summary>
/// </summary>
public class SamlResponseRequest
{
    /// <summary>
    /// </summary>
    [Required]
    public string SamlResponse { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? RelayState { get; set; }
}

/// <summary>
/// </summary>
public class OAuthAuthenticationRequest
{
    /// <summary>
    /// </summary>
    [Required]
    public string TenantIdentifier { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    [Required]
    public string Provider { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// </summary>
    public string? ReturnUrl { get; set; }
}

/// <summary>
/// </summary>
public class OAuthCallbackRequest
{
    /// <summary>
    /// </summary>
    [Required]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// </summary>
    public string? ErrorDescription { get; set; }
}

/// <summary>
/// </summary>
public class OpenIdConnectAuthenticationRequest
{
    /// <summary>
    /// </summary>
    [Required]
    public string TenantIdentifier { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    [Required]
    public string Provider { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? Nonce { get; set; }

    /// <summary>
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// </summary>
    public string? ReturnUrl { get; set; }
}

/// <summary>
/// </summary>
public class OpenIdConnectCallbackRequest
{
    /// <summary>
    /// </summary>
    [Required]
    public string Code { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string? IdToken { get; set; }

    /// <summary>
    /// </summary>
    public string? State { get; set; }

    /// <summary>
    /// </summary>
    public string? Nonce { get; set; }

    /// <summary>
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// </summary>
    public string? ErrorDescription { get; set; }
}

/// <summary>
/// </summary>
public class UpdateSsoConfigurationRequest
{
    /// <summary>
    /// </summary>
    [Required]
    public Guid TenantId { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    public string Provider { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    [Required]
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
    public string? Certificate { get; set; }

    /// <summary>
    /// </summary>
    public string? ClientId { get; set; }

    /// <summary>
    /// </summary>
    public string? ClientSecret { get; set; }

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
    public bool AutoProvisionUsers { get; set; } = true;
}
