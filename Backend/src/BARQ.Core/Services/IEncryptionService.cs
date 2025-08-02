using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface IEncryptionService
{
    /// <summary>
    /// </summary>
    Task<string> EncryptAsync(string plainText, string? keyId = null);

    /// <summary>
    /// </summary>
    Task<string> DecryptAsync(string encryptedText, string? keyId = null);

    /// <summary>
    /// </summary>
    Task<byte[]> EncryptBytesAsync(byte[] plainBytes, string? keyId = null);

    /// <summary>
    /// </summary>
    Task<byte[]> DecryptBytesAsync(byte[] encryptedBytes, string? keyId = null);

    /// <summary>
    /// </summary>
    Task<string> EncryptFieldAsync<T>(T entity, string fieldName, string plainText) where T : class;

    /// <summary>
    /// </summary>
    Task<string> DecryptFieldAsync<T>(T entity, string fieldName, string encryptedText) where T : class;

    /// <summary>
    /// </summary>
    Task<string> CreateSearchableHashAsync(string plainText);

    /// <summary>
    /// </summary>
    Task<bool> VerifySearchableHashAsync(string plainText, string hash);

    /// <summary>
    /// </summary>
    Task<EncryptionAuditDto> LogEncryptionOperationAsync(string operation, string entityType, Guid entityId, string? keyId = null);

    /// <summary>
    /// </summary>
    Task<string> GenerateDataEncryptionKeyAsync();

    /// <summary>
    /// </summary>
    Task<string> RotateKeyAsync(string currentKeyId);

    /// <summary>
    /// </summary>
    Task<bool> ValidateKeyAsync(string keyId);
}
