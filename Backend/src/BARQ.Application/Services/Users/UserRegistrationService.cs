using Microsoft.Extensions.Logging;
using BARQ.Core.Services;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;
using BARQ.Core.Entities;
using BARQ.Core.Repositories;
using System.Security.Cryptography;
using System.Text;

namespace BARQ.Application.Services.Users;

/// <summary>
/// </summary>
public class UserRegistrationService : IUserRegistrationService
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<Organization> _organizationRepository;
    private readonly IPasswordService _passwordService;
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UserRegistrationService> _logger;

    public UserRegistrationService(
        IRepository<User> userRepository,
        IRepository<Organization> organizationRepository,
        IPasswordService passwordService,
        INotificationService notificationService,
        IUnitOfWork unitOfWork,
        ILogger<UserRegistrationService> logger)
    {
        _userRepository = userRepository;
        _organizationRepository = organizationRepository;
        _passwordService = passwordService;
        _notificationService = notificationService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<UserRegistrationResponse> RegisterUserAsync(UserRegistrationRequest request)
    {
        try
        {
            if (!await IsEmailAvailableAsync(request.Email))
            {
                return new UserRegistrationResponse
                {
                    Success = false,
                    Message = "Email address is already registered",
                    Errors = new[] { "Email address is already in use" }
                };
            }

            var organization = await _organizationRepository.GetByIdAsync(request.OrganizationId ?? Guid.Empty);
            if (organization == null && request.OrganizationId.HasValue)
            {
                return new UserRegistrationResponse
                {
                    Success = false,
                    Message = "Invalid organization",
                    Errors = new[] { "Organization not found" }
                };
            }

            var hashedPassword = _passwordService.HashPassword(request.Password);
            var verificationToken = GenerateVerificationToken();

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = request.Email.ToLowerInvariant(),
                PasswordHash = hashedPassword,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                TenantId = request.OrganizationId ?? Guid.Empty,
                Status = BARQ.Core.Enums.UserStatus.Inactive,
                EmailConfirmed = false,
                // This would need to be stored in a separate EmailVerificationToken entity
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _userRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            await SendVerificationEmailAsync(user.Id);

            _logger.LogInformation("User registered successfully: {Email}", request.Email);

            return new UserRegistrationResponse
            {
                Success = true,
                Message = "User registered successfully. Please check your email for verification.",
                UserId = user.Id,
                RequiresEmailVerification = true,
                VerificationToken = verificationToken
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error registering user: {Email}", request.Email);
            return new UserRegistrationResponse
            {
                Success = false,
                Message = "An error occurred during registration",
                Errors = new[] { "Registration failed. Please try again." }
            };
        }
    }

    public async Task<EmailVerificationResponse> SendVerificationEmailAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new EmailVerificationResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (user.EmailConfirmed)
            {
                return new EmailVerificationResponse
                {
                    Success = false,
                    Message = "Email is already verified"
                };
            }

            var verificationToken = GenerateVerificationToken();
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();


            _logger.LogInformation("Verification email sent to user: {UserId}", userId);

            return new EmailVerificationResponse
            {
                Success = true,
                Message = "Verification email sent successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending verification email for user: {UserId}", userId);
            return new EmailVerificationResponse
            {
                Success = false,
                Message = "Failed to send verification email"
            };
        }
    }

    public async Task<EmailVerificationResponse> VerifyEmailAsync(string token)
    {
        try
        {
            User? user = null;
            if (user == null)
            {
                return new EmailVerificationResponse
                {
                    Success = false,
                    Message = "Invalid verification token"
                };
            }

            if (user.EmailVerificationTokenExpiry < DateTime.UtcNow)
            {
                return new EmailVerificationResponse
                {
                    Success = false,
                    Message = "Verification token has expired"
                };
            }

            user.EmailVerified = true;
            user.EmailVerificationToken = null;
            user.EmailVerificationTokenExpiry = null;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Email verified successfully for user: {UserId}", user.Id);

            return new EmailVerificationResponse
            {
                Success = true,
                Message = "Email verified successfully",
                IsVerified = true,
                VerifiedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verifying email with token: {Token}", token);
            return new EmailVerificationResponse
            {
                Success = false,
                Message = "Email verification failed"
            };
        }
    }

    public async Task<UserActivationResponse> ActivateUserAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new UserActivationResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (!user.EmailConfirmed)
            {
                return new UserActivationResponse
                {
                    Success = false,
                    Message = "Email must be verified before activation"
                };
            }

            user.Status = BARQ.Core.Enums.UserStatus.Active;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("User activated successfully: {UserId}", userId);

            return new UserActivationResponse
            {
                Success = true,
                Message = "User activated successfully",
                IsActivated = true,
                ActivatedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error activating user: {UserId}", userId);
            return new UserActivationResponse
            {
                Success = false,
                Message = "User activation failed"
            };
        }
    }

    public async Task<bool> IsEmailAvailableAsync(string email)
    {
        var existingUsers = await _userRepository.FindAsync(u => u.Email == email.ToLowerInvariant());
        var existingUser = existingUsers.FirstOrDefault();
        return existingUser == null;
    }

    private string GenerateVerificationToken()
    {
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[32];
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
    }
}
