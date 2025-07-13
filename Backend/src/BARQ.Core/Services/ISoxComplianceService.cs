using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Services;

public interface ISoxComplianceService
{
    Task<FinancialControlsAuditDto> AuditFinancialControlsAsync(DateTime fromDate, DateTime toDate);
    Task<bool> ValidateSegregationOfDutiesAsync(Guid userId, string financialProcess);
    Task<ChangeControlAuditDto> LogChangeControlAsync(ChangeControlLogDto changeLog);
    Task<IEnumerable<ChangeControlAuditDto>> GetChangeControlAuditAsync(DateTime fromDate, DateTime toDate);
    Task<AccessControlsAuditDto> AuditAccessControlsAsync(string systemName, DateTime auditDate);
    Task<bool> ValidateFinancialDataIntegrityAsync(string dataSource, string validationMethod);
    Task<DocumentationComplianceDto> ValidateDocumentationComplianceAsync(string processName);
    Task<InternalControlsAssessmentDto> AssessInternalControlsAsync(string controlArea);
    Task<bool> ValidateApprovalWorkflowAsync(string transactionType, decimal amount, Guid approverId);
    Task<AuditTrailComplianceDto> ValidateAuditTrailComplianceAsync(string systemName, DateTime fromDate, DateTime toDate);
    Task<DeficiencyReportDto> ReportInternalControlDeficiencyAsync(InternalControlDeficiencyDto deficiency);
    Task<bool> ValidateFinancialReportingControlsAsync(string reportType, DateTime reportDate);
    Task<ComplianceTestingDto> ConductComplianceTestingAsync(string controlName, string testProcedure);
    Task<ManagementAssertionDto> GetManagementAssertionAsync(string controlObjective);
    Task<bool> ValidateEntityLevelControlsAsync(string controlType);
    Task<RemediationPlanDto> CreateRemediationPlanAsync(RemediationRequestDto request);
}
