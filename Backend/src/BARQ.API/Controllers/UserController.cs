using Microsoft.AspNetCore.Mvc;
using MediatR;
using BARQ.Application.Commands.Users;
using BARQ.Application.Queries.Users;
using BARQ.Core.Services;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;
using BARQ.Core.Models.DTOs;
using BARQ.Shared.DTOs;

namespace BARQ.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUserRegistrationService _userRegistrationService;
    private readonly IUserProfileService _userProfileService;
    private readonly IUserRoleService _userRoleService;

    public UserController(
        IMediator mediator,
        IUserRegistrationService userRegistrationService,
        IUserProfileService userProfileService,
        IUserRoleService userRoleService)
    {
        _mediator = mediator;
        _userRegistrationService = userRegistrationService;
        _userProfileService = userProfileService;
        _userRoleService = userRoleService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<UserRegistrationResponse>>> Register([FromBody] RegisterUserCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(new ApiResponse<UserRegistrationResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<UserRegistrationResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("verify-email")]
    public async Task<ActionResult<ApiResponse<EmailVerificationResponse>>> VerifyEmail([FromBody] VerifyEmailRequest request)
    {
        try
        {
            var result = await _userRegistrationService.VerifyEmailAsync(request.Token);
            return Ok(new ApiResponse<EmailVerificationResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<EmailVerificationResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("resend-verification")]
    public async Task<ActionResult<ApiResponse<EmailVerificationResponse>>> ResendVerification([FromBody] ResendVerificationRequest request)
    {
        try
        {
            var result = await _userRegistrationService.SendVerificationEmailAsync(request.UserId);
            return Ok(new ApiResponse<EmailVerificationResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<EmailVerificationResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpGet("profile/{userId:guid}")]
    public async Task<ActionResult<ApiResponse<UserProfileResponse>>> GetProfile(Guid userId)
    {
        try
        {
            var query = new GetUserProfileQuery(userId);
            var result = await _mediator.Send(query);
            return Ok(new ApiResponse<UserProfileDto>
            {
                Success = true,
                Data = result,
                Message = "User profile retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<UserProfileResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPut("profile")]
    public async Task<ActionResult<ApiResponse<UserProfileResponse>>> UpdateProfile([FromBody] UpdateUserProfileRequest request)
    {
        try
        {
            var result = await _userProfileService.UpdateUserProfileAsync(request);
            return Ok(new ApiResponse<UserProfileResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<UserProfileResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("profile/image")]
    public async Task<ActionResult<ApiResponse<ProfileImageResponse>>> UploadProfileImage([FromBody] UploadProfileImageRequest request)
    {
        try
        {
            var result = new ProfileImageResponse
            {
                Success = false,
                Message = "File upload endpoint not yet implemented - requires IFormFile parameter"
            };
            return Ok(new ApiResponse<ProfileImageResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<ProfileImageResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpDelete("profile/image/{userId:guid}")]
    public async Task<ActionResult<ApiResponse<ProfileImageResponse>>> DeleteProfileImage(Guid userId)
    {
        try
        {
            var result = await _userProfileService.DeleteProfileImageAsync(userId);
            return Ok(new ApiResponse<ProfileImageResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<UserProfileResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpGet("preferences/{userId:guid}")]
    public async Task<ActionResult<ApiResponse<UserPreferencesResponse>>> GetPreferences(Guid userId)
    {
        try
        {
            var result = new UserPreferencesResponse
            {
                Success = false,
                Message = "User preferences endpoint not yet implemented"
            };
            return Ok(new ApiResponse<UserPreferencesResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<UserPreferencesResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPut("preferences")]
    public async Task<ActionResult<ApiResponse<UserPreferencesResponse>>> UpdatePreferences([FromBody] UpdateUserPreferencesRequest request)
    {
        try
        {
            var result = await _userProfileService.UpdateUserPreferencesAsync(request);
            return Ok(new ApiResponse<UserPreferencesResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<UserPreferencesResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpGet("roles/{userId:guid}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserRoleDto>>>> GetUserRoles(Guid userId)
    {
        try
        {
            var result = await _userRoleService.GetUserRolesAsync(userId);
            return Ok(new ApiResponse<IEnumerable<RoleDto>>
            {
                Success = true,
                Data = result,
                Message = "User roles retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<IEnumerable<UserRoleDto>>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("roles/assign")]
    public async Task<ActionResult<ApiResponse<UserRoleResponse>>> AssignRole([FromBody] AssignRoleRequest request)
    {
        try
        {
            var result = await _userRoleService.AssignRoleAsync(request);
            return Ok(new ApiResponse<UserRoleResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<UserRoleResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("roles/remove")]
    public async Task<ActionResult<ApiResponse<UserRoleResponse>>> RemoveRole([FromBody] RemoveRoleRequest request)
    {
        try
        {
            var result = await _userRoleService.RemoveRoleAsync(request);
            return Ok(new ApiResponse<UserRoleResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<UserRoleResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpGet("permissions/{userId:guid}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<PermissionDto>>>> GetUserPermissions(Guid userId)
    {
        try
        {
            var result = await _userRoleService.GetUserPermissionsAsync(userId);
            return Ok(new ApiResponse<IEnumerable<PermissionDto>>
            {
                Success = true,
                Data = result,
                Message = "User permissions retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<IEnumerable<PermissionDto>>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult<ApiResponse<UserSearchResponse>>> SearchUsers([FromQuery] string searchTerm, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        try
        {
            var result = await _userProfileService.SearchUsersAsync(searchTerm, page, pageSize);
            var response = new UserSearchResponse
            {
                Success = true,
                Users = result,
                TotalCount = result.Count(),
                PageNumber = page,
                PageSize = pageSize,
                Message = "Users retrieved successfully"
            };
            return Ok(new ApiResponse<UserSearchResponse>
            {
                Success = true,
                Data = response,
                Message = "Users retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<UserSearchResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("deactivate/{userId:guid}")]
    public async Task<ActionResult<ApiResponse<UserActivationResponse>>> DeactivateUser(Guid userId)
    {
        try
        {
            var result = new UserActivationResponse
            {
                Success = false,
                Message = "User deactivation endpoint not yet implemented"
            };
            return Ok(new ApiResponse<UserActivationResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<UserActivationResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("activate/{userId:guid}")]
    public async Task<ActionResult<ApiResponse<UserActivationResponse>>> ActivateUser(Guid userId)
    {
        try
        {
            var result = await _userRegistrationService.ActivateUserAsync(userId);
            return Ok(new ApiResponse<UserActivationResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<UserActivationResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpGet("email-available")]
    public async Task<ActionResult<ApiResponse<bool>>> CheckEmailAvailability([FromQuery] string email)
    {
        try
        {
            var result = await _userRegistrationService.IsEmailAvailableAsync(email);
            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Data = result,
                Message = result ? "Email is available" : "Email is already taken"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<bool>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }
}

public class VerifyEmailRequest
{
    public string Token { get; set; } = string.Empty;
}

public class ResendVerificationRequest
{
    public Guid UserId { get; set; }
}
