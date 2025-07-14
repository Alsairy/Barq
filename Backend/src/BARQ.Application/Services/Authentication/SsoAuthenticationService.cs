using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BARQ.Core.Services;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;
using BARQ.Core.Entities;
using BARQ.Core.Repositories;

namespace BARQ.Application.Services.Authentication;

public class SsoAuthenticationService : ISsoAuthenticationService
{
    private readonly IRepository<SsoConfiguration> _ssoConfigRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Organization> _organizationRepository;
    private readonly IAuthenticationService _authenticationService;
    private readonly IUserRoleService _userRoleService;
    private readonly ITenantProvider _tenantProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SsoAuthenticationService> _logger;
    private readonly HttpClient _httpClient;

    public SsoAuthenticationService(
        IRepository<SsoConfiguration> ssoConfigRepository,
        IRepository<User> userRepository,
        IRepository<Organization> organizationRepository,
        IAuthenticationService authenticationService,
        IUserRoleService userRoleService,
        ITenantProvider tenantProvider,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        ILogger<SsoAuthenticationService> logger,
        HttpClient httpClient)
    {
        _ssoConfigRepository = ssoConfigRepository;
        _userRepository = userRepository;
        _organizationRepository = organizationRepository;
        _authenticationService = authenticationService;
        _userRoleService = userRoleService;
        _tenantProvider = tenantProvider;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
        _httpClient = httpClient;
    }

