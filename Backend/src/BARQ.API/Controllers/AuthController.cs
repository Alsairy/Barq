using Microsoft.AspNetCore.Mvc;
using MediatR;
using BARQ.Application.Commands.Authentication;
using BARQ.Core.Services;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;
using BARQ.Shared.DTOs;

namespace BARQ.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAuthenticationService _authenticationService;
    private readonly IMultiFactorAuthService _mfaService;
    private readonly IPasswordService _passwordService;

    public AuthController(
        IMediator mediator,
        IAuthenticationService authenticationService,
        IMultiFactorAuthService mfaService,
        IPasswordService passwordService)
    {
        _mediator = mediator;
        _authenticationService = authenticationService;
        _mfaService = mfaService;
        _passwordService = passwordService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthenticationResponse>>> Login([FromBody] LoginCommand command)
    {
        try
        {
            var result = await _mediator.Send(command);
            return Ok(new ApiResponse<AuthenticationResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<AuthenticationResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("logout")]
    public async Task<ActionResult<ApiResponse<LogoutResponse>>> Logout([FromBody] LogoutRequest request)
    {
        try
        {
            var result = await _authenticationService.LogoutAsync(request.UserId);
            return Ok(new ApiResponse<LogoutResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<LogoutResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<ApiResponse<AuthenticationResponse>>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var result = await _authenticationService.RefreshTokenAsync(request.RefreshToken);
            return Ok(new ApiResponse<AuthenticationResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<AuthenticationResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult<ApiResponse<PasswordResetResponse>>> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        try
        {
            var result = await _passwordService.InitiatePasswordResetAsync(request.Email);
            return Ok(new ApiResponse<PasswordResetResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<PasswordResetResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult<ApiResponse<PasswordResetResponse>>> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        try
        {
            var passwordResetRequest = new PasswordResetRequest
            {
                ResetToken = request.Token,
                NewPassword = request.NewPassword
            };
            var result = await _passwordService.ResetPasswordAsync(passwordResetRequest);
            return Ok(new ApiResponse<PasswordResetResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<PasswordResetResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("change-password")]
    public async Task<ActionResult<ApiResponse<PasswordChangeResponse>>> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        try
        {
            var passwordChangeRequest = new PasswordChangeRequest
            {
                UserId = request.UserId,
                CurrentPassword = request.CurrentPassword,
                NewPassword = request.NewPassword
            };
            var result = await _passwordService.ChangePasswordAsync(passwordChangeRequest);
            return Ok(new ApiResponse<PasswordChangeResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<PasswordChangeResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("setup-mfa")]
    public async Task<ActionResult<ApiResponse<MfaSetupResponse>>> SetupMfa([FromBody] MfaSetupRequest request)
    {
        try
        {
            var result = await _mfaService.SetupMfaAsync(request.UserId);
            return Ok(new ApiResponse<MfaSetupResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<MfaSetupResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("verify-mfa")]
    public async Task<ActionResult<ApiResponse<MfaVerificationResponse>>> VerifyMfa([FromBody] MfaVerificationRequest request)
    {
        try
        {
            var result = await _mfaService.VerifyMfaCodeAsync(request);
            return Ok(new ApiResponse<MfaVerificationResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<MfaVerificationResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("disable-mfa")]
    public async Task<ActionResult<ApiResponse<MfaDisableResponse>>> DisableMfa([FromBody] MfaDisableRequest request)
    {
        try
        {
            var result = await _mfaService.DisableMfaAsync(request.UserId, request.Password);
            return Ok(new ApiResponse<MfaDisableResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<MfaDisableResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpGet("mfa-backup-codes/{userId:guid}")]
    public async Task<ActionResult<ApiResponse<BackupCodesResponse>>> GetMfaBackupCodes(Guid userId)
    {
        try
        {
            var result = await _mfaService.GenerateBackupCodesAsync(userId);
            return Ok(new ApiResponse<BackupCodesResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<BackupCodesResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("regenerate-backup-codes")]
    public async Task<ActionResult<ApiResponse<BackupCodesResponse>>> RegenerateBackupCodes([FromBody] RegenerateBackupCodesRequest request)
    {
        try
        {
            var result = await _mfaService.GenerateBackupCodesAsync(request.UserId);
            return Ok(new ApiResponse<BackupCodesResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<BackupCodesResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }
}
