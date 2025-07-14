using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface ISsoAuthenticationService
{
    /// <summary>
    /// </summary>
    /// <param name="request">SAML authentication request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<SamlAuthenticationResponse> InitiateSamlAuthenticationAsync(SamlAuthenticationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="request">SAML response request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<AuthenticationResponse> ProcessSamlResponseAsync(SamlResponseRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="request">OAuth authentication request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<OAuthAuthenticationResponse> InitiateOAuthAuthenticationAsync(OAuthAuthenticationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="request">OAuth callback request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<AuthenticationResponse> ProcessOAuthCallbackAsync(OAuthCallbackRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="request">OpenID Connect authentication request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<OpenIdConnectAuthenticationResponse> InitiateOpenIdConnectAuthenticationAsync(OpenIdConnectAuthenticationRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="request">OpenID Connect callback request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<AuthenticationResponse> ProcessOpenIdConnectCallbackAsync(OpenIdConnectCallbackRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="ssoProvider">SSO provider type</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<SsoConfigurationValidationResponse> ValidateSsoConfigurationAsync(Guid tenantId, string ssoProvider, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="tenantId">Tenant ID</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<SsoConfigurationResponse> GetSsoConfigurationAsync(Guid tenantId, CancellationToken cancellationToken = default);

    /// <summary>
    /// </summary>
    /// <param name="request">Update SSO configuration request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    Task<SsoConfigurationResponse> UpdateSsoConfigurationAsync(UpdateSsoConfigurationRequest request, CancellationToken cancellationToken = default);
}
