using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

public interface IUserProfileService
{
    Task<UserProfileDto> GetUserProfileAsync(Guid userId);
    Task<UserProfileResponse> UpdateUserProfileAsync(UpdateUserProfileRequest request);
    Task<ProfileImageResponse> UploadProfileImageAsync(Guid userId, Stream imageStream, string fileName);
    Task<ProfileImageResponse> DeleteProfileImageAsync(Guid userId);
    Task<UserPreferencesResponse> UpdateUserPreferencesAsync(UpdateUserPreferencesRequest request);
    Task<IEnumerable<UserProfileDto>> SearchUsersAsync(string searchTerm, int page = 1, int pageSize = 20);
}
