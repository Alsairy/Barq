using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BARQ.Core.Services;
using BARQ.Core.Interfaces;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;
using BARQ.Core.Enums;
using BARQ.Shared.DTOs;

namespace BARQ.API.Controllers;

/// <summary>
/// </summary>
[ApiController]
[Route("api/[controller]")]
// [Authorize] // Temporarily disabled for development
public class AIRequestController : ControllerBase
{
    private readonly IAIRequestService _aiRequestService;
    private readonly ILogger<AIRequestController> _logger;

    public AIRequestController(
        IAIRequestService aiRequestService,
        ILogger<AIRequestController> logger)
    {
        _aiRequestService = aiRequestService ?? throw new ArgumentNullException(nameof(aiRequestService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<AIRequestResponse>>> CreateRequest([FromBody] CreateAIRequestRequest request)
    {
        try
        {
            _logger.LogInformation("Creating AI request: {Title}, RequestType: {RequestType}, Priority: {Priority}, RequesterId: {RequesterId}", 
                request.Title, request.RequestType, request.Priority, request.RequesterId);

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                
                _logger.LogWarning("Model validation failed for AI request creation: {@Errors}", errors);
                return BadRequest(new ValidationProblemDetails(ModelState));
            }

            var aiRequest = await _aiRequestService.CreateRequestAsync(request);
            
            var response = new AIRequestResponse
            {
                Id = aiRequest.Id.ToString(),
                Title = aiRequest.Title,
                Description = aiRequest.Description,
                RequestType = aiRequest.RequestType.ToString(),
                Priority = aiRequest.Priority.ToString(),
                Status = aiRequest.Status.ToString(),
                CreatedAt = aiRequest.CreatedAt,
                UpdatedAt = aiRequest.UpdatedAt
            };

            return Ok(ApiResponse<AIRequestResponse>.SuccessResponse(response));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating AI request: {Title}", request.Title);
            return BadRequest(ApiResponse<AIRequestResponse>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// </summary>
    [HttpPost("{requestId}/submit")]
    public async Task<ActionResult<ApiResponse<WorkflowExecutionResult>>> SubmitRequest(Guid requestId)
    {
        try
        {
            _logger.LogInformation("Submitting AI request: {RequestId}", requestId);

            var submittedRequest = await _aiRequestService.SubmitRequestAsync(requestId);
            
            var workflowResult = await _aiRequestService.StartWorkflowAsync(requestId);

            return Ok(new ApiResponse<WorkflowExecutionResult>
            {
                Success = true,
                Data = workflowResult
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error submitting AI request: {RequestId}", requestId);
            return BadRequest(new ApiResponse<WorkflowExecutionResult>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("{requestId}")]
    public async Task<ActionResult<ApiResponse<AIRequestResponse>>> GetRequest(Guid requestId)
    {
        try
        {
            var aiRequest = await _aiRequestService.GetRequestAsync(requestId);
            if (aiRequest == null)
            {
                return NotFound(ApiResponse<AIRequestResponse>.ErrorResponse("AI request not found"));
            }

            var response = new AIRequestResponse
            {
                Id = aiRequest.Id.ToString(),
                Title = aiRequest.Title,
                Description = aiRequest.Description,
                RequestType = aiRequest.RequestType.ToString(),
                Priority = aiRequest.Priority.ToString(),
                Status = aiRequest.Status.ToString(),
                CreatedAt = aiRequest.CreatedAt,
                UpdatedAt = aiRequest.UpdatedAt
            };

            return Ok(ApiResponse<AIRequestResponse>.SuccessResponse(response));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting AI request: {RequestId}", requestId);
            return BadRequest(ApiResponse<AIRequestResponse>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<AIRequestResponse>>>> GetUserRequests(
        Guid userId, 
        [FromQuery] AIRequestStatus? status = null)
    {
        try
        {
            var requests = await _aiRequestService.GetUserRequestsAsync(userId, status);
            
            var responses = requests.Select(r => new AIRequestResponse
            {
                Id = r.Id.ToString(),
                Title = r.Title,
                Description = r.Description,
                RequestType = r.RequestType.ToString(),
                Priority = r.Priority.ToString(),
                Status = r.Status.ToString(),
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            });

            return Ok(ApiResponse<IEnumerable<AIRequestResponse>>.SuccessResponse(responses));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user requests: {UserId}", userId);
            return BadRequest(ApiResponse<IEnumerable<AIRequestResponse>>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// </summary>
    [HttpPut("{requestId}")]
    public async Task<ActionResult<ApiResponse<AIRequestResponse>>> UpdateRequest(
        Guid requestId, 
        [FromBody] UpdateAIRequestRequest request)
    {
        try
        {
            var updatedRequest = await _aiRequestService.UpdateRequestAsync(requestId, request);
            
            var response = new AIRequestResponse
            {
                Id = updatedRequest.Id.ToString(),
                Title = updatedRequest.Title,
                Description = updatedRequest.Description,
                RequestType = updatedRequest.RequestType.ToString(),
                Priority = updatedRequest.Priority.ToString(),
                Status = updatedRequest.Status.ToString(),
                CreatedAt = updatedRequest.CreatedAt,
                UpdatedAt = updatedRequest.UpdatedAt
            };

            return Ok(ApiResponse<AIRequestResponse>.SuccessResponse(response));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating AI request: {RequestId}", requestId);
            return BadRequest(ApiResponse<AIRequestResponse>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// </summary>
    [HttpDelete("{requestId}")]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteRequest(Guid requestId)
    {
        try
        {
            var result = await _aiRequestService.DeleteRequestAsync(requestId);
            return Ok(ApiResponse<bool>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting AI request: {RequestId}", requestId);
            return BadRequest(ApiResponse<bool>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// Get pending approvals for a user
    /// </summary>
    [HttpGet("approvals/pending/{userId}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<AIRequestResponse>>>> GetPendingApprovals(Guid userId)
    {
        try
        {
            var requests = await _aiRequestService.GetPendingApprovalsAsync(userId);
            
            var responses = requests.Select(r => new AIRequestResponse
            {
                Id = r.Id.ToString(),
                Title = r.Title,
                Description = r.Description,
                RequestType = r.RequestType.ToString(),
                Priority = r.Priority.ToString(),
                Status = r.Status.ToString(),
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            });

            return Ok(ApiResponse<IEnumerable<AIRequestResponse>>.SuccessResponse(responses));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pending approvals: {UserId}", userId);
            return BadRequest(ApiResponse<IEnumerable<AIRequestResponse>>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// </summary>
    [HttpPost("{requestId}/approve")]
    public async Task<ActionResult<ApiResponse<string>>> ApproveRequest(
        Guid requestId, 
        [FromBody] ApprovalRequest request)
    {
        try
        {
            var approval = await _aiRequestService.ApproveRequestAsync(requestId, request.ApproverId, request.Comments);
            return Ok(ApiResponse<string>.SuccessResponse("Request approved successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving AI request: {RequestId}", requestId);
            return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// </summary>
    [HttpPost("{requestId}/reject")]
    public async Task<ActionResult<ApiResponse<string>>> RejectRequest(
        Guid requestId, 
        [FromBody] RejectionRequest request)
    {
        try
        {
            var approval = await _aiRequestService.RejectRequestAsync(requestId, request.ApproverId, request.Reason);
            return Ok(ApiResponse<string>.SuccessResponse("Request rejected successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rejecting AI request: {RequestId}", requestId);
            return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// </summary>
    [HttpPost("{requestId}/request-changes")]
    public async Task<ActionResult<ApiResponse<string>>> RequestChanges(
        Guid requestId, 
        [FromBody] ChangeRequest request)
    {
        try
        {
            var approval = await _aiRequestService.RequestChangesAsync(requestId, request.ReviewerId, request.ChangeRequests);
            return Ok(ApiResponse<string>.SuccessResponse("Changes requested successfully"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error requesting changes: {RequestId}", requestId);
            return BadRequest(ApiResponse<string>.ErrorResponse(ex.Message));
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("analytics")]
    public async Task<ActionResult<ApiResponse<object>>> GetAnalytics()
    {
        try
        {
            var analytics = await _aiRequestService.GetRequestAnalyticsAsync();
            return Ok(ApiResponse<object>.SuccessResponse(analytics));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting AI request analytics");
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
    }
}

public class ApprovalRequest
{
    public Guid ApproverId { get; set; }
    public string? Comments { get; set; }
}

public class RejectionRequest
{
    public Guid ApproverId { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class ChangeRequest
{
    public Guid ReviewerId { get; set; }
    public string ChangeRequests { get; set; } = string.Empty;
}

public class AIRequestResponse
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string RequestType { get; set; } = string.Empty;
    public string Priority { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
