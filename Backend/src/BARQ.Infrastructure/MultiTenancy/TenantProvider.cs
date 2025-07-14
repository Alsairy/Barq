using Microsoft.AspNetCore.Http;
using BARQ.Core.Services;

namespace BARQ.Infrastructure.MultiTenancy;

public class TenantProvider : ITenantProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private Guid _tenantId;

    public TenantProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetTenantId()
    {
        if (_tenantId != Guid.Empty)
            return _tenantId;

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantHeader) == true)
        {
            if (Guid.TryParse(tenantHeader.FirstOrDefault(), out var tenantId))
            {
                _tenantId = tenantId;
                return _tenantId;
            }
        }

        if (httpContext?.User?.FindFirst("TenantId")?.Value != null)
        {
            if (Guid.TryParse(httpContext.User.FindFirst("TenantId")?.Value, out var claimTenantId))
            {
                _tenantId = claimTenantId;
                return _tenantId;
            }
        }

        return Guid.Empty;
    }

    public void SetTenantId(Guid tenantId)
    {
        _tenantId = tenantId;
    }

    public void SetTenantContext(Guid tenantId, string tenantName)
    {
        _tenantId = tenantId;
    }

    public void ClearTenantContext()
    {
        _tenantId = Guid.Empty;
    }

    public Guid GetCurrentUserId()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext?.User?.FindFirst("UserId")?.Value != null)
        {
            if (Guid.TryParse(httpContext.User.FindFirst("UserId")?.Value, out var userId))
            {
                return userId;
            }
        }
        return Guid.Empty;
    }

    public string GetTenantName()
    {
        return "Default Tenant";
    }

    public void SetTenantName(string tenantName)
    {
    }

    public bool IsMultiTenant()
    {
        return true; // This application supports multi-tenancy
    }
}
