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
public class AITaskController : ControllerBase
{
    private readonly IAIOrchestrationService _aiOrchestrationService;

    public AITaskController(IAIOrchestrationService aiOrchestrationService)
    {
        _aiOrchestrationService = aiOrchestrationService;
    }

    /// <summary>
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<AITaskResponse>>> CreateAITask([FromBody] CreateAITaskRequest request)
    {
        try
        {
            var result = await _aiOrchestrationService.CreateAITaskAsync(request);
            return Ok(new ApiResponse<AITaskResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<AITaskResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    /// <returns>Task execution result</returns>
    [HttpPost("{taskId:guid}/execute")]
    public async Task<ActionResult<ApiResponse<AITaskExecutionResponse>>> ExecuteAITask(Guid taskId)
    {
        try
        {
            var result = await _aiOrchestrationService.ExecuteAITaskAsync(taskId);
            return Ok(new ApiResponse<AITaskExecutionResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<AITaskExecutionResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// Get AI task status
    /// </summary>
    [HttpGet("{taskId:guid}/status")]
    public async Task<ActionResult<ApiResponse<AITaskStatusResponse>>> GetAITaskStatus(Guid taskId)
    {
        try
        {
            var result = await _aiOrchestrationService.GetAITaskStatusAsync(taskId);
            return Ok(new ApiResponse<AITaskStatusResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<AITaskStatusResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// Cancel an AI task
    /// </summary>
    [HttpPost("{taskId:guid}/cancel")]
    public async Task<ActionResult<ApiResponse<AITaskResponse>>> CancelAITask(Guid taskId)
    {
        try
        {
            var result = await _aiOrchestrationService.CancelAITaskAsync(taskId);
            return Ok(new ApiResponse<AITaskResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<AITaskResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("{taskId:guid}/results")]
    public async Task<ActionResult<ApiResponse<AITaskResultResponse>>> GetAITaskResults(Guid taskId)
    {
        try
        {
            var result = await _aiOrchestrationService.GetAITaskResultsAsync(taskId);
            return Ok(new ApiResponse<AITaskResultResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<AITaskResultResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("project/{projectId:guid}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<AITaskDto>>>> GetProjectAITasks(Guid projectId)
    {
        try
        {
            var tasks = await _aiOrchestrationService.GetProjectAITasksAsync(projectId);
            return Ok(new ApiResponse<IEnumerable<AITaskDto>>
            {
                Success = true,
                Data = tasks,
                Message = "Project AI tasks retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<IEnumerable<AITaskDto>>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("analytics")]
    public async Task<ActionResult<ApiResponse<AITaskAnalyticsResponse>>> GetAITaskAnalytics(
        [FromQuery] Guid? projectId = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var result = await _aiOrchestrationService.GetAITaskAnalyticsAsync(projectId, startDate, endDate);
            return Ok(new ApiResponse<AITaskAnalyticsResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<AITaskAnalyticsResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("providers")]
    public async Task<ActionResult<ApiResponse<IEnumerable<AIProviderDto>>>> GetAvailableProviders()
    {
        try
        {
            var providers = await _aiOrchestrationService.GetAvailableProvidersAsync();
            return Ok(new ApiResponse<IEnumerable<AIProviderDto>>
            {
                Success = true,
                Data = providers,
                Message = "Available AI providers retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<IEnumerable<AIProviderDto>>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// Get AI provider health status
    /// </summary>
    /// <param name="providerId">Provider ID</param>
    [HttpGet("providers/{providerId:guid}/health")]
    public async Task<ActionResult<ApiResponse<AIProviderHealthResponse>>> GetProviderHealth(Guid providerId)
    {
        try
        {
            var result = await _aiOrchestrationService.CheckProviderHealthAsync(providerId);
            return Ok(new ApiResponse<AIProviderHealthResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<AIProviderHealthResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpPost("providers/configure")]
    public async Task<ActionResult<ApiResponse<AIProviderConfigurationResponse>>> ConfigureProvider([FromBody] ConfigureAIProviderRequest request)
    {
        try
        {
            var result = await _aiOrchestrationService.ConfigureProviderAsync(request);
            return Ok(new ApiResponse<AIProviderConfigurationResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<AIProviderConfigurationResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpPost("batch")]
    public async Task<ActionResult<ApiResponse<AIBatchExecutionResponse>>> ExecuteBatchTasks([FromBody] ExecuteBatchAITasksRequest request)
    {
        try
        {
            var result = await _aiOrchestrationService.ExecuteBatchTasksAsync(request);
            return Ok(new ApiResponse<AIBatchExecutionResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<AIBatchExecutionResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("queue/status")]
    public async Task<ActionResult<ApiResponse<AITaskQueueStatusResponse>>> GetQueueStatus()
    {
        try
        {
            var result = await _aiOrchestrationService.GetQueueStatusAsync();
            return Ok(new ApiResponse<AITaskQueueStatusResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<AITaskQueueStatusResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("costs")]
    public async Task<ActionResult<ApiResponse<AICostAnalysisResponse>>> GetCostAnalysis(
        [FromQuery] Guid? projectId = null,
        [FromQuery] DateTime? startDate = null,
        [FromQuery] DateTime? endDate = null)
    {
        try
        {
            var result = await _aiOrchestrationService.GetCostAnalysisAsync(projectId, startDate, endDate);
            return Ok(new ApiResponse<AICostAnalysisResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<AICostAnalysisResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }
}
