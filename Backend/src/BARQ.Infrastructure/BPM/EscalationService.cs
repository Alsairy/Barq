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
    public interface IEscalationService
    {
        /// <summary>
        /// </summary>
        Task<EscalationRule> CreateEscalationRuleAsync(EscalationRule rule);
        
        /// <summary>
        /// </summary>
        Task<List<EscalationRule>> GetEscalationRulesAsync(string requestType, WorkflowPriority priority);
        
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
        private readonly IRepository<WorkflowTemplate> _workflowTemplateRepository;
        private readonly IRepository<WorkflowInstance> _workflowInstanceRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// </summary>
        public EscalationService(
            ILogger<EscalationService> logger,
            IConfiguration configuration,
            IRepository<WorkflowTemplate> workflowTemplateRepository,
            IRepository<WorkflowInstance> workflowInstanceRepository,
            IRepository<User> userRepository,
            IUnitOfWork unitOfWork)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _workflowTemplateRepository = workflowTemplateRepository ?? throw new ArgumentNullException(nameof(workflowTemplateRepository));
            _workflowInstanceRepository = workflowInstanceRepository ?? throw new ArgumentNullException(nameof(workflowInstanceRepository));
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<EscalationRule> CreateEscalationRuleAsync(EscalationRule rule)
        {
            try
            {
                _logger.LogInformation("Creating escalation rule for request type {RequestType} and priority {WorkflowPriority}", 
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
                
                var template = await _workflowTemplateRepository.FirstOrDefaultAsync(wt => 
                    wt.Name == rule.RequestType);

                if (template != null)
                {
                    template.EscalationRules = System.Text.Json.JsonSerializer.Serialize(rule);
                    template.UpdatedAt = DateTime.UtcNow;
                    
                    await _workflowTemplateRepository.UpdateAsync(template);
                    await _unitOfWork.SaveChangesAsync();
                }

                return rule;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating escalation rule for request type {RequestType}", rule.RequestType);
                throw;
            }
        }

        public async Task<List<EscalationRule>> GetEscalationRulesAsync(string requestType, WorkflowPriority priority)
        {
            try
            {
                _logger.LogInformation("Getting escalation rules for request type {RequestType} and priority {WorkflowPriority}", 
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

                var template = await _workflowTemplateRepository.FirstOrDefaultAsync(wt => 
                    wt.Name == requestType);

                if (template?.EscalationRules != null)
                {
                    try
                    {
                        var existingRule = System.Text.Json.JsonSerializer.Deserialize<EscalationRule>(template.EscalationRules);
                        if (existingRule != null)
                        {
                            rules.Clear();
                            rules.Add(existingRule);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to deserialize escalation rules for template {TemplateId}", template.Id);
                    }
                }

                return rules;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting escalation rules for request type {RequestType} and priority {WorkflowPriority}", 
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

                var workflow = await _workflowInstanceRepository.FirstOrDefaultAsync(wi => 
                    wi.Id.ToString() == workflowInstanceId);

                if (workflow != null)
                {
                    workflow.Status = WorkflowStatus.Escalated;
                    workflow.UpdatedAt = DateTime.UtcNow;
                    
                    await _workflowInstanceRepository.UpdateAsync(workflow);
                    await _unitOfWork.SaveChangesAsync();
                }

                return true;
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

                var user = await _userRepository.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
                if (user != null)
                {
                    var userHierarchy = await _userRepository.FindAsync(u => 
                        u.OrganizationId == user.OrganizationId);
                    
                    hierarchy = userHierarchy.Select((u, index) => new OrganizationalLevel
                    {
                        Level = index + 1,
                        Title = u.Role?.Name ?? "Employee",
                        Description = $"User: {u.FirstName} {u.LastName}"
                    }).ToList();
                }

                return hierarchy;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting organizational hierarchy for user {UserId}", userId);
                throw;
            }
        }

        /// <summary>
        /// </summary>
        private List<EscalationLevel> GetDefaultEscalationLevels(WorkflowPriority priority)
        {
            return priority switch
            {
                WorkflowPriority.Critical => new List<EscalationLevel>
                {
                    new EscalationLevel { Level = 1, TriggerAfter = TimeSpan.FromMinutes(30), TargetRole = "Team Lead" },
                    new EscalationLevel { Level = 2, TriggerAfter = TimeSpan.FromHours(1), TargetRole = "Department Manager" },
                    new EscalationLevel { Level = 3, TriggerAfter = TimeSpan.FromHours(2), TargetRole = "Director" }
                },
                WorkflowPriority.High => new List<EscalationLevel>
                {
                    new EscalationLevel { Level = 1, TriggerAfter = TimeSpan.FromHours(4), TargetRole = "Team Lead" },
                    new EscalationLevel { Level = 2, TriggerAfter = TimeSpan.FromHours(12), TargetRole = "Department Manager" }
                },
                WorkflowPriority.Medium => new List<EscalationLevel>
                {
                    new EscalationLevel { Level = 1, TriggerAfter = TimeSpan.FromDays(1), TargetRole = "Team Lead" }
                },
                WorkflowPriority.Low => new List<EscalationLevel>
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
        public WorkflowPriority Priority { get; set; }
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
