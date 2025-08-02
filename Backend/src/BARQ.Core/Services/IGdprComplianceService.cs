using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface IGdprComplianceService
{
    /// <summary>
    /// </summary>
    Task<DataSubjectRightsResponseDto> ProcessDataSubjectRequestAsync(DataSubjectRequestDto request);

    /// <summary>
    /// </summary>
    Task<ConsentManagementResponseDto> UpdateConsentAsync(ConsentUpdateRequestDto request);

    /// <summary>
    /// </summary>
    Task<ConsentStatusDto> GetConsentStatusAsync(Guid userId, string consentType);

    /// <summary>
    /// </summary>
    Task<DataPortabilityResponseDto> ExportUserDataAsync(Guid userId, string format = "JSON");

    /// <summary>
    /// </summary>
    Task<DataErasureResponseDto> EraseUserDataAsync(Guid userId, bool verifyIdentity = true);

    /// <summary>
    /// </summary>
    Task<DataProcessingAuditDto> GetDataProcessingAuditAsync(Guid userId, DateTime? fromDate = null, DateTime? toDate = null);

    /// <summary>
    /// </summary>
    Task<PrivacyImpactAssessmentDto> ConductPrivacyImpactAssessmentAsync(string processName, string description);

    /// <summary>
    /// </summary>
    Task<bool> ValidateDataMinimizationAsync(string dataType, string purpose);

    /// <summary>
    /// </summary>
    Task<LawfulBasisValidationDto> ValidateLawfulBasisAsync(string processingActivity, string lawfulBasis);

    /// <summary>
    /// </summary>
    Task<DataRetentionPolicyDto> GetDataRetentionPolicyAsync(string dataType);

    /// <summary>
    /// </summary>
    Task<bool> ApplyDataRetentionPolicyAsync(string dataType, TimeSpan retentionPeriod);

    /// <summary>
    /// </summary>
    Task<BreachNotificationDto> ReportDataBreachAsync(DataBreachReportDto breachReport);

    /// <summary>
    /// </summary>
    Task<IEnumerable<ConsentRecordDto>> GetConsentHistoryAsync(Guid userId);

    /// <summary>
    /// </summary>
    Task<bool> ValidateConsentWithdrawalAsync(Guid userId, string consentType);
}
