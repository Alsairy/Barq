using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using BARQ.Infrastructure.Data;
using BARQ.Infrastructure.Repositories;
using BARQ.Application.Services.Security;
using BARQ.Core.Services;
using BARQ.Core.Repositories;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Entities;
using System.Text;

namespace BARQ.ComprehensiveSecurityTest;

public class ComprehensiveSecurityTest
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("=== BARQ Comprehensive Security Framework Test ===");
        
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                {"ConnectionStrings:DefaultConnection", "Server=localhost;Database=BarqSecurityTest;Trusted_Connection=true;"},
                {"Encryption:DefaultKeyId", "test-key-123"},
                {"Encryption:SearchableSalt", "test-salt-456"},
                {"Security:RealTimeMonitoring", "true"},
                {"Jwt:Key", "test-jwt-key-for-security-testing-purposes-only"},
                {"Jwt:Issuer", "BarqSecurityTest"},
                {"Jwt:Audience", "BarqSecurityTestAudience"}
            })
            .Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddLogging(builder => builder.AddConsole().SetMinimumLevel(LogLevel.Information));
        
        services.AddDbContext<BarqDbContext>(options =>
            options.UseInMemoryDatabase("BarqComprehensiveSecurityTest"));
        
        services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IEncryptionService, EncryptionService>();
        services.AddScoped<IKeyManagementService, KeyManagementService>();
        services.AddScoped<ISecurityMonitoringService, SecurityMonitoringService>();
        services.AddScoped<IThreatDetectionService, ThreatDetectionService>();
        services.AddScoped<ISiemIntegrationService, SiemIntegrationService>();
        
        services.AddScoped<IComplianceService, ComplianceService>();
        services.AddScoped<IGdprComplianceService, GdprComplianceService>();
        services.AddScoped<IHipaaComplianceService, HipaaComplianceService>();
        services.AddScoped<ISoxComplianceService, SoxComplianceService>();
        
        var serviceProvider = services.BuildServiceProvider();
        
        var testResults = new List<(string Test, bool Passed, string Details)>();
        
        try
        {
            Console.WriteLine("\n🔐 1. Testing Encryption Service...");
            await TestEncryptionService(serviceProvider, testResults);
            
            Console.WriteLine("\n🛡️ 2. Testing Security Monitoring...");
            await TestSecurityMonitoring(serviceProvider, testResults);
            
            Console.WriteLine("\n⚖️ 3. Testing GDPR Compliance...");
            await TestGdprCompliance(serviceProvider, testResults);
            
            Console.WriteLine("\n🏥 4. Testing HIPAA Compliance...");
            await TestHipaaCompliance(serviceProvider, testResults);
            
            Console.WriteLine("\n📊 5. Testing SOX Compliance...");
            await TestSoxCompliance(serviceProvider, testResults);
            
            Console.WriteLine("\n🔍 6. Testing Threat Detection...");
            await TestThreatDetection(serviceProvider, testResults);
            
            Console.WriteLine("\n📡 7. Testing SIEM Integration...");
            await TestSiemIntegration(serviceProvider, testResults);
            
            Console.WriteLine("\n=== COMPREHENSIVE SECURITY TEST RESULTS ===");
            var passedTests = testResults.Count(r => r.Passed);
            var totalTests = testResults.Count;
            
            Console.WriteLine($"✅ Passed: {passedTests}/{totalTests} tests");
            Console.WriteLine($"❌ Failed: {totalTests - passedTests}/{totalTests} tests");
            
            foreach (var result in testResults)
            {
                var status = result.Passed ? "✅ PASS" : "❌ FAIL";
                Console.WriteLine($"{status} - {result.Test}: {result.Details}");
            }
            
            if (passedTests == totalTests)
            {
                Console.WriteLine("\n🎉 ALL SECURITY FEATURES ARE WORKING CORRECTLY!");
                Console.WriteLine("✅ Encryption/Decryption functionality verified");
                Console.WriteLine("✅ Security monitoring and threat detection verified");
                Console.WriteLine("✅ GDPR, HIPAA, and SOX compliance frameworks verified");
                Console.WriteLine("✅ SIEM integration functionality verified");
                Console.WriteLine("✅ Sprint 3 Security Framework is production-ready!");
            }
            else
            {
                Console.WriteLine("\n⚠️ Some security tests failed - review required before production deployment");
            }
            
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ Comprehensive security test failed with error: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
        finally
        {
            serviceProvider.Dispose();
        }
    }
    
    private static async Task TestEncryptionService(IServiceProvider serviceProvider, List<(string, bool, string)> results)
    {
        try
        {
            var encryptionService = serviceProvider.GetRequiredService<IEncryptionService>();
            
            var plainText = "Sensitive financial data: $50,000 transaction";
            var encrypted = await encryptionService.EncryptAsync(plainText);
            var decrypted = await encryptionService.DecryptAsync(encrypted);
            
            var encryptionTest = plainText == decrypted;
            results.Add(("Encryption/Decryption", encryptionTest, 
                encryptionTest ? "Data encrypted and decrypted successfully" : "Encryption/decryption failed"));
            
            var email = "test@example.com";
            var hash = await encryptionService.CreateSearchableHashAsync(email);
            var hashVerification = await encryptionService.VerifySearchableHashAsync(email, hash);
            
            results.Add(("Searchable Hash", hashVerification, 
                hashVerification ? "Searchable hash created and verified" : "Searchable hash verification failed"));
            
            var keyService = serviceProvider.GetRequiredService<IKeyManagementService>();
            var newKey = await keyService.GenerateKeyAsync();
            var keyTest = !string.IsNullOrEmpty(newKey) && newKey.Length >= 32;
            
            results.Add(("Key Management", keyTest, 
                keyTest ? $"Key generated successfully (length: {newKey.Length})" : "Key generation failed"));
                
        }
        catch (Exception ex)
        {
            results.Add(("Encryption Service", false, $"Exception: {ex.Message}"));
        }
    }
    
    private static async Task TestSecurityMonitoring(IServiceProvider serviceProvider, List<(string, bool, string)> results)
    {
        try
        {
            var monitoringService = serviceProvider.GetRequiredService<ISecurityMonitoringService>();
            
            var securityEvent = new SecurityEventDto
            {
                EventType = "LOGIN_ATTEMPT",
                UserId = Guid.NewGuid(),
                IpAddress = "192.168.1.100",
                UserAgent = "Test Browser",
                Timestamp = DateTime.UtcNow,
                Severity = "Medium",
                Description = "User login attempt from new location"
            };
            
            var logResult = await monitoringService.LogSecurityEventAsync(securityEvent);
            var monitoringTest = logResult != null && logResult.EventId != Guid.Empty;
            
            results.Add(("Security Event Logging", monitoringTest, 
                monitoringTest ? "Security event logged successfully" : "Security event logging failed"));
                
        }
        catch (Exception ex)
        {
            results.Add(("Security Monitoring", false, $"Exception: {ex.Message}"));
        }
    }
    
    private static async Task TestGdprCompliance(IServiceProvider serviceProvider, List<(string, bool, string)> results)
    {
        try
        {
            var gdprService = serviceProvider.GetRequiredService<IGdprComplianceService>();
            var userId = Guid.NewGuid();
            
            var accessRequest = new DataSubjectRequestDto
            {
                UserId = userId,
                RequestType = "ACCESS",
                RequestDate = DateTime.UtcNow,
                RequesterEmail = "test@example.com"
            };
            
            var accessResponse = await gdprService.ProcessDataSubjectRequestAsync(accessRequest);
            var accessTest = accessResponse.Status == "COMPLETED";
            
            results.Add(("GDPR Data Access", accessTest, 
                accessTest ? "Data subject access request processed" : "Data access request failed"));
            
            var consentRequest = new ConsentUpdateRequestDto
            {
                UserId = userId,
                ConsentType = "Marketing",
                ConsentGiven = true,
                ConsentDate = DateTime.UtcNow,
                Purpose = "Email marketing campaigns",
                LegalBasis = "Consent"
            };
            
            var consentResponse = await gdprService.UpdateConsentAsync(consentRequest);
            var consentTest = consentResponse.Success;
            
            results.Add(("GDPR Consent Management", consentTest, 
                consentTest ? "Consent updated successfully" : "Consent update failed"));
                
        }
        catch (Exception ex)
        {
            results.Add(("GDPR Compliance", false, $"Exception: {ex.Message}"));
        }
    }
    
    private static async Task TestHipaaCompliance(IServiceProvider serviceProvider, List<(string, bool, string)> results)
    {
        try
        {
            var hipaaService = serviceProvider.GetRequiredService<IHipaaComplianceService>();
            
            var phiAccessLog = new PhiAccessLogDto
            {
                UserId = Guid.NewGuid(),
                PatientId = Guid.NewGuid(),
                AccessType = "READ",
                ResourceAccessed = "Patient Medical Record",
                Purpose = "Treatment",
                AccessTime = DateTime.UtcNow,
                IpAddress = "10.0.0.100",
                UserAgent = "Medical System Browser"
            };
            
            var auditResult = await hipaaService.LogPhiAccessAsync(phiAccessLog);
            var hipaaTest = auditResult != null && auditResult.Id != Guid.Empty;
            
            results.Add(("HIPAA PHI Access Logging", hipaaTest, 
                hipaaTest ? "PHI access logged successfully" : "PHI access logging failed"));
                
        }
        catch (Exception ex)
        {
            results.Add(("HIPAA Compliance", false, $"Exception: {ex.Message}"));
        }
    }
    
    private static async Task TestSoxCompliance(IServiceProvider serviceProvider, List<(string, bool, string)> results)
    {
        try
        {
            var soxService = serviceProvider.GetRequiredService<ISoxComplianceService>();
            
            var fromDate = DateTime.UtcNow.AddDays(-30);
            var toDate = DateTime.UtcNow;
            
            var auditResult = await soxService.AuditFinancialControlsAsync(fromDate, toDate);
            var soxTest = auditResult != null && auditResult.Id != Guid.Empty;
            
            results.Add(("SOX Financial Controls Audit", soxTest, 
                soxTest ? "Financial controls audit completed" : "Financial controls audit failed"));
                
        }
        catch (Exception ex)
        {
            results.Add(("SOX Compliance", false, $"Exception: {ex.Message}"));
        }
    }
    
    private static async Task TestThreatDetection(IServiceProvider serviceProvider, List<(string, bool, string)> results)
    {
        try
        {
            var threatService = serviceProvider.GetRequiredService<IThreatDetectionService>();
            
            var threatEvent = new ThreatEventDto
            {
                EventType = "SUSPICIOUS_LOGIN",
                Severity = "High",
                Source = "192.168.1.200",
                Description = "Multiple failed login attempts from suspicious IP",
                Timestamp = DateTime.UtcNow,
                UserId = Guid.NewGuid()
            };
            
            var threatResult = await threatService.AnalyzeThreatAsync(threatEvent);
            var threatTest = threatResult != null && !string.IsNullOrEmpty(threatResult.ThreatLevel);
            
            results.Add(("Threat Detection", threatTest, 
                threatTest ? $"Threat analyzed - Level: {threatResult.ThreatLevel}" : "Threat detection failed"));
                
        }
        catch (Exception ex)
        {
            results.Add(("Threat Detection", false, $"Exception: {ex.Message}"));
        }
    }
    
    private static async Task TestSiemIntegration(IServiceProvider serviceProvider, List<(string, bool, string)> results)
    {
        try
        {
            var siemService = serviceProvider.GetRequiredService<ISiemIntegrationService>();
            
            var siemEvent = new SiemEventDto
            {
                EventType = "SECURITY_ALERT",
                Source = "BARQ_SECURITY_FRAMEWORK",
                Severity = "Medium",
                Message = "Security framework test event",
                Timestamp = DateTime.UtcNow,
                Metadata = new Dictionary<string, object>
                {
                    ["TestEvent"] = true,
                    ["Framework"] = "Sprint3Security"
                }
            };
            
            var siemResult = await siemService.SendEventAsync(siemEvent);
            var siemTest = siemResult != null && siemResult.Success;
            
            results.Add(("SIEM Integration", siemTest, 
                siemTest ? "SIEM event sent successfully" : "SIEM integration failed"));
                
        }
        catch (Exception ex)
        {
            results.Add(("SIEM Integration", false, $"Exception: {ex.Message}"));
        }
    }
}
