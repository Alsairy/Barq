using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface IAuthenticationService
{
    /// <summary>
    /// </summary>
    Task<AuthenticationResponse> AuthenticateAsync(LoginRequest request);
    
    /// <summary>
    /// </summary>
    Task<AuthenticationResponse> RefreshTokenAsync(string refreshToken);
    
    /// <summary>
    /// </summary>
    Task<LogoutResponse> LogoutAsync(Guid userId);
    
    /// <summary>
    /// </summary>
    Task<SessionValidationResponse> ValidateSessionAsync(string token);
    
    /// <summary>
    /// </summary>
    Task<AccountLockoutResponse> CheckAccountLockoutAsync(string email);
    
    /// <summary>
    /// </summary>
    Task IncrementFailedLoginAttemptAsync(string email);
    
    /// <summary>
    /// </summary>
    Task ResetFailedLoginAttemptsAsync(string email);
}
