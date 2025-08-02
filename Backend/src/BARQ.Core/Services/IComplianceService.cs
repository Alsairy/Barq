using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface IComplianceService
{
    /// <summary>
    /// </summary>
    Task<ComplianceAssessmentDto> AssessComplianceAsync(string framework, Guid? organizationId = null);
    
    /// <summary>
    /// </summary>
    Task<ComplianceReportDto> GenerateComplianceReportAsync(string framework, DateTime fromDate, DateTime toDate, Guid? organizationId = null);
    
    /// <summary>
    /// </summary>
    Task<bool> ValidateComplianceRuleAsync(string ruleId, object data);
    
    /// <summary>
    /// </summary>
    Task<IEnumerable<ComplianceViolationDto>> GetComplianceViolationsAsync(string framework, DateTime? fromDate = null, DateTime? toDate = null);
    
    /// <summary>
    /// </summary>
    Task<bool> ResolveComplianceViolationAsync(Guid violationId, string resolution, string resolvedBy);
    
    /// <summary>
    /// </summary>
    Task<ComplianceConfigurationDto> GetComplianceConfigurationAsync(string framework);
    
    /// <summary>
    /// </summary>
    Task<bool> UpdateComplianceConfigurationAsync(string framework, ComplianceConfigurationDto configuration);
    
    /// <summary>
    /// </summary>
    Task<IEnumerable<ComplianceAuditTrailDto>> GetComplianceAuditTrailAsync(string framework, DateTime fromDate, DateTime toDate);
    
    /// <summary>
    /// </summary>
    Task<bool> EnableComplianceFrameworkAsync(string framework, Guid organizationId);
    
    /// <summary>
    /// </summary>
    Task<bool> DisableComplianceFrameworkAsync(string framework, Guid organizationId);
}
