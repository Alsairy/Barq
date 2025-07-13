using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

public interface IUserRegistrationService
{
    Task<UserRegistrationResponse> RegisterUserAsync(UserRegistrationRequest request);
    Task<EmailVerificationResponse> SendVerificationEmailAsync(Guid userId);
    Task<EmailVerificationResponse> VerifyEmailAsync(string token);
    Task<UserActivationResponse> ActivateUserAsync(Guid userId);
    Task<bool> IsEmailAvailableAsync(string email);
}
