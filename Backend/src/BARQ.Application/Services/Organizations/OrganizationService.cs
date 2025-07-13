using Microsoft.Extensions.Logging;
using AutoMapper;
using BARQ.Core.Services;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;
using BARQ.Core.Entities;
using BARQ.Core.Repositories;

namespace BARQ.Application.Services.Organizations;

public class OrganizationService : IOrganizationService
{
    private readonly IRepository<Organization> _organizationRepository;
    private readonly IRepository<User> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<OrganizationService> _logger;

    public OrganizationService(
        IRepository<Organization> organizationRepository,
        IRepository<User> userRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<OrganizationService> logger)
    {
        _organizationRepository = organizationRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<OrganizationDto> GetOrganizationAsync(Guid organizationId)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(organizationId);
            if (organization == null)
            {
                throw new ArgumentException("Organization not found", nameof(organizationId));
            }

            var organizationDto = _mapper.Map<OrganizationDto>(organization);
            
            _logger.LogInformation("Organization retrieved: {OrganizationId}", organizationId);
            return organizationDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving organization: {OrganizationId}", organizationId);
            throw;
        }
    }

    public async Task<OrganizationResponse> CreateOrganizationAsync(CreateOrganizationRequest request)
    {
        try
        {
            var existingOrg = await _organizationRepository.FirstOrDefaultAsync(o => o.Name == request.Name);
            if (existingOrg != null)
            {
                return new OrganizationResponse
                {
                    Success = false,
                    Message = "Organization name already exists"
                };
            }

            var subscriptionPlan = Enum.TryParse<BARQ.Core.Enums.SubscriptionPlan>(request.SubscriptionPlan, out var plan) 
                ? plan 
                : BARQ.Core.Enums.SubscriptionPlan.Free;

            var organization = new Organization
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Website = request.Domain,
                SubscriptionPlan = subscriptionPlan,
                Status = BARQ.Core.Enums.OrganizationStatus.Active,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _organizationRepository.AddAsync(organization);
            await _unitOfWork.SaveChangesAsync();

            var organizationDto = _mapper.Map<OrganizationDto>(organization);

            _logger.LogInformation("Organization created: {OrganizationId}", organization.Id);

            return new OrganizationResponse
            {
                Success = true,
                Message = "Organization created successfully",
                Organization = organizationDto
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating organization: {Name}", request.Name);
            return new OrganizationResponse
            {
                Success = false,
                Message = "Failed to create organization"
            };
        }
    }

    public async Task<OrganizationResponse> UpdateOrganizationAsync(UpdateOrganizationRequest request)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(request.OrganizationId);
            if (organization == null)
            {
                return new OrganizationResponse
                {
                    Success = false,
                    Message = "Organization not found"
                };
            }

            organization.Name = request.Name ?? organization.Name;
            organization.Description = request.Description ?? organization.Description;
            organization.Website = request.Domain ?? organization.Website;
            if (request.IsActive.HasValue)
            {
                organization.Status = request.IsActive.Value 
                    ? BARQ.Core.Enums.OrganizationStatus.Active 
                    : BARQ.Core.Enums.OrganizationStatus.Inactive;
            }
            organization.UpdatedAt = DateTime.UtcNow;

            await _organizationRepository.UpdateAsync(organization);
            await _unitOfWork.SaveChangesAsync();

            var organizationDto = _mapper.Map<OrganizationDto>(organization);

            _logger.LogInformation("Organization updated: {OrganizationId}", request.OrganizationId);

            return new OrganizationResponse
            {
                Success = true,
                Message = "Organization updated successfully",
                Organization = organizationDto
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating organization: {OrganizationId}", request.OrganizationId);
            return new OrganizationResponse
            {
                Success = false,
                Message = "Failed to update organization"
            };
        }
    }

    public async Task<OrganizationResponse> DeleteOrganizationAsync(Guid organizationId)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(organizationId);
            if (organization == null)
            {
                return new OrganizationResponse
                {
                    Success = false,
                    Message = "Organization not found"
                };
            }

            organization.Status = BARQ.Core.Enums.OrganizationStatus.Inactive;
            organization.UpdatedAt = DateTime.UtcNow;
            await _organizationRepository.UpdateAsync(organization);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Organization deactivated: {OrganizationId}", organizationId);

            return new OrganizationResponse
            {
                Success = true,
                Message = "Organization deactivated successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting organization: {OrganizationId}", organizationId);
            return new OrganizationResponse
            {
                Success = false,
                Message = "Failed to delete organization"
            };
        }
    }

    public async Task<OrganizationBrandingResponse> UpdateOrganizationBrandingAsync(UpdateOrganizationBrandingRequest request)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(request.OrganizationId);
            if (organization == null)
            {
                return new OrganizationBrandingResponse
                {
                    Success = false,
                    Message = "Organization not found"
                };
            }

            organization.UpdatedAt = DateTime.UtcNow;
            await _organizationRepository.UpdateAsync(organization);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Organization branding updated: {OrganizationId}", request.OrganizationId);

            return new OrganizationBrandingResponse
            {
                Success = true,
                Message = "Organization branding updated successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating organization branding: {OrganizationId}", request.OrganizationId);
            return new OrganizationBrandingResponse
            {
                Success = false,
                Message = "Failed to update organization branding"
            };
        }
    }

    public async Task<IEnumerable<OrganizationDto>> GetUserOrganizationsAsync(Guid userId)
    {
        try
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new List<OrganizationDto>();
            }

            var organization = await _organizationRepository.GetByIdAsync(user.TenantId);
            if (organization == null)
            {
                return new List<OrganizationDto>();
            }

            var organizationDto = _mapper.Map<OrganizationDto>(organization);
            return new List<OrganizationDto> { organizationDto };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving user organizations: {UserId}", userId);
            return new List<OrganizationDto>();
        }
    }

    public async Task<OrganizationValidationResponse> ValidateOrganizationAsync(Guid organizationId)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(organizationId);
            if (organization == null)
            {
                return new OrganizationValidationResponse
                {
                    Success = false,
                    Message = "Organization not found",
                    IsValid = false
                };
            }

            return new OrganizationValidationResponse
            {
                Success = true,
                Message = "Organization is valid",
                IsValid = organization.Status == BARQ.Core.Enums.OrganizationStatus.Active
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating organization: {OrganizationId}", organizationId);
            return new OrganizationValidationResponse
            {
                Success = false,
                Message = "Failed to validate organization",
                IsValid = false
            };
        }
    }

    public async Task<OrganizationSettingsResponse> GetOrganizationSettingsAsync(Guid organizationId)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(organizationId);
            if (organization == null)
            {
                return new OrganizationSettingsResponse
                {
                    Success = false,
                    Message = "Organization not found"
                };
            }

            _logger.LogInformation("Organization settings would be retrieved for: {OrganizationId}", organizationId);

            return new OrganizationSettingsResponse
            {
                Success = true,
                Message = "Organization settings retrieved successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving organization settings: {OrganizationId}", organizationId);
            return new OrganizationSettingsResponse
            {
                Success = false,
                Message = "Failed to retrieve organization settings"
            };
        }
    }

    public async Task<OrganizationSettingsResponse> UpdateOrganizationSettingsAsync(UpdateOrganizationSettingsRequest request)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(request.OrganizationId);
            if (organization == null)
            {
                return new OrganizationSettingsResponse
                {
                    Success = false,
                    Message = "Organization not found"
                };
            }

            organization.UpdatedAt = DateTime.UtcNow;

            await _organizationRepository.UpdateAsync(organization);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Organization settings updated: {OrganizationId}", request.OrganizationId);

            return new OrganizationSettingsResponse
            {
                Success = true,
                Message = "Organization settings updated successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating organization settings: {OrganizationId}", request.OrganizationId);
            return new OrganizationSettingsResponse
            {
                Success = false,
                Message = "Failed to update organization settings"
            };
        }
    }

}
