using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using BARQ.Infrastructure.MultiTenancy;

namespace BARQ.Infrastructure.SignalR;

[Authorize]
public class NotificationHub : Hub
{
    private readonly ITenantProvider _tenantProvider;

    public NotificationHub(ITenantProvider tenantProvider)
    {
        _tenantProvider = tenantProvider;
    }

    public async Task JoinUserGroup(string userId)
    {
        var tenantId = _tenantProvider.GetTenantId();
        var groupName = $"user_{tenantId}_{userId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveUserGroup(string userId)
    {
        var tenantId = _tenantProvider.GetTenantId();
        var groupName = $"user_{tenantId}_{userId}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task JoinProjectGroup(string projectId)
    {
        var tenantId = _tenantProvider.GetTenantId();
        var groupName = $"project_{tenantId}_{projectId}";
        await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
    }

    public async Task LeaveProjectGroup(string projectId)
    {
        var tenantId = _tenantProvider.GetTenantId();
        var groupName = $"project_{tenantId}_{projectId}";
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await JoinUserGroup(userId);
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.UserIdentifier;
        if (!string.IsNullOrEmpty(userId))
        {
            await LeaveUserGroup(userId);
        }
        await base.OnDisconnectedAsync(exception);
    }
}
