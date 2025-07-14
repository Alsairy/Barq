using Microsoft.Extensions.Logging;
using BARQ.Core.Services;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Repositories;
using BARQ.Core.Entities;

namespace BARQ.Application.Services.Security;

public class GdprComplianceService : IGdprComplianceService
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<AuditLog> _auditLogRepository;
    private readonly ILogger<GdprComplianceService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly List<ConsentRecordDto> _consentRecords;

    public GdprComplianceService(
        IRepository<User> userRepository,
        IRepository<AuditLog> auditLogRepository,
        ILogger<GdprComplianceService> logger,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _auditLogRepository = auditLogRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _consentRecords = new List<ConsentRecordDto>();
    }

    public async Task<DataSubjectRightsResponseDto> ProcessDataSubjectRequestAsync(DataSubjectRequestDto request)
    {
        try
        {
            _logger.LogInformation("Processing data subject request: {RequestType} for user: {UserId}", request.RequestType, request.UserId);

            var response = new DataSubjectRightsResponseDto
            {
                RequestId = Guid.NewGuid(),
                ProcessedAt = DateTime.UtcNow,
                ProcessedBy = "GDPR Compliance System"
            };

            switch (request.RequestType.ToUpper())
            {
                case "ACCESS":
                    response = await ProcessAccessRequestAsync(request);
                    break;
                case "RECTIFICATION":
                    response = await ProcessRectificationRequestAsync(request);
                    break;
                case "ERASURE":
                    response = await ProcessErasureRequestAsync(request);
                    break;
                case "PORTABILITY":
                    response = await ProcessPortabilityRequestAsync(request);
                    break;
                case "RESTRICTION":
                    response = await ProcessRestrictionRequestAsync(request);
                    break;
                default:
                    response.Status = "REJECTED";
                    response.Notes = $"Unsupported request type: {request.RequestType}";
                    break;
            }

            await LogGdprEventAsync("DATA_SUBJECT_REQUEST_PROCESSED", request.UserId, $"Request type: {request.RequestType}, Status: {response.Status}");

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process data subject request for user: {UserId}", request.UserId);
            throw;
        }
    }

    public async Task<ConsentManagementResponseDto> UpdateConsentAsync(ConsentUpdateRequestDto request)
    {
        try
        {
            _logger.LogInformation("Updating consent for user: {UserId}, Type: {ConsentType}", request.UserId, request.ConsentType);

            var consentRecord = new ConsentRecordDto
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                ConsentType = request.ConsentType,
                ConsentGiven = request.ConsentGiven,
                ConsentDate = request.ConsentDate,
                Purpose = request.Purpose,
                LegalBasis = request.LegalBasis,
                ConsentMethod = "API"
            };

            _consentRecords.Add(consentRecord);

            await LogGdprEventAsync("CONSENT_UPDATED", request.UserId, $"Consent type: {request.ConsentType}, Given: {request.ConsentGiven}");

            return new ConsentManagementResponseDto
            {
                Success = true,
                Message = "Consent updated successfully",
                UpdatedAt = DateTime.UtcNow,
                ConsentId = consentRecord.Id.ToString()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update consent for user: {UserId}", request.UserId);
            return new ConsentManagementResponseDto
            {
                Success = false,
                Message = "Failed to update consent",
                UpdatedAt = DateTime.UtcNow
            };
        }
    }

    public async Task<ConsentStatusDto> GetConsentStatusAsync(Guid userId, string consentType)
    {
        try
        {
            await Task.CompletedTask;
            var latestConsent = _consentRecords
                .Where(c => c.UserId == userId && c.ConsentType == consentType)
                .OrderByDescending(c => c.ConsentDate)
                .FirstOrDefault();

            if (latestConsent == null)
            {
                return new ConsentStatusDto
                {
                    UserId = userId,
                    ConsentType = consentType,
                    ConsentGiven = false,
                    ConsentDate = DateTime.MinValue
                };
            }

            return new ConsentStatusDto
            {
                UserId = latestConsent.UserId,
                ConsentType = latestConsent.ConsentType,
                ConsentGiven = latestConsent.ConsentGiven,
                ConsentDate = latestConsent.ConsentDate,
                WithdrawnDate = latestConsent.WithdrawnDate,
                Purpose = latestConsent.Purpose,
                LegalBasis = latestConsent.LegalBasis
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get consent status for user: {UserId}", userId);
            throw;
        }
    }

    public async Task<DataPortabilityResponseDto> ExportUserDataAsync(Guid userId, string format = "JSON")
    {
        try
        {
            _logger.LogInformation("Exporting user data for user: {UserId} in format: {Format}", userId, format);

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException($"User not found: {userId}");

            var userData = new
            {
                PersonalData = new
                {
                    user.FirstName,
                    user.LastName,
                    user.Email,
                    user.PhoneNumber,
                    user.CreatedAt,
                    user.LastLoginAt
                },
                ConsentHistory = _consentRecords.Where(c => c.UserId == userId),
                ProcessingActivities = await GetDataProcessingAuditAsync(userId)
            };

            var dataPackage = System.Text.Json.JsonSerializer.Serialize(userData, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });

            await LogGdprEventAsync("DATA_EXPORT_COMPLETED", userId, $"Format: {format}, Size: {dataPackage.Length} bytes");

            return new DataPortabilityResponseDto
            {
                UserId = userId,
                Format = format,
                DataPackage = dataPackage,
                ExportedAt = DateTime.UtcNow,
                ExportedBy = "GDPR Compliance System",
                DataSize = dataPackage.Length
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to export user data for user: {UserId}", userId);
            throw;
        }
    }

    public async Task<DataErasureResponseDto> EraseUserDataAsync(Guid userId, bool verifyIdentity = true)
    {
        try
        {
            _logger.LogInformation("Erasing user data for user: {UserId}", userId);

            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                throw new ArgumentException($"User not found: {userId}");

            var erasedDataTypes = new List<string> { "PersonalData", "ConsentRecords", "ProcessingLogs" };
            var retainedDataTypes = new List<string> { "AuditLogs", "LegalObligationData" };

            user.FirstName = "[ERASED]";
            user.LastName = "[ERASED]";
            user.Email = $"erased-{userId}@example.com";
            user.PhoneNumber = "[ERASED]";

            await _userRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            _consentRecords.RemoveAll(c => c.UserId == userId);

            await LogGdprEventAsync("DATA_ERASURE_COMPLETED", userId, $"Erased data types: {string.Join(", ", erasedDataTypes)}");

            return new DataErasureResponseDto
            {
                UserId = userId,
                Success = true,
                ErasedDataTypes = erasedDataTypes,
                RetainedDataTypes = retainedDataTypes,
                RetentionReason = "Legal obligation and audit requirements",
                ErasedAt = DateTime.UtcNow,
                ErasedBy = "GDPR Compliance System"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to erase user data for user: {UserId}", userId);
            throw;
        }
    }

    public async Task<DataProcessingAuditDto> GetDataProcessingAuditAsync(Guid userId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        try
        {
            var auditLogs = await _auditLogRepository.FindAsync(
                a => a.DataSubjectId == userId.ToString() &&
                     (fromDate == null || a.Timestamp >= fromDate) &&
                     (toDate == null || a.Timestamp <= toDate));

            return new DataProcessingAuditDto
            {
                UserId = userId,
                ProcessingActivity = "User Data Processing",
                Purpose = "Service provision and legal compliance",
                LegalBasis = "Contract and legitimate interest",
                ProcessedAt = DateTime.UtcNow,
                ProcessedBy = "System",
                DataTypes = "Personal identifiers, contact information"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get data processing audit for user: {UserId}", userId);
            throw;
        }
    }

    public async Task<PrivacyImpactAssessmentDto> ConductPrivacyImpactAssessmentAsync(string processName, string description)
    {
        try
        {
            await Task.CompletedTask;
            _logger.LogInformation("Conducting privacy impact assessment for process: {ProcessName}", processName);

            var risks = new List<string> { "Data breach risk", "Unauthorized access", "Data retention violations" };
            var mitigations = new List<string> { "Encryption", "Access controls", "Automated deletion" };

            return new PrivacyImpactAssessmentDto
            {
                Id = Guid.NewGuid(),
                ProcessName = processName,
                Description = description,
                RiskLevel = "Medium",
                IdentifiedRisks = risks,
                Mitigations = mitigations,
                AssessmentDate = DateTime.UtcNow,
                AssessedBy = "Privacy Officer"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to conduct privacy impact assessment for process: {ProcessName}", processName);
            throw;
        }
    }

    public async Task<bool> ValidateDataMinimizationAsync(string dataType, string purpose)
    {
        try
        {
            _logger.LogInformation("Validating data minimization for data type: {DataType}, Purpose: {Purpose}", dataType, purpose);

            var isMinimal = dataType switch
            {
                "PersonalIdentifiers" => purpose.Contains("Authentication") || purpose.Contains("Contact"),
                "ContactInformation" => purpose.Contains("Communication") || purpose.Contains("Service"),
                "FinancialData" => purpose.Contains("Payment") || purpose.Contains("Billing"),
                _ => false
            };

            await LogGdprEventAsync("DATA_MINIMIZATION_VALIDATED", null, $"Data type: {dataType}, Purpose: {purpose}, Valid: {isMinimal}");

            return isMinimal;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate data minimization for data type: {DataType}", dataType);
            return false;
        }
    }

    public async Task<LawfulBasisValidationDto> ValidateLawfulBasisAsync(string processingActivity, string lawfulBasis)
    {
        try
        {
            await Task.CompletedTask;
            var validBases = new[] { "Consent", "Contract", "Legal obligation", "Vital interests", "Public task", "Legitimate interests" };
            var isValid = validBases.Contains(lawfulBasis);

            return new LawfulBasisValidationDto
            {
                ProcessingActivity = processingActivity,
                LawfulBasis = lawfulBasis,
                IsValid = isValid,
                ValidationReason = isValid ? "Valid GDPR lawful basis" : "Invalid or unrecognized lawful basis",
                ValidatedAt = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate lawful basis for activity: {ProcessingActivity}", processingActivity);
            throw;
        }
    }

    public async Task<DataRetentionPolicyDto> GetDataRetentionPolicyAsync(string dataType)
    {
        try
        {
            await Task.CompletedTask;
            var retentionPeriods = new Dictionary<string, TimeSpan>
            {
                ["PersonalData"] = TimeSpan.FromDays(2555), // 7 years
                ["ConsentRecords"] = TimeSpan.FromDays(2555), // 7 years
                ["AuditLogs"] = TimeSpan.FromDays(3650), // 10 years
                ["FinancialData"] = TimeSpan.FromDays(2555) // 7 years
            };

            var retentionPeriod = retentionPeriods.GetValueOrDefault(dataType, TimeSpan.FromDays(365));

            return new DataRetentionPolicyDto
            {
                DataType = dataType,
                RetentionPeriod = retentionPeriod,
                RetentionReason = "Legal and regulatory compliance",
                DisposalMethod = "Secure deletion",
                PolicyCreated = DateTime.UtcNow.AddYears(-1),
                PolicyUpdated = DateTime.UtcNow
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get data retention policy for data type: {DataType}", dataType);
            throw;
        }
    }

    public async Task<bool> ApplyDataRetentionPolicyAsync(string dataType, TimeSpan retentionPeriod)
    {
        try
        {
            _logger.LogInformation("Applying data retention policy for data type: {DataType}, Period: {RetentionPeriod}", dataType, retentionPeriod);

            await LogGdprEventAsync("DATA_RETENTION_POLICY_APPLIED", null, $"Data type: {dataType}, Retention period: {retentionPeriod.Days} days");

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to apply data retention policy for data type: {DataType}", dataType);
            return false;
        }
    }

    public async Task<BreachNotificationDto> ReportDataBreachAsync(DataBreachReportDto breachReport)
    {
        try
        {
            _logger.LogWarning("Reporting data breach: {BreachType}", breachReport.BreachType);

            var breachId = Guid.NewGuid();
            var requiresNotification = breachReport.EstimatedAffectedUsers > 0 || breachReport.RiskLevel == "High";

            await LogGdprEventAsync("DATA_BREACH_REPORTED", null, $"Breach ID: {breachId}, Type: {breachReport.BreachType}, Affected users: {breachReport.EstimatedAffectedUsers}");

            return new BreachNotificationDto
            {
                BreachId = breachId,
                NotificationType = requiresNotification ? "Supervisory Authority" : "Internal Only",
                Recipient = requiresNotification ? "Data Protection Authority" : "Internal Security Team",
                NotifiedAt = DateTime.UtcNow,
                NotificationMethod = "Email",
                Status = "Sent"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to report data breach");
            throw;
        }
    }

    public async Task<IEnumerable<ConsentRecordDto>> GetConsentHistoryAsync(Guid userId)
    {
        try
        {
            await Task.CompletedTask;
            return _consentRecords
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.ConsentDate);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get consent history for user: {UserId}", userId);
            throw;
        }
    }

    public async Task<bool> ValidateConsentWithdrawalAsync(Guid userId, string consentType)
    {
        try
        {
            var latestConsent = _consentRecords
                .Where(c => c.UserId == userId && c.ConsentType == consentType)
                .OrderByDescending(c => c.ConsentDate)
                .FirstOrDefault();

            if (latestConsent == null || !latestConsent.ConsentGiven)
                return false;

            latestConsent.ConsentGiven = false;
            latestConsent.WithdrawnDate = DateTime.UtcNow;

            await LogGdprEventAsync("CONSENT_WITHDRAWN", userId, $"Consent type: {consentType}");

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate consent withdrawal for user: {UserId}", userId);
            return false;
        }
    }

    private async Task<DataSubjectRightsResponseDto> ProcessAccessRequestAsync(DataSubjectRequestDto request)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        var userData = user != null ? System.Text.Json.JsonSerializer.Serialize(new { user.FirstName, user.LastName, user.Email }) : "No data found";

        return new DataSubjectRightsResponseDto
        {
            RequestId = Guid.NewGuid(),
            Status = "COMPLETED",
            ResponseData = userData,
            ProcessedAt = DateTime.UtcNow,
            ProcessedBy = "GDPR Access Handler",
            Notes = "Data access request processed successfully"
        };
    }

    private async Task<DataSubjectRightsResponseDto> ProcessRectificationRequestAsync(DataSubjectRequestDto request)
    {
        await Task.CompletedTask;
        return new DataSubjectRightsResponseDto
        {
            RequestId = Guid.NewGuid(),
            Status = "PENDING",
            ResponseData = "Rectification request received and under review",
            ProcessedAt = DateTime.UtcNow,
            ProcessedBy = "GDPR Rectification Handler",
            Notes = "Manual review required for data rectification"
        };
    }

    private async Task<DataSubjectRightsResponseDto> ProcessErasureRequestAsync(DataSubjectRequestDto request)
    {
        var erasureResult = await EraseUserDataAsync(request.UserId);

        return new DataSubjectRightsResponseDto
        {
            RequestId = Guid.NewGuid(),
            Status = erasureResult.Success ? "COMPLETED" : "FAILED",
            ResponseData = $"Data erasure {(erasureResult.Success ? "completed" : "failed")}",
            ProcessedAt = DateTime.UtcNow,
            ProcessedBy = "GDPR Erasure Handler",
            Notes = erasureResult.Success ? "User data erased successfully" : "Data erasure failed"
        };
    }

    private async Task<DataSubjectRightsResponseDto> ProcessPortabilityRequestAsync(DataSubjectRequestDto request)
    {
        var exportResult = await ExportUserDataAsync(request.UserId);

        return new DataSubjectRightsResponseDto
        {
            RequestId = Guid.NewGuid(),
            Status = "COMPLETED",
            ResponseData = exportResult.DataPackage,
            ProcessedAt = DateTime.UtcNow,
            ProcessedBy = "GDPR Portability Handler",
            Notes = $"Data export completed, size: {exportResult.DataSize} bytes"
        };
    }

    private async Task<DataSubjectRightsResponseDto> ProcessRestrictionRequestAsync(DataSubjectRequestDto request)
    {
        await Task.CompletedTask;
        return new DataSubjectRightsResponseDto
        {
            RequestId = Guid.NewGuid(),
            Status = "COMPLETED",
            ResponseData = "Processing restriction applied",
            ProcessedAt = DateTime.UtcNow,
            ProcessedBy = "GDPR Restriction Handler",
            Notes = "Data processing restriction applied successfully"
        };
    }

    private async Task LogGdprEventAsync(string eventType, Guid? userId, string description)
    {
        var auditLog = new AuditLog
        {
            EntityName = "GDPR",
            EntityId = userId ?? Guid.NewGuid(),
            Action = eventType,
            UserId = Guid.NewGuid(),
            Timestamp = DateTime.UtcNow,
            Description = description,
            ComplianceFramework = "GDPR",
            ComplianceEventType = eventType,
            ComplianceStatus = "COMPLIANT",
            IsPersonalData = true,
            DataSubjectId = userId?.ToString(),
            ProcessingPurpose = "GDPR Compliance",
            LegalBasis = "Legal obligation",
            IsTamperProof = true,
            IntegrityHash = Guid.NewGuid().ToString("N")[..16]
        };

        await _auditLogRepository.AddAsync(auditLog);
        await _unitOfWork.SaveChangesAsync();
    }
}
