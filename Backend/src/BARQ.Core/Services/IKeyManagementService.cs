using BARQ.Core.Models.DTOs;

namespace BARQ.Core.Services;

public interface IKeyManagementService
{
    Task<string> CreateKeyAsync(string keyName, string keyType = "AES256");
    Task<string> GetKeyAsync(string keyId);
    Task<bool> RotateKeyAsync(string keyId);
    Task<bool> DeleteKeyAsync(string keyId);
    Task<IEnumerable<KeyInfoDto>> ListKeysAsync();
    Task<bool> ValidateKeyAsync(string keyId);
    Task<string> GetCurrentKeyIdAsync();
    Task<KeyEscrowDto> CreateKeyEscrowAsync(string keyId, string reason);
    Task<bool> RecoverFromEscrowAsync(string escrowId, string authorizationCode);
    Task<KeyUsageAuditDto> LogKeyUsageAsync(string keyId, string operation, string entityType, Guid entityId);
}
