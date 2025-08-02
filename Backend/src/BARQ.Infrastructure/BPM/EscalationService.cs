using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BARQ.Core.Entities;
using BARQ.Core.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace BARQ.Infrastructure.BPM
{
    /// <summary>
    /// </summary>
    public interface IEscalationService
    {
        /// <summary>
        /// </summary>
        Task<EscalationRule> CreateEscalationRuleAsync(EscalationRule rule);
        
        /// <summary>
        /// </summary>
        Task<List<EscalationRule>> GetEscalationRulesAsync(string requestType, Priority priority);
        
        /// <summary>
        /// </summary>
        Task<bool> TriggerEscalationAsync(string workflowInstanceId, EscalationTrigger trigger);
        
        /// <summary>
        /// </summary>
        Task<List<OrganizationalLevel>> GetOrganizationalHierarchyAsync(string userId);
    }

    /// <summary>
    /// </summary>
    public class EscalationService : IEscalationService
    {
        private readonly ILogger<EscalationService> _logger;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// </summary>
        public EscalationService(
            ILogger<EscalationService> logger,
            IConfiguration configuration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<EscalationRule> CreateEscalationRuleAsync(EscalationRule rule)
        {
            try
            {
                _logger.LogInformation("Creating escalation rule for request type {RequestType} and priority {Priority}", 
                    rule.RequestType, rule.Priority);

                if (rule.EscalationLevels == null || rule.EscalationLevels.Count == 0)
                {
                    throw new ArgumentException("Escalation rule must have at least one escalation level", nameof(rule));
                }

                for (int i = 1; i < rule.EscalationLevels.Count; i++)
                {
                    if (rule.EscalationLevels[i].TriggerAfter <= rule.EscalationLevels[i - 1].TriggerAfter)
                    {
                        throw new ArgumentException("Escalation levels must be in ascending order of trigger time", nameof(rule));
                    }
                }

                rule.Id = Guid.NewGuid().ToString();
                rule.CreatedAt = DateTime.UtcNow;
                rule.IsActive = true;

                _logger.LogInformation("Escalation rule created with ID {RuleId}", rule.Id);
                
                return await Task.FromResult(rule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating escalation rule for request type {RequestType}", rule.RequestType);
                throw;
            }
        }

        public async Task<List<EscalationRule>> GetEscalationRulesAsync(string requestType, Priority priority)
        {
            try
            {
                _logger.LogInformation("Getting escalation rules for request type {RequestType} and priority {Priority}", 
                    requestType, priority);

                var rules = new List<EscalationRule>();

                var defaultRule = new EscalationRule
                {
                    Id = Guid.NewGuid().ToString(),
                    RequestType = requestType,
                    Priority = priority,
                    EscalationLevels = GetDefaultEscalationLevels(priority),
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                rules.Add(defaultRule);

                return await Task.FromResult(rules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting escalation rules for request type {RequestType} and priority {Priority}", 
                    requestType, priority);
                throw;
            }
        }

        public async Task<bool> TriggerEscalationAsync(string workflowInstanceId, EscalationTrigger trigger)
        {
            try
            {
                _logger.LogInformation("Triggering escalation for workflow instance {WorkflowInstanceId} with trigger {Trigger}", 
                    workflowInstanceId, trigger.TriggerType);


                _logger.LogInformation("Escalation triggered for workflow {WorkflowInstanceId}: {Description}", 
                    workflowInstanceId, trigger.Description);

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error triggering escalation for workflow instance {WorkflowInstanceId}", workflowInstanceId);
                return false;
            }
        }

        public async Task<List<OrganizationalLevel>> GetOrganizationalHierarchyAsync(string userId)
        {
            try
            {
                _logger.LogInformation("Getting organizational hierarchy for user {UserId}", userId);

                var hierarchy = new List<OrganizationalLevel>
                {
                    new OrganizationalLevel { Level = 1, Title = "Team Lead", Description = "Direct supervisor" },
                    new OrganizationalLevel { Level = 2, Title = "Department Manager", Description = "Department head" },
                    new OrganizationalLevel { Level = 3, Title = "Director", Description = "Division director" },
                    new OrganizationalLevel { Level = 4, Title = "VP", Description = "Vice president" },
                    new OrganizationalLevel { Level = 5, Title = "C-Level", Description = "Executive leadership" }
                };

                return await Task.FromResult(hierarchy);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting organizational hierarchy for user {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// </summary>
        private List<EscalationLevel> GetDefaultEscalationLevels(Priority priority)
        {
            return priority switch
            {
                Priority.Critical => new List<EscalationLevel>
                {
                    new EscalationLevel { Level = 1, TriggerAfter = TimeSpan.FromMinutes(30), TargetRole = "Team Lead" },
                    new EscalationLevel { Level = 2, TriggerAfter = TimeSpan.FromHours(1), TargetRole = "Department Manager" },
                    new EscalationLevel { Level = 3, TriggerAfter = TimeSpan.FromHours(2), TargetRole = "Director" }
                },
                Priority.High => new List<EscalationLevel>
                {
                    new EscalationLevel { Level = 1, TriggerAfter = TimeSpan.FromHours(4), TargetRole = "Team Lead" },
                    new EscalationLevel { Level = 2, TriggerAfter = TimeSpan.FromHours(12), TargetRole = "Department Manager" }
                },
                Priority.Medium => new List<EscalationLevel>
                {
                    new EscalationLevel { Level = 1, TriggerAfter = TimeSpan.FromDays(1), TargetRole = "Team Lead" }
                },
                Priority.Low => new List<EscalationLevel>
                {
                    new EscalationLevel { Level = 1, TriggerAfter = TimeSpan.FromDays(3), TargetRole = "Team Lead" }
                },
                _ => new List<EscalationLevel>()
            };
        }
    }

    /// <summary>
    /// </summary>
    public class EscalationRule
    {
        public string Id { get; set; } = string.Empty;
        public string RequestType { get; set; } = string.Empty;
        public Priority Priority { get; set; }
        public List<EscalationLevel> EscalationLevels { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// </summary>
    public class EscalationLevel
    {
        public int Level { get; set; }
        public TimeSpan TriggerAfter { get; set; }
        public string TargetRole { get; set; } = string.Empty;
        public string TargetUserId { get; set; } = string.Empty;
        public List<EscalationAction> Actions { get; set; } = new();
    }

    /// <summary>
    /// </summary>
    public class EscalationAction
    {
        public string ActionType { get; set; } = string.Empty; // notification, reassignment, delegation
        public string Target { get; set; } = string.Empty;
        public Dictionary<string, object> Parameters { get; set; } = new();
    }

    /// <summary>
    /// </summary>
    public class EscalationTrigger
    {
        public string TriggerType { get; set; } = string.Empty; // sla_violation, manual, timeout
        public string Description { get; set; } = string.Empty;
        public DateTime TriggeredAt { get; set; }
        public Dictionary<string, object> Context { get; set; } = new();
    }

    /// <summary>
    /// </summary>
    public class OrganizationalLevel
    {
        public int Level { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
