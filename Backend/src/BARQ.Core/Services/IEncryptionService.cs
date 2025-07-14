using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Services;

public interface IEncryptionService
{
    Task<string> EncryptAsync(string plainText, string? keyId = null);
    Task<string> DecryptAsync(string encryptedText, string? keyId = null);
    Task<byte[]> EncryptBytesAsync(byte[] plainBytes, string? keyId = null);
    Task<byte[]> DecryptBytesAsync(byte[] encryptedBytes, string? keyId = null);
    Task<string> EncryptFieldAsync<T>(T entity, string fieldName, string plainText) where T : class;
    Task<string> DecryptFieldAsync<T>(T entity, string fieldName, string encryptedText) where T : class;
    Task<string> CreateSearchableHashAsync(string plainText);
    Task<bool> VerifySearchableHashAsync(string plainText, string hash);
    Task<EncryptionAuditDto> LogEncryptionOperationAsync(string operation, string entityType, Guid entityId, string? keyId = null);
    Task<string> GenerateDataEncryptionKeyAsync();
    Task<string> RotateKeyAsync(string currentKeyId);
    Task<bool> ValidateKeyAsync(string keyId);
}
