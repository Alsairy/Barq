using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using BARQ.Core.Services;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;
using BARQ.Core.Entities;
using BARQ.Core.Repositories;

namespace BARQ.Application.Services.Authentication;

public class AuthenticationService : IAuthenticationService
{
    private readonly IRepository<User> _userRepository;
    private readonly IPasswordService _passwordService;
    private readonly IUserRoleService _userRoleService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationService> _logger;

    public AuthenticationService(
        IRepository<User> userRepository,
        IPasswordService passwordService,
        IUserRoleService userRoleService,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        ILogger<AuthenticationService> logger)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
        _userRoleService = userRoleService;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<AuthenticationResponse> AuthenticateAsync(LoginRequest request)
    {
        try
        {
            var lockoutResponse = await CheckAccountLockoutAsync(request.Email);
            if (lockoutResponse.IsLockedOut)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Account is locked due to multiple failed login attempts",
                    RequiresMfa = false
                };
            }

            var users = await _userRepository.FindAsync(u => u.Email == request.Email.ToLowerInvariant());
            var user = users.FirstOrDefault();
            
            _logger.LogInformation("Authentication attempt - User found: {UserFound}, Email: {Email}", user != null, request.Email);
            if (user != null)
            {
                _logger.LogInformation("User details - ID: {UserId}, Status: {Status}, EmailConfirmed: {EmailConfirmed}", 
                    user.Id, user.Status, user.EmailConfirmed);
                
                var passwordValid = _passwordService.VerifyPassword(request.Password, user.PasswordHash ?? string.Empty);
                _logger.LogInformation("Password verification result: {PasswordValid}", passwordValid);
            }
            
