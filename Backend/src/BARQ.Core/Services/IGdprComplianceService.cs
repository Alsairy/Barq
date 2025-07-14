using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Services;

public interface IGdprComplianceService
{
    Task<DataSubjectRightsResponseDto> ProcessDataSubjectRequestAsync(DataSubjectRequestDto request);
    Task<ConsentManagementResponseDto> UpdateConsentAsync(ConsentUpdateRequestDto request);
    Task<ConsentStatusDto> GetConsentStatusAsync(Guid userId, string consentType);
    Task<DataPortabilityResponseDto> ExportUserDataAsync(Guid userId, string format = "JSON");
    Task<DataErasureResponseDto> EraseUserDataAsync(Guid userId, bool verifyIdentity = true);
    Task<DataProcessingAuditDto> GetDataProcessingAuditAsync(Guid userId, DateTime? fromDate = null, DateTime? toDate = null);
    Task<PrivacyImpactAssessmentDto> ConductPrivacyImpactAssessmentAsync(string processName, string description);
    Task<bool> ValidateDataMinimizationAsync(string dataType, string purpose);
    Task<LawfulBasisValidationDto> ValidateLawfulBasisAsync(string processingActivity, string lawfulBasis);
    Task<DataRetentionPolicyDto> GetDataRetentionPolicyAsync(string dataType);
    Task<bool> ApplyDataRetentionPolicyAsync(string dataType, TimeSpan retentionPeriod);
    Task<BreachNotificationDto> ReportDataBreachAsync(DataBreachReportDto breachReport);
    Task<IEnumerable<ConsentRecordDto>> GetConsentHistoryAsync(Guid userId);
    Task<bool> ValidateConsentWithdrawalAsync(Guid userId, string consentType);
}
