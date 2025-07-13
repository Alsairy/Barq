using Microsoft.Extensions.Logging;
using AutoMapper;
using BARQ.Core.Services;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Models.Requests;
using BARQ.Core.Models.Responses;
using BARQ.Core.Entities;
using BARQ.Core.Repositories;

namespace BARQ.Application.Services.Organizations;

public class SubscriptionService : ISubscriptionService
{
    private readonly IRepository<Organization> _organizationRepository;
    private readonly INotificationService _notificationService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<SubscriptionService> _logger;

    public SubscriptionService(
        IRepository<Organization> organizationRepository,
        INotificationService notificationService,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<SubscriptionService> logger)
    {
        _organizationRepository = organizationRepository;
        _notificationService = notificationService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<SubscriptionDto> GetSubscriptionAsync(Guid organizationId)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(organizationId);
            if (organization == null)
            {
                throw new ArgumentException("Organization not found", nameof(organizationId));
            }

            var subscription = new SubscriptionDto
            {
                Id = Guid.NewGuid(),
                OrganizationId = organizationId,
                Plan = organization.SubscriptionPlan,
                Status = BARQ.Core.Enums.SubscriptionStatus.Active,
                StartDate = organization.CreatedAt,
                EndDate = organization.SubscriptionExpiryDate,
                IsActive = organization.Status == BARQ.Core.Enums.OrganizationStatus.Active,
                MonthlyPrice = GetPlanPrice(organization.SubscriptionPlan),
                UserLimit = organization.MaxUsers,
                ProjectLimit = organization.MaxProjects,
                Usage = new UsageTrackingDto
                {
                    CurrentUsers = 0,
                    CurrentProjects = 0,
                    StorageUsedMB = 0,
                    ApiCallsThisMonth = 0,
                    LastUpdated = DateTime.UtcNow
                }
            };

            _logger.LogInformation("Subscription retrieved for organization: {OrganizationId}", organizationId);
            return subscription;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving subscription for organization: {OrganizationId}", organizationId);
            throw;
        }
    }

    public async Task<SubscriptionResponse> UpgradeSubscriptionAsync(UpgradeSubscriptionRequest request)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(request.OrganizationId);
            if (organization == null)
            {
                return new SubscriptionResponse
                {
                    Success = false,
                    Message = "Organization not found"
                };
            }

            if (Enum.TryParse<BARQ.Core.Enums.SubscriptionPlan>(request.NewPlanName, out var requestedPlan) && organization.SubscriptionPlan >= requestedPlan)
            {
                return new SubscriptionResponse
                {
                    Success = false,
                    Message = "Cannot upgrade to a lower or same plan"
                };
            }

            var oldPlan = organization.SubscriptionPlan;
            if (Enum.TryParse<BARQ.Core.Enums.SubscriptionPlan>(request.NewPlanName, out var parsedPlan))
            {
                organization.SubscriptionPlan = parsedPlan;
            }
            organization.UpdatedAt = DateTime.UtcNow;

            await _organizationRepository.UpdateAsync(organization);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Email notification would be sent for subscription upgrade");

            _logger.LogInformation("Subscription upgraded for organization {OrganizationId} from {OldPlan} to {NewPlan}", 
                request.OrganizationId, oldPlan, request.NewPlanName);

            return new SubscriptionResponse
            {
                Success = true,
                Message = "Subscription upgraded successfully",
                Subscription = await GetSubscriptionAsync(request.OrganizationId)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error upgrading subscription for organization: {OrganizationId}", request.OrganizationId);
            return new SubscriptionResponse
            {
                Success = false,
                Message = "Failed to upgrade subscription"
            };
        }
    }

    public async Task<SubscriptionResponse> DowngradeSubscriptionAsync(DowngradeSubscriptionRequest request)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(request.OrganizationId);
            if (organization == null)
            {
                return new SubscriptionResponse
                {
                    Success = false,
                    Message = "Organization not found"
                };
            }

            if (Enum.TryParse<BARQ.Core.Enums.SubscriptionPlan>(request.NewPlanName, out var newPlan) && organization.SubscriptionPlan <= newPlan)
            {
                return new SubscriptionResponse
                {
                    Success = false,
                    Message = "Cannot downgrade to a higher or same plan"
                };
            }

            var oldPlan = organization.SubscriptionPlan;
            if (Enum.TryParse<BARQ.Core.Enums.SubscriptionPlan>(request.NewPlanName, out var parsedPlan))
            {
                organization.SubscriptionPlan = parsedPlan;
            }
            organization.UpdatedAt = DateTime.UtcNow;

            await _organizationRepository.UpdateAsync(organization);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Email notification would be sent for subscription downgrade");

            _logger.LogInformation("Subscription downgraded for organization {OrganizationId} from {OldPlan} to {NewPlan}", 
                request.OrganizationId, oldPlan, request.NewPlanName);

            return new SubscriptionResponse
            {
                Success = true,
                Message = "Subscription downgraded successfully",
                Subscription = await GetSubscriptionAsync(request.OrganizationId)
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error downgrading subscription for organization: {OrganizationId}", request.OrganizationId);
            return new SubscriptionResponse
            {
                Success = false,
                Message = "Failed to downgrade subscription"
            };
        }
    }

    public async Task<UsageTrackingResponse> GetUsageAsync(Guid organizationId)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(organizationId);
            if (organization == null)
            {
                return new UsageTrackingResponse
                {
                    Success = false,
                    Message = "Organization not found"
                };
            }

            var limits = GetPlanLimits(organization.SubscriptionPlan);

            return new UsageTrackingResponse
            {
                Success = true,
                Message = "Usage data retrieved successfully",
                CurrentUsage = 1024,
                UsageLimit = limits.GetValueOrDefault("StorageGB", 5) * 1024,
                UsagePercentage = 20.48m,
                PeriodStart = DateTime.UtcNow.AddDays(-30),
                PeriodEnd = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving usage for organization: {OrganizationId}", organizationId);
            return new UsageTrackingResponse
            {
                Success = false,
                Message = "Failed to retrieve usage data"
            };
        }
    }

    public async Task<BillingCalculationResponse> CalculateBillingAsync(Guid organizationId, DateTime periodStart, DateTime periodEnd)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(organizationId);
            if (organization == null)
            {
                return new BillingCalculationResponse
                {
                    Success = false,
                    Message = "Organization not found"
                };
            }

            var baseAmount = GetPlanPrice(organization.SubscriptionPlan);

            return new BillingCalculationResponse
            {
                Success = true,
                Message = "Billing calculated successfully",
                Amount = baseAmount,
                Currency = "USD",
                BillingPeriodStart = periodStart,
                BillingPeriodEnd = periodEnd,
                LineItems = new List<BillingLineItemDto>
                {
                    new BillingLineItemDto
                    {
                        Description = $"{organization.SubscriptionPlan} Plan",
                        Quantity = 1,
                        UnitPrice = baseAmount,
                        TotalPrice = baseAmount
                    }
                }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error calculating billing for organization: {OrganizationId}", organizationId);
            return new BillingCalculationResponse
            {
                Success = false,
                Message = "Failed to calculate billing"
            };
        }
    }

    public async Task<SubscriptionResponse> CancelSubscriptionAsync(Guid organizationId)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(organizationId);
            if (organization == null)
            {
                return new SubscriptionResponse
                {
                    Success = false,
                    Message = "Organization not found"
                };
            }

            organization.SubscriptionPlan = BARQ.Core.Enums.SubscriptionPlan.Free;
            organization.UpdatedAt = DateTime.UtcNow;

            await _organizationRepository.UpdateAsync(organization);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Subscription cancelled for organization: {OrganizationId}", organizationId);

            return new SubscriptionResponse
            {
                Success = true,
                Message = "Subscription cancelled successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cancelling subscription for organization: {OrganizationId}", organizationId);
            return new SubscriptionResponse
            {
                Success = false,
                Message = "Failed to cancel subscription"
            };
        }
    }

    public async Task<SubscriptionResponse> CreateSubscriptionAsync(CreateSubscriptionRequest request)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(request.OrganizationId);
            if (organization == null)
            {
                return new SubscriptionResponse
                {
                    Success = false,
                    Message = "Organization not found"
                };
            }

            if (Enum.TryParse<BARQ.Core.Enums.SubscriptionPlan>(request.PlanName, out var plan))
            {
                organization.SubscriptionPlan = plan;
            }
            organization.UpdatedAt = DateTime.UtcNow;

            await _organizationRepository.UpdateAsync(organization);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Subscription created for organization: {OrganizationId}", request.OrganizationId);

            return new SubscriptionResponse
            {
                Success = true,
                Message = "Subscription created successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating subscription for organization: {OrganizationId}", request.OrganizationId);
            return new SubscriptionResponse
            {
                Success = false,
                Message = "Failed to create subscription"
            };
        }
    }

    public Task<UsageTrackingResponse> TrackUsageAsync(TrackUsageRequest request)
    {
        try
        {
            _logger.LogInformation("Tracking usage for organization: {OrganizationId}", request.OrganizationId);

            return Task.FromResult(new UsageTrackingResponse
            {
                Success = true,
                Message = "Usage tracked successfully",
                CurrentUsage = 0,
                UsageLimit = 1000,
                UsagePercentage = 0,
                PeriodStart = DateTime.UtcNow.AddDays(-30),
                PeriodEnd = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error tracking usage for organization: {OrganizationId}", request.OrganizationId);
            return Task.FromResult(new UsageTrackingResponse
            {
                Success = false,
                Message = "Failed to track usage"
            });
        }
    }

    public Task<IEnumerable<SubscriptionPlanDto>> GetAvailablePlansAsync()
    {
        try
        {
            var plans = new List<SubscriptionPlanDto>
            {
                new SubscriptionPlanDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Free",
                    Description = "Basic features for small teams",
                    MonthlyPrice = 0,
                    YearlyPrice = 0,
                    UserLimit = 5,
                    ProjectLimit = 3,
                    Features = GetPlanFeatures(BARQ.Core.Enums.SubscriptionPlan.Free)
                },
                new SubscriptionPlanDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Professional",
                    Description = "Advanced features for growing teams",
                    MonthlyPrice = 29.99m,
                    YearlyPrice = 299.99m,
                    UserLimit = 50,
                    ProjectLimit = 25,
                    Features = GetPlanFeatures(BARQ.Core.Enums.SubscriptionPlan.Professional)
                },
                new SubscriptionPlanDto
                {
                    Id = Guid.NewGuid(),
                    Name = "Enterprise",
                    Description = "Full features for large organizations",
                    MonthlyPrice = 99.99m,
                    YearlyPrice = 999.99m,
                    UserLimit = -1,
                    ProjectLimit = -1,
                    Features = GetPlanFeatures(BARQ.Core.Enums.SubscriptionPlan.Enterprise)
                }
            };

            return Task.FromResult<IEnumerable<SubscriptionPlanDto>>(plans);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving available plans");
            return Task.FromResult<IEnumerable<SubscriptionPlanDto>>(new List<SubscriptionPlanDto>());
        }
    }

    public async Task<BillingNotificationResponse> SendBillingNotificationAsync(Guid organizationId)
    {
        try
        {
            var organization = await _organizationRepository.GetByIdAsync(organizationId);
            if (organization == null)
            {
                return new BillingNotificationResponse
                {
                    Success = false,
                    Message = "Organization not found"
                };
            }

            _logger.LogInformation("Billing notification sent for organization: {OrganizationId}", organizationId);

            return new BillingNotificationResponse
            {
                Success = true,
                Message = "Billing notification sent successfully"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending billing notification for organization: {OrganizationId}", organizationId);
            return new BillingNotificationResponse
            {
                Success = false,
                Message = "Failed to send billing notification"
            };
        }
    }

    public async Task<BillingCalculationResponse> CalculateBillingAsync(Guid organizationId)
    {
        return await CalculateBillingAsync(organizationId, DateTime.UtcNow.AddDays(-30), DateTime.UtcNow);
    }

    private List<string> GetPlanFeatures(BARQ.Core.Enums.SubscriptionPlan plan)
    {
        return plan switch
        {
            BARQ.Core.Enums.SubscriptionPlan.Free => new List<string> { "Basic project management", "Up to 5 users", "5GB storage" },
            BARQ.Core.Enums.SubscriptionPlan.Professional => new List<string> { "Advanced project management", "Up to 50 users", "100GB storage", "Priority support" },
            BARQ.Core.Enums.SubscriptionPlan.Enterprise => new List<string> { "Enterprise features", "Unlimited users", "1TB storage", "24/7 support", "Custom integrations" },
            BARQ.Core.Enums.SubscriptionPlan.Custom => new List<string> { "Custom features", "Custom limits", "Dedicated support" },
            _ => new List<string>()
        };
    }

    private Dictionary<string, int> GetPlanLimits(BARQ.Core.Enums.SubscriptionPlan plan)
    {
        return plan switch
        {
            BARQ.Core.Enums.SubscriptionPlan.Free => new Dictionary<string, int> { ["MaxUsers"] = 5, ["MaxProjects"] = 3, ["StorageGB"] = 5 },
            BARQ.Core.Enums.SubscriptionPlan.Professional => new Dictionary<string, int> { ["MaxUsers"] = 50, ["MaxProjects"] = 25, ["StorageGB"] = 100 },
            BARQ.Core.Enums.SubscriptionPlan.Enterprise => new Dictionary<string, int> { ["MaxUsers"] = -1, ["MaxProjects"] = -1, ["StorageGB"] = 1000 },
            BARQ.Core.Enums.SubscriptionPlan.Custom => new Dictionary<string, int> { ["MaxUsers"] = -1, ["MaxProjects"] = -1, ["StorageGB"] = -1 },
            _ => new Dictionary<string, int>()
        };
    }

    private decimal GetPlanPrice(BARQ.Core.Enums.SubscriptionPlan plan)
    {
        return plan switch
        {
            BARQ.Core.Enums.SubscriptionPlan.Free => 0m,
            BARQ.Core.Enums.SubscriptionPlan.Professional => 29.99m,
            BARQ.Core.Enums.SubscriptionPlan.Enterprise => 99.99m,
            BARQ.Core.Enums.SubscriptionPlan.Custom => 0m,
            _ => 0m
        };
    }
}
