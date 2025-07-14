using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.DirectoryServices.Protocols;
using System.Net;
using BARQ.Core.Services;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;
using BARQ.Core.Entities;
using BARQ.Core.Repositories;

namespace BARQ.Application.Services.Authentication;

public class LdapAuthenticationService : ILdapAuthenticationService
{
    private readonly IRepository<LdapConfiguration> _ldapConfigRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Organization> _organizationRepository;
    private readonly IUserRoleService _userRoleService;
    private readonly ITenantProvider _tenantProvider;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ILogger<LdapAuthenticationService> _logger;
    private readonly IEncryptionService _encryptionService;

    public LdapAuthenticationService(
        IRepository<LdapConfiguration> ldapConfigRepository,
        IRepository<User> userRepository,
        IRepository<Organization> organizationRepository,
        IUserRoleService userRoleService,
        ITenantProvider tenantProvider,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        ILogger<LdapAuthenticationService> logger,
        IEncryptionService encryptionService)
    {
        _ldapConfigRepository = ldapConfigRepository;
        _userRepository = userRepository;
        _organizationRepository = organizationRepository;
        _userRoleService = userRoleService;
        _tenantProvider = tenantProvider;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
        _encryptionService = encryptionService;
    }

    public async Task<AuthenticationResponse> AuthenticateAsync(LdapAuthenticationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var ldapConfig = await GetLdapConfigurationByTenantIdentifierAsync(request.TenantIdentifier);
            if (ldapConfig == null || !ldapConfig.IsEnabled)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "LDAP authentication is not configured or enabled for this tenant",
                    RequiresMfa = false
                };
            }

            var ldapUser = await AuthenticateWithLdapAsync(ldapConfig, request.Username, request.Password);
            if (ldapUser == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Invalid LDAP credentials",
                    RequiresMfa = false
                };
            }

            var user = await GetOrCreateUserFromLdapAsync(ldapConfig, ldapUser);
            if (user == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Failed to process user from LDAP response",
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

            ldapConfig.LastSuccessfulAuth = DateTime.UtcNow;
            await _ldapConfigRepository.UpdateAsync(ldapConfig);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("LDAP authentication successful for user: {Username}", request.Username);

            return new AuthenticationResponse
            {
                Success = true,
                Message = "LDAP authentication successful",
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
            _logger.LogError(ex, "Error during LDAP authentication for user: {Username}", request.Username);
            return new AuthenticationResponse
            {
                Success = false,
                Message = "LDAP authentication failed",
                RequiresMfa = false
            };
        }
    }

    public async Task<LdapConfigurationValidationResponse> ValidateLdapConfigurationAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        try
        {
            var ldapConfigs = await _ldapConfigRepository.FindAsync(c => c.TenantId == tenantId);
            var ldapConfig = ldapConfigs.FirstOrDefault();

            if (ldapConfig == null)
            {
                return new LdapConfigurationValidationResponse
                {
                    IsValid = false,
                    Message = "LDAP configuration not found",
                    Errors = new List<string> { "No LDAP configuration found for the specified tenant" }
                };
            }

            var validationResult = await ValidateConfigurationAsync(ldapConfig);
            
            ldapConfig.IsValid = validationResult.IsValid;
            ldapConfig.LastValidation = DateTime.UtcNow;
            ldapConfig.ValidationError = validationResult.IsValid ? null : string.Join("; ", validationResult.Errors);
            
            await _ldapConfigRepository.UpdateAsync(ldapConfig);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("LDAP configuration validation completed for tenant: {TenantId}, valid: {IsValid}", 
                tenantId, validationResult.IsValid);

            return validationResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating LDAP configuration for tenant: {TenantId}", tenantId);
            return new LdapConfigurationValidationResponse
            {
                IsValid = false,
                Message = "Validation failed due to error",
                Errors = new List<string> { ex.Message }
            };
        }
    }

    public async Task<LdapConfigurationResponse> GetLdapConfigurationAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        try
        {
            var ldapConfigs = await _ldapConfigRepository.FindAsync(c => c.TenantId == tenantId);
            var ldapConfig = ldapConfigs.FirstOrDefault();

            if (ldapConfig == null)
            {
                return new LdapConfigurationResponse
                {
                    Success = false,
                    Message = "LDAP configuration not found",
                    TenantId = tenantId
                };
            }

            var groupRoleMappings = string.IsNullOrEmpty(ldapConfig.GroupRoleMappings) 
                ? new Dictionary<string, string>() 
                : JsonSerializer.Deserialize<Dictionary<string, string>>(ldapConfig.GroupRoleMappings) ?? new Dictionary<string, string>();

            return new LdapConfigurationResponse
            {
                Success = true,
                Message = "LDAP configuration retrieved successfully",
                ConfigurationId = ldapConfig.Id,
                TenantId = ldapConfig.TenantId,
                Host = ldapConfig.Host,
                Port = ldapConfig.Port,
                UseSsl = ldapConfig.UseSsl,
                UseStartTls = ldapConfig.UseStartTls,
                BaseDn = ldapConfig.BaseDn,
                BindDn = ldapConfig.BindDn,
                UserSearchFilter = ldapConfig.UserSearchFilter,
                GroupSearchFilter = ldapConfig.GroupSearchFilter,
                UserDnPattern = ldapConfig.UserDnPattern,
                EmailAttribute = ldapConfig.EmailAttribute,
                FirstNameAttribute = ldapConfig.FirstNameAttribute,
                LastNameAttribute = ldapConfig.LastNameAttribute,
                DisplayNameAttribute = ldapConfig.DisplayNameAttribute,
                GroupMembershipAttribute = ldapConfig.GroupMembershipAttribute,
                IsEnabled = ldapConfig.IsEnabled,
                IsRequired = ldapConfig.IsRequired,
                AutoProvisionUsers = ldapConfig.AutoProvisionUsers,
                DefaultRole = ldapConfig.DefaultRole,
                GroupRoleMappings = groupRoleMappings,
                ConnectionTimeout = ldapConfig.ConnectionTimeout,
                SearchTimeout = ldapConfig.SearchTimeout,
                IsValid = ldapConfig.IsValid,
                LastSuccessfulAuth = ldapConfig.LastSuccessfulAuth,
                LastValidation = ldapConfig.LastValidation,
                LastSynchronization = ldapConfig.LastSynchronization
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting LDAP configuration for tenant: {TenantId}", tenantId);
            return new LdapConfigurationResponse
            {
                Success = false,
                Message = "Failed to retrieve LDAP configuration",
                TenantId = tenantId,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<LdapConfigurationResponse> UpdateLdapConfigurationAsync(UpdateLdapConfigurationRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var ldapConfigs = await _ldapConfigRepository.FindAsync(c => c.TenantId == request.TenantId);
            var ldapConfig = ldapConfigs.FirstOrDefault();

            if (ldapConfig == null)
            {
                ldapConfig = new LdapConfiguration
                {
                    Id = Guid.NewGuid(),
                    TenantId = request.TenantId,
                    CreatedAt = DateTime.UtcNow
                };
                await _ldapConfigRepository.AddAsync(ldapConfig);
            }

            ldapConfig.Host = request.Host;
            ldapConfig.Port = request.Port;
            ldapConfig.UseSsl = request.UseSsl;
            ldapConfig.UseStartTls = request.UseStartTls;
            ldapConfig.BaseDn = request.BaseDn;
            ldapConfig.BindDn = request.BindDn ?? string.Empty;
            ldapConfig.BindPassword = await _encryptionService.EncryptAsync(request.BindPassword ?? string.Empty);
            ldapConfig.UserSearchFilter = request.UserSearchFilter;
            ldapConfig.GroupSearchFilter = request.GroupSearchFilter;
            ldapConfig.UserDnPattern = request.UserDnPattern ?? string.Empty;
            ldapConfig.EmailAttribute = request.EmailAttribute;
            ldapConfig.FirstNameAttribute = request.FirstNameAttribute;
            ldapConfig.LastNameAttribute = request.LastNameAttribute;
            ldapConfig.DisplayNameAttribute = request.DisplayNameAttribute;
            ldapConfig.GroupMembershipAttribute = request.GroupMembershipAttribute;
            ldapConfig.IsEnabled = request.IsEnabled;
            ldapConfig.IsRequired = request.IsRequired;
            ldapConfig.AutoProvisionUsers = request.AutoProvisionUsers;
            ldapConfig.DefaultRole = request.DefaultRole;
            ldapConfig.ConnectionTimeout = request.ConnectionTimeout;
            ldapConfig.SearchTimeout = request.SearchTimeout;
            ldapConfig.UpdatedAt = DateTime.UtcNow;

            if (request.GroupRoleMappings != null)
            {
                ldapConfig.GroupRoleMappings = JsonSerializer.Serialize(request.GroupRoleMappings);
            }

            if (ldapConfig.Id != Guid.Empty)
            {
                await _ldapConfigRepository.UpdateAsync(ldapConfig);
            }

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("LDAP configuration updated for tenant: {TenantId}", request.TenantId);

            return await GetLdapConfigurationAsync(request.TenantId, cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating LDAP configuration for tenant: {TenantId}", request.TenantId);
            return new LdapConfigurationResponse
            {
                Success = false,
                Message = "Failed to update LDAP configuration",
                TenantId = request.TenantId,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<LdapUserSearchResponse> SearchUsersAsync(LdapUserSearchRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var ldapConfigs = await _ldapConfigRepository.FindAsync(c => c.TenantId == request.TenantId);
            var ldapConfig = ldapConfigs.FirstOrDefault();

            if (ldapConfig == null || !ldapConfig.IsEnabled)
            {
                return new LdapUserSearchResponse
                {
                    Success = false,
                    Message = "LDAP configuration not found or disabled"
                };
            }

            var startTime = DateTime.UtcNow;
            var users = await SearchLdapUsersAsync(ldapConfig, request.SearchQuery, request.Attributes, request.MaxResults);
            var endTime = DateTime.UtcNow;

            return new LdapUserSearchResponse
            {
                Success = true,
                Message = $"Found {users.Count} users",
                Users = users,
                TotalCount = users.Count,
                ExecutionTimeMs = (long)(endTime - startTime).TotalMilliseconds
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching LDAP users for tenant: {TenantId}", request.TenantId);
            return new LdapUserSearchResponse
            {
                Success = false,
                Message = "LDAP user search failed",
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<LdapSynchronizationResponse> SynchronizeUsersAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        var startTime = DateTime.UtcNow;
        var response = new LdapSynchronizationResponse
        {
            StartTime = startTime
        };

        try
        {
            var ldapConfigs = await _ldapConfigRepository.FindAsync(c => c.TenantId == tenantId);
            var ldapConfig = ldapConfigs.FirstOrDefault();

            if (ldapConfig == null || !ldapConfig.IsEnabled)
            {
                response.Success = false;
                response.Message = "LDAP configuration not found or disabled";
                response.EndTime = DateTime.UtcNow;
                return response;
            }

            var ldapUsers = await SearchLdapUsersAsync(ldapConfig, "*", null, 1000);
            var existingUsers = await _userRepository.FindAsync(u => u.TenantId == tenantId);

            foreach (var ldapUser in ldapUsers)
            {
                var existingUser = existingUsers.FirstOrDefault(u => u.Email.ToLower() == ldapUser.Email.ToLower());
                
                if (existingUser == null && ldapConfig.AutoProvisionUsers)
                {
                    var newUser = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = ldapUser.Email.ToLowerInvariant(),
                        FirstName = ldapUser.FirstName,
                        LastName = ldapUser.LastName,
                        EmailConfirmed = true,
                        Status = BARQ.Core.Enums.UserStatus.Active,
                        TenantId = tenantId,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _userRepository.AddAsync(newUser);
                    response.UsersCreated++;
                }
                else if (existingUser != null)
                {
                    existingUser.FirstName = ldapUser.FirstName;
                    existingUser.LastName = ldapUser.LastName;
                    existingUser.UpdatedAt = DateTime.UtcNow;
                    
                    await _userRepository.UpdateAsync(existingUser);
                    response.UsersUpdated++;
                }
            }

            ldapConfig.LastSynchronization = DateTime.UtcNow;
            await _ldapConfigRepository.UpdateAsync(ldapConfig);
            await _unitOfWork.SaveChangesAsync();

            response.Success = true;
            response.Message = "LDAP synchronization completed successfully";
            response.UsersSynchronized = response.UsersCreated + response.UsersUpdated;
            response.EndTime = DateTime.UtcNow;

            _logger.LogInformation("LDAP synchronization completed for tenant: {TenantId}. Created: {Created}, Updated: {Updated}", 
                tenantId, response.UsersCreated, response.UsersUpdated);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during LDAP synchronization for tenant: {TenantId}", tenantId);
            response.Success = false;
            response.Message = "LDAP synchronization failed";
            response.ErrorMessage = ex.Message;
            response.EndTime = DateTime.UtcNow;
            return response;
        }
    }

    public async Task<LdapConnectionTestResponse> TestConnectionAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        try
        {
            var ldapConfigs = await _ldapConfigRepository.FindAsync(c => c.TenantId == tenantId);
            var ldapConfig = ldapConfigs.FirstOrDefault();

            if (ldapConfig == null)
            {
                return new LdapConnectionTestResponse
                {
                    IsConnected = false,
                    Message = "LDAP configuration not found"
                };
            }

            var startTime = DateTime.UtcNow;
            var connectionResult = await TestLdapConnectionAsync(ldapConfig);
            var endTime = DateTime.UtcNow;

            return new LdapConnectionTestResponse
            {
                IsConnected = connectionResult.Success,
                Message = connectionResult.Message,
                ConnectionTimeMs = (long)(endTime - startTime).TotalMilliseconds,
                ServerInfo = connectionResult.ServerInfo,
                SupportedAuthMethods = connectionResult.SupportedAuthMethods,
                TestDetails = connectionResult.TestDetails,
                ErrorMessage = connectionResult.ErrorMessage
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error testing LDAP connection for tenant: {TenantId}", tenantId);
            return new LdapConnectionTestResponse
            {
                IsConnected = false,
                Message = "Connection test failed",
                ErrorMessage = ex.Message
            };
        }
    }

    private async Task<LdapConfiguration?> GetLdapConfigurationByTenantIdentifierAsync(string tenantIdentifier)
    {
        Guid tenantId;
        
        if (Guid.TryParse(tenantIdentifier, out tenantId))
        {
            var ldapConfigs = await _ldapConfigRepository.FindAsync(c => c.TenantId == tenantId);
            return ldapConfigs.FirstOrDefault();
        }
        
        var organizations = await _organizationRepository.FindAsync(o => o.Domain == tenantIdentifier);
        var organization = organizations.FirstOrDefault();
        if (organization != null)
        {
            var ldapConfigs = await _ldapConfigRepository.FindAsync(c => c.TenantId == organization.Id);
            return ldapConfigs.FirstOrDefault();
        }

        return null;
    }

    private async Task<LdapUserInfo?> AuthenticateWithLdapAsync(LdapConfiguration config, string username, string password)
    {
        try
        {
            await Task.CompletedTask;
            
            // using System.DirectoryServices.Protocols or similar library
            
            using var connection = new LdapConnection(new LdapDirectoryIdentifier(config.Host, config.Port));
            
            if (config.UseSsl)
            {
                connection.SessionOptions.SecureSocketLayer = true;
            }
            
            if (config.UseStartTls)
            {
                connection.SessionOptions.StartTransportLayerSecurity(null);
            }
            
            connection.Timeout = TimeSpan.FromMilliseconds(config.ConnectionTimeout);
            
            if (!string.IsNullOrEmpty(config.BindDn) && !string.IsNullOrEmpty(config.BindPassword))
            {
                var decryptedPassword = await _encryptionService.DecryptAsync(config.BindPassword);
                var bindCredential = new NetworkCredential(config.BindDn, decryptedPassword);
                connection.Bind(bindCredential);
            }
            
            var searchFilter = config.UserSearchFilter.Replace("{0}", username);
            var searchRequest = new SearchRequest(config.BaseDn, searchFilter, SearchScope.Subtree);
            searchRequest.Attributes.Add("cn");
            searchRequest.Attributes.Add("mail");
            searchRequest.Attributes.Add("givenName");
            searchRequest.Attributes.Add("sn");
            searchRequest.Attributes.Add("displayName");
            searchRequest.Attributes.Add("memberOf");
            
            var searchResponse = (SearchResponse)connection.SendRequest(searchRequest);
            
            if (searchResponse.Entries.Count == 0)
            {
                _logger.LogWarning("LDAP user not found: {Username}", username);
                return null;
            }
            
            var userEntry = searchResponse.Entries[0];
            var userDn = userEntry.DistinguishedName;
            
            try
            {
                var userCredential = new NetworkCredential(userDn, password);
                using var userConnection = new LdapConnection(new LdapDirectoryIdentifier(config.Host, config.Port));
                
                if (config.UseSsl)
                {
                    userConnection.SessionOptions.SecureSocketLayer = true;
                }
                
                userConnection.Bind(userCredential);
                
                var ldapUser = new LdapUserInfo
                {
                    Username = username,
                    Email = GetAttributeValue(userEntry, "mail") ?? $"{username}@{config.Host}",
                    FirstName = GetAttributeValue(userEntry, "givenName") ?? "",
                    LastName = GetAttributeValue(userEntry, "sn") ?? "",
                    DisplayName = GetAttributeValue(userEntry, "displayName") ?? $"{username}",
                    DistinguishedName = userDn,
                    Groups = GetAttributeValues(userEntry, "memberOf"),
                    IsActive = true
                };
                
                return ldapUser;
            }
            catch (LdapException ex)
            {
                _logger.LogWarning("LDAP authentication failed for user {Username}: {Error}", username, ex.Message);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error authenticating with LDAP for user: {Username}", username);
            return null;
        }
    }
    
    private string? GetAttributeValue(SearchResultEntry entry, string attributeName)
    {
        if (entry.Attributes.Contains(attributeName))
        {
            var attribute = entry.Attributes[attributeName];
            return attribute.Count > 0 ? attribute[0] as string : null;
        }
        return null;
    }
    
    private List<string> GetAttributeValues(SearchResultEntry entry, string attributeName)
    {
        var values = new List<string>();
        if (entry.Attributes.Contains(attributeName))
        {
            var attribute = entry.Attributes[attributeName];
            for (int i = 0; i < attribute.Count; i++)
            {
                if (attribute[i] is string value)
                {
                    var groupName = value.Split(',')[0].Replace("CN=", "");
                    values.Add(groupName);
                }
            }
        }
        return values;
    }

    private async Task<User?> GetOrCreateUserFromLdapAsync(LdapConfiguration config, LdapUserInfo ldapUser)
    {
        var users = await _userRepository.FindAsync(u => u.Email == ldapUser.Email.ToLowerInvariant());
        var user = users.FirstOrDefault();

        if (user == null && config.AutoProvisionUsers)
        {
            user = new User
            {
                Id = Guid.NewGuid(),
                Email = ldapUser.Email.ToLowerInvariant(),
                FirstName = ldapUser.FirstName,
                LastName = ldapUser.LastName,
                EmailConfirmed = true,
                Status = BARQ.Core.Enums.UserStatus.Active,
                TenantId = config.TenantId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
        }
        else if (user != null)
        {
            user.FirstName = ldapUser.FirstName;
            user.LastName = ldapUser.LastName;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
        }

        return user;
    }

    private async Task<List<LdapUserInfo>> SearchLdapUsersAsync(LdapConfiguration config, string searchQuery, List<string>? attributes, int maxResults)
    {
        try
        {
            var users = new List<LdapUserInfo>();
            
            using var connection = new LdapConnection(new LdapDirectoryIdentifier(config.Host, config.Port));
            
            if (config.UseSsl)
            {
                connection.SessionOptions.SecureSocketLayer = true;
            }
            else if (config.UseStartTls)
            {
                connection.SessionOptions.StartTransportLayerSecurity(null);
            }
            
            connection.Timeout = TimeSpan.FromMilliseconds(config.ConnectionTimeout);
            
            if (!string.IsNullOrEmpty(config.BindDn) && !string.IsNullOrEmpty(config.BindPassword))
            {
                var decryptedPassword = await _encryptionService.DecryptAsync(config.BindPassword);
                var bindCredential = new NetworkCredential(config.BindDn, decryptedPassword);
                connection.Bind(bindCredential);
            }
            
            var searchFilter = config.UserSearchFilter.Replace("{0}", searchQuery);
            
            var requestedAttributes = new List<string>
            {
                config.EmailAttribute,
                config.FirstNameAttribute,
                config.LastNameAttribute,
                config.DisplayNameAttribute,
                config.GroupMembershipAttribute,
                "userAccountControl"
            };
            
            if (attributes != null && attributes.Any())
            {
                requestedAttributes.AddRange(attributes);
            }
            
            var searchRequest = new SearchRequest(
                config.BaseDn,
                searchFilter,
                SearchScope.Subtree,
                requestedAttributes.Distinct().ToArray()
            );
            searchRequest.SizeLimit = maxResults;
            searchRequest.TimeLimit = TimeSpan.FromMilliseconds(config.SearchTimeout);

            var searchResponse = (SearchResponse)connection.SendRequest(searchRequest);

            foreach (SearchResultEntry entry in searchResponse.Entries)
            {
                var user = new LdapUserInfo
                {
                    DistinguishedName = entry.DistinguishedName,
                    Username = ExtractUsernameFromDn(entry.DistinguishedName),
                    Email = GetAttributeValue(entry, config.EmailAttribute) ?? string.Empty,
                    FirstName = GetAttributeValue(entry, config.FirstNameAttribute) ?? string.Empty,
                    LastName = GetAttributeValue(entry, config.LastNameAttribute) ?? string.Empty,
                    DisplayName = GetAttributeValue(entry, config.DisplayNameAttribute) ?? string.Empty,
                    Groups = GetGroupMemberships(entry, config.GroupMembershipAttribute),
                    IsActive = IsUserActive(entry)
                };

                if (!string.IsNullOrEmpty(user.Email))
                {
                    users.Add(user);
                }
            }

            _logger.LogInformation("Successfully searched LDAP users, found {UserCount} users for query '{SearchQuery}'", users.Count, searchQuery);
            return users;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching LDAP users for query '{SearchQuery}'", searchQuery);
            return new List<LdapUserInfo>();
        }
    }

    private async Task<LdapConfigurationValidationResponse> ValidateConfigurationAsync(LdapConfiguration config)
    {
        var response = new LdapConfigurationValidationResponse
        {
            IsValid = true,
            Message = "Configuration is valid",
            ConfigurationDetails = new Dictionary<string, object>(),
            LastValidated = DateTime.UtcNow
        };

        try
        {
            if (string.IsNullOrEmpty(config.Host))
                response.Errors.Add("Host is required");

            if (config.Port <= 0 || config.Port > 65535)
                response.Errors.Add("Port must be between 1 and 65535");

            if (string.IsNullOrEmpty(config.BaseDn))
                response.Errors.Add("Base DN is required");

            if (string.IsNullOrEmpty(config.UserSearchFilter))
                response.Errors.Add("User search filter is required");

            if (config.UseSsl && config.UseStartTls)
                response.Warnings.Add("Both SSL and StartTLS are enabled, SSL will take precedence");

            if (config.ConnectionTimeout <= 0)
                response.Warnings.Add("Connection timeout should be greater than 0");

            if (config.SearchTimeout <= 0)
                response.Warnings.Add("Search timeout should be greater than 0");

            response.ConfigurationDetails["Host"] = config.Host;
            response.ConfigurationDetails["Port"] = config.Port;
            response.ConfigurationDetails["BaseDn"] = config.BaseDn;
            response.ConfigurationDetails["UseSsl"] = config.UseSsl;
            response.ConfigurationDetails["UseStartTls"] = config.UseStartTls;

            var connectionTest = await TestLdapConnectionAsync(config);
            if (!connectionTest.Success)
            {
                response.Errors.Add($"Connection test failed: {connectionTest.ErrorMessage}");
            }
        }
        catch (Exception ex)
        {
            response.Errors.Add($"Validation error: {ex.Message}");
        }

        if (response.Errors.Any())
        {
            response.IsValid = false;
            response.Message = "Configuration validation failed";
        }

        return response;
    }

    private async Task<(bool Success, string Message, string? ServerInfo, List<string> SupportedAuthMethods, Dictionary<string, object> TestDetails, string? ErrorMessage)> TestLdapConnectionAsync(LdapConfiguration config)
    {
        try
        {
            using var connection = new LdapConnection(new LdapDirectoryIdentifier(config.Host, config.Port));
            
            if (config.UseSsl)
            {
                connection.SessionOptions.SecureSocketLayer = true;
            }
            else if (config.UseStartTls)
            {
                connection.SessionOptions.StartTransportLayerSecurity(null);
            }
            
            connection.Timeout = TimeSpan.FromMilliseconds(Math.Max(config.ConnectionTimeout, 5000));
            
            if (!string.IsNullOrEmpty(config.BindDn) && !string.IsNullOrEmpty(config.BindPassword))
            {
                var decryptedPassword = await _encryptionService.DecryptAsync(config.BindPassword);
                var bindCredential = new NetworkCredential(config.BindDn, decryptedPassword);
                connection.Bind(bindCredential);
            }
            else
            {
                connection.Bind();
            }

            var testSearchRequest = new SearchRequest(
                config.BaseDn,
                "(objectClass=*)",
                SearchScope.Base
            );
            testSearchRequest.SizeLimit = 1;
            testSearchRequest.TimeLimit = TimeSpan.FromMilliseconds(Math.Max(config.SearchTimeout, 5000));

            var testSearchResponse = (SearchResponse)connection.SendRequest(testSearchRequest);
            
            var supportedAuthMethods = new List<string>();
            var serverInfo = $"LDAP Server at {config.Host}:{config.Port}";
            
            try
            {
                var rootDseRequest = new SearchRequest(
                    "",
                    "(objectClass=*)",
                    SearchScope.Base,
                    "supportedSASLMechanisms",
                    "namingContexts",
                    "serverName"
                );
                rootDseRequest.SizeLimit = 1;
                rootDseRequest.TimeLimit = TimeSpan.FromMilliseconds(5000);

                var rootDseResponse = (SearchResponse)connection.SendRequest(rootDseRequest);
                if (rootDseResponse.Entries.Count > 0)
                {
                    var entry = rootDseResponse.Entries[0];
                    
                    if (entry.Attributes.Contains("supportedSASLMechanisms"))
                    {
                        var saslMechanisms = entry.Attributes["supportedSASLMechanisms"];
                        for (int i = 0; i < saslMechanisms.Count; i++)
                        {
                            supportedAuthMethods.Add(saslMechanisms[i].ToString() ?? "");
                        }
                    }
                    
                    if (entry.Attributes.Contains("serverName"))
                    {
                        var serverName = entry.Attributes["serverName"];
                        if (serverName.Count > 0)
                        {
                            serverInfo = serverName[0].ToString() ?? serverInfo;
                        }
                    }
                }
            }
            catch
            {
                supportedAuthMethods.Add("Simple");
            }

            if (supportedAuthMethods.Count == 0)
            {
                supportedAuthMethods.Add("Simple");
            }

            return (
                Success: true,
                Message: "Connection successful",
                ServerInfo: serverInfo,
                SupportedAuthMethods: supportedAuthMethods,
                TestDetails: new Dictionary<string, object>
                {
                    ["Host"] = config.Host,
                    ["Port"] = config.Port,
                    ["SSL"] = config.UseSsl,
                    ["StartTLS"] = config.UseStartTls,
                    ["BaseDN"] = config.BaseDn,
                    ["BindDN"] = config.BindDn ?? "Anonymous",
                    ["ConnectionTimeout"] = config.ConnectionTimeout,
                    ["SearchTimeout"] = config.SearchTimeout
                },
                ErrorMessage: null
            );
        }
        catch (Exception ex)
        {
            return (
                Success: false,
                Message: "Connection failed",
                ServerInfo: null,
                SupportedAuthMethods: new List<string>(),
                TestDetails: new Dictionary<string, object>
                {
                    ["Host"] = config.Host,
                    ["Port"] = config.Port,
                    ["Error"] = ex.Message
                },
                ErrorMessage: ex.Message
            );
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

    private string GetJwtSecret() 
    {
        var secret = _configuration["Jwt:Secret"];
        if (string.IsNullOrEmpty(secret))
        {
            throw new InvalidOperationException("JWT secret is not configured. Please set Jwt:Secret in configuration.");
        }
        
        if (secret.Length < 32)
        {
            throw new InvalidOperationException("JWT secret must be at least 32 characters long for security.");
        }
        
        return secret;
    }
    private int GetTokenExpiryMinutes() => int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60");

    private string ExtractUsernameFromDn(string distinguishedName)
    {
        try
        {
            var cnStart = distinguishedName.IndexOf("CN=", StringComparison.OrdinalIgnoreCase);
            if (cnStart == -1) return string.Empty;

            var cnValue = distinguishedName.Substring(cnStart + 3);
            var commaIndex = cnValue.IndexOf(',');
            
            return commaIndex > 0 ? cnValue.Substring(0, commaIndex) : cnValue;
        }
        catch
        {
            return string.Empty;
        }
    }


    private List<string> GetGroupMemberships(SearchResultEntry entry, string? groupAttribute)
    {
        var groups = new List<string>();
        
        if (string.IsNullOrEmpty(groupAttribute) || !entry.Attributes.Contains(groupAttribute))
            return groups;

        var attribute = entry.Attributes[groupAttribute];
        for (int i = 0; i < attribute.Count; i++)
        {
            var groupDn = attribute[i].ToString();
            if (!string.IsNullOrEmpty(groupDn))
            {
                var groupName = ExtractUsernameFromDn(groupDn);
                if (!string.IsNullOrEmpty(groupName))
                {
                    groups.Add(groupName);
                }
            }
        }

        return groups;
    }

    private bool IsUserActive(SearchResultEntry entry)
    {
        try
        {
            if (!entry.Attributes.Contains("userAccountControl"))
                return true;

            var uacAttribute = entry.Attributes["userAccountControl"];
            if (uacAttribute.Count == 0)
                return true;

            if (int.TryParse(uacAttribute[0].ToString(), out var userAccountControl))
            {
                const int ACCOUNTDISABLE = 0x0002;
                return (userAccountControl & ACCOUNTDISABLE) == 0;
            }

            return true;
        }
        catch
        {
            return true;
        }
    }
}
