using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

/// <summary>
/// Service for managing user profiles, preferences, and profile-related operations.
/// </summary>
public interface IUserProfileService
{
    /// <summary>
    /// Service for managing user profiles, preferences, and profile-related operations.
/// </summary>
    Task<UserProfileDto> GetUserProfileAsync(Guid userId);

    /// <summary>
    /// Service for managing user profiles, preferences, and profile-related operations.
/// </summary>
    Task<UserProfileResponse> UpdateUserProfileAsync(UpdateUserProfileRequest request);

    /// <summary>
    /// Service for managing user profiles, preferences, and profile-related operations.
/// </summary>
    Task<ProfileImageResponse> UploadProfileImageAsync(Guid userId, Stream imageStream, string fileName);

    /// <summary>
    /// Service for managing user profiles, preferences, and profile-related operations.
/// </summary>
    Task<ProfileImageResponse> DeleteProfileImageAsync(Guid userId);

    /// <summary>
    /// Service for managing user profiles, preferences, and profile-related operations.
/// </summary>
    Task<UserPreferencesResponse> UpdateUserPreferencesAsync(UpdateUserPreferencesRequest request);

    /// <summary>
    /// Service for managing user profiles, preferences, and profile-related operations.
/// </summary>
    Task<IEnumerable<UserProfileDto>> SearchUsersAsync(string searchTerm, int page = 1, int pageSize = 20);
}
