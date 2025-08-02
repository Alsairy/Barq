using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface IOrganizationService
{
    /// <summary>
    /// </summary>
    Task<OrganizationDto> GetOrganizationAsync(Guid organizationId);

    /// <summary>
    /// </summary>
    Task<OrganizationResponse> CreateOrganizationAsync(CreateOrganizationRequest request);

    /// <summary>
    /// </summary>
    Task<OrganizationResponse> UpdateOrganizationAsync(UpdateOrganizationRequest request);

    /// <summary>
    /// </summary>
    Task<OrganizationResponse> DeleteOrganizationAsync(Guid organizationId);

    /// <summary>
    /// </summary>
    Task<OrganizationSettingsResponse> UpdateOrganizationSettingsAsync(UpdateOrganizationSettingsRequest request);

    /// <summary>
    /// </summary>
    Task<OrganizationBrandingResponse> UpdateOrganizationBrandingAsync(UpdateOrganizationBrandingRequest request);

    /// <summary>
    /// </summary>
    Task<IEnumerable<OrganizationDto>> GetUserOrganizationsAsync(Guid userId);

    /// <summary>
    /// </summary>
    Task<OrganizationValidationResponse> ValidateOrganizationAsync(Guid organizationId);
}
