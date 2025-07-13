using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using BARQ.Core.Services;
using BARQ.Core.Models.DTOs;
using BARQ.Core.Repositories;
using BARQ.Core.Entities;

namespace BARQ.Application.Services.Security;

public class KeyManagementService : IKeyManagementService
{
    private readonly IRepository<AuditLog> _auditRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;
    private readonly ILogger<KeyManagementService> _logger;
    private readonly Dictionary<string, string> _keyStore;
    private readonly Dictionary<string, KeyInfoDto> _keyMetadata;

    public KeyManagementService(
        IRepository<AuditLog> auditRepository,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        ILogger<KeyManagementService> logger)
    {
        _auditRepository = auditRepository;
        _unitOfWork = unitOfWork;
        _configuration = configuration;
        _logger = logger;
        _keyStore = new Dictionary<string, string>();
        _keyMetadata = new Dictionary<string, KeyInfoDto>();
        
        InitializeDefaultKeys();
    }

    public async Task<string> CreateKeyAsync(string keyName, string keyType = "AES256")
    {
        try
        {
            var keyId = Guid.NewGuid().ToString();
            string keyValue;

            switch (keyType.ToUpperInvariant())
            {
                case "AES256":
                    using (var aes = Aes.Create())
                    {
                        aes.GenerateKey();
                        keyValue = Convert.ToBase64String(aes.Key);
                    }
                    break;
                default:
                    throw new ArgumentException($"Unsupported key type: {keyType}");
            }

            _keyStore[keyId] = keyValue;
            _keyMetadata[keyId] = new KeyInfoDto
            {
                KeyId = keyId,
                KeyName = keyName,
                KeyType = keyType,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Status = "Active"
            };

            await LogKeyUsageAsync(keyId, "CREATE_KEY", "Key", Guid.Empty);
            _logger.LogInformation("Created new key: {KeyId} with name: {KeyName}", keyId, keyName);
            
            return keyId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create key: {KeyName}", keyName);
            throw;
        }
    }

    public async Task<string> GetKeyAsync(string keyId)
    {
        try
        {
            if (!_keyStore.TryGetValue(keyId, out var key))
            {
                throw new KeyNotFoundException($"Key not found: {keyId}");
            }

            if (!_keyMetadata.TryGetValue(keyId, out var metadata) || !metadata.IsActive)
            {
                throw new InvalidOperationException($"Key is not active: {keyId}");
            }

            await LogKeyUsageAsync(keyId, "GET_KEY", "Key", Guid.Empty);
            return key;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get key: {KeyId}", keyId);
            throw;
        }
    }

    public async Task<bool> RotateKeyAsync(string keyId)
    {
        try
        {
            if (!_keyMetadata.TryGetValue(keyId, out var metadata))
            {
                throw new KeyNotFoundException($"Key not found: {keyId}");
            }

            string newKeyValue;
            switch (metadata.KeyType.ToUpperInvariant())
            {
                case "AES256":
                    using (var aes = Aes.Create())
                    {
                        aes.GenerateKey();
                        newKeyValue = Convert.ToBase64String(aes.Key);
                    }
                    break;
                default:
                    throw new ArgumentException($"Unsupported key type for rotation: {metadata.KeyType}");
            }

            _keyStore[keyId] = newKeyValue;
            metadata.LastRotatedAt = DateTime.UtcNow;

            await LogKeyUsageAsync(keyId, "ROTATE_KEY", "Key", Guid.Empty);
            _logger.LogInformation("Rotated key: {KeyId}", keyId);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to rotate key: {KeyId}", keyId);
            return false;
        }
    }

    public async Task<bool> DeleteKeyAsync(string keyId)
    {
        try
        {
            if (!_keyMetadata.ContainsKey(keyId))
            {
                return false;
            }

            _keyStore.Remove(keyId);
            _keyMetadata[keyId].IsActive = false;
            _keyMetadata[keyId].Status = "Deleted";

            await LogKeyUsageAsync(keyId, "DELETE_KEY", "Key", Guid.Empty);
            _logger.LogInformation("Deleted key: {KeyId}", keyId);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete key: {KeyId}", keyId);
            return false;
        }
    }

    public async Task<IEnumerable<KeyInfoDto>> ListKeysAsync()
    {
        try
        {
            await LogKeyUsageAsync("ALL", "LIST_KEYS", "Key", Guid.Empty);
            return _keyMetadata.Values.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to list keys");
            throw;
        }
    }

