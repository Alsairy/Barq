using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface ISiemIntegrationService
{
    /// <summary>
    /// </summary>
    Task<bool> SendEventToSiemAsync(SiemEventDto siemEvent);

    /// <summary>
    /// </summary>
    Task<bool> ConfigureSiemEndpointAsync(string endpoint, string apiKey, string format = "JSON");

    /// <summary>
    /// </summary>
    Task<IEnumerable<SiemCorrelationDto>> GetCorrelatedEventsAsync(string eventId);

    /// <summary>
    /// </summary>
    Task<bool> CreateSiemRuleAsync(string ruleName, string ruleCondition, string action);

    /// <summary>
    /// </summary>
    Task<bool> EnableSiemIntegrationAsync();

    /// <summary>
    /// </summary>
    Task<bool> DisableSiemIntegrationAsync();

    /// <summary>
    /// </summary>
    Task<SiemHealthCheckDto> CheckSiemConnectivityAsync();

    /// <summary>
    /// </summary>
    Task<bool> BulkSendEventsAsync(IEnumerable<SiemEventDto> events);

    /// <summary>
    /// </summary>
    Task<SiemConfigurationDto> GetSiemConfigurationAsync();

    /// <summary>
    /// </summary>
    Task<bool> UpdateSiemConfigurationAsync(SiemConfigurationDto configuration);

    /// <summary>
    /// </summary>
    Task<IEnumerable<SiemAlertDto>> GetSiemAlertsAsync(DateTime? fromDate = null, DateTime? toDate = null);

    /// <summary>
    /// </summary>
    Task<bool> AcknowledgeSiemAlertAsync(string alertId, string acknowledgment);
}
