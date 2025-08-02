namespace BARQ.Core.Services;

/// <summary>
/// </summary>
public interface ITenantProvider
{
    /// <summary>
    /// </summary>
    Guid GetTenantId();

    /// <summary>
    /// </summary>
    void SetTenantId(Guid tenantId);

    /// <summary>
    /// </summary>
    string GetTenantName();

    /// <summary>
    /// </summary>
    void SetTenantName(string tenantName);

    /// <summary>
    /// </summary>
    bool IsMultiTenant();

    /// <summary>
    /// </summary>
    void ClearTenantContext();

    /// <summary>
    /// </summary>
    Guid GetCurrentUserId();
}
