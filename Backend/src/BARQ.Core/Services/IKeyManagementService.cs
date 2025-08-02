using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface IKeyManagementService
{
    /// <summary>
    /// </summary>
    Task<string> CreateKeyAsync(string keyName, string keyType = "AES256");

    /// <summary>
    /// </summary>
    Task<string> GetKeyAsync(string keyId);

    /// <summary>
    /// </summary>
    Task<bool> RotateKeyAsync(string keyId);

    /// <summary>
    /// </summary>
    Task<bool> DeleteKeyAsync(string keyId);

    /// <summary>
    /// </summary>
    Task<IEnumerable<KeyInfoDto>> ListKeysAsync();

    /// <summary>
    /// </summary>
    Task<bool> ValidateKeyAsync(string keyId);

    /// <summary>
    /// </summary>
    Task<string> GetCurrentKeyIdAsync();

    /// <summary>
    /// </summary>
    Task<KeyEscrowDto> CreateKeyEscrowAsync(string keyId, string reason);

    /// <summary>
    /// </summary>
    Task<bool> RecoverFromEscrowAsync(string escrowId, string authorizationCode);

    /// <summary>
    /// </summary>
    Task<KeyUsageAuditDto> LogKeyUsageAsync(string keyId, string operation, string entityType, Guid entityId);
}
