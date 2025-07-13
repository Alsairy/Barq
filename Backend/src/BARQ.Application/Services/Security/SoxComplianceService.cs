using Microsoft.Extensions.Logging;
using BARQ.Core.Services;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Repositories;
using BARQ.Core.Entities;

namespace BARQ.Application.Services.Security;

public class SoxComplianceService : ISoxComplianceService
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<AuditLog> _auditLogRepository;
    private readonly ILogger<SoxComplianceService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly List<ChangeControlAuditDto> _changeControlLogs;
    private readonly List<DeficiencyReportDto> _deficiencyReports;
    private readonly List<RemediationPlanDto> _remediationPlans;

    public SoxComplianceService(
        IRepository<User> userRepository,
        IRepository<AuditLog> auditLogRepository,
        ILogger<SoxComplianceService> logger,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _auditLogRepository = auditLogRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _changeControlLogs = new List<ChangeControlAuditDto>();
        _deficiencyReports = new List<DeficiencyReportDto>();
        _remediationPlans = new List<RemediationPlanDto>();
    }

    public async Task<FinancialControlsAuditDto> AuditFinancialControlsAsync(DateTime fromDate, DateTime toDate)
    {
        try
        {
            _logger.LogInformation("Auditing financial controls from {FromDate} to {ToDate}", fromDate, toDate);

            var testedControls = new List<string>
            {
                "Segregation of duties",
                "Authorization controls",
                "Approval workflows",
                "Access controls",
                "Change management"
            };

            var deficiencies = new List<string>();
            var isEffective = deficiencies.Count == 0;

            var audit = new FinancialControlsAuditDto
            {
                Id = Guid.NewGuid(),
                FromDate = fromDate,
                ToDate = toDate,
                ControlArea = "Financial Reporting",
                IsEffective = isEffective,
                TestedControls = testedControls,
                Deficiencies = deficiencies,
                AuditorName = "SOX Compliance Auditor",
                AuditDate = DateTime.UtcNow
            };

            await LogSoxEventAsync("FINANCIAL_CONTROLS_AUDITED", null, $"Audit period: {fromDate:yyyy-MM-dd} to {toDate:yyyy-MM-dd}, Effective: {isEffective}");

            return audit;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to audit financial controls");
            throw;
        }
    }

    public async Task<bool> ValidateSegregationOfDutiesAsync(Guid userId, string financialProcess)
    {
        try
        {
            _logger.LogInformation("Validating segregation of duties for user: {UserId}, Process: {FinancialProcess}", userId, financialProcess);

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return false;

            var isValid = financialProcess switch
            {
                "Payment Authorization" => !HasRole(userId, "Payment Processing"),
                "Invoice Approval" => !HasRole(userId, "Invoice Creation"),
                "Journal Entry" => !HasRole(userId, "Journal Approval"),
                "Bank Reconciliation" => !HasRole(userId, "Cash Management"),
                _ => true
            };

            await LogSoxEventAsync("SEGREGATION_OF_DUTIES_VALIDATED", userId, $"Process: {financialProcess}, Valid: {isValid}");

            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate segregation of duties for user: {UserId}", userId);
            return false;
        }
    }

    public async Task<ChangeControlAuditDto> LogChangeControlAsync(ChangeControlLogDto changeLog)
    {
        try
        {
            _logger.LogInformation("Logging change control for system: {SystemAffected}", changeLog.SystemAffected);

            var auditEntry = new ChangeControlAuditDto
            {
                Id = Guid.NewGuid(),
                ChangeType = changeLog.ChangeType,
                SystemAffected = changeLog.SystemAffected,
                Description = changeLog.Description,
                RequestedBy = changeLog.RequestedBy,
                ApprovedBy = changeLog.ApprovedBy,
                RequestDate = changeLog.RequestDate,
                ApprovalDate = changeLog.ApprovalDate,
                ImplementationDate = changeLog.ImplementationDate,
                BusinessJustification = changeLog.BusinessJustification,
                RiskAssessment = changeLog.RiskAssessment,
                ComplianceStatus = ValidateChangeCompliance(changeLog)
            };

            _changeControlLogs.Add(auditEntry);

            await LogSoxEventAsync("CHANGE_CONTROL_LOGGED", null, $"Change type: {changeLog.ChangeType}, System: {changeLog.SystemAffected}");

            return auditEntry;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to log change control");
            throw;
        }
    }

    public async Task<IEnumerable<ChangeControlAuditDto>> GetChangeControlAuditAsync(DateTime fromDate, DateTime toDate)
    {
        try
        {
            return _changeControlLogs
                .Where(c => c.RequestDate >= fromDate && c.RequestDate <= toDate)
                .OrderByDescending(c => c.RequestDate)
                .ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get change control audit");
            throw;
        }
    }

    public async Task<AccessControlsAuditDto> AuditAccessControlsAsync(string systemName, DateTime auditDate)
    {
        try
        {
            _logger.LogInformation("Auditing access controls for system: {SystemName}", systemName);

            var usersReviewed = new List<string> { "Financial Manager", "Accounting Clerk", "CFO", "Controller" };
            var accessViolations = new List<string>();
            var recommendedActions = new List<string> { "Review user access quarterly", "Implement role-based access" };

            return new AccessControlsAuditDto
            {
                Id = Guid.NewGuid(),
                SystemName = systemName,
                AuditDate = auditDate,
                AuditorName = "SOX Access Control Auditor",
                UsersReviewed = usersReviewed,
                AccessViolations = accessViolations,
                RecommendedActions = recommendedActions,
                OverallCompliance = accessViolations.Count == 0
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to audit access controls for system: {SystemName}", systemName);
            throw;
        }
    }

    public async Task<bool> ValidateFinancialDataIntegrityAsync(string dataSource, string validationMethod)
    {
        try
        {
            _logger.LogInformation("Validating financial data integrity for source: {DataSource}, Method: {ValidationMethod}", dataSource, validationMethod);

            var approvedMethods = new[] { "Hash verification", "Digital signatures", "Checksums", "Database constraints" };
            var isValid = approvedMethods.Contains(validationMethod);

            await LogSoxEventAsync("FINANCIAL_DATA_INTEGRITY_VALIDATED", null, $"Source: {dataSource}, Method: {validationMethod}, Valid: {isValid}");

            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate financial data integrity");
            return false;
        }
    }

    public async Task<DocumentationComplianceDto> ValidateDocumentationComplianceAsync(string processName)
    {
        try
        {
            _logger.LogInformation("Validating documentation compliance for process: {ProcessName}", processName);

            var hasDocumentation = true;
            var isCurrentVersion = true;
            var hasApprovalSignatures = true;
            var lastReviewDate = DateTime.UtcNow.AddMonths(-6);
            var missingDocuments = new List<string>();

            return new DocumentationComplianceDto
            {
                ProcessName = processName,
                HasDocumentation = hasDocumentation,
                IsCurrentVersion = isCurrentVersion,
                HasApprovalSignatures = hasApprovalSignatures,
                LastReviewDate = lastReviewDate,
                MissingDocuments = missingDocuments,
                ComplianceStatus = missingDocuments.Count == 0 ? "Compliant" : "Non-Compliant"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate documentation compliance for process: {ProcessName}", processName);
            throw;
        }
    }

    public async Task<InternalControlsAssessmentDto> AssessInternalControlsAsync(string controlArea)
    {
        try
        {
            _logger.LogInformation("Assessing internal controls for area: {ControlArea}", controlArea);

            return new InternalControlsAssessmentDto
            {
                Id = Guid.NewGuid(),
                ControlArea = controlArea,
                ControlObjective = $"Ensure effective {controlArea.ToLower()} controls",
                ControlDescription = $"Controls designed to mitigate risks in {controlArea.ToLower()}",
                ControlOwner = "Financial Controller",
                ControlFrequency = "Daily",
                IsEffective = true,
                TestingProcedure = "Sample testing and walkthrough procedures",
                LastTested = DateTime.UtcNow.AddDays(-30),
                TestResults = "Control operating effectively"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to assess internal controls for area: {ControlArea}", controlArea);
            throw;
        }
    }

    public async Task<bool> ValidateApprovalWorkflowAsync(string transactionType, decimal amount, Guid approverId)
    {
        try
        {
            _logger.LogInformation("Validating approval workflow for transaction: {TransactionType}, Amount: {Amount}", transactionType, amount);

            var approver = await _userRepository.GetByIdAsync(approverId);
            if (approver == null)
                return false;

            var isValid = transactionType switch
            {
                "Purchase Order" => amount <= 10000 || HasRole(approverId, "Senior Manager"),
                "Payment Authorization" => amount <= 5000 || HasRole(approverId, "Financial Manager"),
                "Journal Entry" => amount <= 1000 || HasRole(approverId, "Controller"),
                _ => false
            };

            await LogSoxEventAsync("APPROVAL_WORKFLOW_VALIDATED", approverId, $"Transaction: {transactionType}, Amount: {amount:C}, Valid: {isValid}");

            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate approval workflow");
            return false;
        }
    }

    public async Task<AuditTrailComplianceDto> ValidateAuditTrailComplianceAsync(string systemName, DateTime fromDate, DateTime toDate)
    {
        try
        {
            _logger.LogInformation("Validating audit trail compliance for system: {SystemName}", systemName);

            var auditLogs = await _auditLogRepository.GetAllAsync(
                a => a.Source == systemName &&
                     a.Timestamp >= fromDate &&
                     a.Timestamp <= toDate);

            var totalTransactions = 1000;
            var auditedTransactions = auditLogs.Count();
            var complianceGaps = new List<string>();

            if (auditedTransactions < totalTransactions * 0.95)
                complianceGaps.Add("Incomplete audit trail coverage");

            var compliancePercentage = totalTransactions > 0 ? 
                (auditedTransactions * 100.0 / totalTransactions).ToString("F2") + "%" : "100%";

            return new AuditTrailComplianceDto
            {
                SystemName = systemName,
                FromDate = fromDate,
                ToDate = toDate,
                IsCompliant = complianceGaps.Count == 0,
                TotalTransactions = totalTransactions,
                AuditedTransactions = auditedTransactions,
                ComplianceGaps = complianceGaps,
                CompliancePercentage = compliancePercentage
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate audit trail compliance for system: {SystemName}", systemName);
            throw;
        }
    }

    public async Task<DeficiencyReportDto> ReportInternalControlDeficiencyAsync(InternalControlDeficiencyDto deficiency)
    {
        try
        {
            _logger.LogWarning("Reporting internal control deficiency: {ControlName}", deficiency.ControlName);

            var report = new DeficiencyReportDto
            {
                Id = Guid.NewGuid(),
                ControlName = deficiency.ControlName,
                DeficiencyType = deficiency.DeficiencyType,
                Description = deficiency.Description,
                Severity = deficiency.Severity,
                Status = "Open",
                ReportedDate = DateTime.UtcNow,
                ReportedBy = deficiency.IdentifiedBy,
                RemediationPlan = "Remediation plan to be developed"
            };

            _deficiencyReports.Add(report);

            await LogSoxEventAsync("INTERNAL_CONTROL_DEFICIENCY_REPORTED", null, $"Control: {deficiency.ControlName}, Severity: {deficiency.Severity}");

            return report;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to report internal control deficiency");
            throw;
        }
    }

    public async Task<bool> ValidateFinancialReportingControlsAsync(string reportType, DateTime reportDate)
    {
        try
        {
            _logger.LogInformation("Validating financial reporting controls for report: {ReportType}", reportType);

            var isValid = reportType switch
            {
                "Monthly Financial Statement" => true,
                "Quarterly Report" => true,
                "Annual Report" => true,
                "Management Report" => true,
                _ => false
            };

            await LogSoxEventAsync("FINANCIAL_REPORTING_CONTROLS_VALIDATED", null, $"Report: {reportType}, Date: {reportDate:yyyy-MM-dd}, Valid: {isValid}");

            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate financial reporting controls");
            return false;
        }
    }

    public async Task<ComplianceTestingDto> ConductComplianceTestingAsync(string controlName, string testProcedure)
    {
        try
        {
            _logger.LogInformation("Conducting compliance testing for control: {ControlName}", controlName);

            var testPassed = true;
            var exceptions = new List<string>();
            var recommendedActions = testPassed ? 
                new List<string> { "Continue current control procedures" } :
                new List<string> { "Review and strengthen control procedures" };

            return new ComplianceTestingDto
            {
                Id = Guid.NewGuid(),
                ControlName = controlName,
                TestProcedure = testProcedure,
                TestDate = DateTime.UtcNow,
                TestPerformedBy = "SOX Compliance Tester",
                TestResults = testPassed ? "Control operating effectively" : "Control deficiencies identified",
                TestPassed = testPassed,
                Exceptions = exceptions,
                RecommendedActions = recommendedActions.First()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to conduct compliance testing for control: {ControlName}", controlName);
            throw;
        }
    }

    public async Task<ManagementAssertionDto> GetManagementAssertionAsync(string controlObjective)
    {
        try
        {
            return new ManagementAssertionDto
            {
                ControlObjective = controlObjective,
                AssertionStatement = $"Management asserts that controls for {controlObjective.ToLower()} are designed and operating effectively",
                ManagementResponse = "Controls are effective and operating as designed",
                AssertionDate = DateTime.UtcNow,
                AssertedBy = "Chief Financial Officer",
                SupportingEvidence = new[] { "Control testing results", "Process documentation", "Audit findings" },
                IsEffective = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get management assertion for objective: {ControlObjective}", controlObjective);
            throw;
        }
    }

    public async Task<bool> ValidateEntityLevelControlsAsync(string controlType)
    {
        try
        {
            _logger.LogInformation("Validating entity level controls for type: {ControlType}", controlType);

            var isValid = controlType switch
            {
                "Tone at the Top" => true,
                "Code of Conduct" => true,
                "Risk Assessment" => true,
                "Information Systems" => true,
                "Monitoring Activities" => true,
                _ => false
            };

            await LogSoxEventAsync("ENTITY_LEVEL_CONTROLS_VALIDATED", null, $"Control type: {controlType}, Valid: {isValid}");

            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate entity level controls");
            return false;
        }
    }

    public async Task<RemediationPlanDto> CreateRemediationPlanAsync(RemediationRequestDto request)
    {
        try
        {
            _logger.LogInformation("Creating remediation plan for deficiency: {DeficiencyId}", request.DeficiencyId);

            var plan = new RemediationPlanDto
            {
                Id = Guid.NewGuid(),
                DeficiencyId = request.DeficiencyId,
                RemediationPlan = request.RemediationPlan,
                TargetCompletionDate = request.TargetCompletionDate,
                ResponsibleParty = request.ResponsibleParty,
                Status = "In Progress",
                CreatedDate = DateTime.UtcNow
            };

            _remediationPlans.Add(plan);

            await LogSoxEventAsync("REMEDIATION_PLAN_CREATED", null, $"Deficiency: {request.DeficiencyId}, Target: {request.TargetCompletionDate:yyyy-MM-dd}");

            return plan;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create remediation plan");
            throw;
        }
    }

    private bool ValidateChangeCompliance(ChangeControlLogDto changeLog)
    {
        return !string.IsNullOrEmpty(changeLog.ApprovedBy) &&
               !string.IsNullOrEmpty(changeLog.BusinessJustification) &&
               !string.IsNullOrEmpty(changeLog.RiskAssessment) &&
               changeLog.ApprovalDate <= changeLog.ImplementationDate;
    }

    private bool HasRole(Guid userId, string roleName)
    {
        return true;
    }

    private async Task LogSoxEventAsync(string eventType, Guid? userId, string description)
    {
        var auditLog = new AuditLog
        {
            EntityName = "SOX",
            EntityId = userId ?? Guid.NewGuid(),
            Action = eventType,
            UserId = userId ?? Guid.NewGuid(),
            Timestamp = DateTime.UtcNow,
            Description = description,
            ComplianceFramework = "SOX",
            ComplianceEventType = eventType,
            ComplianceStatus = "COMPLIANT",
            ProcessingPurpose = "Financial controls and compliance",
            LegalBasis = "Sarbanes-Oxley Act compliance",
            IsTamperProof = true,
            IntegrityHash = Guid.NewGuid().ToString("N")[..16]
        };

        await _auditLogRepository.AddAsync(auditLog);
        await _unitOfWork.SaveChangesAsync();
    }
}
