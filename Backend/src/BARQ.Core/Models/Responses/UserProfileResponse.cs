using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

public class UserProfileResponse : BaseResponse
{
    public UserProfileDto? UserProfile { get; set; }
}

public class ProfileImageResponse : BaseResponse
{
    public string? ImageUrl { get; set; }
}

public class UserPreferencesResponse : BaseResponse
{
    public UserPreferencesDto? Preferences { get; set; }
}
