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
public class ProjectController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    /// <summary>
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ApiResponse<ProjectDto>>> GetProject(Guid id)
    {
        try
        {
            var project = await _projectService.GetProjectAsync(id);
            return Ok(new ApiResponse<ProjectDto>
            {
                Success = true,
                Data = project,
                Message = "Project retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<ProjectDto>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ProjectResponse>>> CreateProject([FromBody] CreateProjectRequest request)
    {
        try
        {
            var result = await _projectService.CreateProjectAsync(request);
            return Ok(new ApiResponse<ProjectResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<ProjectResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpPut]
    public async Task<ActionResult<ApiResponse<ProjectResponse>>> UpdateProject([FromBody] UpdateProjectRequest request)
    {
        try
        {
            var result = await _projectService.UpdateProjectAsync(request);
            return Ok(new ApiResponse<ProjectResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<ProjectResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<ApiResponse<ProjectResponse>>> DeleteProject(Guid id)
    {
        try
        {
            var result = await _projectService.DeleteProjectAsync(id);
            return Ok(new ApiResponse<ProjectResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<ProjectResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpPost("{id:guid}/archive")]
    public async Task<ActionResult<ApiResponse<ProjectResponse>>> ArchiveProject(Guid id)
    {
        try
        {
            var result = await _projectService.ArchiveProjectAsync(id);
            return Ok(new ApiResponse<ProjectResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<ProjectResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpPost("{id:guid}/restore")]
    public async Task<ActionResult<ApiResponse<ProjectResponse>>> RestoreProject(Guid id)
    {
        try
        {
            var result = await _projectService.RestoreProjectAsync(id);
            return Ok(new ApiResponse<ProjectResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<ProjectResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("organization/{organizationId:guid}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProjectDto>>>> GetOrganizationProjects(Guid organizationId)
    {
        try
        {
            var projects = await _projectService.GetOrganizationProjectsAsync(organizationId);
            return Ok(new ApiResponse<IEnumerable<ProjectDto>>
            {
                Success = true,
                Data = projects,
                Message = "Organization projects retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<IEnumerable<ProjectDto>>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="userId">User ID</param>
    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProjectDto>>>> GetUserProjects(Guid userId)
    {
        try
        {
            var projects = await _projectService.GetUserProjectsAsync(userId);
            return Ok(new ApiResponse<IEnumerable<ProjectDto>>
            {
                Success = true,
                Data = projects,
                Message = "User projects retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<IEnumerable<ProjectDto>>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("{id:guid}/analytics")]
    public async Task<ActionResult<ApiResponse<ProjectAnalyticsResponse>>> GetProjectAnalytics(Guid id)
    {
        try
        {
            var analytics = await _projectService.GetProjectAnalyticsAsync(id);
            return Ok(new ApiResponse<ProjectAnalyticsResponse>
            {
                Success = analytics.Success,
                Data = analytics,
                Message = analytics.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<ProjectAnalyticsResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("{id:guid}/timeline")]
    public async Task<ActionResult<ApiResponse<ProjectTimelineResponse>>> GetProjectTimeline(Guid id)
    {
        try
        {
            var timeline = await _projectService.GetProjectTimelineAsync(id);
            return Ok(new ApiResponse<ProjectTimelineResponse>
            {
                Success = timeline.Success,
                Data = timeline,
                Message = timeline.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<ProjectTimelineResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpPost("members")]
    public async Task<ActionResult<ApiResponse<ProjectMemberResponse>>> AddProjectMember([FromBody] AddProjectMemberRequest request)
    {
        try
        {
            var result = await _projectService.AddProjectMemberAsync(request);
            return Ok(new ApiResponse<ProjectMemberResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<ProjectMemberResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpPut("members")]
    public async Task<ActionResult<ApiResponse<ProjectMemberResponse>>> UpdateProjectMember([FromBody] UpdateProjectMemberRequest request)
    {
        try
        {
            var result = await _projectService.UpdateProjectMemberAsync(request);
            return Ok(new ApiResponse<ProjectMemberResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<ProjectMemberResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="userId">User ID</param>
    [HttpDelete("{projectId:guid}/members/{userId:guid}")]
    public async Task<ActionResult<ApiResponse<ProjectMemberResponse>>> RemoveProjectMember(Guid projectId, Guid userId)
    {
        try
        {
            var result = await _projectService.RemoveProjectMemberAsync(projectId, userId);
            return Ok(new ApiResponse<ProjectMemberResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<ProjectMemberResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("{id:guid}/members")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProjectMemberDto>>>> GetProjectMembers(Guid id)
    {
        try
        {
            var members = await _projectService.GetProjectMembersAsync(id);
            return Ok(new ApiResponse<IEnumerable<ProjectMemberDto>>
            {
                Success = true,
                Data = members,
                Message = "Project members retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<IEnumerable<ProjectMemberDto>>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("{id:guid}/budget")]
    public async Task<ActionResult<ApiResponse<ProjectBudgetDto>>> GetProjectBudget(Guid id)
    {
        try
        {
            var budget = await _projectService.GetProjectBudgetAsync(id);
            return Ok(new ApiResponse<ProjectBudgetDto>
            {
                Success = true,
                Data = budget,
                Message = "Project budget retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<ProjectBudgetDto>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpPut("budget")]
    public async Task<ActionResult<ApiResponse<ProjectBudgetResponse>>> UpdateProjectBudget([FromBody] UpdateProjectBudgetRequest request)
    {
        try
        {
            var result = await _projectService.UpdateProjectBudgetAsync(request);
            return Ok(new ApiResponse<ProjectBudgetResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<ProjectBudgetResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("{id:guid}/risks")]
    public async Task<ActionResult<ApiResponse<IEnumerable<ProjectRiskDto>>>> GetProjectRisks(Guid id)
    {
        try
        {
            var risks = await _projectService.GetProjectRisksAsync(id);
            return Ok(new ApiResponse<IEnumerable<ProjectRiskDto>>
            {
                Success = true,
                Data = risks,
                Message = "Project risks retrieved successfully"
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<IEnumerable<ProjectRiskDto>>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpPost("risks")]
    public async Task<ActionResult<ApiResponse<ProjectRiskResponse>>> AddProjectRisk([FromBody] AddProjectRiskRequest request)
    {
        try
        {
            var result = await _projectService.AddProjectRiskAsync(request);
            return Ok(new ApiResponse<ProjectRiskResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<ProjectRiskResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpPost("{id:guid}/validate")]
    public async Task<ActionResult<ApiResponse<ProjectValidationResponse>>> ValidateProject(Guid id)
    {
        try
        {
            var result = await _projectService.ValidateProjectAsync(id);
            return Ok(new ApiResponse<ProjectValidationResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<ProjectValidationResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("{id:guid}/health")]
    public async Task<ActionResult<ApiResponse<ProjectHealthCheckResponse>>> GetProjectHealth(Guid id)
    {
        try
        {
            var result = await _projectService.GetProjectHealthAsync(id);
            return Ok(new ApiResponse<ProjectHealthCheckResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<ProjectHealthCheckResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }

    /// <summary>
    /// </summary>
    [HttpGet("{id:guid}/compliance")]
    public async Task<ActionResult<ApiResponse<ProjectComplianceResponse>>> CheckProjectCompliance(Guid id)
    {
        try
        {
            var result = await _projectService.CheckProjectComplianceAsync(id);
            return Ok(new ApiResponse<ProjectComplianceResponse>
            {
                Success = result.Success,
                Data = result,
                Message = result.Message
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new ApiResponse<ProjectComplianceResponse>
            {
                Success = false,
                Message = ex.Message
            });
        }
    }
}
