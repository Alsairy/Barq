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
    public interface ISLAConfigurationService
    {
        /// <summary>
        /// </summary>
        Task<SLAConfiguration> GetSLAConfigurationAsync(string requestType, WorkflowPriority priority);
        
        /// <summary>
        /// </summary>
        Task<SLAConfiguration> CreateOrUpdateSLAConfigurationAsync(SLAConfiguration configuration);
        
        /// <summary>
        /// </summary>
        Task<List<SLAViolation>> MonitorSLAComplianceAsync();
        
        /// <summary>
        /// </summary>
        Task<DateTime> CalculateSLATargetDateAsync(DateTime startDate, TimeSpan slaTarget, WorkflowPriority priority);
    }

    /// <summary>
    /// </summary>
    public class SLAConfigurationService : ISLAConfigurationService
    {
        private readonly ILogger<SLAConfigurationService> _logger;
        private readonly IConfiguration _configuration;
        private readonly Dictionary<WorkflowPriority, TimeSpan> _defaultSLATargets;
        private readonly IRepository<WorkflowTemplate> _workflowTemplateRepository;
        private readonly IRepository<WorkflowInstance> _workflowInstanceRepository;
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// </summary>
        public SLAConfigurationService(
            ILogger<SLAConfigurationService> logger,
            IConfiguration configuration,
            IRepository<WorkflowTemplate> workflowTemplateRepository,
            IRepository<WorkflowInstance> workflowInstanceRepository,
            IUnitOfWork unitOfWork)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _workflowTemplateRepository = workflowTemplateRepository ?? throw new ArgumentNullException(nameof(workflowTemplateRepository));
            _workflowInstanceRepository = workflowInstanceRepository ?? throw new ArgumentNullException(nameof(workflowInstanceRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            
            _defaultSLATargets = new Dictionary<WorkflowPriority, TimeSpan>
            {
                { WorkflowPriority.Critical, TimeSpan.FromHours(4) },
                { WorkflowPriority.High, TimeSpan.FromHours(24) },
                { WorkflowPriority.Medium, TimeSpan.FromDays(3) },
                { WorkflowPriority.Low, TimeSpan.FromDays(7) }
            };
        }

        public async Task<SLAConfiguration> GetSLAConfigurationAsync(string requestType, WorkflowPriority priority)
        {
            try
            {
                _logger.LogInformation("Getting SLA configuration for request type {RequestType} and priority {Priority}", 
                    requestType, priority);

                var slaTarget = _defaultSLATargets.GetValueOrDefault(priority, TimeSpan.FromDays(5));
                
                var configuration = new SLAConfiguration
                {
                    Id = Guid.NewGuid().ToString(),
                    RequestType = requestType,
                    Priority = priority,
                    TargetDuration = slaTarget,
                    EscalationThreshold = TimeSpan.FromMinutes(slaTarget.TotalMinutes * 0.8), // 80% of SLA
                    BusinessHoursOnly = priority != WorkflowPriority.Critical,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                return configuration;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting SLA configuration for request type {RequestType} and priority {Priority}", 
                    requestType, priority);
                throw;
            }
        }

        public async Task<SLAConfiguration> CreateOrUpdateSLAConfigurationAsync(SLAConfiguration configuration)
        {
            try
            {
                _logger.LogInformation("Creating or updating SLA configuration for request type {RequestType}", 
                    configuration.RequestType);

                if (configuration.TargetDuration <= TimeSpan.Zero)
                {
                    throw new ArgumentException("SLA target duration must be positive", nameof(configuration));
                }

                if (configuration.EscalationThreshold >= configuration.TargetDuration)
                {
                    throw new ArgumentException("Escalation threshold must be less than SLA target duration", nameof(configuration));
                }

                configuration.UpdatedAt = DateTime.UtcNow;
                
                _logger.LogInformation("SLA configuration saved for request type {RequestType}", configuration.RequestType);
                
                var existingTemplate = await _workflowTemplateRepository.FirstOrDefaultAsync(wt => 
                    wt.Name == configuration.RequestType);

                if (existingTemplate != null)
                {
                    existingTemplate.SlaHours = (int)configuration.TargetDuration.TotalHours;
                    existingTemplate.SLAConfiguration = System.Text.Json.JsonSerializer.Serialize(configuration);
                    existingTemplate.UpdatedAt = DateTime.UtcNow;
                    
                    await _workflowTemplateRepository.UpdateAsync(existingTemplate);
                    await _unitOfWork.SaveChangesAsync();
                }

                return configuration;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating or updating SLA configuration for request type {RequestType}", 
                    configuration.RequestType);
                throw;
            }
        }

        public async Task<List<SLAViolation>> MonitorSLAComplianceAsync()
        {
            try
            {
                _logger.LogInformation("Monitoring SLA compliance for active workflows");

                var violations = new List<SLAViolation>();

                var activeWorkflows = await _workflowInstanceRepository.FindAsync(wi => 
                    wi.Status == WorkflowStatus.Running || wi.Status == WorkflowStatus.Pending);

                foreach (var workflow in activeWorkflows)
                {
                    var template = await _workflowTemplateRepository.GetByIdAsync(workflow.WorkflowTemplateId);
                    if (template?.SlaHours.HasValue == true)
                    {
                        var slaTarget = workflow.CreatedAt.AddHours(template.SlaHours.Value);
                        if (DateTime.UtcNow > slaTarget)
                        {
                            var violation = new SLAViolation
                            {
                                Id = Guid.NewGuid().ToString(),
                                WorkflowInstanceId = workflow.Id.ToString(),
                                RequestType = template.Name,
                                Priority = WorkflowPriority.Medium,
                                StartDate = workflow.CreatedAt,
                                TargetDate = slaTarget,
                                ViolationDate = DateTime.UtcNow,
                                Overdue = DateTime.UtcNow - slaTarget,
                                EscalationTriggered = false,
                                Description = $"Workflow {workflow.Id} has exceeded SLA target of {template.SlaHours} hours"
                            };
                            violations.Add(violation);
                        }
                    }
                }

                return violations;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error monitoring SLA compliance");
                throw;
            }
        }

        public async Task<DateTime> CalculateSLATargetDateAsync(DateTime startDate, TimeSpan slaTarget, WorkflowPriority priority)
        {
            try
            {
                _logger.LogInformation("Calculating SLA target date for priority {Priority} with target {SLATarget}", 
                    priority, slaTarget);

                var targetDate = startDate;

                if (priority == WorkflowPriority.Critical)
                {
                    targetDate = startDate.Add(slaTarget);
                }
                else
                {
                    var remainingTime = slaTarget;
                    var currentDate = startDate;

                    while (remainingTime > TimeSpan.Zero)
                    {
                        if (currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday)
                        {
                            currentDate = currentDate.AddDays(1).Date.AddHours(9); // Start of next business day
                            continue;
                        }

                        var businessDayStart = currentDate.Date.AddHours(9);
                        var businessDayEnd = currentDate.Date.AddHours(18);

                        if (currentDate < businessDayStart)
                        {
                            currentDate = businessDayStart;
                        }

                        var availableTimeToday = businessDayEnd - currentDate;
                        if (availableTimeToday <= TimeSpan.Zero)
                        {
                            currentDate = currentDate.AddDays(1).Date.AddHours(9);
                            continue;
                        }

                        if (remainingTime <= availableTimeToday)
                        {
                            targetDate = currentDate.Add(remainingTime);
                            break;
                        }
                        else
                        {
                            remainingTime -= availableTimeToday;
                            currentDate = currentDate.AddDays(1).Date.AddHours(9);
                        }
                    }
                }

                _logger.LogInformation("SLA target date calculated: {TargetDate}", targetDate);
                
                return targetDate;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calculating SLA target date");
                throw;
            }
        }
    }

    /// <summary>
    /// </summary>
    public class SLAConfiguration
    {
        public string Id { get; set; } = string.Empty;
        public string RequestType { get; set; } = string.Empty;
        public WorkflowPriority Priority { get; set; }
        public TimeSpan TargetDuration { get; set; }
        public TimeSpan EscalationThreshold { get; set; }
        public bool BusinessHoursOnly { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// </summary>
    public class SLAViolation
    {
        public string Id { get; set; } = string.Empty;
        public string WorkflowInstanceId { get; set; } = string.Empty;
        public string RequestType { get; set; } = string.Empty;
        public WorkflowPriority Priority { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime TargetDate { get; set; }
        public DateTime ViolationDate { get; set; }
        public TimeSpan Overdue { get; set; }
        public bool EscalationTriggered { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
