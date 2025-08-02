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
            _logger.LogInformation("Starting authentication for user: {Email}", request.Email);
            
            var lockoutResponse = await CheckAccountLockoutAsync(request.Email);
            if (lockoutResponse.IsLockedOut)
            {
                _logger.LogWarning("User account is locked: {Email}", request.Email);
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Account is locked due to multiple failed login attempts",
                    RequiresMfa = false
                };
            }

            var users = await _userRepository.FindAsync(u => u.Email == request.Email.ToLowerInvariant());
            var user = users.FirstOrDefault();
            _logger.LogInformation("User lookup result for {Email}: {Found}", request.Email, user != null);
            
            if (user == null)
            {
                _logger.LogWarning("User not found: {Email}", request.Email);
                await IncrementFailedLoginAttemptAsync(request.Email);
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Invalid email or password",
                    RequiresMfa = false
                };
            }

            var passwordValid = _passwordService.VerifyPassword(request.Password, user.PasswordHash ?? string.Empty);
            _logger.LogInformation("Password verification for {Email}: {Valid}", request.Email, passwordValid);
            
            if (!passwordValid)
            {
                await IncrementFailedLoginAttemptAsync(request.Email);
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Invalid email or password",
                    RequiresMfa = false
                };
            }

            _logger.LogInformation("User status for {Email}: {Status}", request.Email, user.Status);
            if (user.Status != BARQ.Core.Enums.UserStatus.Active)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Account is not active",
                    RequiresMfa = false
                };
            }

            _logger.LogInformation("Email confirmed for {Email}: {Confirmed}", request.Email, user.EmailConfirmed);
            if (!user.EmailConfirmed)
            {
                return new AuthenticationResponse
                {
                    Success = false,
                    Message = "Email address is not verified",
                    RequiresMfa = false
                };
            }

            _logger.LogInformation("Two factor enabled for {Email}: {Enabled}", request.Email, user.TwoFactorEnabled);
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
            _logger.LogInformation("User roles for {Email}: {Roles}", request.Email, string.Join(", ", roleNames));

            var accessToken = GenerateAccessToken(user, roleNames);
            var refreshToken = GenerateRefreshToken();
            _logger.LogInformation("Generated tokens for {Email}: AccessToken length={AccessTokenLength}, RefreshToken length={RefreshTokenLength}", 
                request.Email, accessToken?.Length ?? 0, refreshToken?.Length ?? 0);

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
            _logger.LogError(ex, "Error refreshing token: {RefreshToken}", refreshToken);
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
            var key = Encoding.ASCII.GetBytes(GetJwtSecret());

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
            _logger.LogError(ex, "Error validating session token");
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
            Issuer = _configuration["Jwt:Issuer"] ?? "TestIssuer",
            Audience = _configuration["Jwt:Audience"] ?? "TestAudience",
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

    private string GenerateMfaToken(Guid userId)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(GetJwtSecret());

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new("token_type", "mfa")
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(5), // Short-lived MFA token
            Issuer = _configuration["Jwt:Issuer"] ?? "TestIssuer",
            Audience = _configuration["Jwt:Audience"] ?? "TestAudience",
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GetJwtSecret() 
    {
        var secret = _configuration["Jwt:Secret"] ?? _configuration["Jwt:Key"] ?? "default-secret-key-that-is-at-least-32-characters-long-for-security";
        
        if (secret.Length < 32)
        {
            _logger.LogCritical("JWT Secret is too short. Must be at least 32 characters for security.");
            throw new InvalidOperationException("JWT Secret must be at least 32 characters long for security.");
        }
        
        return secret;
    }
    private int GetTokenExpiryMinutes() => int.Parse(_configuration["Jwt:ExpiryMinutes"] ?? "60");
    private int GetMaxFailedAttempts() => int.Parse(_configuration["Security:MaxFailedAttempts"] ?? "5");
    private int GetLockoutDurationMinutes() => int.Parse(_configuration["Security:LockoutDurationMinutes"] ?? "15");
}
