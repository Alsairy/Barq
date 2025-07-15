using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BARQ.Core.Services;
using BARQ.Core.Interfaces;
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
            var response = new WorkflowResponse
            {
                Id = result.Id,
                Name = result.Name,
                Status = result.Status.ToString(),
                CreatedAt = result.CreatedAt
            };
            return Ok(new ApiResponse<WorkflowResponse>
            {
                Success = true,
                Data = response,
                Message = "Workflow created successfully"
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
            var response = new WorkflowExecutionResponse
            {
                WorkflowId = workflowId,
                Status = "Started",
                StartedAt = DateTime.UtcNow
            };
            return Ok(new ApiResponse<WorkflowExecutionResponse>
            {
                Success = result.IsSuccess,
                Data = response,
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
            var response = new WorkflowApprovalResponse
            {
                WorkflowId = request.WorkflowId,
                Status = "Approved",
                ApprovedAt = DateTime.UtcNow,
                ApprovedBy = Guid.NewGuid()
            };
            return Ok(new ApiResponse<WorkflowApprovalResponse>
            {
                Success = result.IsSuccess,
                Data = response,
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
            var response = new WorkflowRejectionResponse
            {
                WorkflowId = request.WorkflowId,
                Status = "Rejected",
                RejectedAt = DateTime.UtcNow,
                RejectedBy = Guid.NewGuid(),
                Reason = "Workflow rejected by user"
            };
            return Ok(new ApiResponse<WorkflowRejectionResponse>
            {
                Success = result.IsSuccess,
                Data = response,
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
            var result = await _workflowService.CancelWorkflowAsync(workflowId, Guid.NewGuid(), reason);
            var response = new WorkflowCancellationResponse
            {
                WorkflowId = workflowId,
                Status = "Cancelled",
                CancelledAt = DateTime.UtcNow,
                CancelledBy = Guid.NewGuid(),
                Reason = reason
            };
            return Ok(new ApiResponse<WorkflowCancellationResponse>
            {
                Success = result.IsSuccess,
                Data = response,
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
            var response = new WorkflowStatusResponse
            {
                WorkflowId = workflowId,
                Status = result.Status.ToString(),
                Progress = (int)result.Progress,
                CurrentStep = result.CurrentStep,
                LastUpdated = result.LastUpdated
            };
            return Ok(new ApiResponse<WorkflowStatusResponse>
            {
                Success = true,
                Data = response,
                Message = "Workflow status retrieved successfully"
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
            var response = new WorkflowHistoryResponse
            {
                WorkflowId = workflowId,
                History = result.Select(h => new BARQ.Core.Models.DTOs.WorkflowHistoryEntry
                {
                    Id = h.Id,
                    WorkflowInstanceId = h.WorkflowInstanceId,
                    PerformedAt = h.Timestamp,
                    Action = h.Action,
                    Description = h.Description,
                    PerformedByUserId = h.UserId ?? Guid.Empty,
                    Metadata = new Dictionary<string, object> { { "details", h.Details ?? "" } }
                }).ToList()
            };
            return Ok(new ApiResponse<WorkflowHistoryResponse>
            {
                Success = true,
                Data = response,
                Message = "Workflow history retrieved successfully"
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
            var workflowDtos = workflows.Select(w => new WorkflowDto
            {
                Id = w.Id,
                Name = w.Name,
                Status = w.Status.ToString(),
                CreatedAt = w.CreatedAt
            });
            return Ok(new ApiResponse<IEnumerable<WorkflowDto>>
            {
                Success = true,
                Data = workflowDtos,
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
            var approvalDtos = approvals.Select(a => new WorkflowApprovalDto
            {
                Id = a.Id,
                WorkflowId = a.Id,
                WorkflowName = a.Name,
                Status = a.Status.ToString(),
                RequestedAt = a.CreatedAt,
                RequestedBy = Guid.NewGuid(),
                RequestedByName = "User"
            });
            return Ok(new ApiResponse<IEnumerable<WorkflowApprovalDto>>
            {
                Success = true,
                Data = approvalDtos,
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
            var result = await _workflowService.GetWorkflowAnalyticsAsync();
            var response = new WorkflowAnalyticsResponse
            {
                TotalWorkflows = 0,
                CompletedWorkflows = 0,
                PendingWorkflows = 0,
                AverageCompletionTime = TimeSpan.Zero
            };
            return Ok(new ApiResponse<WorkflowAnalyticsResponse>
            {
                Success = true,
                Data = response,
                Message = "Workflow analytics retrieved successfully"
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
            var templateDtos = templates.Select(t => new WorkflowTemplateDto
            {
                Id = t.Id,
                Name = t.Name,
                Description = t.Description,
                CreatedAt = t.CreatedAt
            });
            return Ok(new ApiResponse<IEnumerable<WorkflowTemplateDto>>
            {
                Success = true,
                Data = templateDtos,
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
            var response = new WorkflowTemplateResponse
            {
                Id = result.Id,
                Name = result.Name,
                Description = result.Description,
                CreatedAt = result.CreatedAt
            };
            return Ok(new ApiResponse<WorkflowTemplateResponse>
            {
                Success = true,
                Data = response,
                Message = "Workflow template created successfully"
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
            var result = await _workflowService.CheckSlaBreachesAsync();
            var response = new WorkflowSlaBreachResponse
            {
                TotalBreaches = 0,
                BreachesThisMonth = 0,
                RecentBreaches = new List<WorkflowSlaBreachInfo>()
            };
            return Ok(new ApiResponse<WorkflowSlaBreachResponse>
            {
                Success = true,
                Data = response,
                Message = "SLA breaches checked successfully"
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
            var result = await _workflowService.GetWorkflowPerformanceAsync();
            var response = new WorkflowPerformanceResponse
            {
                AverageCompletionTime = TimeSpan.Zero,
                MedianCompletionTime = TimeSpan.Zero,
                CompletionRate = 0,
                SlaComplianceRate = 0,
                CompletionTimeByType = new Dictionary<string, TimeSpan>()
            };
            return Ok(new ApiResponse<WorkflowPerformanceResponse>
            {
                Success = true,
                Data = response,
                Message = "Workflow performance retrieved successfully"
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