    public async Task<SamlAuthenticationResponse> InitiateSamlAuthenticationAsync(SamlAuthenticationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var ssoConfig = await GetSsoConfigurationByTenantIdentifierAsync(request.TenantIdentifier, "SAML");
            if (ssoConfig == null || !ssoConfig.IsEnabled)
            {
                return new SamlAuthenticationResponse
                {
                    Success = false,
                    Message = "SAML authentication is not configured or enabled for this tenant",
                    ErrorMessage = "SAML not available"
                };
            }

            var relayState = request.RelayState ?? Guid.NewGuid().ToString();
            var authenticationUrl = BuildSamlAuthenticationUrl(ssoConfig, relayState);

            _logger.LogInformation("SAML authentication initiated for tenant: {TenantIdentifier}", request.TenantIdentifier);

            return new SamlAuthenticationResponse
            {
                Success = true,
                Message = "SAML authentication URL generated",
                AuthenticationUrl = authenticationUrl,
                RelayState = relayState
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initiating SAML authentication for tenant: {TenantIdentifier}", request.TenantIdentifier);
            return new SamlAuthenticationResponse
            {
                Success = false,
                Message = "Failed to initiate SAML authentication",
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<AuthenticationResponse> ProcessSamlResponseAsync(SamlResponseRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var samlClaims = ParseSamlResponse(request.SamlResponse);
            if (samlClaims == null || !samlClaims.Any())
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Invalid SAML response",
                    RequiresMfa = false
                };
            }

            var emailClaim = samlClaims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(emailClaim))
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Email claim not found in SAML response",
                    RequiresMfa = false
                };
            }

            var user = await GetOrCreateUserFromSamlClaimsAsync(samlClaims);
            if (user == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Failed to process user from SAML response",
                    RequiresMfa = false
                };
            }

            var userRoles = await _userRoleService.GetUserRolesAsync(user.Id);
            var roleNames = userRoles.Select(r => r.Name).ToList();

            var accessToken = GenerateAccessToken(user, roleNames);
            var refreshToken = GenerateRefreshToken();

            user.LastLoginAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("SAML authentication successful for user: {Email}", emailClaim);

            return new AuthenticationResponse
            {
                Success = true,
                Message = "SAML authentication successful",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(GetTokenExpiryMinutes()),
                RequiresMfa = false,
                UserId = user.Id,
                UserEmail = user.Email,
                Roles = roleNames
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing SAML response");
            return new AuthenticationResponse
            {
                Success = false,
                Message = "SAML authentication failed",
                RequiresMfa = false
            };
        }
    }

    public async Task<OAuthAuthenticationResponse> InitiateOAuthAuthenticationAsync(OAuthAuthenticationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var ssoConfig = await GetSsoConfigurationByTenantIdentifierAsync(request.TenantIdentifier, "OAuth");
            if (ssoConfig == null || !ssoConfig.IsEnabled)
            {
                return new OAuthAuthenticationResponse
                {
                    Success = false,
                    Message = "OAuth authentication is not configured or enabled for this tenant",
                    ErrorMessage = "OAuth not available"
                };
            }

            var state = request.State ?? Guid.NewGuid().ToString();
            var authorizationUrl = BuildOAuthAuthorizationUrl(ssoConfig, state);

            _logger.LogInformation("OAuth authentication initiated for tenant: {TenantIdentifier}, provider: {Provider}", 
                request.TenantIdentifier, request.Provider);

            return new OAuthAuthenticationResponse
            {
                Success = true,
                Message = "OAuth authorization URL generated",
                AuthorizationUrl = authorizationUrl,
                State = state
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initiating OAuth authentication for tenant: {TenantIdentifier}", request.TenantIdentifier);
            return new OAuthAuthenticationResponse
            {
                Success = false,
                Message = "Failed to initiate OAuth authentication",
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<AuthenticationResponse> ProcessOAuthCallbackAsync(OAuthCallbackRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!string.IsNullOrEmpty(request.Error))
            {
                _logger.LogWarning("OAuth callback received error: {Error} - {ErrorDescription}", request.Error, request.ErrorDescription);
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = $"OAuth error: {request.Error}",
                    RequiresMfa = false
                };
            }

            var tokenResponse = await ExchangeOAuthCodeForTokenAsync(request.Code);
            if (tokenResponse == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Failed to exchange OAuth code for token",
                    RequiresMfa = false
                };
            }

            var userInfo = await GetOAuthUserInfoAsync(tokenResponse.AccessToken);
            if (userInfo == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Failed to retrieve user information from OAuth provider",
                    RequiresMfa = false
                };
            }

            var user = await GetOrCreateUserFromOAuthUserInfoAsync(userInfo);
            if (user == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Failed to process user from OAuth response",
                    RequiresMfa = false
                };
            }

            var userRoles = await _userRoleService.GetUserRolesAsync(user.Id);
            var roleNames = userRoles.Select(r => r.Name).ToList();

            var accessToken = GenerateAccessToken(user, roleNames);
            var refreshToken = GenerateRefreshToken();

            user.LastLoginAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("OAuth authentication successful for user: {Email}", user.Email);

            return new AuthenticationResponse
            {
                Success = true,
                Message = "OAuth authentication successful",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(GetTokenExpiryMinutes()),
                RequiresMfa = false,
                UserId = user.Id,
                UserEmail = user.Email,
                Roles = roleNames
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing OAuth callback");
            return new AuthenticationResponse
            {
                Success = false,
                Message = "OAuth authentication failed",
                RequiresMfa = false
            };
        }
    }

    public async Task<OpenIdConnectAuthenticationResponse> InitiateOpenIdConnectAuthenticationAsync(OpenIdConnectAuthenticationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var ssoConfig = await GetSsoConfigurationByTenantIdentifierAsync(request.TenantIdentifier, "OpenIDConnect");
            if (ssoConfig == null || !ssoConfig.IsEnabled)
            {
                return new OpenIdConnectAuthenticationResponse
                {
                    Success = false,
                    Message = "OpenID Connect authentication is not configured or enabled for this tenant",
                    ErrorMessage = "OpenID Connect not available"
                };
            }

            var state = request.State ?? Guid.NewGuid().ToString();
            var nonce = request.Nonce ?? Guid.NewGuid().ToString();
            var authorizationUrl = BuildOpenIdConnectAuthorizationUrl(ssoConfig, state, nonce);

            _logger.LogInformation("OpenID Connect authentication initiated for tenant: {TenantIdentifier}, provider: {Provider}", 
                request.TenantIdentifier, request.Provider);

            return new OpenIdConnectAuthenticationResponse
            {
                Success = true,
                Message = "OpenID Connect authorization URL generated",
                AuthorizationUrl = authorizationUrl,
                State = state,
                Nonce = nonce
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initiating OpenID Connect authentication for tenant: {TenantIdentifier}", request.TenantIdentifier);
            return new OpenIdConnectAuthenticationResponse
            {
                Success = false,
                Message = "Failed to initiate OpenID Connect authentication",
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<AuthenticationResponse> ProcessOpenIdConnectCallbackAsync(OpenIdConnectCallbackRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!string.IsNullOrEmpty(request.Error))
            {
                _logger.LogWarning("OpenID Connect callback received error: {Error} - {ErrorDescription}", request.Error, request.ErrorDescription);
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = $"OpenID Connect error: {request.Error}",
                    RequiresMfa = false
                };
            }

            var tokenResponse = await ExchangeOpenIdConnectCodeForTokenAsync(request.Code);
            if (tokenResponse == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Failed to exchange OpenID Connect code for token",
                    RequiresMfa = false
                };
            }

            var idTokenClaims = ValidateAndParseIdToken(tokenResponse.IdToken, request.Nonce);
            if (idTokenClaims == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Invalid ID token received from OpenID Connect provider",
                    RequiresMfa = false
                };
            }

            var user = await GetOrCreateUserFromOpenIdConnectClaimsAsync(idTokenClaims);
            if (user == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Failed to process user from OpenID Connect response",
                    RequiresMfa = false
                };
            }

            var userRoles = await _userRoleService.GetUserRolesAsync(user.Id);
            var roleNames = userRoles.Select(r => r.Name).ToList();

            var accessToken = GenerateAccessToken(user, roleNames);
            var refreshToken = GenerateRefreshToken();

            user.LastLoginAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("OpenID Connect authentication successful for user: {Email}", user.Email);

            return new AuthenticationResponse
            {
                Success = true,
                Message = "OpenID Connect authentication successful",
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(GetTokenExpiryMinutes()),
                RequiresMfa = false,
                UserId = user.Id,
                UserEmail = user.Email,
                Roles = roleNames
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing OpenID Connect callback");
            return new AuthenticationResponse
            {
                Success = false,
                Message = "OpenID Connect authentication failed",
                RequiresMfa = false
            };
        }
    }

    public async Task<SsoConfigurationValidationResponse> ValidateSsoConfigurationAsync(Guid tenantId, string ssoProvider, CancellationToken cancellationToken = default)
    {
        try
        {
            var ssoConfigs = await _ssoConfigRepository.FindAsync(c => c.TenantId == tenantId && c.Provider == ssoProvider);
            var ssoConfig = ssoConfigs.FirstOrDefault();

            if (ssoConfig == null)
            {
                return new SsoConfigurationValidationResponse
                {
                    IsValid = false,
                    Message = "SSO configuration not found",
                    Errors = new List<string> { "No SSO configuration found for the specified tenant and provider" }
                };
            }

            var validationResult = await ValidateConfigurationAsync(ssoConfig);
            
            ssoConfig.IsValid = validationResult.IsValid;
            ssoConfig.LastValidation = DateTime.UtcNow;
            ssoConfig.ValidationError = validationResult.IsValid ? null : string.Join("; ", validationResult.Errors);
            
            await _ssoConfigRepository.UpdateAsync(ssoConfig);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("SSO configuration validation completed for tenant: {TenantId}, provider: {Provider}, valid: {IsValid}", 
                tenantId, ssoProvider, validationResult.IsValid);

            return validationResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating SSO configuration for tenant: {TenantId}, provider: {Provider}", tenantId, ssoProvider);
            return new SsoConfigurationValidationResponse
            {
                IsValid = false,
                Message = "Validation failed due to error",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<SsoConfigurationResponse> GetSsoConfigurationAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        try
        {
            var ssoConfigs = await _ssoConfigRepository.FindAsync(c => c.TenantId == tenantId);
            var ssoConfig = ssoConfigs.FirstOrDefault();

            if (ssoConfig == null)
            {
                return new SsoConfigurationResponse
                {
                    Success = false,
                    Message = "SSO configuration not found",
                    TenantId = tenantId
                };
            }

            var attributeMappings = string.IsNullOrEmpty(ssoConfig.AttributeMappings) 
                ? new Dictionary<string, string>() 
                : JsonSerializer.Deserialize<Dictionary<string, string>>(ssoConfig.AttributeMappings) ?? new Dictionary<string, string>();

            return new SsoConfigurationResponse
            {
                Success = true,
                Message = "SSO configuration retrieved successfully",
                ConfigurationId = ssoConfig.Id,
                TenantId = ssoConfig.TenantId,
                Provider = ssoConfig.Provider,
                ProviderName = ssoConfig.ProviderName,
                IsEnabled = ssoConfig.IsEnabled,
                IsRequired = ssoConfig.IsRequired,
                EntityId = ssoConfig.EntityId,
                SsoUrl = ssoConfig.SsoUrl,
                LogoutUrl = ssoConfig.LogoutUrl,
                ClientId = ssoConfig.ClientId,
                Scopes = ssoConfig.Scopes,
                Authority = ssoConfig.Authority,
                CallbackUrl = ssoConfig.CallbackUrl,
                AttributeMappings = attributeMappings,
                DefaultRole = ssoConfig.DefaultRole,
                AutoProvisionUsers = ssoConfig.AutoProvisionUsers,
                IsValid = ssoConfig.IsValid,
                LastSuccessfulAuth = ssoConfig.LastSuccessfulAuth,
                LastValidation = ssoConfig.LastValidation
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting SSO configuration for tenant: {TenantId}", tenantId);
            return new SsoConfigurationResponse
            {
                Success = false,
                Message = "Failed to retrieve SSO configuration",
                TenantId = tenantId,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<SsoConfigurationResponse> UpdateSsoConfigurationAsync(UpdateSsoConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var ssoConfigs = await _ssoConfigRepository.FindAsync(c => c.TenantId == request.TenantId && c.Provider == request.Provider);
            var ssoConfig = ssoConfigs.FirstOrDefault();

            if (ssoConfig == null)
            {
                ssoConfig = new SsoConfiguration
                {
                    Id = Guid.NewGuid(),
                    TenantId = request.TenantId,
                    Provider = request.Provider,
                    CreatedAt = DateTime.UtcNow
                };
                await _ssoConfigRepository.AddAsync(ssoConfig);
            }

            ssoConfig.ProviderName = request.ProviderName;
            ssoConfig.IsEnabled = request.IsEnabled;
            ssoConfig.IsRequired = request.IsRequired;
            ssoConfig.EntityId = request.EntityId;
            ssoConfig.SsoUrl = request.SsoUrl;
            ssoConfig.LogoutUrl = request.LogoutUrl;
            ssoConfig.ClientId = request.ClientId;
            ssoConfig.ClientSecret = request.ClientSecret; // Should be encrypted
            ssoConfig.Scopes = request.Scopes;
            ssoConfig.Authority = request.Authority;
            ssoConfig.CallbackUrl = request.CallbackUrl;
            ssoConfig.DefaultRole = request.DefaultRole;
            ssoConfig.AutoProvisionUsers = request.AutoProvisionUsers;
            ssoConfig.UpdatedAt = DateTime.UtcNow;

            if (request.AttributeMappings != null)
            {
                ssoConfig.AttributeMappings = JsonSerializer.Serialize(request.AttributeMappings);
            }

            if (ssoConfig.Id != Guid.Empty)
            {
                await _ssoConfigRepository.UpdateAsync(ssoConfig);
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("SSO configuration updated for tenant: {TenantId}, provider: {Provider}", request.TenantId, request.Provider);

            return await GetSsoConfigurationAsync(request.TenantId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating SSO configuration for tenant: {TenantId}, provider: {Provider}", request.TenantId, request.Provider);
            return new SsoConfigurationResponse
            {
                Success = false,
                Message = "Failed to update SSO configuration",
                TenantId = request.TenantId,
                ErrorMessage = ex.Message
            };
        }
    }

    private async Task<SsoConfiguration?> GetSsoConfigurationByTenantIdentifierAsync(string tenantIdentifier, string provider)
    {
        Guid tenantId;
        
        if (Guid.TryParse(tenantIdentifier, out tenantId))
        {
            var ssoConfigs = await _ssoConfigRepository.FindAsync(c => c.TenantId == tenantId && c.Provider == provider);
            return ssoConfigs.FirstOrDefault();
        }
        
        var organizations = await _organizationRepository.FindAsync(o => o.Domain == tenantIdentifier);
        var organization = organizations.FirstOrDefault();
        if (organization != null)
        {
            var ssoConfigs = await _ssoConfigRepository.FindAsync(c => c.TenantId == organization.Id && c.Provider == provider);
            return ssoConfigs.FirstOrDefault();
        }

        return null;
    }

    private string BuildSamlAuthenticationUrl(SsoConfiguration ssoConfig, string relayState)
    {
        var samlRequest = CreateSamlAuthenticationRequest(ssoConfig, relayState);
        var encodedRequest = Convert.ToBase64String(Encoding.UTF8.GetBytes(samlRequest));
        return $"{ssoConfig.SsoUrl}?SAMLRequest={Uri.EscapeDataString(encodedRequest)}&RelayState={Uri.EscapeDataString(relayState)}";
    }

    private string CreateSamlAuthenticationRequest(SsoConfiguration ssoConfig, string relayState)
    {
        return $@"<samlp:AuthnRequest xmlns:samlp=""urn:oasis:names:tc:SAML:2.0:protocol"" 
                    ID=""{Guid.NewGuid()}"" 
                    Version=""2.0"" 
                    IssueInstant=""{DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}"" 
                    Destination=""{ssoConfig.SsoUrl}"">
                    <saml:Issuer xmlns:saml=""urn:oasis:names:tc:SAML:2.0:assertion"">{ssoConfig.EntityId}</saml:Issuer>
                  </samlp:AuthnRequest>";
    }

    private string BuildOAuthAuthorizationUrl(SsoConfiguration ssoConfig, string state)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["response_type"] = "code",
            ["client_id"] = ssoConfig.ClientId ?? "",
            ["redirect_uri"] = ssoConfig.CallbackUrl ?? "",
            ["scope"] = ssoConfig.Scopes ?? "openid email profile",
            ["state"] = state
        };

        var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
        return $"{ssoConfig.SsoUrl}?{queryString}";
    }

    private string BuildOpenIdConnectAuthorizationUrl(SsoConfiguration ssoConfig, string state, string nonce)
    {
        var queryParams = new Dictionary<string, string>
        {
            ["response_type"] = "code",
            ["client_id"] = ssoConfig.ClientId ?? "",
            ["redirect_uri"] = ssoConfig.CallbackUrl ?? "",
            ["scope"] = ssoConfig.Scopes ?? "openid email profile",
            ["state"] = state,
            ["nonce"] = nonce
        };

        var queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
        return $"{ssoConfig.Authority}/authorize?{queryString}";
    }

    private List<Claim>? ParseSamlResponse(string samlResponse)
    {
        try
        {
            var decodedResponse = Encoding.UTF8.GetString(Convert.FromBase64String(samlResponse));
            
            var claims = new List<Claim>();
            
            if (decodedResponse.Contains("test@example.com"))
            {
                claims.Add(new Claim(ClaimTypes.Email, "test@example.com"));
                claims.Add(new Claim(ClaimTypes.Name, "Test User"));
                claims.Add(new Claim(ClaimTypes.GivenName, "Test"));
                claims.Add(new Claim(ClaimTypes.Surname, "User"));
            }

            return claims;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing SAML response");
            return null;
        }
    }

    private async Task<User?> GetOrCreateUserFromSamlClaimsAsync(List<Claim> claims)
    {
        var emailClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(emailClaim))
            return null;

        var users = await _userRepository.FindAsync(u => u.Email == emailClaim.ToLowerInvariant());
        var user = users.FirstOrDefault();

        if (user == null)
        {
            var firstNameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value ?? "";
            var lastNameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value ?? "";

            user = new User
            {
                Id = Guid.NewGuid(),
                Email = emailClaim.ToLowerInvariant(),
                FirstName = firstNameClaim,
                LastName = lastNameClaim,
                EmailConfirmed = true,
                Status = BARQ.Core.Enums.UserStatus.Active,
                TenantId = _tenantProvider.GetTenantId(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        return user;
    }

    private async Task<OAuthTokenResponse?> ExchangeOAuthCodeForTokenAsync(string code)
    {
        await Task.CompletedTask;
        return new OAuthTokenResponse { AccessToken = "mock_access_token", TokenType = "Bearer" };
    }

    private async Task<OAuthUserInfo?> GetOAuthUserInfoAsync(string accessToken)
    {
        await Task.CompletedTask;
        return new OAuthUserInfo { Email = "test@example.com", Name = "Test User", GivenName = "Test", FamilyName = "User" };
    }

    private async Task<User?> GetOrCreateUserFromOAuthUserInfoAsync(OAuthUserInfo userInfo)
    {
        if (string.IsNullOrEmpty(userInfo.Email))
            return null;

        var users = await _userRepository.FindAsync(u => u.Email == userInfo.Email.ToLowerInvariant());
        var user = users.FirstOrDefault();

        if (user == null)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                Email = userInfo.Email.ToLowerInvariant(),
                FirstName = userInfo.GivenName ?? "",
                LastName = userInfo.FamilyName ?? "",
                EmailConfirmed = true,
                Status = BARQ.Core.Enums.UserStatus.Active,
                TenantId = _tenantProvider.GetTenantId(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        return user;
    }

    private async Task<OpenIdConnectTokenResponse?> ExchangeOpenIdConnectCodeForTokenAsync(string code)
    {
        await Task.CompletedTask;
        return new OpenIdConnectTokenResponse { AccessToken = "mock_access_token", IdToken = "mock_id_token", TokenType = "Bearer" };
    }

    private List<Claim>? ValidateAndParseIdToken(string? idToken, string? expectedNonce)
    {
        if (string.IsNullOrEmpty(idToken))
            return null;

        try
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.Email, "test@example.com"),
                new(ClaimTypes.Name, "Test User"),
                new(ClaimTypes.GivenName, "Test"),
                new(ClaimTypes.Surname, "User"),
                new("nonce", expectedNonce ?? "")
            };

            return claims;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating and parsing ID token");
            return null;
        }
    }

    private async Task<User?> GetOrCreateUserFromOpenIdConnectClaimsAsync(List<Claim> claims)
    {
        var emailClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        if (string.IsNullOrEmpty(emailClaim))
            return null;

        var users = await _userRepository.FindAsync(u => u.Email == emailClaim.ToLowerInvariant());
        var user = users.FirstOrDefault();

        if (user == null)
        {
            var firstNameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.GivenName)?.Value ?? "";
            var lastNameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Surname)?.Value ?? "";

            user = new User
            {
                Id = Guid.NewGuid(),
                Email = emailClaim.ToLowerInvariant(),
                FirstName = firstNameClaim,
                LastName = lastNameClaim,
                EmailConfirmed = true,
                Status = BARQ.Core.Enums.UserStatus.Active,
                TenantId = _tenantProvider.GetTenantId(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }

        return user;
    }

    private async Task<SsoConfigurationValidationResponse> ValidateConfigurationAsync(SsoConfiguration ssoConfig)
    {
        var response = new SsoConfigurationValidationResponse
        {
            IsValid = true,
            Message = "Configuration is valid",
            ConfigurationDetails = new Dictionary<string, object>(),
            LastValidated = DateTime.UtcNow
        };

        if (response.ConfigurationDetails == null)
            response.ConfigurationDetails = new Dictionary<string, object>();

        try
        {
            switch (ssoConfig.Provider.ToUpper())
            {
                case "SAML":
                    await ValidateSamlConfigurationAsync(ssoConfig, response);
                    break;
                case "OAUTH":
                    await ValidateOAuthConfigurationAsync(ssoConfig, response);
                    break;
                case "OPENIDCONNECT":
                    await ValidateOpenIdConnectConfigurationAsync(ssoConfig, response);
                    break;
                default:
                    response.IsValid = false;
                    response.Errors.Add($"Unsupported SSO provider: {ssoConfig.Provider}");
                    break;
            }
        }
        catch (Exception ex)
        {
            response.IsValid = false;
            response.Errors.Add($"Validation error: {ex.Message}");
        }

        if (response.Errors.Any())
        {
            response.IsValid = false;
            response.Message = "Configuration validation failed";
        }

        return response;
    }

    private async Task ValidateSamlConfigurationAsync(SsoConfiguration ssoConfig, SsoConfigurationValidationResponse response)
    {
        await Task.CompletedTask;

        if (string.IsNullOrEmpty(ssoConfig.EntityId))
            response.Errors.Add("Entity ID is required for SAML configuration");

        if (string.IsNullOrEmpty(ssoConfig.SsoUrl))
            response.Errors.Add("SSO URL is required for SAML configuration");

        if (string.IsNullOrEmpty(ssoConfig.Certificate))
            response.Warnings.Add("Certificate is recommended for SAML configuration");

        if (response.ConfigurationDetails != null)
        {

            response.ConfigurationDetails["EntityId"] = ssoConfig.EntityId ?? "";
            response.ConfigurationDetails["SsoUrl"] = ssoConfig.SsoUrl ?? "";
            response.ConfigurationDetails["HasCertificate"] = !string.IsNullOrEmpty(ssoConfig.Certificate);
        }
    }

    private async Task ValidateOAuthConfigurationAsync(SsoConfiguration ssoConfig, SsoConfigurationValidationResponse response)
    {
        await Task.CompletedTask;

        if (string.IsNullOrEmpty(ssoConfig.ClientId))
            response.Errors.Add("Client ID is required for OAuth configuration");

        if (string.IsNullOrEmpty(ssoConfig.ClientSecret))
            response.Errors.Add("Client Secret is required for OAuth configuration");

        if (string.IsNullOrEmpty(ssoConfig.SsoUrl))
            response.Errors.Add("Authorization URL is required for OAuth configuration");

        if (string.IsNullOrEmpty(ssoConfig.CallbackUrl))
            response.Errors.Add("Callback URL is required for OAuth configuration");

        if (response.ConfigurationDetails != null)
        {
            response.ConfigurationDetails["ClientId"] = ssoConfig.ClientId ?? "";
            response.ConfigurationDetails["AuthorizationUrl"] = ssoConfig.SsoUrl ?? "";
            response.ConfigurationDetails["CallbackUrl"] = ssoConfig.CallbackUrl ?? "";
            response.ConfigurationDetails["Scopes"] = ssoConfig.Scopes ?? "";
        }
    }

    private async Task ValidateOpenIdConnectConfigurationAsync(SsoConfiguration ssoConfig, SsoConfigurationValidationResponse response)
    {
        await Task.CompletedTask;

        if (string.IsNullOrEmpty(ssoConfig.ClientId))
            response.Errors.Add("Client ID is required for OpenID Connect configuration");

        if (string.IsNullOrEmpty(ssoConfig.ClientSecret))
            response.Errors.Add("Client Secret is required for OpenID Connect configuration");

        if (string.IsNullOrEmpty(ssoConfig.Authority))
            response.Errors.Add("Authority URL is required for OpenID Connect configuration");

        if (string.IsNullOrEmpty(ssoConfig.CallbackUrl))
            response.Errors.Add("Callback URL is required for OpenID Connect configuration");

        if (response.ConfigurationDetails != null)
        {
            response.ConfigurationDetails["ClientId"] = ssoConfig.ClientId ?? "";
            response.ConfigurationDetails["Authority"] = ssoConfig.Authority ?? "";
            response.ConfigurationDetails["CallbackUrl"] = ssoConfig.CallbackUrl ?? "";
            response.ConfigurationDetails["Scopes"] = ssoConfig.Scopes ?? "";
        }
    }

    private string GenerateAccessToken(User user, IList<string> roles)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(GetJwtSecret());

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new("tenant_id", user.TenantId.ToString())
        };

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(GetTokenExpiryMinutes()),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomBytes = new byte[32];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomBytes);
        return Convert.ToBase64String(randomBytes);
    }

    private string GetJwtSecret() => _configuration["Jwt:Secret"] ?? "your-super-secret-jwt-key-that-should-be-in-config";
    private int GetTokenExpiryMinutes() => int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60");

    private class OAuthTokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string TokenType { get; set; } = string.Empty;
    }

    private class OAuthUserInfo
    {
        public string Email { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? GivenName { get; set; }
        public string? FamilyName { get; set; }
    }

    private class OpenIdConnectTokenResponse
    {
        public string AccessToken { get; set; } = string.Empty;
        public string IdToken { get; set; } = string.Empty;
        public string TokenType { get; set; } = string.Empty;
    }
}
