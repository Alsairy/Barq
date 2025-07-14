using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Services;

public interface ISiemIntegrationService
{
    Task<bool> SendEventToSiemAsync(SiemEventDto siemEvent);
    Task<bool> ConfigureSiemEndpointAsync(string endpoint, string apiKey, string format = "JSON");
    Task<IEnumerable<SiemCorrelationDto>> GetCorrelatedEventsAsync(string eventId);
    Task<bool> CreateSiemRuleAsync(string ruleName, string ruleCondition, string action);
    Task<bool> EnableSiemIntegrationAsync();
    Task<bool> DisableSiemIntegrationAsync();
    Task<SiemHealthCheckDto> CheckSiemConnectivityAsync();
    Task<bool> BulkSendEventsAsync(IEnumerable<SiemEventDto> events);
    Task<SiemConfigurationDto> GetSiemConfigurationAsync();
    Task<bool> UpdateSiemConfigurationAsync(SiemConfigurationDto configuration);
    Task<IEnumerable<SiemAlertDto>> GetSiemAlertsAsync(DateTime? fromDate = null, DateTime? toDate = null);
    Task<bool> AcknowledgeSiemAlertAsync(string alertId, string acknowledgment);
}
