# BARQ Security Framework Documentation

## Overview

This document provides comprehensive documentation for the security framework implemented in Sprint 3 of the BARQ project. The security framework includes data encryption and protection, security monitoring and threat detection, compliance framework implementation, and security hardening with best practices.

## Table of Contents

1. [Data Encryption and Protection](#data-encryption-and-protection)
2. [Security Monitoring and Threat Detection](#security-monitoring-and-threat-detection)
3. [Compliance Framework](#compliance-framework)
4. [Security Hardening and Best Practices](#security-hardening-and-best-practices)
5. [Configuration](#configuration)
6. [API Reference](#api-reference)

## Data Encryption and Protection

### Overview
The BARQ platform implements comprehensive data encryption and protection mechanisms to secure sensitive data both at rest and in transit.

### Features

#### 1. Application-Level Field Encryption
- **Service**: `EncryptionService`
- **Algorithm**: AES-256 encryption
- **Key Management**: Integrated with Azure Key Vault/AWS KMS
- **Usage**: Encrypt/decrypt sensitive fields in entities

```csharp
// Example usage
var encryptedData = await encryptionService.EncryptAsync("sensitive data");
var decryptedData = await encryptionService.DecryptAsync(encryptedData);
```

#### 2. Searchable Hash Generation
- **Purpose**: Enable searching on encrypted fields
- **Algorithm**: HMAC-SHA256 with configurable salt
- **Usage**: Create searchable hashes for encrypted email addresses, usernames, etc.

```csharp
// Example usage
var searchableHash = await encryptionService.CreateSearchableHashAsync("user@example.com");
var isMatch = await encryptionService.VerifySearchableHashAsync("user@example.com", searchableHash);
```

#### 3. Key Management
- **Service**: `KeyManagementService`
- **Features**: Key generation, rotation, validation, and secure storage
- **Integration**: Azure Key Vault and AWS KMS support

#### 4. Database Encryption (TDE)
- **Configuration**: `TdeConfiguration`
- **Features**: Transparent Data Encryption for database files
- **Scope**: Encrypts entire database at rest

### Entity Field Encryption

Use the `[EncryptedField]` attribute to mark fields for automatic encryption:

```csharp
public class User : BaseEntity
{
    [EncryptedField]
    public string Email { get; set; }
    
    [EncryptedField]
    public string PhoneNumber { get; set; }
}
```

## Security Monitoring and Threat Detection

### Overview
Comprehensive security monitoring system that tracks, analyzes, and responds to security events in real-time.

### Components

#### 1. Security Monitoring Service
- **Service**: `SecurityMonitoringService`
- **Features**: 
  - Real-time security event logging
  - Security dashboard generation
  - Alert management
  - Metrics collection

#### 2. Threat Detection Service
- **Service**: `ThreatDetectionService`
- **Features**:
  - Behavioral analysis
  - Anomaly detection
  - Risk assessment
  - Threat categorization

#### 3. SIEM Integration Service
- **Service**: `SiemIntegrationService`
- **Features**:
  - Integration with external SIEM systems
  - Event forwarding
  - Alert correlation
  - Compliance reporting

### Security Events

The system monitors and logs the following security events:
- Failed login attempts
- Brute force attacks
- Privilege escalation attempts
- Data access violations
- Suspicious user behavior
- Malicious file uploads

### Security Dashboard

Access comprehensive security metrics through the security dashboard:
- Total security events
- Active threats
- Alert distribution by severity
- Threat trends over time
- Response time metrics

## Compliance Framework

### Overview
Multi-regulatory compliance framework supporting GDPR, HIPAA, and SOX requirements.

### Supported Regulations

#### 1. GDPR Compliance
- **Service**: `GdprComplianceService`
- **Features**:
  - Data subject rights management
  - Consent tracking and management
  - Data portability
  - Right to erasure (right to be forgotten)
  - Privacy impact assessments
  - Data breach reporting

#### 2. HIPAA Compliance
- **Service**: `HipaaComplianceService`
- **Features**:
  - PHI (Protected Health Information) access logging
  - Business associate agreements
  - Minimum necessary data validation
  - Security incident reporting
  - Risk assessments

#### 3. SOX Compliance
- **Service**: `SoxComplianceService`
- **Features**:
  - Financial controls auditing
  - Segregation of duties validation
  - Change control logging
  - Internal controls assessment
  - Compliance testing

### Compliance Operations

#### Data Subject Rights (GDPR)
```csharp
// Process data subject request
var request = new DataSubjectRequestDto
{
    RequestType = "ACCESS",
    UserId = "user-id",
    Description = "User requesting access to personal data"
};
var result = await gdprService.ProcessDataSubjectRequestAsync(request);
```

#### PHI Access Logging (HIPAA)
```csharp
// Log PHI access
await hipaaService.LogPhiAccessAsync("user-id", "patient-record-id", "VIEW", "Medical consultation");
```

#### Financial Controls Audit (SOX)
```csharp
// Audit financial controls
var auditResult = await soxService.AuditFinancialControlsAsync("control-id", "user-id");
```

## Security Hardening and Best Practices

### Web Application Firewall (WAF)

#### Features
- **Middleware**: `WafMiddleware`
- **Protection Against**:
  - SQL Injection
  - Cross-Site Scripting (XSS)
  - Command Injection
  - Path Traversal
  - LDAP Injection
  - XML Injection
  - Server-Side Request Forgery (SSRF)

#### Configuration
```json
{
  "Waf": {
    "EnableSqlInjectionProtection": true,
    "EnableXssProtection": true,
    "EnableCommandInjectionProtection": true,
    "EnablePathTraversalProtection": true,
    "MaxRequestSizeBytes": 1048576
  }
}
```

### Security Headers

#### Features
- **Middleware**: `SecurityHeadersMiddleware`
- **Headers Applied**:
  - Content Security Policy (CSP)
  - HTTP Strict Transport Security (HSTS)
  - X-Frame-Options
  - X-Content-Type-Options
  - X-XSS-Protection
  - Referrer-Policy

### Rate Limiting and DDoS Protection

#### Features
- **Middleware**: `RateLimitingMiddleware`
- **Protection**:
  - Request rate limiting per IP
  - Adaptive throttling
  - DDoS attack mitigation
  - Configurable limits per endpoint

#### Configuration
```json
{
  "RateLimit": {
    "EnableRateLimiting": true,
    "RequestsPerMinute": 60,
    "BurstLimit": 10,
    "EnableAdaptiveThrottling": true
  }
}
```

### Input Validation

#### Features
- **Middleware**: `InputValidationMiddleware`
- **Validation**:
  - Header validation
  - Query parameter validation
  - Request body validation
  - Content length limits
  - Malicious pattern detection

## Configuration

### Application Settings

#### Security Configuration
```json
{
  "Security": {
    "RealTimeMonitoring": true,
    "EncryptionKeyRotationDays": 90,
    "SessionTimeoutMinutes": 30,
    "MaxFailedLoginAttempts": 5,
    "AccountLockoutDurationMinutes": 15
  },
  "Encryption": {
    "DefaultKeyId": "barq-encryption-key-2024",
    "SearchableSalt": "barq-searchable-salt-2024",
    "KeyVaultUrl": "https://barq-keyvault.vault.azure.net/",
    "RotationIntervalDays": 90
  },
  "Compliance": {
    "EnableGdprCompliance": true,
    "EnableHipaaCompliance": true,
    "EnableSoxCompliance": true,
    "DataRetentionDays": 2555,
    "AuditLogRetentionDays": 3650
  }
}
```

#### Database Configuration
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=Barq;Trusted_Connection=true;Encrypt=true;TrustServerCertificate=false;"
  },
  "Database": {
    "EnableTDE": true,
    "EncryptionKeyName": "BarqTDEKey",
    "BackupEncryption": true
  }
}
```

### Environment Variables

Required environment variables for production:
- `AZURE_KEY_VAULT_URL`: Azure Key Vault URL for key management
- `ENCRYPTION_MASTER_KEY`: Master encryption key
- `SIEM_ENDPOINT`: SIEM system endpoint for event forwarding
- `COMPLIANCE_REPORTING_ENDPOINT`: Compliance reporting system endpoint

## API Reference

### Encryption Service

#### Methods
- `EncryptAsync(string plainText, string? keyId = null)`: Encrypt plain text
- `DecryptAsync(string encryptedText, string? keyId = null)`: Decrypt encrypted text
- `EncryptFieldAsync<T>(T entity, string fieldName, string plainText)`: Encrypt entity field
- `DecryptFieldAsync<T>(T entity, string fieldName, string encryptedText)`: Decrypt entity field
- `CreateSearchableHashAsync(string plainText)`: Create searchable hash
- `VerifySearchableHashAsync(string plainText, string hash)`: Verify searchable hash

### Security Monitoring Service

#### Methods
- `LogSecurityEventAsync(string eventType, string description, string? userId, string? ipAddress)`: Log security event
- `GetSecurityEventsAsync(DateTime? fromDate, DateTime? toDate, string? eventType)`: Get security events
- `AnalyzeThreatAsync(string eventData, string eventType)`: Analyze threat
- `GetSecurityDashboardAsync(DateTime? fromDate, DateTime? toDate)`: Get security dashboard
- `CreateSecurityAlertAsync(string alertType, string message, string severity, string? userId)`: Create security alert

### Compliance Services

#### GDPR Service Methods
- `ProcessDataSubjectRequestAsync(DataSubjectRequestDto request)`: Process data subject request
- `UpdateConsentAsync(string userId, ConsentUpdateDto consent)`: Update user consent
- `ExportUserDataAsync(string userId)`: Export user data
- `EraseUserDataAsync(string userId, DataErasureRequestDto request)`: Erase user data

#### HIPAA Service Methods
- `LogPhiAccessAsync(string userId, string patientId, string action, string purpose)`: Log PHI access
- `CreateBusinessAssociateAgreementAsync(BusinessAssociateAgreementDto agreement)`: Create BAA
- `ValidateMinimumNecessaryAsync(string userId, string dataType, string purpose)`: Validate minimum necessary
- `ReportSecurityIncidentAsync(SecurityIncidentDto incident)`: Report security incident

#### SOX Service Methods
- `AuditFinancialControlsAsync(string controlId, string auditorId)`: Audit financial controls
- `ValidateSegregationOfDutiesAsync(string userId, string processType)`: Validate segregation of duties
- `LogChangeControlEventAsync(ChangeControlEventDto changeEvent)`: Log change control event
- `AssessInternalControlsAsync(string controlFramework)`: Assess internal controls

## Security Best Practices

### Development Guidelines

1. **Data Classification**: Classify all data according to sensitivity levels
2. **Encryption**: Encrypt all sensitive data at rest and in transit
3. **Access Control**: Implement principle of least privilege
4. **Audit Logging**: Log all security-relevant events
5. **Input Validation**: Validate and sanitize all user inputs
6. **Error Handling**: Implement secure error handling that doesn't leak sensitive information

### Deployment Guidelines

1. **TLS Configuration**: Use TLS 1.3 for all communications
2. **Certificate Management**: Implement proper certificate lifecycle management
3. **Security Headers**: Configure all security headers properly
4. **Rate Limiting**: Implement appropriate rate limiting for all endpoints
5. **Monitoring**: Set up comprehensive security monitoring and alerting

### Maintenance Guidelines

1. **Key Rotation**: Rotate encryption keys according to policy
2. **Security Updates**: Apply security updates promptly
3. **Vulnerability Scanning**: Perform regular vulnerability assessments
4. **Compliance Audits**: Conduct regular compliance audits
5. **Incident Response**: Maintain and test incident response procedures

## Support and Troubleshooting

### Common Issues

#### Encryption Service Issues
- **Key Not Found**: Verify key exists in key management system
- **Decryption Failed**: Check key rotation and version compatibility
- **Performance Issues**: Consider key caching and connection pooling

#### Compliance Issues
- **Audit Trail Gaps**: Verify audit logging is enabled for all operations
- **Data Retention**: Check data retention policies and cleanup procedures
- **Consent Management**: Ensure consent records are properly maintained

#### Security Monitoring Issues
- **Missing Events**: Verify event logging configuration
- **False Positives**: Tune threat detection algorithms
- **Performance Impact**: Optimize monitoring queries and indexing

### Logging and Diagnostics

Security events are logged with the following structure:
```json
{
  "timestamp": "2024-01-15T10:30:00Z",
  "eventType": "SECURITY_EVENT",
  "severity": "HIGH",
  "userId": "user-123",
  "ipAddress": "192.168.1.100",
  "description": "Suspicious login attempt detected",
  "metadata": {
    "userAgent": "Mozilla/5.0...",
    "location": "Unknown",
    "riskScore": 0.85
  }
}
```

For additional support, refer to the application logs and security monitoring dashboard for detailed diagnostics and troubleshooting information.
