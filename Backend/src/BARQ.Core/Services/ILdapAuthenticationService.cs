using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface ILdapAuthenticationService
{
    /// <summary>
    /// </summary>
    /// <param name="request">LDAP authentication request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<AuthenticationResponse> AuthenticateAsync(LdapAuthenticationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<LdapConfigurationValidationResponse> ValidateLdapConfigurationAsync(Guid tenantId, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<LdapConfigurationResponse> GetLdapConfigurationAsync(Guid tenantId, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="request">Update LDAP configuration request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<LdapConfigurationResponse> UpdateLdapConfigurationAsync(UpdateLdapConfigurationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="request">LDAP user search request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<LdapUserSearchResponse> SearchUsersAsync(LdapUserSearchRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<LdapSynchronizationResponse> SynchronizeUsersAsync(Guid tenantId, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<LdapConnectionTestResponse> TestConnectionAsync(Guid tenantId, CancellationToken cancellationToken = default);
}
