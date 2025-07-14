using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Services;

public interface IComplianceService
{
    Task<ComplianceAssessmentDto> AssessComplianceAsync(string framework, Guid? organizationId = null);
    Task<ComplianceReportDto> GenerateComplianceReportAsync(string framework, DateTime fromDate, DateTime toDate, Guid? organizationId = null);
    Task<bool> ValidateComplianceRuleAsync(string ruleId, object data);
    Task<IEnumerable<ComplianceViolationDto>> GetComplianceViolationsAsync(string framework, DateTime? fromDate = null, DateTime? toDate = null);
    Task<bool> ResolveComplianceViolationAsync(Guid violationId, string resolution, string resolvedBy);
    Task<ComplianceConfigurationDto> GetComplianceConfigurationAsync(string framework);
    Task<bool> UpdateComplianceConfigurationAsync(string framework, ComplianceConfigurationDto configuration);
    Task<IEnumerable<ComplianceAuditTrailDto>> GetComplianceAuditTrailAsync(string framework, DateTime fromDate, DateTime toDate);
    Task<bool> EnableComplianceFrameworkAsync(string framework, Guid organizationId);
    Task<bool> DisableComplianceFrameworkAsync(string framework, Guid organizationId);
}
