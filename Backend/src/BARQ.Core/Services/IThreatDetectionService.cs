using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Services;

public interface IThreatDetectionService
{
    Task<ThreatAssessmentDto> AssessThreatLevelAsync(string eventData, string eventType);
    Task<bool> DetectBruteForceAttackAsync(string ipAddress, string? userId = null);
    Task<bool> DetectSuspiciousLoginAsync(string userId, string ipAddress, string userAgent);
    Task<bool> DetectDataExfiltrationAsync(string userId, int dataVolumeBytes, TimeSpan timeWindow);
    Task<bool> DetectPrivilegeEscalationAsync(string userId, string attemptedAction);
    Task<bool> DetectMaliciousFileUploadAsync(byte[] fileContent, string fileName, string contentType);
    Task<IEnumerable<ThreatIndicatorDto>> GetThreatIndicatorsAsync();
    Task<bool> UpdateThreatSignaturesAsync();
    Task<BehavioralAnalysisDto> AnalyzeUserBehaviorAsync(string userId, TimeSpan analysisWindow);
    Task<bool> IsIpAddressBlacklistedAsync(string ipAddress);
    Task<bool> AddToBlacklistAsync(string ipAddress, string reason, TimeSpan? duration = null);
    Task<bool> RemoveFromBlacklistAsync(string ipAddress);
    Task<GeolocationRiskDto> AssessGeolocationRiskAsync(string ipAddress, string? userId = null);
}
