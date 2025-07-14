namespace BARQ.Core.Entities;

/// <summary>
/// </summary>
public class SsoConfiguration : TenantEntity
{

    /// <summary>
    /// </summary>
    public string Provider { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string ProviderName { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public bool IsEnabled { get; set; } = false;

    /// <summary>
    /// </summary>
    public bool IsRequired { get; set; } = false;

    /// <summary>
    /// </summary>
    public string ConfigurationJson { get; set; } = string.Empty;

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
    public string? AttributeMappings { get; set; }

    /// <summary>
    /// </summary>
    public string? DefaultRole { get; set; }

    /// <summary>
    /// </summary>
    public bool AutoProvisionUsers { get; set; } = true;

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
    public virtual Organization? Tenant { get; set; }
}
