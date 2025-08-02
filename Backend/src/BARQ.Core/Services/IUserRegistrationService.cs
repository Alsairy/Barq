using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface IUserRegistrationService
{
    /// <summary>
    /// </summary>
    Task<UserRegistrationResponse> RegisterUserAsync(UserRegistrationRequest request);

    /// <summary>
    /// </summary>
    Task<EmailVerificationResponse> SendVerificationEmailAsync(Guid userId);

    /// <summary>
    /// </summary>
    Task<EmailVerificationResponse> VerifyEmailAsync(string token);

    /// <summary>
    /// </summary>
    Task<UserActivationResponse> ActivateUserAsync(Guid userId);

    /// <summary>
    /// </summary>
    Task<bool> IsEmailAvailableAsync(string email);
}
