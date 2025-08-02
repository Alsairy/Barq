using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface IThreatDetectionService
{
    /// <summary>
    /// </summary>
    Task<ThreatAssessmentDto> AssessThreatLevelAsync(string eventData, string eventType);

    /// <summary>
    /// </summary>
    Task<bool> DetectBruteForceAttackAsync(string ipAddress, string? userId = null);

    /// <summary>
    /// </summary>
    Task<bool> DetectSuspiciousLoginAsync(string userId, string ipAddress, string userAgent);

    /// <summary>
    /// </summary>
    Task<bool> DetectDataExfiltrationAsync(string userId, int dataVolumeBytes, TimeSpan timeWindow);

    /// <summary>
    /// </summary>
    Task<bool> DetectPrivilegeEscalationAsync(string userId, string attemptedAction);

    /// <summary>
    /// </summary>
    Task<bool> DetectMaliciousFileUploadAsync(byte[] fileContent, string fileName, string contentType);

    /// <summary>
    /// </summary>
    Task<IEnumerable<ThreatIndicatorDto>> GetThreatIndicatorsAsync();

    /// <summary>
    /// </summary>
    Task<bool> UpdateThreatSignaturesAsync();

    /// <summary>
    /// </summary>
    Task<BehavioralAnalysisDto> AnalyzeUserBehaviorAsync(string userId, TimeSpan analysisWindow);

    /// <summary>
    /// </summary>
    Task<bool> IsIpAddressBlacklistedAsync(string ipAddress);

    /// <summary>
    /// </summary>
    Task<bool> AddToBlacklistAsync(string ipAddress, string reason, TimeSpan? duration = null);

    /// <summary>
    /// </summary>
    Task<bool> RemoveFromBlacklistAsync(string ipAddress);

    /// <summary>
    /// </summary>
    Task<GeolocationRiskDto> AssessGeolocationRiskAsync(string ipAddress, string? userId = null);
}
