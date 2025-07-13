using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Text;
using BARQ.Core.Services;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Repositories;
using BARQ.Core.Entities;

namespace BARQ.Application.Services.Security;

public class EncryptionService : IEncryptionService
{
    private readonly IKeyManagementService _keyManagementService;
    private readonly IRepository<AuditLog> _auditRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EncryptionService> _logger;
    private readonly string _defaultKeyId;

    public EncryptionService(
        IKeyManagementService keyManagementService,
        IRepository<AuditLog> auditRepository,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        ILogger<EncryptionService> logger)
    {
        _keyManagementService = keyManagementService;
        _auditRepository = auditRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
        _defaultKeyId = configuration["Encryption:DefaultKeyId"] ?? "default-key";
    }

    public async Task<string> EncryptAsync(string plainText, string? keyId = null)
    {
        try
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            keyId ??= await _keyManagementService.GetCurrentKeyIdAsync();
            var key = await _keyManagementService.GetKeyAsync(keyId);

            using var aes = Aes.Create();
            aes.Key = Convert.FromBase64String(key);
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            var result = Convert.ToBase64String(aes.IV.Concat(encryptedBytes).ToArray());
            
            await LogEncryptionOperationAsync("ENCRYPT", "String", Guid.Empty, keyId);
            return $"{keyId}:{result}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Encryption failed for keyId: {KeyId}", keyId);
            await LogEncryptionOperationAsync("ENCRYPT_FAILED", "String", Guid.Empty, keyId);
            throw;
        }
    }

    public async Task<string> DecryptAsync(string encryptedText, string? keyId = null)
    {
        try
        {
            if (string.IsNullOrEmpty(encryptedText))
                return string.Empty;

            var parts = encryptedText.Split(':', 2);
            if (parts.Length == 2)
            {
                keyId = parts[0];
                encryptedText = parts[1];
            }

            keyId ??= await _keyManagementService.GetCurrentKeyIdAsync();
            var key = await _keyManagementService.GetKeyAsync(keyId);

            var encryptedData = Convert.FromBase64String(encryptedText);
            var iv = encryptedData.Take(16).ToArray();
            var cipherText = encryptedData.Skip(16).ToArray();

            using var aes = Aes.Create();
            aes.Key = Convert.FromBase64String(key);
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            var decryptedBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
            
            await LogEncryptionOperationAsync("DECRYPT", "String", Guid.Empty, keyId);
            return Encoding.UTF8.GetString(decryptedBytes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Decryption failed for keyId: {KeyId}", keyId);
            await LogEncryptionOperationAsync("DECRYPT_FAILED", "String", Guid.Empty, keyId);
            throw;
        }
    }

    public async Task<byte[]> EncryptBytesAsync(byte[] plainBytes, string? keyId = null)
    {
        try
        {
            if (plainBytes == null || plainBytes.Length == 0)
                return Array.Empty<byte>();

            keyId ??= await _keyManagementService.GetCurrentKeyIdAsync();
            var key = await _keyManagementService.GetKeyAsync(keyId);

            using var aes = Aes.Create();
            aes.Key = Convert.FromBase64String(key);
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            await LogEncryptionOperationAsync("ENCRYPT_BYTES", "Bytes", Guid.Empty, keyId);
            return aes.IV.Concat(encryptedBytes).ToArray();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Byte encryption failed for keyId: {KeyId}", keyId);
            await LogEncryptionOperationAsync("ENCRYPT_BYTES_FAILED", "Bytes", Guid.Empty, keyId);
            throw;
        }
    }

    public async Task<byte[]> DecryptBytesAsync(byte[] encryptedBytes, string? keyId = null)
    {
        try
        {
            if (encryptedBytes == null || encryptedBytes.Length == 0)
                return Array.Empty<byte>();

            keyId ??= await _keyManagementService.GetCurrentKeyIdAsync();
            var key = await _keyManagementService.GetKeyAsync(keyId);

            var iv = encryptedBytes.Take(16).ToArray();
            var cipherText = encryptedBytes.Skip(16).ToArray();

            using var aes = Aes.Create();
            aes.Key = Convert.FromBase64String(key);
            aes.IV = iv;

            using var decryptor = aes.CreateDecryptor();
            var decryptedBytes = decryptor.TransformFinalBlock(cipherText, 0, cipherText.Length);
            
            await LogEncryptionOperationAsync("DECRYPT_BYTES", "Bytes", Guid.Empty, keyId);
            return decryptedBytes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Byte decryption failed for keyId: {KeyId}", keyId);
            await LogEncryptionOperationAsync("DECRYPT_BYTES_FAILED", "Bytes", Guid.Empty, keyId);
            throw;
        }
    }

    public async Task<string> EncryptFieldAsync<T>(T entity, string fieldName, string plainText) where T : class
    {
        var entityType = typeof(T).Name;
        var entityId = GetEntityId(entity);
        
        try
        {
            var encrypted = await EncryptAsync(plainText);
            await LogEncryptionOperationAsync("ENCRYPT_FIELD", entityType, entityId);
            return encrypted;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Field encryption failed for {EntityType}.{FieldName}", entityType, fieldName);
            await LogEncryptionOperationAsync("ENCRYPT_FIELD_FAILED", entityType, entityId);
            throw;
        }
    }

    public async Task<string> DecryptFieldAsync<T>(T entity, string fieldName, string encryptedText) where T : class
    {
        var entityType = typeof(T).Name;
        var entityId = GetEntityId(entity);
        
        try
        {
            var decrypted = await DecryptAsync(encryptedText);
            await LogEncryptionOperationAsync("DECRYPT_FIELD", entityType, entityId);
            return decrypted;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Field decryption failed for {EntityType}.{FieldName}", entityType, fieldName);
            await LogEncryptionOperationAsync("DECRYPT_FIELD_FAILED", entityType, entityId);
            throw;
        }
    }

    public async Task<string> CreateSearchableHashAsync(string plainText)
    {
        try
        {
            if (string.IsNullOrEmpty(plainText))
                return string.Empty;

            var salt = _configuration["Encryption:SearchableSalt"] ?? "default-searchable-salt";
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(salt));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(plainText.ToLowerInvariant()));
            
            await LogEncryptionOperationAsync("CREATE_SEARCHABLE_HASH", "String", Guid.Empty);
            return Convert.ToBase64String(hash);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Searchable hash creation failed");
            await LogEncryptionOperationAsync("CREATE_SEARCHABLE_HASH_FAILED", "String", Guid.Empty);
            throw;
        }
    }

    public async Task<bool> VerifySearchableHashAsync(string plainText, string hash)
    {
        try
        {
            var computedHash = await CreateSearchableHashAsync(plainText);
            var result = computedHash.Equals(hash, StringComparison.Ordinal);
            
            await LogEncryptionOperationAsync("VERIFY_SEARCHABLE_HASH", "String", Guid.Empty);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Searchable hash verification failed");
            await LogEncryptionOperationAsync("VERIFY_SEARCHABLE_HASH_FAILED", "String", Guid.Empty);
            return false;
        }
    }

    public async Task<EncryptionAuditDto> LogEncryptionOperationAsync(string operation, string entityType, Guid entityId, string? keyId = null)
    {
        try
        {
            var auditEntry = new AuditLog
            {
                Id = Guid.NewGuid(),
                EntityName = "EncryptionOperation",
                EntityId = entityId,
                Action = operation,
                OldValues = null,
                NewValues = $"EntityType: {entityType}, KeyId: {keyId}",
                IPAddress = "System",
                UserAgent = "EncryptionService",
                UserId = Guid.Empty,
                CreatedAt = DateTime.UtcNow,
                TenantId = Guid.Empty
            };

            await _auditRepository.AddAsync(auditEntry);
            await _unitOfWork.SaveChangesAsync();

            return new EncryptionAuditDto
            {
                Id = auditEntry.Id,
                Operation = operation,
                EntityType = entityType,
                EntityId = entityId,
                KeyId = keyId,
                Timestamp = auditEntry.CreatedAt,
                UserId = auditEntry.UserId,
                IPAddress = auditEntry.IPAddress,
                UserAgent = auditEntry.UserAgent,
                Success = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to log encryption operation: {Operation}", operation);
            return new EncryptionAuditDto
            {
                Id = Guid.NewGuid(),
                Operation = operation,
                EntityType = entityType,
                EntityId = entityId,
                KeyId = keyId,
                Timestamp = DateTime.UtcNow,
                UserId = Guid.Empty,
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    public async Task<string> GenerateDataEncryptionKeyAsync()
    {
        try
        {
            using var aes = Aes.Create();
            aes.GenerateKey();
            var keyBase64 = Convert.ToBase64String(aes.Key);
            
            await LogEncryptionOperationAsync("GENERATE_DEK", "Key", Guid.Empty);
            return keyBase64;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Data encryption key generation failed");
            await LogEncryptionOperationAsync("GENERATE_DEK_FAILED", "Key", Guid.Empty);
            throw;
        }
    }

    public async Task<string> RotateKeyAsync(string currentKeyId)
    {
        try
        {
            var rotationSuccess = await _keyManagementService.RotateKeyAsync(currentKeyId);
            await LogEncryptionOperationAsync("ROTATE_KEY", "Key", Guid.Empty, currentKeyId);
            return rotationSuccess ? "Key rotation successful" : "Key rotation failed";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Key rotation failed for keyId: {KeyId}", currentKeyId);
            await LogEncryptionOperationAsync("ROTATE_KEY_FAILED", "Key", Guid.Empty, currentKeyId);
            throw;
        }
    }

    public async Task<bool> ValidateKeyAsync(string keyId)
    {
        try
        {
            var isValid = await _keyManagementService.ValidateKeyAsync(keyId);
            await LogEncryptionOperationAsync("VALIDATE_KEY", "Key", Guid.Empty, keyId);
            return isValid;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Key validation failed for keyId: {KeyId}", keyId);
            await LogEncryptionOperationAsync("VALIDATE_KEY_FAILED", "Key", Guid.Empty, keyId);
            return false;
        }
    }

    private static Guid GetEntityId<T>(T entity) where T : class
    {
        var idProperty = typeof(T).GetProperty("Id");
        if (idProperty?.GetValue(entity) is Guid id)
            return id;
        return Guid.Empty;
    }
}
