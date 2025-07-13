using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Services;

public interface IHipaaComplianceService
{
    Task<PhiAccessAuditDto> LogPhiAccessAsync(PhiAccessLogDto accessLog);
    Task<IEnumerable<PhiAccessAuditDto>> GetPhiAccessAuditAsync(Guid? patientId = null, DateTime? fromDate = null, DateTime? toDate = null);
    Task<BusinessAssociateAgreementDto> CreateBusinessAssociateAgreementAsync(BusinessAssociateRequestDto request);
    Task<bool> ValidateMinimumNecessaryAsync(string requestedData, string purpose);
    Task<EncryptionComplianceDto> ValidatePhiEncryptionAsync(string dataLocation, string encryptionMethod);
    Task<SecurityIncidentResponseDto> ReportSecurityIncidentAsync(SecurityIncidentDto incident);
    Task<RiskAssessmentDto> ConductHipaaRiskAssessmentAsync(string systemName, string description);
    Task<bool> ValidateAccessControlsAsync(Guid userId, string resourceType, string action);
    Task<AuditLogComplianceDto> ValidateAuditLogComplianceAsync(DateTime fromDate, DateTime toDate);
    Task<BreachNotificationDto> ReportHipaaBreachAsync(HipaaBreachReportDto breachReport);
    Task<bool> ValidateDataBackupComplianceAsync(string backupLocation, string encryptionStatus);
    Task<WorkforceTrainingDto> TrackWorkforceTrainingAsync(Guid userId, string trainingType, DateTime completionDate);
    Task<IEnumerable<WorkforceTrainingDto>> GetWorkforceTrainingRecordsAsync(Guid? userId = null);
    Task<bool> ValidatePhiDisposalAsync(string disposalMethod, string dataType);
    Task<ContingencyPlanDto> GetContingencyPlanAsync(string planType);
    Task<bool> TestContingencyPlanAsync(string planType, DateTime testDate);
}
