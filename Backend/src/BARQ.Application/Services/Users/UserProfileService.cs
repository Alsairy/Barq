using Microsoft.Extensions.Logging;
using AutoMapper;
using BARQ.Core.Services;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;
using BARQ.Core.Entities;
using BARQ.Core.Repositories;

namespace BARQ.Application.Services.Users;

public class UserProfileService : IUserProfileService
{
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UserProfileService> _logger;

    public UserProfileService(
        IRepository<User> userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UserProfileService> logger)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<UserProfileDto> GetUserProfileAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException("User not found", nameof(userId));
            }

            var userProfile = _mapper.Map<UserProfileDto>(user);
            
            _logger.LogInformation("User profile retrieved for user: {UserId}", userId);
            return userProfile;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user profile for user: {UserId}", userId);
            throw;
        }
    }

    public async Task<UserProfileResponse> UpdateUserProfileAsync(UpdateUserProfileRequest request)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return new UserProfileResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            user.FirstName = request.FirstName ?? user.FirstName;
            user.LastName = request.LastName ?? user.LastName;
            user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
            user.TimeZone = request.TimeZone ?? user.TimeZone;
            user.Language = request.Language ?? user.Language;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var updatedProfile = _mapper.Map<UserProfileDto>(user);

            _logger.LogInformation("User profile updated for user: {UserId}", request.UserId);

            return new UserProfileResponse
            {
                Success = true,
                Message = "Profile updated successfully",
                UserProfile = updatedProfile
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user profile for user: {UserId}", request.UserId);
            return new UserProfileResponse
            {
                Success = false,
                Message = "Failed to update profile"
            };
        }
    }

    public async Task<UserProfileResponse> UploadProfileImageAsync(UploadProfileImageRequest request)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return new UserProfileResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (request.ImageData == null || request.ImageData.Length == 0)
            {
                return new UserProfileResponse
                {
                    Success = false,
                    Message = "No image file provided"
                };
            }

            if (request.ImageData.Length > 5 * 1024 * 1024)
            {
                return new UserProfileResponse
                {
                    Success = false,
                    Message = "Image file size must be less than 5MB"
                };
            }

            var allowedTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif" };
            if (!allowedTypes.Contains(request.ImageContentType.ToLowerInvariant()))
            {
                return new UserProfileResponse
                {
                    Success = false,
                    Message = "Only JPEG, PNG, and GIF images are allowed"
                };
            }

            var fileExtension = Path.GetExtension(request.ImageFileName);
            var fileName = $"{user.Id}_{DateTime.UtcNow:yyyyMMddHHmmss}{fileExtension}";
            var filePath = Path.Combine("uploads", "profile-images", fileName);

            var directory = Path.GetDirectoryName(filePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await File.WriteAllBytesAsync(filePath, request.ImageData);

            user.ProfilePictureUrl = $"/uploads/profile-images/{fileName}";
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var updatedProfile = _mapper.Map<UserProfileDto>(user);

            _logger.LogInformation("Profile image uploaded for user: {UserId}", request.UserId);

            return new UserProfileResponse
            {
                Success = true,
                Message = "Profile image uploaded successfully",
                UserProfile = updatedProfile
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading profile image for user: {UserId}", request.UserId);
            return new UserProfileResponse
            {
                Success = false,
                Message = "Failed to upload profile image"
            };
        }
    }

    public async Task<UserPreferencesResponse> GetUserPreferencesAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new UserPreferencesResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var preferences = new UserPreferencesDto
            {
                UserId = user.Id,
                TimeZone = user.TimeZone,
                Language = user.Language,
                EmailNotifications = true, // Default value
                PushNotifications = true, // Default value
                Theme = "light", // Default value
                DateFormat = "MM/dd/yyyy", // Default value
                TimeFormat = "12h" // Default value
            };

            _logger.LogInformation("User preferences retrieved for user: {UserId}", userId);

            return new UserPreferencesResponse
            {
                Success = true,
                Message = "Preferences retrieved successfully",
                Preferences = preferences
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user preferences for user: {UserId}", userId);
            return new UserPreferencesResponse
            {
                Success = false,
                Message = "Failed to retrieve preferences"
            };
        }
    }

    public async Task<UserPreferencesResponse> UpdateUserPreferencesAsync(UpdateUserPreferencesRequest request)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                return new UserPreferencesResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            user.TimeZone = request.TimeZone ?? user.TimeZone;
            user.Language = request.Language ?? user.Language;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var updatedPreferences = new UserPreferencesDto
            {
                UserId = user.Id,
                TimeZone = user.TimeZone,
                Language = user.Language,
                EmailNotifications = true, // Default value
                PushNotifications = true, // Default value
                Theme = "light", // Default value
                DateFormat = "MM/dd/yyyy", // Default value
                TimeFormat = "12h" // Default value
            };

            _logger.LogInformation("User preferences updated for user: {UserId}", request.UserId);

            return new UserPreferencesResponse
            {
                Success = true,
                Message = "Preferences updated successfully",
                Preferences = updatedPreferences
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user preferences for user: {UserId}", request.UserId);
            return new UserPreferencesResponse
            {
                Success = false,
                Message = "Failed to update preferences"
            };
        }
    }

    public async Task<UserProfileResponse> DeactivateUserAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new UserProfileResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            if (user.Status != BARQ.Core.Enums.UserStatus.Active)
            {
                return new UserProfileResponse
                {
                    Success = false,
                    Message = "User is already deactivated"
                };
            }

            user.Status = BARQ.Core.Enums.UserStatus.Inactive;
            user.UpdatedAt = DateTime.UtcNow;


            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("User deactivated: {UserId}", userId);

            return new UserProfileResponse
            {
                Success = true,
                Message = "User deactivated successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating user: {UserId}", userId);
            return new UserProfileResponse
            {
                Success = false,
                Message = "Failed to deactivate user"
            };
        }
    }

    public async Task<bool> IsEmailAvailableAsync(string email, Guid? excludeUserId = null)
    {
        try
        {
            var users = await _userRepository.FindAsync(u => u.Email == email.ToLowerInvariant());
            
            if (excludeUserId.HasValue)
            {
                users = users.Where(u => u.Id != excludeUserId.Value);
            }

            return !users.Any();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking email availability: {Email}", email);
            return false;
        }
    }

    public async Task<ProfileImageResponse> UploadProfileImageAsync(Guid userId, Stream imageStream, string fileName)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ProfileImageResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            using var memoryStream = new MemoryStream();
            await imageStream.CopyToAsync(memoryStream);
            var imageData = memoryStream.ToArray();

            user.ProfilePictureUrl = $"/api/users/{userId}/profile-image";
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Profile image uploaded successfully: {UserId}", userId);

            return new ProfileImageResponse
            {
                Success = true,
                Message = "Profile image uploaded successfully",
                ImageUrl = user.ProfilePictureUrl
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading profile image: {UserId}", userId);
            return new ProfileImageResponse
            {
                Success = false,
                Message = "Failed to upload profile image"
            };
        }
    }

    public async Task<ProfileImageResponse> DeleteProfileImageAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new ProfileImageResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            user.ProfilePictureUrl = null;
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Profile image deleted successfully: {UserId}", userId);

            return new ProfileImageResponse
            {
                Success = true,
                Message = "Profile image deleted successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting profile image: {UserId}", userId);
            return new ProfileImageResponse
            {
                Success = false,
                Message = "Failed to delete profile image"
            };
        }
    }

    public async Task<IEnumerable<UserProfileDto>> SearchUsersAsync(string searchTerm, int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            var users = await _userRepository.FindAsync(u => 
                u.FirstName.Contains(searchTerm) || 
                u.LastName.Contains(searchTerm) || 
                u.Email.Contains(searchTerm));

            var pagedUsers = users.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            var userDtos = pagedUsers.Select(u => new UserProfileDto
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                PhoneNumber = u.PhoneNumber,
                ProfileImageUrl = u.ProfilePictureUrl,
                IsActive = u.Status == BARQ.Core.Enums.UserStatus.Active,
                EmailVerified = u.EmailConfirmed,
                CreatedAt = u.CreatedAt
            }).ToList();

            _logger.LogInformation("Found {Count} users for search term: {SearchTerm}", userDtos.Count, searchTerm);
            return userDtos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching users with term: {SearchTerm}", searchTerm);
            return new List<UserProfileDto>();
        }
    }
}
