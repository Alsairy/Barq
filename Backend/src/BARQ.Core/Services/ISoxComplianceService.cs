using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface ISoxComplianceService
{
    /// <summary>
    /// </summary>
    Task<FinancialControlsAuditDto> AuditFinancialControlsAsync(DateTime fromDate, DateTime toDate);

    /// <summary>
    /// </summary>
    Task<bool> ValidateSegregationOfDutiesAsync(Guid userId, string financialProcess);

    /// <summary>
    /// </summary>
    Task<ChangeControlAuditDto> LogChangeControlAsync(ChangeControlLogDto changeLog);

    /// <summary>
    /// </summary>
    Task<IEnumerable<ChangeControlAuditDto>> GetChangeControlAuditAsync(DateTime fromDate, DateTime toDate);

    /// <summary>
    /// </summary>
    Task<AccessControlsAuditDto> AuditAccessControlsAsync(string systemName, DateTime auditDate);

    /// <summary>
    /// </summary>
    Task<bool> ValidateFinancialDataIntegrityAsync(string dataSource, string validationMethod);

    /// <summary>
    /// </summary>
    Task<DocumentationComplianceDto> ValidateDocumentationComplianceAsync(string processName);

    /// <summary>
    /// </summary>
    Task<InternalControlsAssessmentDto> AssessInternalControlsAsync(string controlArea);

    /// <summary>
    /// </summary>
    Task<bool> ValidateApprovalWorkflowAsync(string transactionType, decimal amount, Guid approverId);

    /// <summary>
    /// </summary>
    Task<AuditTrailComplianceDto> ValidateAuditTrailComplianceAsync(string systemName, DateTime fromDate, DateTime toDate);

    /// <summary>
    /// </summary>
    Task<DeficiencyReportDto> ReportInternalControlDeficiencyAsync(InternalControlDeficiencyDto deficiency);

    /// <summary>
    /// </summary>
    Task<bool> ValidateFinancialReportingControlsAsync(string reportType, DateTime reportDate);

    /// <summary>
    /// </summary>
    Task<ComplianceTestingDto> ConductComplianceTestingAsync(string controlName, string testProcedure);

    /// <summary>
    /// </summary>
    Task<ManagementAssertionDto> GetManagementAssertionAsync(string controlObjective);

    /// <summary>
    /// </summary>
    Task<bool> ValidateEntityLevelControlsAsync(string controlType);

    /// <summary>
    /// </summary>
    Task<RemediationPlanDto> CreateRemediationPlanAsync(RemediationRequestDto request);
}
