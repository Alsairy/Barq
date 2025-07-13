using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

public interface IOrganizationService
{
    Task<OrganizationDto> GetOrganizationAsync(Guid organizationId);
    Task<OrganizationResponse> CreateOrganizationAsync(CreateOrganizationRequest request);
    Task<OrganizationResponse> UpdateOrganizationAsync(UpdateOrganizationRequest request);
    Task<OrganizationResponse> DeleteOrganizationAsync(Guid organizationId);
    Task<OrganizationSettingsResponse> UpdateOrganizationSettingsAsync(UpdateOrganizationSettingsRequest request);
    Task<OrganizationBrandingResponse> UpdateOrganizationBrandingAsync(UpdateOrganizationBrandingRequest request);
    Task<IEnumerable<OrganizationDto>> GetUserOrganizationsAsync(Guid userId);
    Task<OrganizationValidationResponse> ValidateOrganizationAsync(Guid organizationId);
}
