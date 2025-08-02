using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BARQ.Core.Entities;
using BARQ.Core.Enums;
using BARQ.Core.Repositories;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace BARQ.Infrastructure.BPM
{
    /// <summary>
    /// </summary>
    public interface IDelegationService
    {
        /// <summary>
        /// </summary>
        Task<Delegation> CreateDelegationAsync(Delegation delegation);
        
        /// <summary>
        /// </summary>
        Task<List<Delegation>> GetActiveDelegationsAsync(string userId);
        
        /// <summary>
        /// </summary>
        Task<DelegationValidationResult> ValidateDelegationAsync(Delegation delegation);
        
        /// <summary>
        /// </summary>
        Task<bool> ExpireDelegationAsync(string delegationId);
        
        /// <summary>
        /// </summary>
        Task<List<DelegationChain>> GetDelegationChainAsync(string userId);
    }

    /// <summary>
    /// </summary>
    public class DelegationService : IDelegationService
    {
        private readonly ILogger<DelegationService> _logger;
        private readonly IConfiguration _configuration;
        private readonly IRepository<AIRequestApproval> _approvalRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// </summary>
        public DelegationService(
            ILogger<DelegationService> logger,
            IConfiguration configuration,
            IRepository<AIRequestApproval> approvalRepository,
            IUnitOfWork unitOfWork)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _approvalRepository = approvalRepository ?? throw new ArgumentNullException(nameof(approvalRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Delegation> CreateDelegationAsync(Delegation delegation)
        {
            try
            {
                _logger.LogInformation("Creating delegation from {DelegatorId} to {DelegateId} for type {DelegationType}", 
                    delegation.DelegatorId, delegation.DelegateId, delegation.DelegationType);

                var validationResult = await ValidateDelegationAsync(delegation);
                if (!validationResult.IsValid)
                {
                    throw new InvalidOperationException($"Delegation validation failed: {validationResult.ErrorMessage}");
                }

                delegation.Id = Guid.NewGuid().ToString();
                delegation.CreatedAt = DateTime.UtcNow;
                delegation.Status = DelegationStatus.Active;

                _logger.LogInformation("Delegation created with ID {DelegationId}", delegation.Id);
                
                var approval = new AIRequestApproval
                {
                    Id = Guid.NewGuid(),
                    AIRequestId = Guid.Parse(delegation.Id),
                    ApproverId = Guid.Parse(delegation.DelegateId),
                    DelegatedFromId = Guid.Parse(delegation.DelegatorId),
                    DelegatedAt = DateTime.UtcNow,
                    DelegationReason = delegation.Reason,
                    Status = ApprovalStatus.Pending,
                    CreatedAt = DateTime.UtcNow
                };

                await _approvalRepository.AddAsync(approval);
                await _unitOfWork.SaveChangesAsync();

                return delegation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating delegation from {DelegatorId} to {DelegateId}", 
                    delegation.DelegatorId, delegation.DelegateId);
                throw;
            }
        }

        public async Task<List<Delegation>> GetActiveDelegationsAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Getting active delegations for user {UserId}", userId);

                var approvals = await _approvalRepository.FindAsync(a => 
                    a.DelegatedFromId.ToString() == userId && 
                    a.Status == ApprovalStatus.Pending &&
                    a.DelegatedAt.HasValue);

                var delegations = approvals.Select(a => new Delegation
                {
                    Id = a.Id.ToString(),
                    DelegatorId = a.DelegatedFromId?.ToString() ?? string.Empty,
                    DelegateId = a.ApproverId.ToString(),
                    DelegationType = DelegationType.ApprovalAuthority,
                    Scope = "AI Request Approval",
                    StartDate = a.DelegatedAt ?? DateTime.UtcNow,
                    EndDate = a.DelegatedAt?.AddDays(30) ?? DateTime.UtcNow.AddDays(30),
                    Status = DelegationStatus.Active,
                    Reason = a.DelegationReason ?? string.Empty,
                    CreatedAt = a.CreatedAt,
                    CreatedBy = a.DelegatedFromId?.ToString() ?? string.Empty
                }).ToList();

                return delegations;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active delegations for user {UserId}", userId);
                throw;
            }
        }

        public async Task<DelegationValidationResult> ValidateDelegationAsync(Delegation delegation)
        {
            try
            {
                _logger.LogInformation("Validating delegation from {DelegatorId} to {DelegateId}", 
                    delegation.DelegatorId, delegation.DelegateId);

                var result = new DelegationValidationResult { IsValid = true };

                if (delegation.StartDate >= delegation.EndDate)
                {
                    result.IsValid = false;
                    result.ErrorMessage = "Delegation end date must be after start date";
                    return result;
                }

                if (delegation.EndDate <= DateTime.UtcNow)
                {
                    result.IsValid = false;
                    result.ErrorMessage = "Delegation end date cannot be in the past";
                    return result;
                }

                var maxDelegationDays = _configuration.GetValue<int>("Delegation:MaxDurationDays", 30);
                if ((delegation.EndDate - delegation.StartDate).TotalDays > maxDelegationDays)
                {
                    result.IsValid = false;
                    result.ErrorMessage = $"Delegation duration cannot exceed {maxDelegationDays} days";
                    return result;
                }

                if (delegation.DelegatorId == delegation.DelegateId)
                {
                    result.IsValid = false;
                    result.ErrorMessage = "Cannot delegate to yourself";
                    return result;
                }


                _logger.LogInformation("Delegation validation completed: {IsValid}", result.IsValid);
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating delegation from {DelegatorId} to {DelegateId}", 
                    delegation.DelegatorId, delegation.DelegateId);
                throw;
            }
        }

        public async Task<bool> ExpireDelegationAsync(string delegationId)
        {
            try
            {
                _logger.LogInformation("Expiring delegation with ID {DelegationId}", delegationId);


                _logger.LogInformation("Delegation expired with ID {DelegationId}", delegationId);
                
                var approval = await _approvalRepository.FirstOrDefaultAsync(a => a.Id.ToString() == delegationId);
                if (approval != null)
                {
                    approval.Status = ApprovalStatus.Expired;
                    approval.UpdatedAt = DateTime.UtcNow;
                    await _approvalRepository.UpdateAsync(approval);
                    await _unitOfWork.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error expiring delegation with ID {DelegationId}", delegationId);
                return false;
            }
        }

        public async Task<List<DelegationChain>> GetDelegationChainAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Getting delegation chain for user {UserId}", userId);


                var userApprovals = await _approvalRepository.FindAsync(a => 
                    a.ApproverId.ToString() == userId || a.DelegatedFromId.ToString() == userId);

                var chains = new List<DelegationChain>();
                
                var delegationChain = new DelegationChain
                {
                    UserId = userId,
                    DelegationPath = userApprovals.Where(a => a.DelegatedFromId != null)
                                                 .Select(a => a.DelegatedFromId!.ToString())
                                                 .Distinct()
                                                 .ToList(),
                    ChainLength = userApprovals.Count(a => a.DelegatedFromId != null),
                    HasCycle = false,
                    EffectiveAuthority = "Approval Authority"
                };

                chains.Add(delegationChain);

                return chains;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting delegation chain for user {UserId}", userId);
                throw;
            }
        }
    }

    /// <summary>
    /// </summary>
    public class Delegation
    {
        public string Id { get; set; } = string.Empty;
        public string DelegatorId { get; set; } = string.Empty;
        public string DelegateId { get; set; } = string.Empty;
        public DelegationType DelegationType { get; set; }
        public string Scope { get; set; } = string.Empty; // specific processes, all processes, etc.
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DelegationStatus Status { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }

    /// <summary>
    /// </summary>
    public enum DelegationType
    {
        Task,
        ApprovalAuthority,
        ProcessOwnership,
        Emergency
    }

    /// <summary>
    /// </summary>
    public enum DelegationStatus
    {
        Active,
        Expired,
        Revoked,
        Pending
    }

    /// <summary>
    /// </summary>
    public class DelegationValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public List<string> Warnings { get; set; } = new();
    }

    /// <summary>
    /// </summary>
    public class DelegationChain
    {
        public string UserId { get; set; } = string.Empty;
        public List<string> DelegationPath { get; set; } = new();
        public int ChainLength { get; set; }
        public bool HasCycle { get; set; }
        public string EffectiveAuthority { get; set; } = string.Empty;
    }
}