    public async Task<bool> ValidateKeyAsync(string keyId)
    {
        try
        {
            var exists = _keyStore.ContainsKey(keyId) && 
                        _keyMetadata.TryGetValue(keyId, out var metadata) && 
                        metadata.IsActive;

            await LogKeyUsageAsync(keyId, "VALIDATE_KEY", "Key", Guid.Empty);
            return exists;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to validate key: {KeyId}", keyId);
            return false;
        }
    }

    public async Task<string> GetCurrentKeyIdAsync()
    {
        try
        {
            var currentKeyId = _configuration["Encryption:CurrentKeyId"];
            if (string.IsNullOrEmpty(currentKeyId))
            {
                var activeKeys = _keyMetadata.Values.Where(k => k.IsActive).OrderByDescending(k => k.CreatedAt);
                currentKeyId = activeKeys.FirstOrDefault()?.KeyId ?? "default-key";
            }

            await LogKeyUsageAsync(currentKeyId, "GET_CURRENT_KEY", "Key", Guid.Empty);
            return currentKeyId;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get current key ID");
            throw;
        }
    }

    public async Task<KeyEscrowDto> CreateKeyEscrowAsync(string keyId, string reason)
    {
        try
        {
            if (!_keyStore.ContainsKey(keyId))
            {
                throw new KeyNotFoundException($"Key not found: {keyId}");
            }

            var escrowId = Guid.NewGuid().ToString();
            var escrow = new KeyEscrowDto
            {
                EscrowId = escrowId,
                KeyId = keyId,
                Reason = reason,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(30),
                Status = "Active",
                AuthorizationRequired = "MultipleApprovers"
            };

            await LogKeyUsageAsync(keyId, "CREATE_ESCROW", "KeyEscrow", Guid.Empty);
            _logger.LogInformation("Created key escrow: {EscrowId} for key: {KeyId}", escrowId, keyId);
            
            return escrow;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create key escrow for key: {KeyId}", keyId);
            throw;
        }
    }

    public async Task<bool> RecoverFromEscrowAsync(string escrowId, string authorizationCode)
    {
        try
        {
            await LogKeyUsageAsync(escrowId, "RECOVER_FROM_ESCROW", "KeyEscrow", Guid.Empty);
            _logger.LogInformation("Key recovery attempted from escrow: {EscrowId}", escrowId);
            
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to recover from escrow: {EscrowId}", escrowId);
            return false;
        }
    }

    public async Task<KeyUsageAuditDto> LogKeyUsageAsync(string keyId, string operation, string entityType, Guid entityId)
    {
        try
        {
            var auditEntry = new AuditLog
            {
                Id = Guid.NewGuid(),
                EntityName = "KeyUsage",
                EntityId = entityId,
                Action = operation,
                OldValues = null,
                NewValues = $"KeyId: {keyId}, EntityType: {entityType}",
                IPAddress = "System",
                UserAgent = "KeyManagementService",
                UserId = Guid.Empty,
                CreatedAt = DateTime.UtcNow,
                TenantId = Guid.Empty
            };

            await _auditRepository.AddAsync(auditEntry);
            await _unitOfWork.SaveChangesAsync();

            return new KeyUsageAuditDto
            {
                Id = auditEntry.Id,
                KeyId = keyId,
                Operation = operation,
                EntityType = entityType,
                EntityId = entityId,
                Timestamp = auditEntry.CreatedAt,
                UserId = auditEntry.UserId,
                Success = true
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to log key usage: {Operation} for key: {KeyId}", operation, keyId);
            return new KeyUsageAuditDto
            {
                Id = Guid.NewGuid(),
                KeyId = keyId,
                Operation = operation,
                EntityType = entityType,
                EntityId = entityId,
                Timestamp = DateTime.UtcNow,
                UserId = Guid.Empty,
                Success = false,
                ErrorMessage = ex.Message
            };
        }
    }

    private void InitializeDefaultKeys()
    {
        try
        {
            var defaultKeyId = "default-key";
            if (!_keyStore.ContainsKey(defaultKeyId))
            {
                using var aes = Aes.Create();
                aes.GenerateKey();
                var keyValue = Convert.ToBase64String(aes.Key);

                _keyStore[defaultKeyId] = keyValue;
                _keyMetadata[defaultKeyId] = new KeyInfoDto
                {
                    KeyId = defaultKeyId,
                    KeyName = "Default System Key",
                    KeyType = "AES256",
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    Status = "Active"
                };

                _logger.LogInformation("Initialized default encryption key");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to initialize default keys");
        }
    }
}
