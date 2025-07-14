using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

public interface IProjectService
{
    Task<ProjectDto> GetProjectAsync(Guid projectId);
    Task<ProjectResponse> CreateProjectAsync(CreateProjectRequest request);
    Task<ProjectResponse> UpdateProjectAsync(UpdateProjectRequest request);
    Task<ProjectResponse> DeleteProjectAsync(Guid projectId);
    Task<ProjectResponse> ArchiveProjectAsync(Guid projectId);
    Task<ProjectResponse> RestoreProjectAsync(Guid projectId);
    
    Task<IEnumerable<ProjectDto>> GetOrganizationProjectsAsync(Guid organizationId);
    Task<IEnumerable<ProjectDto>> GetUserProjectsAsync(Guid userId);
    Task<ProjectAnalyticsResponse> GetProjectAnalyticsAsync(Guid projectId);
    Task<ProjectTimelineResponse> GetProjectTimelineAsync(Guid projectId);
    
    Task<ProjectMemberResponse> AddProjectMemberAsync(AddProjectMemberRequest request);
    Task<ProjectMemberResponse> UpdateProjectMemberAsync(UpdateProjectMemberRequest request);
    Task<ProjectMemberResponse> RemoveProjectMemberAsync(Guid projectId, Guid userId);
    Task<IEnumerable<ProjectMemberDto>> GetProjectMembersAsync(Guid projectId);
    
    Task<ProjectResourceResponse> AllocateResourceAsync(AllocateResourceRequest request);
    Task<ProjectResourceResponse> DeallocateResourceAsync(Guid projectId, Guid resourceId);
    Task<IEnumerable<ProjectResourceDto>> GetProjectResourcesAsync(Guid projectId);
    
    Task<ProjectBudgetResponse> UpdateProjectBudgetAsync(UpdateProjectBudgetRequest request);
    Task<ProjectBudgetDto> GetProjectBudgetAsync(Guid projectId);
    Task<ProjectCostAnalysisResponse> GetProjectCostAnalysisAsync(Guid projectId);
    
    Task<ProjectRiskResponse> AddProjectRiskAsync(AddProjectRiskRequest request);
    Task<ProjectRiskResponse> UpdateProjectRiskAsync(UpdateProjectRiskRequest request);
    Task<ProjectRiskResponse> RemoveProjectRiskAsync(Guid riskId);
    Task<IEnumerable<ProjectRiskDto>> GetProjectRisksAsync(Guid projectId);
    
    Task<ProjectValidationResponse> ValidateProjectAsync(Guid projectId);
    Task<ProjectHealthCheckResponse> GetProjectHealthAsync(Guid projectId);
    Task<ProjectComplianceResponse> CheckProjectComplianceAsync(Guid projectId);
}
