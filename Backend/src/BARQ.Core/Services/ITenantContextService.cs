using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface ITenantContextService
{
    /// <summary>
    /// </summary>
    Task<TenantContextDto> GetCurrentTenantContextAsync();

    /// <summary>
    /// </summary>
    Task<TenantConfigurationResponse> GetTenantConfigurationAsync(Guid tenantId);

    /// <summary>
    /// </summary>
    Task<TenantConfigurationResponse> UpdateTenantConfigurationAsync(UpdateTenantConfigurationRequest request);

    /// <summary>
    /// </summary>
    Task<TenantValidationResponse> ValidateTenantAccessAsync(Guid tenantId, Guid userId);

    /// <summary>
    /// </summary>
    Task<TenantIsolationResponse> ValidateDataIsolationAsync(Guid tenantId);

    /// <summary>
    /// </summary>
    Task<IEnumerable<TenantDto>> GetUserTenantsAsync(Guid userId);

    /// <summary>
    /// </summary>
    Task<TenantSwitchResponse> SwitchTenantContextAsync(Guid tenantId);

    /// <summary>
    /// </summary>
    Task<TenantResourceUsageDto> GetTenantResourceUsageAsync(Guid tenantId);
}
