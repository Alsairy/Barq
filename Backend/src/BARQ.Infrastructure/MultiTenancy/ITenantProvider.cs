namespace BARQ.Infrastructure.MultiTenancy;

public interface ITenantProvider
{
    Guid GetTenantId();
    void SetTenantId(Guid tenantId);
}
