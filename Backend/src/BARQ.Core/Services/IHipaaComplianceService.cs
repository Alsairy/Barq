using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface IHipaaComplianceService
{
    /// <summary>
    /// </summary>
    Task<PhiAccessAuditDto> LogPhiAccessAsync(PhiAccessLogDto accessLog);

    /// <summary>
    /// </summary>
    Task<IEnumerable<PhiAccessAuditDto>> GetPhiAccessAuditAsync(Guid? patientId = null, DateTime? fromDate = null, DateTime? toDate = null);

    /// <summary>
    /// </summary>
    Task<BusinessAssociateAgreementDto> CreateBusinessAssociateAgreementAsync(BusinessAssociateRequestDto request);

    /// <summary>
    /// </summary>
    Task<bool> ValidateMinimumNecessaryAsync(string requestedData, string purpose);

    /// <summary>
    /// </summary>
    Task<EncryptionComplianceDto> ValidatePhiEncryptionAsync(string dataLocation, string encryptionMethod);

    /// <summary>
    /// </summary>
    Task<SecurityIncidentResponseDto> ReportSecurityIncidentAsync(SecurityIncidentDto incident);

    /// <summary>
    /// </summary>
    Task<RiskAssessmentDto> ConductHipaaRiskAssessmentAsync(string systemName, string description);

    /// <summary>
    /// </summary>
    Task<bool> ValidateAccessControlsAsync(Guid userId, string resourceType, string action);

    /// <summary>
    /// </summary>
    Task<AuditLogComplianceDto> ValidateAuditLogComplianceAsync(DateTime fromDate, DateTime toDate);

    /// <summary>
    /// </summary>
    Task<BreachNotificationDto> ReportHipaaBreachAsync(HipaaBreachReportDto breachReport);

    /// <summary>
    /// </summary>
    Task<bool> ValidateDataBackupComplianceAsync(string backupLocation, string encryptionStatus);

    /// <summary>
    /// </summary>
    Task<WorkforceTrainingDto> TrackWorkforceTrainingAsync(Guid userId, string trainingType, DateTime completionDate);

    /// <summary>
    /// </summary>
    Task<IEnumerable<WorkforceTrainingDto>> GetWorkforceTrainingRecordsAsync(Guid? userId = null);

    /// <summary>
    /// </summary>
    Task<bool> ValidatePhiDisposalAsync(string disposalMethod, string dataType);

    /// <summary>
    /// </summary>
    Task<ContingencyPlanDto> GetContingencyPlanAsync(string planType);

    /// <summary>
    /// </summary>
    Task<bool> TestContingencyPlanAsync(string planType, DateTime testDate);
}
