using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

public interface IAuthenticationService
{
    Task<AuthenticationResponse> AuthenticateAsync(LoginRequest request);
    Task<AuthenticationResponse> RefreshTokenAsync(string refreshToken);
    Task<LogoutResponse> LogoutAsync(Guid userId);
    Task<SessionValidationResponse> ValidateSessionAsync(string token);
    Task<AccountLockoutResponse> CheckAccountLockoutAsync(string email);
    Task IncrementFailedLoginAttemptAsync(string email);
    Task ResetFailedLoginAttemptsAsync(string email);
}
