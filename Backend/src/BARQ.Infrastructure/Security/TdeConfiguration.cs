using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BARQ.Infrastructure.Security;

public class TdeConfiguration
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<TdeConfiguration> _logger;

    public TdeConfiguration(IConfiguration configuration, ILogger<TdeConfiguration> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<bool> EnableTdeAsync(string databaseName)
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                _logger.LogError("Database connection string not found");
                return false;
            }

            _logger.LogInformation("TDE configuration initiated for database: {DatabaseName}", databaseName);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to enable TDE for database: {DatabaseName}", databaseName);
            return false;
        }
    }

    public string GetTdeConfigurationScript(string databaseName)
    {
        return $@"
-- Enable TDE for {databaseName}
-- Step 1: Create Database Master Key
USE master;
IF NOT EXISTS (SELECT * FROM sys.symmetric_keys WHERE name = '##MS_DatabaseMasterKey##')
BEGIN
    CREATE MASTER KEY ENCRYPTION BY PASSWORD = 'BarqTDE_MasterKey_2024!';
END

-- Step 2: Create Certificate
IF NOT EXISTS (SELECT * FROM sys.certificates WHERE name = 'BarqTDECert')
BEGIN
    CREATE CERTIFICATE BarqTDECert
    WITH SUBJECT = 'BARQ TDE Certificate',
    EXPIRY_DATE = '2025-12-31';
END

-- Step 3: Backup Certificate (Important for recovery)
BACKUP CERTIFICATE BarqTDECert
TO FILE = 'C:\\TDE\\BarqTDECert.cer'
WITH PRIVATE KEY (
    FILE = 'C:\\TDE\\BarqTDECert.pvk',
    ENCRYPTION BY PASSWORD = 'BarqTDE_CertKey_2024!'
);

-- Step 4: Create Database Encryption Key
USE [{databaseName}];
IF NOT EXISTS (SELECT * FROM sys.dm_database_encryption_keys WHERE database_id = DB_ID())
BEGIN
    CREATE DATABASE ENCRYPTION KEY
    WITH ALGORITHM = AES_256
    ENCRYPTION BY SERVER CERTIFICATE BarqTDECert;
END

-- Step 5: Enable TDE
ALTER DATABASE [{databaseName}] SET ENCRYPTION ON;

-- Step 6: Monitor encryption progress
-- SELECT 
--     db_name(database_id) as DatabaseName,
--     encryption_state,
--     encryption_state_desc,
--     percent_complete,
--     key_algorithm,
--     key_length
-- FROM sys.dm_database_encryption_keys;
";
    }

    public string GetTdeMonitoringScript()
    {
        return @"
-- Monitor TDE encryption status
SELECT 
    db_name(database_id) as DatabaseName,
    encryption_state,
    CASE encryption_state
        WHEN 0 THEN 'No database encryption key present, no encryption'
        WHEN 1 THEN 'Unencrypted'
        WHEN 2 THEN 'Encryption in progress'
        WHEN 3 THEN 'Encrypted'
        WHEN 4 THEN 'Key change in progress'
        WHEN 5 THEN 'Decryption in progress'
        WHEN 6 THEN 'Protection change in progress'
    END as encryption_state_desc,
    percent_complete,
    key_algorithm,
    key_length,
    encryptor_thumbprint,
    encryptor_type
FROM sys.dm_database_encryption_keys;

-- Check certificate expiration
SELECT 
    name,
    certificate_id,
    principal_id,
    pvt_key_encryption_type_desc,
    is_active_for_begin_dialog,
    issuer_name,
    cert_serial_number,
    thumbprint,
    subject,
    expiry_date,
    start_date
FROM sys.certificates 
WHERE name = 'BarqTDECert';
";
    }

    public async Task<bool> RotateTdeCertificateAsync(string databaseName)
    {
        try
        {
            _logger.LogInformation("Initiating TDE certificate rotation for database: {DatabaseName}", databaseName);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to rotate TDE certificate for database: {DatabaseName}", databaseName);
            return false;
        }
    }

    public string GetCertificateRotationScript(string databaseName)
    {
        var newCertName = $"BarqTDECert_{DateTime.UtcNow:yyyyMMdd}";
        return $@"
-- TDE Certificate Rotation for {databaseName}
USE master;

-- Step 1: Create new certificate
CREATE CERTIFICATE {newCertName}
WITH SUBJECT = 'BARQ TDE Certificate - Rotated {DateTime.UtcNow:yyyy-MM-dd}',
EXPIRY_DATE = '2025-12-31';

-- Step 2: Backup new certificate
BACKUP CERTIFICATE {newCertName}
TO FILE = 'C:\\TDE\\{newCertName}.cer'
WITH PRIVATE KEY (
    FILE = 'C:\\TDE\\{newCertName}.pvk',
    ENCRYPTION BY PASSWORD = 'BarqTDE_CertKey_2024!'
);

-- Step 3: Change database encryption key to use new certificate
USE [{databaseName}];
ALTER DATABASE ENCRYPTION KEY
REGENERATE WITH ALGORITHM = AES_256
ENCRYPTION BY SERVER CERTIFICATE {newCertName};

-- Step 4: Monitor the key change progress
-- SELECT 
--     db_name(database_id) as DatabaseName,
--     encryption_state,
--     encryption_state_desc,
--     percent_complete
-- FROM sys.dm_database_encryption_keys
-- WHERE database_id = DB_ID();

-- Step 5: After completion, drop old certificate (MANUAL STEP)
-- DROP CERTIFICATE BarqTDECert;
";
    }
}
