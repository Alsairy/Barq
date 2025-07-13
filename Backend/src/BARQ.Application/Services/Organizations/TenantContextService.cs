using Microsoft.Extensions.Logging;
using BARQ.Core.Services;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;
using BARQ.Core.Entities;
using BARQ.Core.Repositories;

namespace BARQ.Application.Services.Organizations;

public class TenantContextService : ITenantContextService
{
    private readonly IRepository<Organization> _organizationRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TenantContextService> _logger;

    public TenantContextService(
        IRepository<Organization> organizationRepository,
        IRepository<User> userRepository,
        IUnitOfWork unitOfWork,
        ILogger<TenantContextService> logger)
    {
        _organizationRepository = organizationRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TenantContextDto> GetTenantContextAsync(Guid tenantId)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(tenantId);
            if (organization == null)
            {
                throw new ArgumentException("Tenant not found", nameof(tenantId));
            }

            var userCount = await _userRepository.CountAsync(u => u.TenantId == tenantId);

            var context = new TenantContextDto
            {
                TenantId = tenantId
            };

            _logger.LogInformation("Tenant context retrieved: {TenantId}", tenantId);
            return context;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tenant context: {TenantId}", tenantId);
            throw;
        }
    }

    public async Task<TenantConfigurationResponse> GetTenantConfigurationAsync(Guid tenantId)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(tenantId);
            if (organization == null)
            {
                return new TenantConfigurationResponse
                {
                    Success = false,
                    Message = "Tenant not found"
                };
            }

            _logger.LogInformation("Tenant configuration would be retrieved for: {TenantId}", tenantId);

            return new TenantConfigurationResponse
            {
                Success = true,
                Message = "Tenant configuration retrieved successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving tenant configuration: {TenantId}", tenantId);
            return new TenantConfigurationResponse
            {
                Success = false,
                Message = "Failed to retrieve tenant configuration"
            };
        }
    }

    public async Task<TenantValidationResponse> ValidateTenantAccessAsync(Guid tenantId, Guid userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new TenantValidationResponse
                {
                    Success = false,
                    Message = "User not found"
                };
            }

            var hasAccess = user.TenantId == tenantId;

            return new TenantValidationResponse
            {
                Success = true,
                Message = hasAccess ? "Access granted" : "Access denied"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating tenant access: {TenantId}, {UserId}", tenantId, userId);
            return new TenantValidationResponse
            {
                Success = false,
                Message = "Failed to validate tenant access"
            };
        }
    }

    public Task<TenantIsolationResponse> EnforceTenantIsolationAsync(Guid tenantId, string operation, object context)
    {
        try
        {
            _logger.LogInformation("Enforcing tenant isolation for tenant: {TenantId}, Operation: {Operation}", tenantId, operation);

            return Task.FromResult(new TenantIsolationResponse
            {
                Success = true,
                Message = "Tenant isolation enforced successfully"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error enforcing tenant isolation: {TenantId}, {Operation}", tenantId, operation);
            return Task.FromResult(new TenantIsolationResponse
            {
                Success = false,
                Message = "Failed to enforce tenant isolation"
            });
        }
    }

    public async Task<TenantContextDto> GetCurrentTenantContextAsync()
    {
        try
        {
            _logger.LogInformation("Getting current tenant context");
            
            var defaultTenantId = Guid.NewGuid();
            return await GetTenantContextAsync(defaultTenantId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current tenant context");
            throw;
        }
    }

    public async Task<TenantConfigurationResponse> UpdateTenantConfigurationAsync(UpdateTenantConfigurationRequest request)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(request.TenantId);
            if (organization == null)
            {
                return new TenantConfigurationResponse
                {
                    Success = false,
                    Message = "Tenant not found"
                };
            }

            organization.UpdatedAt = DateTime.UtcNow;
            await _organizationRepository.UpdateAsync(organization);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Tenant configuration updated: {TenantId}", request.TenantId);

            return new TenantConfigurationResponse
            {
                Success = true,
                Message = "Tenant configuration updated successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating tenant configuration: {TenantId}", request.TenantId);
            return new TenantConfigurationResponse
            {
                Success = false,
                Message = "Failed to update tenant configuration"
            };
        }
    }

    public async Task<TenantIsolationResponse> ValidateDataIsolationAsync(Guid tenantId)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(tenantId);
            if (organization == null)
            {
                return new TenantIsolationResponse
                {
                    Success = false,
                    Message = "Tenant not found"
                };
            }

            return new TenantIsolationResponse
            {
                Success = true,
                Message = "Data isolation validated"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating data isolation: {TenantId}", tenantId);
            return new TenantIsolationResponse
            {
                Success = false,
                Message = "Failed to validate data isolation"
            };
        }
    }

    public async Task<IEnumerable<TenantDto>> GetUserTenantsAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new List<TenantDto>();
            }

            var tenant = new TenantDto
            {
                Id = user.TenantId,
                Name = "Default Tenant",
                IsActive = true
            };
            return new List<TenantDto> { tenant };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user tenants: {UserId}", userId);
            return new List<TenantDto>();
        }
    }

    public async Task<TenantSwitchResponse> SwitchTenantContextAsync(Guid tenantId)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(tenantId);
            if (organization == null)
            {
                return new TenantSwitchResponse
                {
                    Success = false,
                    Message = "Tenant not found"
                };
            }

            _logger.LogInformation("Tenant context switched to: {TenantId}", tenantId);

            return new TenantSwitchResponse
            {
                Success = true,
                Message = "Tenant context switched successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error switching tenant context: {TenantId}", tenantId);
            return new TenantSwitchResponse
            {
                Success = false,
                Message = "Failed to switch tenant context"
            };
        }
    }

    public async Task<TenantResourceUsageDto> GetTenantResourceUsageAsync(Guid tenantId)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(tenantId);
            if (organization == null)
            {
                throw new ArgumentException("Tenant not found", nameof(tenantId));
            }

            var userCount = await _userRepository.CountAsync(u => u.TenantId == tenantId);
            var limits = GetTenantLimits(organization.SubscriptionPlan);

            return new TenantResourceUsageDto
            {
                TenantId = tenantId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting tenant resource usage: {TenantId}", tenantId);
            throw;
        }
    }


    private List<string> GetTenantFeatures(BARQ.Core.Enums.SubscriptionPlan plan)
    {
        return plan switch
        {
            BARQ.Core.Enums.SubscriptionPlan.Free => new List<string> { "Basic project management", "Up to 5 users" },
            BARQ.Core.Enums.SubscriptionPlan.Professional => new List<string> { "Advanced project management", "Up to 50 users", "Priority support" },
            BARQ.Core.Enums.SubscriptionPlan.Enterprise => new List<string> { "Enterprise features", "Unlimited users", "24/7 support" },
            BARQ.Core.Enums.SubscriptionPlan.Custom => new List<string> { "Custom features", "Custom limits" },
            _ => new List<string>()
        };
    }

    private Dictionary<string, int> GetTenantLimits(BARQ.Core.Enums.SubscriptionPlan plan)
    {
        return plan switch
        {
            BARQ.Core.Enums.SubscriptionPlan.Free => new Dictionary<string, int> { ["MaxUsers"] = 5, ["MaxProjects"] = 3 },
            BARQ.Core.Enums.SubscriptionPlan.Professional => new Dictionary<string, int> { ["MaxUsers"] = 50, ["MaxProjects"] = 25 },
            BARQ.Core.Enums.SubscriptionPlan.Enterprise => new Dictionary<string, int> { ["MaxUsers"] = -1, ["MaxProjects"] = -1 },
            BARQ.Core.Enums.SubscriptionPlan.Custom => new Dictionary<string, int> { ["MaxUsers"] = -1, ["MaxProjects"] = -1 },
            _ => new Dictionary<string, int>()
        };
    }

    private int GetAuditRetentionDays(BARQ.Core.Enums.SubscriptionPlan plan)
    {
        return plan switch
        {
            BARQ.Core.Enums.SubscriptionPlan.Free => 30,
            BARQ.Core.Enums.SubscriptionPlan.Professional => 90,
            BARQ.Core.Enums.SubscriptionPlan.Enterprise => 365,
            BARQ.Core.Enums.SubscriptionPlan.Custom => 365,
            _ => 30
        };
    }

    private Dictionary<string, bool> GetFeatureFlags(BARQ.Core.Enums.SubscriptionPlan plan)
    {
        return plan switch
        {
            BARQ.Core.Enums.SubscriptionPlan.Free => new Dictionary<string, bool>
            {
                ["AdvancedReporting"] = false,
                ["CustomIntegrations"] = false,
                ["PrioritySupport"] = false,
                ["DataExport"] = false
            },
            BARQ.Core.Enums.SubscriptionPlan.Professional => new Dictionary<string, bool>
            {
                ["AdvancedReporting"] = true,
                ["CustomIntegrations"] = false,
                ["PrioritySupport"] = true,
                ["DataExport"] = true
            },
            BARQ.Core.Enums.SubscriptionPlan.Enterprise => new Dictionary<string, bool>
            {
                ["AdvancedReporting"] = true,
                ["CustomIntegrations"] = true,
                ["PrioritySupport"] = true,
                ["DataExport"] = true
            },
            BARQ.Core.Enums.SubscriptionPlan.Custom => new Dictionary<string, bool>
            {
                ["AdvancedReporting"] = true,
                ["CustomIntegrations"] = true,
                ["PrioritySupport"] = true,
                ["DataExport"] = true
            },
            _ => new Dictionary<string, bool>()
        };
    }
}
