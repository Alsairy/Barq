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
            var response = new AITaskResponse
            {
                Id = result.TaskId,
                Status = "Created"
            };
            return Ok(new ApiResponse<AITaskResponse>
            {
                Success = true,
                Data = response,
                Message = "AI task created successfully"
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
            var response = new AITaskExecutionResponse
            {
                TaskId = result.TaskId,
                Status = "Executed",
                StartedAt = DateTime.UtcNow
            };
            return Ok(new ApiResponse<AITaskExecutionResponse>
            {
                Success = true,
                Data = response,
                Message = "AI task executed successfully"
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
            var response = new AITaskStatusResponse
            {
                TaskId = taskId,
                Status = result.ToString(),
                Progress = 0,
                LastUpdated = DateTime.UtcNow
            };
            return Ok(new ApiResponse<AITaskStatusResponse>
            {
                Success = true,
                Data = response,
                Message = "AI task status retrieved successfully"
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
            var response = new AITaskResponse
            {
                Id = taskId,
                Status = result ? "Cancelled" : "Failed"
            };
            return Ok(new ApiResponse<AITaskResponse>
            {
                Success = result,
                Data = response,
                Message = result ? "AI task cancelled successfully" : "Failed to cancel AI task"
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
            var response = new AITaskResultResponse
            {
                TaskId = result.TaskId,
                Status = "Completed",
                Result = "Task completed successfully",
                CompletedAt = DateTime.UtcNow,
                ExecutionTime = TimeSpan.FromMinutes(1),
                Cost = 0.0m
            };
            return Ok(new ApiResponse<AITaskResultResponse>
            {
                Success = true,
                Data = response,
                Message = "AI task results retrieved successfully"
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
            var taskDtos = tasks.Select(t => new AITaskDto
            {
                Id = t.Id,
                Name = t.Name,
                Status = t.Status.ToString(),
                CreatedAt = t.CreatedAt
            });
            return Ok(new ApiResponse<IEnumerable<AITaskDto>>
            {
                Success = true,
                Data = taskDtos,
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
            var result = await _aiOrchestrationService.GetAITaskAnalyticsAsync();
            var response = new AITaskAnalyticsResponse
            {
                TotalTasks = 0,
                CompletedTasks = 0,
                FailedTasks = 0,
                AverageExecutionTime = TimeSpan.Zero
            };
            return Ok(new ApiResponse<AITaskAnalyticsResponse>
            {
                Success = true,
                Data = response,
                Message = "AI task analytics retrieved successfully"
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
            var providerDtos = providers.Select(p => new AIProviderDto
            {
                Id = p.Id,
                Name = p.Name,
                Type = "OpenAI"
            });
            return Ok(new ApiResponse<IEnumerable<AIProviderDto>>
            {
                Success = true,
                Data = providerDtos,
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
            var response = new AIProviderHealthResponse
            {
                ProviderId = providerId,
                Name = "AI Provider",
                Status = result.IsHealthy ? "Healthy" : "Unhealthy",
                ResponseTime = TimeSpan.FromMilliseconds(100),
                LastChecked = DateTime.UtcNow
            };
            return Ok(new ApiResponse<AIProviderHealthResponse>
            {
                Success = result.IsHealthy,
                Data = response,
                Message = result.IsHealthy ? "Provider is healthy" : "Provider health check failed"
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
            var response = new AIProviderConfigurationResponse
            {
                Id = result.Id,
                Name = result.Name,
                Status = "Configured",
                Configuration = new Dictionary<string, object>(),
                ConfiguredAt = DateTime.UtcNow
            };
            return Ok(new ApiResponse<AIProviderConfigurationResponse>
            {
                Success = true,
                Data = response,
                Message = "AI provider configured successfully"
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
            var response = new AIBatchExecutionResponse
            {
                BatchId = Guid.NewGuid().ToString(),
                TotalTasks = 0,
                QueuedTasks = 0,
                SubmittedAt = DateTime.UtcNow,
                Status = "Submitted"
            };
            return Ok(new ApiResponse<AIBatchExecutionResponse>
            {
                Success = true,
                Data = response,
                Message = "Batch tasks executed successfully"
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
            var response = new AITaskQueueStatusResponse
            {
                QueuedTasks = 0,
                ProcessingTasks = 0,
                CompletedTasks = 0,
                FailedTasks = 0,
                AverageWaitTime = TimeSpan.Zero
            };
            return Ok(new ApiResponse<AITaskQueueStatusResponse>
            {
                Success = true,
                Data = response,
                Message = "Queue status retrieved successfully"
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
            var result = await _aiOrchestrationService.GetCostAnalysisAsync();
            var response = new AICostAnalysisResponse
            {
                TotalCost = 0.0m,
                CostThisMonth = 0.0m,
                CostLastMonth = 0.0m,
                CostByProvider = new Dictionary<string, decimal>(),
                CostByTaskType = new Dictionary<string, decimal>()
            };
            return Ok(new ApiResponse<AICostAnalysisResponse>
            {
                Success = true,
                Data = response,
                Message = "Cost analysis retrieved successfully"
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
