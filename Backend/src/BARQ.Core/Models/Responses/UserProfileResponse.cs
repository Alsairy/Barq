using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Models.Responses;

/// <summary>
/// </summary>
public class UserProfileResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public UserProfileDto? UserProfile { get; set; }
}

/// <summary>
/// </summary>
public class ProfileImageResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public string? ImageUrl { get; set; }
}

/// <summary>
/// </summary>
public class UserPreferencesResponse : BaseResponse
{
    /// <summary>
    /// </summary>
    public UserPreferencesDto? Preferences { get; set; }
}
