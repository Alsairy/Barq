using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BARQ.Core.Services;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;
using BARQ.Core.Models.DTOs;
using BARQ.Shared.DTOs;

namespace BARQ.API.Controllers;

/// <summary>
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class WorkflowController : ControllerBase
{
    private readonly IWorkflowService _workflowService;

    public WorkflowController(IWorkflowService workflowService)
    {
        _workflowService = workflowService;
    }

    /// <summary>
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<WorkflowResponse>>> CreateWorkflow([FromBody] CreateWorkflowRequest request)
    {
        try
        {
            var result = await _workflowService.CreateWorkflowAsync(request);
            return Ok(new ApiResponse<WorkflowResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<WorkflowResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// Start a workflow instance
    /// </summary>
    [HttpPost("{workflowId:guid}/start")]
    public async Task<ActionResult<ApiResponse<WorkflowExecutionResponse>>> StartWorkflow(Guid workflowId)
    {
        try
        {
            var result = await _workflowService.StartWorkflowAsync(workflowId);
            return Ok(new ApiResponse<WorkflowExecutionResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<WorkflowExecutionResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    /// <returns>Approval result</returns>
    [HttpPost("approve")]
    public async Task<ActionResult<ApiResponse<WorkflowApprovalResponse>>> ApproveWorkflow([FromBody] ApproveWorkflowRequest request)
    {
        try
        {
            var result = await _workflowService.ApproveWorkflowAsync(request);
            return Ok(new ApiResponse<WorkflowApprovalResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<WorkflowApprovalResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    /// <returns>Rejection result</returns>
    [HttpPost("reject")]
    public async Task<ActionResult<ApiResponse<WorkflowRejectionResponse>>> RejectWorkflow([FromBody] RejectWorkflowRequest request)
    {
        try
        {
            var result = await _workflowService.RejectWorkflowAsync(request);
            return Ok(new ApiResponse<WorkflowRejectionResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<WorkflowRejectionResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// Cancel a workflow instance
    /// </summary>
    /// <param name="reason">Cancellation reason</param>
    /// <returns>Cancellation result</returns>
    [HttpPost("{workflowId:guid}/cancel")]
    public async Task<ActionResult<ApiResponse<WorkflowCancellationResponse>>> CancelWorkflow(Guid workflowId, [FromQuery] string reason = "")
    {
        try
        {
            var result = await _workflowService.CancelWorkflowAsync(workflowId, reason);
            return Ok(new ApiResponse<WorkflowCancellationResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<WorkflowCancellationResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("{workflowId:guid}/status")]
    public async Task<ActionResult<ApiResponse<WorkflowStatusResponse>>> GetWorkflowStatus(Guid workflowId)
    {
        try
        {
            var result = await _workflowService.GetWorkflowStatusAsync(workflowId);
            return Ok(new ApiResponse<WorkflowStatusResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<WorkflowStatusResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("{workflowId:guid}/history")]
    public async Task<ActionResult<ApiResponse<WorkflowHistoryResponse>>> GetWorkflowHistory(Guid workflowId)
    {
        try
        {
            var result = await _workflowService.GetWorkflowHistoryAsync(workflowId);
            return Ok(new ApiResponse<WorkflowHistoryResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<WorkflowHistoryResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("project/{projectId:guid}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<WorkflowDto>>>> GetProjectWorkflows(Guid projectId)
    {
        try
        {
            var workflows = await _workflowService.GetProjectWorkflowsAsync(projectId);
            return Ok(new ApiResponse<IEnumerable<WorkflowDto>>
            {
                Success = true,
                Data = workflows,
                Message = "Project workflows retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<IEnumerable<WorkflowDto>>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// Get pending approvals for a user
    /// </summary>
    /// <param name="userId">User ID</param>
    [HttpGet("pending-approvals/{userId:guid}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<WorkflowApprovalDto>>>> GetPendingApprovals(Guid userId)
    {
        try
        {
            var approvals = await _workflowService.GetPendingApprovalsAsync(userId);
            return Ok(new ApiResponse<IEnumerable<WorkflowApprovalDto>>
            {
                Success = true,
                Data = approvals,
                Message = "Pending approvals retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<IEnumerable<WorkflowApprovalDto>>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("analytics")]
    public async Task<ActionResult<ApiResponse<WorkflowAnalyticsResponse>>> GetWorkflowAnalytics(
        [FromQuery] Guid? projectId = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var result = await _workflowService.GetWorkflowAnalyticsAsync(projectId, startDate, endDate);
            return Ok(new ApiResponse<WorkflowAnalyticsResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<WorkflowAnalyticsResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("templates")]
    public async Task<ActionResult<ApiResponse<IEnumerable<WorkflowTemplateDto>>>> GetWorkflowTemplates()
    {
        try
        {
            var templates = await _workflowService.GetWorkflowTemplatesAsync();
            return Ok(new ApiResponse<IEnumerable<WorkflowTemplateDto>>
            {
                Success = true,
                Data = templates,
                Message = "Workflow templates retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<IEnumerable<WorkflowTemplateDto>>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpPost("templates")]
    public async Task<ActionResult<ApiResponse<WorkflowTemplateResponse>>> CreateWorkflowTemplate([FromBody] CreateWorkflowTemplateRequest request)
    {
        try
        {
            var result = await _workflowService.CreateWorkflowTemplateAsync(request);
            return Ok(new ApiResponse<WorkflowTemplateResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<WorkflowTemplateResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("sla-breaches")]
    public async Task<ActionResult<ApiResponse<WorkflowSlaBreachResponse>>> CheckSlaBreaches([FromQuery] Guid? projectId = null)
    {
        try
        {
            var result = await _workflowService.CheckSlaBreachesAsync(projectId);
            return Ok(new ApiResponse<WorkflowSlaBreachResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<WorkflowSlaBreachResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("performance")]
    public async Task<ActionResult<ApiResponse<WorkflowPerformanceResponse>>> GetWorkflowPerformance(
        [FromQuery] Guid? projectId = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var result = await _workflowService.GetWorkflowPerformanceAsync(projectId, startDate, endDate);
            return Ok(new ApiResponse<WorkflowPerformanceResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<WorkflowPerformanceResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }
}
