using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;

namespace BARQ.Core.Services;

public interface ITenantContextService
{
    Task<TenantContextDto> GetCurrentTenantContextAsync();
    Task<TenantConfigurationResponse> GetTenantConfigurationAsync(Guid tenantId);
    Task<TenantConfigurationResponse> UpdateTenantConfigurationAsync(UpdateTenantConfigurationRequest request);
    Task<TenantValidationResponse> ValidateTenantAccessAsync(Guid tenantId, Guid userId);
    Task<TenantIsolationResponse> ValidateDataIsolationAsync(Guid tenantId);
    Task<IEnumerable<TenantDto>> GetUserTenantsAsync(Guid userId);
    Task<TenantSwitchResponse> SwitchTenantContextAsync(Guid tenantId);
    Task<TenantResourceUsageDto> GetTenantResourceUsageAsync(Guid tenantId);
}
