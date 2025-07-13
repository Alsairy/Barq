using Microsoft.AspNetCore.Mvc;
using BARQ.Core.Services;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;
using BARQ.Core.Models.DTOs;
using BARQ.Shared.DTOs;

namespace BARQ.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrganizationController : ControllerBase
{
    private readonly IOrganizationService _organizationService;
    private readonly IUserInvitationService _userInvitationService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly ITenantContextService _tenantContextService;

    public OrganizationController(
        IOrganizationService organizationService,
        IUserInvitationService userInvitationService,
        ISubscriptionService subscriptionService,
        ITenantContextService tenantContextService)
    {
        _organizationService = organizationService;
        _userInvitationService = userInvitationService;
        _subscriptionService = subscriptionService;
        _tenantContextService = tenantContextService;
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<OrganizationResponse>>> CreateOrganization([FromBody] CreateOrganizationRequest request)
    {
        try
        {
            var result = await _organizationService.CreateOrganizationAsync(request);
            return Ok(new ApiResponse<OrganizationResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<OrganizationResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpGet("{organizationId:guid}")]
    public async Task<ActionResult<ApiResponse<OrganizationResponse>>> GetOrganization(Guid organizationId)
    {
        try
        {
            var result = await _organizationService.GetOrganizationAsync(organizationId);
            return Ok(new ApiResponse<OrganizationDto>
            {
                Success = true,
                Data = result,
                Message = "Organization retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<OrganizationResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPut("{organizationId:guid}")]
    public async Task<ActionResult<ApiResponse<OrganizationResponse>>> UpdateOrganization(Guid organizationId, [FromBody] UpdateOrganizationRequest request)
    {
        try
        {
            request.OrganizationId = organizationId;
            var result = await _organizationService.UpdateOrganizationAsync(request);
            return Ok(new ApiResponse<OrganizationResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<OrganizationResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpDelete("{organizationId:guid}")]
    public async Task<ActionResult<ApiResponse<OrganizationResponse>>> DeleteOrganization(Guid organizationId)
    {
        try
        {
            var result = await _organizationService.DeleteOrganizationAsync(organizationId);
            return Ok(new ApiResponse<OrganizationResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<OrganizationResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpGet("{organizationId:guid}/members")]
    public async Task<ActionResult<ApiResponse<IEnumerable<OrganizationMemberDto>>>> GetOrganizationMembers(Guid organizationId)
    {
        try
        {
            var emptyResult = new List<OrganizationMemberDto>();
            return Ok(new ApiResponse<IEnumerable<OrganizationMemberDto>>
            {
                Success = true,
                Data = emptyResult,
                Message = "Organization members endpoint not yet implemented"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<IEnumerable<OrganizationMemberDto>>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("{organizationId:guid}/invitations")]
    public async Task<ActionResult<ApiResponse<UserInvitationResponse>>> SendInvitation(Guid organizationId, [FromBody] SendInvitationRequest request)
    {
        try
        {
            var invitationRequest = new SendUserInvitationRequest
            {
                OrganizationId = organizationId,
                Email = request.Email,
                RoleIds = request.RoleIds,
                PersonalMessage = request.PersonalMessage
            };
            var result = await _userInvitationService.SendInvitationAsync(invitationRequest);
            return Ok(new ApiResponse<UserInvitationResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<UserInvitationResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("{organizationId:guid}/invitations/bulk")]
    public async Task<ActionResult<ApiResponse<BulkInvitationResponse>>> SendBulkInvitations(Guid organizationId, [FromBody] BulkInvitationRequest request)
    {
        try
        {
            var bulkRequest = new BulkUserInvitationRequest
            {
                OrganizationId = organizationId,
                Emails = request.Emails,
                RoleIds = request.RoleIds,
                PersonalMessage = request.PersonalMessage
            };
            var result = await _userInvitationService.SendBulkInvitationsAsync(bulkRequest);
            return Ok(new ApiResponse<BulkInvitationResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<BulkInvitationResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("invitations/{invitationId:guid}/accept")]
    public async Task<ActionResult<ApiResponse<InvitationAcceptanceResponse>>> AcceptInvitation(Guid invitationId, [FromBody] AcceptInvitationRequest request)
    {
        try
        {
            var result = await _userInvitationService.AcceptInvitationAsync(request);
            return Ok(new ApiResponse<InvitationAcceptanceResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<InvitationAcceptanceResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("invitations/{invitationId:guid}/resend")]
    public async Task<ActionResult<ApiResponse<UserInvitationResponse>>> ResendInvitation(Guid invitationId)
    {
        try
        {
            var result = await _userInvitationService.ResendInvitationAsync(invitationId);
            return Ok(new ApiResponse<UserInvitationResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<UserInvitationResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpDelete("invitations/{invitationId:guid}")]
    public async Task<ActionResult<ApiResponse<UserInvitationResponse>>> CancelInvitation(Guid invitationId)
    {
        try
        {
            var result = await _userInvitationService.CancelInvitationAsync(invitationId);
            return Ok(new ApiResponse<UserInvitationResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<UserInvitationResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpGet("{organizationId:guid}/invitations/pending")]
    public async Task<ActionResult<ApiResponse<IEnumerable<UserInvitationDto>>>> GetPendingInvitations(Guid organizationId)
    {
        try
        {
            var result = await _userInvitationService.GetPendingInvitationsAsync(organizationId);
            return Ok(new ApiResponse<IEnumerable<UserInvitationDto>>
            {
                Success = true,
                Data = result,
                Message = "Pending invitations retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<IEnumerable<UserInvitationDto>>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpGet("{organizationId:guid}/subscription")]
    public async Task<ActionResult<ApiResponse<SubscriptionDto>>> GetSubscription(Guid organizationId)
    {
        try
        {
            var result = await _subscriptionService.GetSubscriptionAsync(organizationId);
            return Ok(new ApiResponse<SubscriptionDto>
            {
                Success = true,
                Data = result,
                Message = "Subscription retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<SubscriptionDto>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("{organizationId:guid}/subscription/upgrade")]
    public async Task<ActionResult<ApiResponse<SubscriptionUpgradeResponse>>> UpgradeSubscription(Guid organizationId, [FromBody] UpgradeSubscriptionRequest request)
    {
        try
        {
            request.OrganizationId = organizationId;
            var result = await _subscriptionService.UpgradeSubscriptionAsync(request);
            var upgradeResponse = new SubscriptionUpgradeResponse
            {
                Success = result.Success,
                SubscriptionId = result.Subscription?.Id ?? Guid.Empty,
                NewPlan = result.Subscription?.Plan.ToString() ?? "Unknown",
                EffectiveDate = result.Subscription?.StartDate ?? DateTime.UtcNow,
                Message = result.Message
            };
            return Ok(new ApiResponse<SubscriptionUpgradeResponse>
            {
                Success = upgradeResponse.Success,
                Data = upgradeResponse,
                Message = upgradeResponse.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<SubscriptionUpgradeResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPost("{organizationId:guid}/subscription/downgrade")]
    public async Task<ActionResult<ApiResponse<SubscriptionDowngradeResponse>>> DowngradeSubscription(Guid organizationId, [FromBody] DowngradeSubscriptionRequest request)
    {
        try
        {
            request.OrganizationId = organizationId;
            var result = await _subscriptionService.DowngradeSubscriptionAsync(request);
            var downgradeResponse = new SubscriptionDowngradeResponse
            {
                Success = result.Success,
                SubscriptionId = result.Subscription?.Id ?? Guid.Empty,
                NewPlan = result.Subscription?.Plan.ToString() ?? "Unknown",
                EffectiveDate = result.Subscription?.StartDate ?? DateTime.UtcNow,
                Message = result.Message
            };
            return Ok(new ApiResponse<SubscriptionDowngradeResponse>
            {
                Success = downgradeResponse.Success,
                Data = downgradeResponse,
                Message = downgradeResponse.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<SubscriptionDowngradeResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpGet("{organizationId:guid}/usage")]
    public async Task<ActionResult<ApiResponse<UsageTrackingResponse>>> GetUsageTracking(Guid organizationId)
    {
        try
        {
            var trackingRequest = new TrackUsageRequest
            {
                OrganizationId = organizationId,
                ResourceType = "API_CALLS",
                UsageAmount = 0
            };
            var result = await _subscriptionService.TrackUsageAsync(trackingRequest);
            return Ok(new ApiResponse<UsageTrackingResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<UsageTrackingResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpGet("{organizationId:guid}/context")]
    public async Task<ActionResult<ApiResponse<TenantContextResponse>>> GetTenantContext(Guid organizationId)
    {
        try
        {
            var tenantDto = await _tenantContextService.GetCurrentTenantContextAsync();
            var response = new TenantContextResponse
            {
                Success = true,
                TenantContext = tenantDto,
                Message = "Tenant context retrieved successfully"
            };
            return Ok(new ApiResponse<TenantContextResponse>
            {
                Success = true,
                Data = response,
                Message = "Tenant context retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<TenantContextResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    [HttpPut("{organizationId:guid}/settings")]
    public async Task<ActionResult<ApiResponse<OrganizationSettingsResponse>>> UpdateSettings(Guid organizationId, [FromBody] UpdateOrganizationSettingsRequest request)
    {
        try
        {
            request.OrganizationId = organizationId;
            var result = await _organizationService.UpdateOrganizationSettingsAsync(request);
            return Ok(new ApiResponse<OrganizationSettingsResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<OrganizationSettingsResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }
}
