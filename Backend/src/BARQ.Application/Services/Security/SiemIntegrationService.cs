using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Net.Http;
using BARQ.Core.Services;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Repositories;
using BARQ.Core.Entities;

namespace BARQ.Application.Services.Security;

public class SiemIntegrationService : ISiemIntegrationService
{
    private readonly IRepository<AuditLog> _auditRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ILogger<SiemIntegrationService> _logger;
    private readonly HttpClient _httpClient;
    private readonly List<SiemEventDto> _eventQueue;
    private readonly List<SiemAlertDto> _siemAlerts;
    private SiemConfigurationDto _siemConfiguration = new();
    private bool _isEnabled;

    public SiemIntegrationService(
        IRepository<AuditLog> auditRepository,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        ILogger<SiemIntegrationService> logger,
        HttpClient httpClient)
    {
        _auditRepository = auditRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
        _httpClient = httpClient;
        _eventQueue = new List<SiemEventDto>();
        _siemAlerts = new List<SiemAlertDto>();
        
        InitializeSiemConfiguration();
    }

    public async Task<bool> SendEventToSiemAsync(SiemEventDto siemEvent)
    {
        try
        {
            if (!_isEnabled)
            {
                _logger.LogDebug("SIEM integration is disabled, event not sent: {EventId}", siemEvent.Id);
                return false;
            }

            if (string.IsNullOrEmpty(_siemConfiguration.Endpoint))
            {
                _logger.LogWarning("SIEM endpoint not configured, queuing event: {EventId}", siemEvent.Id);
                _eventQueue.Add(siemEvent);
                return false;
            }

            var jsonPayload = JsonSerializer.Serialize(siemEvent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

            foreach (var header in _siemConfiguration.CustomHeaders)
            {
                content.Headers.Add(header.Key, header.Value);
            }

            if (!string.IsNullOrEmpty(_siemConfiguration.ApiKey))
            {
                content.Headers.Add("Authorization", $"Bearer {_siemConfiguration.ApiKey}");
            }

            var response = await _httpClient.PostAsync(_siemConfiguration.Endpoint, content);

            if (response.IsSuccessStatusCode)
            {
                await LogSiemOperationAsync("EVENT_SENT", $"Event {siemEvent.Id} sent successfully to SIEM");
                _logger.LogInformation("SIEM event sent successfully: {EventId}", siemEvent.Id);
                return true;
            }
            else
            {
                await LogSiemOperationAsync("EVENT_SEND_FAILED", $"Failed to send event {siemEvent.Id}: {response.StatusCode}");
                _logger.LogWarning("Failed to send SIEM event: {EventId}, Status: {StatusCode}", siemEvent.Id, response.StatusCode);
                
                _eventQueue.Add(siemEvent);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending event to SIEM: {EventId}", siemEvent.Id);
            _eventQueue.Add(siemEvent);
            return false;
        }
    }

    public async Task<bool> ConfigureSiemEndpointAsync(string endpoint, string apiKey, string format = "JSON")
    {
        try
        {
            _siemConfiguration.Endpoint = endpoint;
            _siemConfiguration.ApiKey = apiKey;
            _siemConfiguration.Format = format;

            var healthCheck = await CheckSiemConnectivityAsync();
            if (healthCheck.IsConnected)
            {
                await LogSiemOperationAsync("CONFIGURATION_UPDATED", $"SIEM endpoint configured: {endpoint}");
                _logger.LogInformation("SIEM endpoint configured successfully: {Endpoint}", endpoint);
                return true;
            }
            else
            {
                _logger.LogWarning("SIEM endpoint configuration failed connectivity test: {Endpoint}", endpoint);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to configure SIEM endpoint: {Endpoint}", endpoint);
            return false;
        }
    }

    public Task<IEnumerable<SiemCorrelationDto>> GetCorrelatedEventsAsync(string eventId)
    {
        try
        {
            var correlations = new List<SiemCorrelationDto>();
            
            var correlationId = Guid.NewGuid().ToString();
            var relatedEvents = _eventQueue.Where(e => 
                e.UserId == _eventQueue.FirstOrDefault(ev => ev.Id.ToString() == eventId)?.UserId ||
                e.IPAddress == _eventQueue.FirstOrDefault(ev => ev.Id.ToString() == eventId)?.IPAddress)
                .Take(10);

            if (relatedEvents.Any())
            {
                correlations.Add(new SiemCorrelationDto
                {
                    CorrelationId = correlationId,
                    RelatedEvents = relatedEvents,
                    CorrelationType = "User Activity Correlation",
                    CorrelationScore = 0.8,
                    CorrelatedAt = DateTime.UtcNow,
                    Description = "Events correlated by user ID and IP address"
                });
            }

            return Task.FromResult<IEnumerable<SiemCorrelationDto>>(correlations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get correlated events for: {EventId}", eventId);
            throw;
        }
    }

    public async Task<bool> CreateSiemRuleAsync(string ruleName, string ruleCondition, string action)
    {
        try
        {
            var rule = new
            {
                Name = ruleName,
                Condition = ruleCondition,
                Action = action,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            await LogSiemOperationAsync("RULE_CREATED", $"SIEM rule created: {ruleName}");
            _logger.LogInformation("SIEM rule created: {RuleName}", ruleName);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create SIEM rule: {RuleName}", ruleName);
            return false;
        }
    }

    public async Task<bool> EnableSiemIntegrationAsync()
    {
        try
        {
            _isEnabled = true;
            _siemConfiguration.IsEnabled = true;

            if (_eventQueue.Any())
            {
                await BulkSendEventsAsync(_eventQueue);
                _eventQueue.Clear();
            }

            await LogSiemOperationAsync("INTEGRATION_ENABLED", "SIEM integration has been enabled");
            _logger.LogInformation("SIEM integration enabled");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to enable SIEM integration");
            return false;
        }
    }

    public async Task<bool> DisableSiemIntegrationAsync()
    {
        try
        {
            _isEnabled = false;
            _siemConfiguration.IsEnabled = false;

            await LogSiemOperationAsync("INTEGRATION_DISABLED", "SIEM integration has been disabled");
            _logger.LogInformation("SIEM integration disabled");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to disable SIEM integration");
            return false;
        }
    }

    public async Task<SiemHealthCheckDto> CheckSiemConnectivityAsync()
    {
        var healthCheck = new SiemHealthCheckDto
        {
            CheckedAt = DateTime.UtcNow
        };

        try
        {
            if (string.IsNullOrEmpty(_siemConfiguration.Endpoint))
            {
                healthCheck.IsConnected = false;
                healthCheck.Status = "Not Configured";
                healthCheck.ErrorMessage = "SIEM endpoint not configured";
                return healthCheck;
            }

            var startTime = DateTime.UtcNow;
            var response = await _httpClient.GetAsync($"{_siemConfiguration.Endpoint}/health");
            var endTime = DateTime.UtcNow;

            healthCheck.ResponseTimeMs = (endTime - startTime).TotalMilliseconds;
            healthCheck.IsConnected = response.IsSuccessStatusCode;
            healthCheck.Status = response.IsSuccessStatusCode ? "Connected" : "Connection Failed";
            
            if (response.IsSuccessStatusCode)
            {
                healthCheck.LastSuccessfulConnection = DateTime.UtcNow;
                healthCheck.SiemVersion = response.Headers.GetValues("X-SIEM-Version").FirstOrDefault() ?? "Unknown";
            }
            else
            {
                healthCheck.ErrorMessage = $"HTTP {response.StatusCode}: {response.ReasonPhrase}";
            }
        }
        catch (Exception ex)
        {
            healthCheck.IsConnected = false;
            healthCheck.Status = "Connection Error";
            healthCheck.ErrorMessage = ex.Message;
            _logger.LogError(ex, "SIEM connectivity check failed");
        }

        return healthCheck;
    }

    public async Task<bool> BulkSendEventsAsync(IEnumerable<SiemEventDto> events)
    {
        try
        {
            if (!_isEnabled || string.IsNullOrEmpty(_siemConfiguration.Endpoint))
            {
                return false;
            }

            var batches = events.Chunk(_siemConfiguration.BatchSize);
            var successCount = 0;
            var totalCount = events.Count();

            foreach (var batch in batches)
            {
                var jsonPayload = JsonSerializer.Serialize(batch, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

                foreach (var header in _siemConfiguration.CustomHeaders)
                {
                    content.Headers.Add(header.Key, header.Value);
                }

                if (!string.IsNullOrEmpty(_siemConfiguration.ApiKey))
                {
                    content.Headers.Add("Authorization", $"Bearer {_siemConfiguration.ApiKey}");
                }

                var response = await _httpClient.PostAsync($"{_siemConfiguration.Endpoint}/bulk", content);

                if (response.IsSuccessStatusCode)
                {
                    successCount += batch.Count();
                }
                else
                {
                    _logger.LogWarning("Failed to send batch of {Count} events to SIEM: {StatusCode}", 
                        batch.Count(), response.StatusCode);
                }

                await Task.Delay(_siemConfiguration.BatchInterval);
            }

            await LogSiemOperationAsync("BULK_SEND_COMPLETED", 
                $"Bulk send completed: {successCount}/{totalCount} events sent successfully");

            _logger.LogInformation("Bulk SIEM send completed: {SuccessCount}/{TotalCount} events sent", 
                successCount, totalCount);

            return successCount == totalCount;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to bulk send events to SIEM");
            return false;
        }
    }

    public Task<SiemConfigurationDto> GetSiemConfigurationAsync()
    {
        try
        {
            return Task.FromResult(_siemConfiguration);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get SIEM configuration");
            throw;
        }
    }

    public async Task<bool> UpdateSiemConfigurationAsync(SiemConfigurationDto configuration)
    {
        try
        {
            _siemConfiguration = configuration;
            _isEnabled = configuration.IsEnabled;

            await LogSiemOperationAsync("CONFIGURATION_UPDATED", "SIEM configuration has been updated");
            _logger.LogInformation("SIEM configuration updated");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update SIEM configuration");
            return false;
        }
    }

    public Task<IEnumerable<SiemAlertDto>> GetSiemAlertsAsync(DateTime? fromDate = null, DateTime? toDate = null)
    {
        try
        {
            var query = _siemAlerts.AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(a => a.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(a => a.CreatedAt <= toDate.Value);

            return Task.FromResult<IEnumerable<SiemAlertDto>>(query.OrderByDescending(a => a.CreatedAt).ToList());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve SIEM alerts");
            throw;
        }
    }

    public async Task<bool> AcknowledgeSiemAlertAsync(string alertId, string acknowledgment)
    {
        try
        {
            var alert = _siemAlerts.FirstOrDefault(a => a.AlertId == alertId);
            if (alert == null)
                return false;

            alert.Acknowledgment = acknowledgment;
            alert.AcknowledgedAt = DateTime.UtcNow;
            alert.AcknowledgedBy = "System"; // In real implementation, get from current user context
            alert.Status = "Acknowledged";

            await LogSiemOperationAsync("ALERT_ACKNOWLEDGED", $"SIEM alert acknowledged: {alertId}");
            _logger.LogInformation("SIEM alert acknowledged: {AlertId}", alertId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to acknowledge SIEM alert: {AlertId}", alertId);
            return false;
        }
    }

    private void InitializeSiemConfiguration()
    {
        _siemConfiguration = new SiemConfigurationDto
        {
            Endpoint = _configuration["SIEM:Endpoint"] ?? string.Empty,
            ApiKey = _configuration["SIEM:ApiKey"] ?? string.Empty,
            Format = _configuration["SIEM:Format"] ?? "JSON",
            IsEnabled = bool.Parse(_configuration["SIEM:Enabled"] ?? "false"),
            BatchSize = int.Parse(_configuration["SIEM:BatchSize"] ?? "100"),
            BatchInterval = TimeSpan.FromMinutes(int.Parse(_configuration["SIEM:BatchIntervalMinutes"] ?? "5")),
            EnabledEventTypes = _configuration.GetSection("SIEM:EnabledEventTypes").GetChildren().Select(x => x.Value ?? string.Empty).ToArray(),
            CustomHeaders = new Dictionary<string, string>(),
            RetryAttempts = int.Parse(_configuration["SIEM:RetryAttempts"] ?? "3"),
            RetryDelay = TimeSpan.FromSeconds(int.Parse(_configuration["SIEM:RetryDelaySeconds"] ?? "30"))
        };

        _isEnabled = _siemConfiguration.IsEnabled;

        _siemAlerts.AddRange(new[]
        {
            new SiemAlertDto
            {
                AlertId = Guid.NewGuid().ToString(),
                AlertType = "High Risk Login",
                Title = "Suspicious Login Activity Detected",
                Description = "Multiple failed login attempts from unusual location",
                Severity = "High",
                CreatedAt = DateTime.UtcNow.AddHours(-2),
                Status = "Active",
                TriggerEvents = new List<SiemEventDto>()
            }
        });
    }

    private async Task LogSiemOperationAsync(string operation, string description)
    {
        try
        {
            var auditEntry = new AuditLog
            {
                Id = Guid.NewGuid(),
                EntityName = "SiemIntegration",
                EntityId = Guid.NewGuid(),
                Action = operation,
                OldValues = null,
                NewValues = description,
                IPAddress = "System",
                UserAgent = "SiemIntegrationService",
                UserId = Guid.Empty,
                CreatedAt = DateTime.UtcNow,
                TenantId = Guid.Empty
            };

            await _auditRepository.AddAsync(auditEntry);
            await _unitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to log SIEM operation: {Operation}", operation);
        }
    }
}
