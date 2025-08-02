using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface IProjectService
{
    /// <summary>
    /// </summary>
    Task<ProjectDto> GetProjectAsync(Guid projectId);

    /// <summary>
    /// </summary>
    Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest request);

    /// <summary>
    /// </summary>
    Task<ProjectResponse> UpdateProjectAsync(UpdateProjectRequest request);

    /// <summary>
    /// </summary>
    Task<ProjectResponse> DeleteProjectAsync(Guid projectId);

    /// <summary>
    /// </summary>
    Task<ProjectResponse> ArchiveProjectAsync(Guid projectId);

    /// <summary>
    /// </summary>
    Task<ProjectResponse> RestoreProjectAsync(Guid projectId);
    
    /// <summary>
    /// </summary>
    Task<IEnumerable<ProjectDto>> GetOrganizationProjectsAsync(Guid organizationId);

    /// <summary>
    /// </summary>
    Task<IEnumerable<ProjectDto>> GetUserProjectsAsync(Guid userId);

    /// <summary>
    /// </summary>
    Task<ProjectAnalyticsResponse> GetProjectAnalyticsAsync(Guid projectId);
    /// <summary>
    /// </summary>
    Task<ProjectTimelineResponse> GetProjectTimelineAsync(Guid projectId);
    
    /// <summary>
    /// </summary>
    Task<ProjectMemberResponse> AddProjectMemberAsync(AddProjectMemberRequest request);

    /// <summary>
    /// </summary>
    Task<ProjectMemberResponse> UpdateProjectMemberAsync(UpdateProjectMemberRequest request);

    /// <summary>
    /// </summary>
    Task<ProjectMemberResponse> RemoveProjectMemberAsync(Guid projectId, Guid userId);

    /// <summary>
    /// </summary>
    Task<IEnumerable<ProjectMemberDto>> GetProjectMembersAsync(Guid projectId);
    
    /// <summary>
    /// </summary>
    Task<ProjectResourceResponse> AllocateResourceAsync(AllocateResourceRequest request);

    /// <summary>
    /// </summary>
    Task<ProjectResourceResponse> DeallocateResourceAsync(Guid projectId, Guid resourceId);

    /// <summary>
    /// </summary>
    Task<IEnumerable<ProjectResourceDto>> GetProjectResourcesAsync(Guid projectId);
    
    /// <summary>
    /// </summary>
    Task<ProjectBudgetResponse> UpdateProjectBudgetAsync(UpdateProjectBudgetRequest request);

    /// <summary>
    /// </summary>
    Task<ProjectBudgetDto> GetProjectBudgetAsync(Guid projectId);

    /// <summary>
    /// </summary>
    Task<ProjectCostAnalysisResponse> GetProjectCostAnalysisAsync(Guid projectId);
    
    /// <summary>
    /// </summary>
    Task<ProjectRiskResponse> AddProjectRiskAsync(AddProjectRiskRequest request);

    /// <summary>
    /// </summary>
    Task<ProjectRiskResponse> UpdateProjectRiskAsync(UpdateProjectRiskRequest request);

    /// <summary>
    /// </summary>
    Task<ProjectRiskResponse> RemoveProjectRiskAsync(Guid riskId);

    /// <summary>
    /// </summary>
    Task<IEnumerable<ProjectRiskDto>> GetProjectRisksAsync(Guid projectId);
    
    /// <summary>
    /// </summary>
    Task<ProjectValidationResponse> ValidateProjectAsync(Guid projectId);

    /// <summary>
    /// </summary>
    Task<ProjectHealthCheckResponse> GetProjectHealthAsync(Guid projectId);

    /// <summary>
    /// </summary>
    Task<ProjectComplianceResponse> CheckProjectComplianceAsync(Guid projectId);
}