            if (user == null || !_passwordService.VerifyPassword(request.Password, user.PasswordHash ?? string.Empty))
            {
                await IncrementFailedLoginAttemptAsync(request.Email);
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Invalid email or password",
                    RequiresMfa = false
                };
            }

            if (user.Status != BARQ.Core.Enums.UserStatus.Active)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Account is not active",
                    RequiresMfa = false
                };
            }

            if (!user.EmailConfirmed)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Email address is not verified",
                    RequiresMfa = false
                };
            }

            if (user.TwoFactorEnabled && string.IsNullOrEmpty(request.MfaCode))
            {
                var mfaToken = GenerateMfaToken(user.Id);
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Multi-factor authentication required",
                    RequiresMfa = true,
                    MfaToken = mfaToken
                };
            }

            if (user.TwoFactorEnabled && !string.IsNullOrEmpty(request.MfaCode))
            {
            }

            await ResetFailedLoginAttemptsAsync(request.Email);

            user.LastLoginAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var userRoles = await _userRoleService.GetUserRolesAsync(user.Id);
            var roleNames = userRoles.Select(r => r.Name).ToList();

            var accessToken = GenerateAccessToken(user, roleNames);
            var refreshToken = GenerateRefreshToken();
            
            _logger.LogInformation("Generated tokens successfully for user: {UserId}", user.Id);

            // This would need to be stored in a separate RefreshToken entity
            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("User authenticated successfully: {Email}", request.Email);

            return new AuthenticationResponse
            {
                Success = true,
                Message = "Authentication successful",
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
            _logger.LogError(ex, "Error during authentication for user: {Email}", request.Email);
            return new AuthenticationResponse
            {
                Success = false,
                Message = "Authentication failed",
                RequiresMfa = false
            };
        }
    }

    public async Task<AuthenticationResponse> RefreshTokenAsync(string refreshToken)
    {
        try
        {
            User? user = null;
            if (user == null)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Invalid or expired refresh token"
                };
            }

            var userRoles = await _userRoleService.GetUserRolesAsync(user.Id);
            var roleNames = userRoles.Select(r => r.Name).ToList();

            var newAccessToken = GenerateAccessToken(user, roleNames);
            var newRefreshToken = GenerateRefreshToken();

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new AuthenticationResponse
            {
                Success = true,
                Message = "Token refreshed successfully",
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresAt = DateTime.UtcNow.AddMinutes(GetTokenExpiryMinutes()),
                UserId = user.Id,
                UserEmail = user.Email,
                Roles = roleNames
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing token for user session");
            return new AuthenticationResponse
            {
                Success = false,
                Message = "Token refresh failed"
            };
        }
    }

    public async Task<LogoutResponse> LogoutAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user != null)
            {
                await _userRepository.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();
            }

            _logger.LogInformation("User logged out successfully: {UserId}", userId);

            return new LogoutResponse
            {
                Success = true,
                Message = "Logout successful",
                LoggedOut = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during logout for user: {UserId}", userId);
            return new LogoutResponse
            {
                Success = false,
                Message = "Logout failed"
            };
        }
    }

    public Task<SessionValidationResponse> ValidateSessionAsync(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecret = GetJwtSecret();
            
            if (string.IsNullOrEmpty(jwtSecret))
            {
                _logger.LogError("JWT Secret is null or empty for session validation");
                return Task.FromResult(new SessionValidationResponse
                {
                    Success = false,
                    Message = "Session validation failed - configuration error",
                    IsValid = false
                });
            }
            
            var key = Encoding.UTF8.GetBytes(jwtSecret);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (Guid.TryParse(userIdClaim, out var userId))
            {
                return Task.FromResult(new SessionValidationResponse
                {
                    Success = true,
                    Message = "Session is valid",
                    IsValid = true,
                    UserId = userId,
                    ExpiresAt = validatedToken.ValidTo
                });
            }

            return Task.FromResult(new SessionValidationResponse
            {
                Success = false,
                Message = "Invalid session",
                IsValid = false
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating user session");
            return Task.FromResult(new SessionValidationResponse
            {
                Success = false,
                Message = "Session validation failed",
                IsValid = false
            });
        }
    }

    public async Task<AccountLockoutResponse> CheckAccountLockoutAsync(string email)
    {
        try
        {
            var users = await _userRepository.FindAsync(u => u.Email == email.ToLowerInvariant());
            var user = users.FirstOrDefault();
            if (user == null)
            {
                return new AccountLockoutResponse
                {
                    Success = true,
                    IsLockedOut = false,
                    FailedAttempts = 0,
                    MaxAttempts = GetMaxFailedAttempts()
                };
            }

            var isLockedOut = user.LockoutEnd.HasValue && user.LockoutEnd > DateTime.UtcNow;

            return new AccountLockoutResponse
            {
                Success = true,
                IsLockedOut = isLockedOut,
                LockoutExpiresAt = user.LockoutEnd,
                FailedAttempts = user.AccessFailedCount,
                MaxAttempts = GetMaxFailedAttempts()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking account lockout for: {Email}", email);
            return new AccountLockoutResponse
            {
                Success = false,
                Message = "Error checking account lockout"
            };
        }
    }

    public async Task IncrementFailedLoginAttemptAsync(string email)
    {
        try
        {
            var users = await _userRepository.FindAsync(u => u.Email == email.ToLowerInvariant());
            var user = users.FirstOrDefault();
            if (user != null)
            {
                user.AccessFailedCount++;
                
                if (user.AccessFailedCount >= GetMaxFailedAttempts())
                {
                    user.LockoutEnd = DateTime.UtcNow.AddMinutes(GetLockoutDurationMinutes());
                }

                await _userRepository.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error incrementing failed login attempt for: {Email}", email);
        }
    }

    public async Task ResetFailedLoginAttemptsAsync(string email)
    {
        try
        {
            var users = await _userRepository.FindAsync(u => u.Email == email.ToLowerInvariant());
            var user = users.FirstOrDefault();
            if (user != null)
            {
                user.AccessFailedCount = 0;
                user.LockoutEnd = null;
                await _userRepository.UpdateAsync(user);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error resetting failed login attempts for: {Email}", email);
        }
    }

    private string GenerateAccessToken(User user, IList<string> roles)
    {
        try
        {
            _logger.LogInformation("Generating access token for user: {UserId}, roles: {Roles}", user.Id, string.Join(",", roles));
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecret = GetJwtSecret();
            
            
            var key = Encoding.UTF8.GetBytes(jwtSecret);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Email, user.Email),
                new(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new("tenant_id", user.TenantId.ToString())
            };

            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));
            _logger.LogInformation("Created {ClaimsCount} claims for token", claims.Count);

            var symmetricKey = new SymmetricSecurityKey(key);
            symmetricKey.KeyId = "barq-jwt-key"; // Add KeyId to prevent validation errors
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(GetTokenExpiryMinutes()),
                SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            _logger.LogInformation("Successfully generated JWT token for user: {UserId}", user.Id);
            
            return tokenString ?? string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating access token for user: {UserId}", user.Id);
            return string.Empty;
        }
    }

    private string GenerateRefreshToken()
    {
        try
        {
            var randomBytes = new byte[32];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating refresh token");
            return string.Empty;
        }
    }

    private string GenerateMfaToken(Guid userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecret = GetJwtSecret();
        
        if (string.IsNullOrEmpty(jwtSecret))
        {
            _logger.LogError("JWT Secret is null or empty for MFA token generation");
            return string.Empty;
        }
        
        var key = Encoding.UTF8.GetBytes(jwtSecret);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new("token_type", "mfa")
        };

        var symmetricKey = new SymmetricSecurityKey(key);
        symmetricKey.KeyId = "barq-jwt-key"; // Add KeyId to prevent validation errors
        
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(5), // Short-lived MFA token
            SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token) ?? string.Empty;
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
    private int GetMaxFailedAttempts() => int.Parse(_configuration["Security:MaxFailedAttempts"] ?? "5");
    private int GetLockoutDurationMinutes() => int.Parse(_configuration["Security:LockoutDurationMinutes"] ?? "15");
}
