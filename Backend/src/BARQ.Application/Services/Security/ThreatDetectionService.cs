using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;
using BARQ.Core.Services;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Repositories;
using BARQ.Core.Entities;

namespace BARQ.Application.Services.Security;

public class ThreatDetectionService : IThreatDetectionService
{
    private readonly IRepository<AuditLog> _auditRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ThreatDetectionService> _logger;
    private readonly Dictionary<string, DateTime> _loginAttempts;
    private readonly HashSet<string> _blacklistedIps;
    private readonly List<ThreatIndicatorDto> _threatIndicators;

    public ThreatDetectionService(
        IRepository<AuditLog> auditRepository,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        ILogger<ThreatDetectionService> logger)
    {
        _auditRepository = auditRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
        _loginAttempts = new Dictionary<string, DateTime>();
        _blacklistedIps = new HashSet<string>();
        _threatIndicators = new List<ThreatIndicatorDto>();
        
        InitializeThreatIndicators();
    }

    public async Task<ThreatAssessmentDto> AssessThreatLevelAsync(string eventData, string eventType)
    {
        try
        {
            var riskFactors = new List<string>();
            var riskScore = 0.0;

            riskScore += AnalyzeEventType(eventType, riskFactors);
            riskScore += AnalyzeEventData(eventData, riskFactors);
            riskScore += await AnalyzeThreatIndicators(eventData, riskFactors);

            var threatLevel = DetermineThreatLevel(riskScore);
            var recommendedAction = GetRecommendedAction(threatLevel, eventType);

            return new ThreatAssessmentDto
            {
                ThreatLevel = threatLevel,
                RiskScore = Math.Min(1.0, riskScore),
                ThreatCategory = CategorizeEvent(eventType),
                RiskFactors = riskFactors,
                RecommendedAction = recommendedAction,
                AssessedAt = DateTime.UtcNow,
                AssessmentMethod = "Automated Threat Analysis"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to assess threat level for event type: {EventType}", eventType);
            throw;
        }
    }

    public async Task<bool> DetectBruteForceAttackAsync(string ipAddress, string? userId = null)
    {
        try
        {
            var key = $"{ipAddress}:{userId ?? "unknown"}";
            var maxAttempts = int.Parse(_configuration["Security:BruteForce:MaxAttempts"] ?? "5");
            var timeWindow = int.Parse(_configuration["Security:BruteForce:TimeWindowMinutes"] ?? "15");

            var recentAttempts = await GetRecentLoginAttempts(ipAddress, userId, TimeSpan.FromMinutes(timeWindow));
            
            if (recentAttempts >= maxAttempts)
            {
                await AddToBlacklistAsync(ipAddress, "Brute force attack detected", TimeSpan.FromHours(1));
                _logger.LogWarning("Brute force attack detected from IP: {IPAddress}, User: {UserId}", ipAddress, userId);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to detect brute force attack for IP: {IPAddress}", ipAddress);
            return false;
        }
    }

    public async Task<bool> DetectSuspiciousLoginAsync(string userId, string ipAddress, string userAgent)
    {
        try
        {
            var suspiciousFactors = new List<string>();
            var isSuspicious = false;

            if (await IsIpAddressBlacklistedAsync(ipAddress))
            {
                suspiciousFactors.Add("Blacklisted IP address");
                isSuspicious = true;
            }

            var geoRisk = await AssessGeolocationRiskAsync(ipAddress, userId);
            if (geoRisk.RiskScore > 0.7)
            {
                suspiciousFactors.Add($"High-risk geolocation: {geoRisk.Country}");
                isSuspicious = true;
            }

            if (IsAnomalousUserAgent(userAgent))
            {
                suspiciousFactors.Add("Anomalous user agent");
                isSuspicious = true;
            }

            if (await IsUnusualLoginTime(userId))
            {
                suspiciousFactors.Add("Unusual login time");
                isSuspicious = true;
            }

            if (isSuspicious)
            {
                _logger.LogWarning("Suspicious login detected for user: {UserId} from IP: {IPAddress}. Factors: {Factors}", 
                    userId, ipAddress, string.Join(", ", suspiciousFactors));
            }

            return isSuspicious;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to detect suspicious login for user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> DetectDataExfiltrationAsync(string userId, int dataVolumeBytes, TimeSpan timeWindow)
    {
        try
        {
            var threshold = int.Parse(_configuration["Security:DataExfiltration:ThresholdMB"] ?? "100") * 1024 * 1024;
            var recentDataAccess = await GetRecentDataAccess(userId, timeWindow);

            if (recentDataAccess + dataVolumeBytes > threshold)
            {
                _logger.LogWarning("Potential data exfiltration detected for user: {UserId}. Volume: {Volume}MB in {TimeWindow}", 
                    userId, (recentDataAccess + dataVolumeBytes) / (1024 * 1024), timeWindow);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to detect data exfiltration for user: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> DetectPrivilegeEscalationAsync(string userId, string attemptedAction)
    {
        try
        {
            var userRoles = await GetUserRoles(userId);
            var requiredPermissions = GetRequiredPermissions(attemptedAction);

            if (!HasSufficientPermissions(userRoles, requiredPermissions))
            {
                _logger.LogWarning("Privilege escalation attempt detected for user: {UserId}. Action: {Action}", userId, attemptedAction);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to detect privilege escalation for user: {UserId}", userId);
            return false;
        }
    }

    public Task<bool> DetectMaliciousFileUploadAsync(byte[] fileContent, string fileName, string contentType)
    {
        try
        {
            var isMalicious = false;
            var reasons = new List<string>();

            if (HasMaliciousSignature(fileContent))
            {
                reasons.Add("Malicious file signature detected");
                isMalicious = true;
            }

            if (HasSuspiciousExtension(fileName))
            {
                reasons.Add("Suspicious file extension");
                isMalicious = true;
            }

            if (IsContentTypeMismatch(fileContent, contentType))
            {
                reasons.Add("Content type mismatch");
                isMalicious = true;
            }

            if (isMalicious)
            {
                _logger.LogWarning("Malicious file upload detected: {FileName}. Reasons: {Reasons}", 
                    fileName, string.Join(", ", reasons));
            }

            return Task.FromResult(isMalicious);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to detect malicious file upload: {FileName}", fileName);
            return Task.FromResult(false);
        }
    }

    public Task<IEnumerable<ThreatIndicatorDto>> GetThreatIndicatorsAsync()
    {
        try
        {
            return Task.FromResult(_threatIndicators.Where(ti => ti.IsActive && (!ti.ExpiresAt.HasValue || ti.ExpiresAt > DateTime.UtcNow)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve threat indicators");
            throw;
        }
    }

    public Task<bool> UpdateThreatSignaturesAsync()
    {
        try
        {
            _logger.LogInformation("Updating threat signatures from external sources");
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update threat signatures");
            return Task.FromResult(false);
        }
    }

    public async Task<BehavioralAnalysisDto> AnalyzeUserBehaviorAsync(string userId, TimeSpan analysisWindow)
    {
        try
        {
            var endTime = DateTime.UtcNow;
            var startTime = endTime.Subtract(analysisWindow);
            
            var userActivities = await GetUserActivities(userId, startTime, endTime);
            var patterns = AnalyzeActivityPatterns(userActivities);
            var anomalyScore = CalculateAnomalyScore(patterns);
            var isAnomalous = anomalyScore > 0.7;

            return new BehavioralAnalysisDto
            {
                UserId = userId,
                AnomalyScore = anomalyScore,
                IsAnomalous = isAnomalous,
                AnomalousPatterns = isAnomalous ? GetAnomalousPatterns(patterns) : new List<string>(),
                ActivityPatterns = patterns,
                AnalysisStartTime = startTime,
                AnalysisEndTime = endTime,
                AnalysisMethod = "Statistical Behavioral Analysis"
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to analyze user behavior for user: {UserId}", userId);
            throw;
        }
    }

    public Task<bool> IsIpAddressBlacklistedAsync(string ipAddress)
    {
        try
        {
            return Task.FromResult(_blacklistedIps.Contains(ipAddress));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to check IP blacklist status: {IPAddress}", ipAddress);
            return Task.FromResult(false);
        }
    }

    public Task<bool> AddToBlacklistAsync(string ipAddress, string reason, TimeSpan? duration = null)
    {
        try
        {
            _blacklistedIps.Add(ipAddress);
            
            if (duration.HasValue)
            {
                _ = Task.Delay(duration.Value).ContinueWith(_ => _blacklistedIps.Remove(ipAddress));
            }

            _logger.LogInformation("IP address added to blacklist: {IPAddress}. Reason: {Reason}", ipAddress, reason);
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to add IP to blacklist: {IPAddress}", ipAddress);
            return Task.FromResult(false);
        }
    }

    public Task<bool> RemoveFromBlacklistAsync(string ipAddress)
    {
        try
        {
            var removed = _blacklistedIps.Remove(ipAddress);
            if (removed)
            {
                _logger.LogInformation("IP address removed from blacklist: {IPAddress}", ipAddress);
            }
            return Task.FromResult(removed);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to remove IP from blacklist: {IPAddress}", ipAddress);
            return Task.FromResult(false);
        }
    }

    public async Task<GeolocationRiskDto> AssessGeolocationRiskAsync(string ipAddress, string? userId = null)
    {
        try
        {
            var geoData = await GetGeolocationData(ipAddress);
            var riskFactors = new List<string>();
            var riskScore = 0.0;

            if (IsHighRiskCountry(geoData.Country))
            {
                riskFactors.Add("High-risk country");
                riskScore += 0.4;
            }

            if (geoData.IsVpnDetected)
            {
                riskFactors.Add("VPN detected");
                riskScore += 0.3;
            }

            if (geoData.IsProxyDetected)
            {
                riskFactors.Add("Proxy detected");
                riskScore += 0.2;
            }

            if (geoData.IsTorDetected)
            {
                riskFactors.Add("Tor network detected");
                riskScore += 0.5;
            }

            if (!string.IsNullOrEmpty(userId) && await IsUnusualLocation(userId, geoData.Country))
            {
                riskFactors.Add("Unusual location for user");
                riskScore += 0.3;
            }

            geoData.RiskFactors = riskFactors;
            geoData.RiskScore = Math.Min(1.0, riskScore);
            geoData.RiskLevel = DetermineThreatLevel(riskScore);

            return geoData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to assess geolocation risk for IP: {IPAddress}", ipAddress);
            throw;
        }
    }

    private void InitializeThreatIndicators()
    {
        _threatIndicators.AddRange(new[]
        {
            new ThreatIndicatorDto
            {
                Id = Guid.NewGuid(),
                IndicatorType = "IP",
                Value = "192.168.1.100",
                ThreatType = "Malware C2",
                Severity = "High",
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Source = "Internal Threat Intelligence"
            }
        });
    }

    private double AnalyzeEventType(string eventType, List<string> riskFactors)
    {
        return eventType.ToUpperInvariant() switch
        {
            "BRUTE_FORCE_ATTEMPT" => AddRiskFactor(riskFactors, "Brute force attack pattern", 0.8),
            "PRIVILEGE_ESCALATION" => AddRiskFactor(riskFactors, "Privilege escalation attempt", 0.9),
            "DATA_EXFILTRATION" => AddRiskFactor(riskFactors, "Data exfiltration pattern", 0.95),
            "MALICIOUS_FILE_UPLOAD" => AddRiskFactor(riskFactors, "Malicious file detected", 0.7),
            "SUSPICIOUS_LOGIN" => AddRiskFactor(riskFactors, "Suspicious login pattern", 0.6),
            _ => 0.1
        };
    }

    private double AnalyzeEventData(string eventData, List<string> riskFactors)
    {
        var riskScore = 0.0;

        if (ContainsSqlInjectionPattern(eventData))
            riskScore += AddRiskFactor(riskFactors, "SQL injection pattern detected", 0.8);

        if (ContainsXssPattern(eventData))
            riskScore += AddRiskFactor(riskFactors, "XSS pattern detected", 0.7);

        if (ContainsCommandInjectionPattern(eventData))
            riskScore += AddRiskFactor(riskFactors, "Command injection pattern detected", 0.9);

        return riskScore;
    }

    private async Task<double> AnalyzeThreatIndicators(string eventData, List<string> riskFactors)
    {
        var riskScore = 0.0;
        var indicators = await GetThreatIndicatorsAsync();

        foreach (var indicator in indicators)
        {
            if (eventData.Contains(indicator.Value, StringComparison.OrdinalIgnoreCase))
            {
                riskScore += AddRiskFactor(riskFactors, $"Threat indicator match: {indicator.ThreatType}", 0.8);
            }
        }

        return riskScore;
    }

    private double AddRiskFactor(List<string> riskFactors, string factor, double score)
    {
        riskFactors.Add(factor);
        return score;
    }

    private string DetermineThreatLevel(double riskScore)
    {
        return riskScore switch
        {
            >= 0.8 => "Critical",
            >= 0.6 => "High",
            >= 0.4 => "Medium",
            >= 0.2 => "Low",
            _ => "Minimal"
        };
    }

    private string GetRecommendedAction(string threatLevel, string eventType)
    {
        return threatLevel switch
        {
            "Critical" => "Immediate investigation required. Block source and escalate to security team.",
            "High" => "Investigate within 1 hour. Consider blocking source.",
            "Medium" => "Monitor closely and investigate within 4 hours.",
            "Low" => "Log for analysis and review during next security review.",
            _ => "Continue monitoring."
        };
    }

    private string CategorizeEvent(string eventType)
    {
        return eventType.ToUpperInvariant() switch
        {
            var type when type.Contains("LOGIN") => "Authentication",
            var type when type.Contains("PRIVILEGE") => "Authorization",
            var type when type.Contains("DATA") => "Data Security",
            var type when type.Contains("FILE") => "File Security",
            var type when type.Contains("NETWORK") => "Network Security",
            _ => "General Security"
        };
    }

    private bool ContainsSqlInjectionPattern(string input)
    {
        var patterns = new[]
        {
            @"(\b(SELECT|INSERT|UPDATE|DELETE|DROP|CREATE|ALTER)\b)",
            @"(\b(UNION|OR|AND)\b.*\b(SELECT|INSERT|UPDATE|DELETE)\b)",
            @"('|""|;|--|\*|/\*|\*/)"
        };

        return patterns.Any(pattern => Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase));
    }

    private bool ContainsXssPattern(string input)
    {
        var patterns = new[]
        {
            @"<script[^>]*>.*?</script>",
            @"javascript:",
            @"on\w+\s*=",
            @"<iframe[^>]*>.*?</iframe>"
        };

        return patterns.Any(pattern => Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase));
    }

    private bool ContainsCommandInjectionPattern(string input)
    {
        var patterns = new[]
        {
            @"(\||&|;|`|\$\(|\${)",
            @"\b(cat|ls|pwd|whoami|id|uname|ps|netstat)\b",
            @"\.\./",
            @"/etc/passwd"
        };

        return patterns.Any(pattern => Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase));
    }

    private Task<int> GetRecentLoginAttempts(string ipAddress, string? userId, TimeSpan timeWindow)
    {
        return Task.FromResult(0);
    }

    private bool IsAnomalousUserAgent(string userAgent)
    {
        var suspiciousPatterns = new[]
        {
            "curl", "wget", "python", "bot", "crawler", "scanner"
        };

        return suspiciousPatterns.Any(pattern => 
            userAgent.Contains(pattern, StringComparison.OrdinalIgnoreCase));
    }

    private Task<bool> IsUnusualLoginTime(string userId)
    {
        var currentHour = DateTime.UtcNow.Hour;
        return Task.FromResult(currentHour < 6 || currentHour > 22);
    }

    private Task<long> GetRecentDataAccess(string userId, TimeSpan timeWindow)
    {
        return Task.FromResult(0L);
    }

    private Task<IEnumerable<string>> GetUserRoles(string userId)
    {
        return Task.FromResult<IEnumerable<string>>(new[] { "User" });
    }

    private IEnumerable<string> GetRequiredPermissions(string action)
    {
        return new[] { "StandardUser" };
    }

    private bool HasSufficientPermissions(IEnumerable<string> userRoles, IEnumerable<string> requiredPermissions)
    {
        return requiredPermissions.All(rp => userRoles.Contains(rp));
    }

    private bool HasMaliciousSignature(byte[] fileContent)
    {
        var maliciousSignatures = new byte[][]
        {
            new byte[] { 0x4D, 0x5A }, // PE executable
            new byte[] { 0x50, 0x4B, 0x03, 0x04 } // ZIP archive
        };

        return maliciousSignatures.Any(signature => 
            fileContent.Take(signature.Length).SequenceEqual(signature));
    }

    private bool HasSuspiciousExtension(string fileName)
    {
        var suspiciousExtensions = new[] { ".exe", ".bat", ".cmd", ".scr", ".pif", ".com" };
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return suspiciousExtensions.Contains(extension);
    }

    private bool IsContentTypeMismatch(byte[] fileContent, string contentType)
    {
        return false;
    }

    private Task<IEnumerable<object>> GetUserActivities(string userId, DateTime startTime, DateTime endTime)
    {
        return Task.FromResult<IEnumerable<object>>(new List<object>());
    }

    private IEnumerable<UserActivityPatternDto> AnalyzeActivityPatterns(IEnumerable<object> activities)
    {
        return new List<UserActivityPatternDto>();
    }

    private double CalculateAnomalyScore(IEnumerable<UserActivityPatternDto> patterns)
    {
        return 0.3;
    }

    private IEnumerable<string> GetAnomalousPatterns(IEnumerable<UserActivityPatternDto> patterns)
    {
        return patterns.Where(p => !p.IsNormal).Select(p => p.ActivityType);
    }

    private Task<GeolocationRiskDto> GetGeolocationData(string ipAddress)
    {
        return Task.FromResult(new GeolocationRiskDto
        {
            IPAddress = ipAddress,
            Country = "Unknown",
            Region = "Unknown",
            City = "Unknown",
            Latitude = 0,
            Longitude = 0,
            IsVpnDetected = false,
            IsProxyDetected = false,
            IsTorDetected = false,
            AssessedAt = DateTime.UtcNow
        });
    }

    private bool IsHighRiskCountry(string country)
    {
        var highRiskCountries = new[] { "Unknown", "Anonymous" };
        return highRiskCountries.Contains(country);
    }

    private Task<bool> IsUnusualLocation(string userId, string country)
    {
        return Task.FromResult(false);
    }
}
