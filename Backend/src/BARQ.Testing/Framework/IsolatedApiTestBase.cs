using Xunit;

namespace BARQ.Testing.Framework;

public abstract class IsolatedApiTestBase : IClassFixture<ApiTestFramework>, IAsyncLifetime
{
    protected readonly ApiTestFramework _factory;

    protected IsolatedApiTestBase(ApiTestFramework factory)
    {
        _factory = factory;
    }

    public virtual async Task InitializeAsync()
    {
        await _factory.ResetDatabaseAsync();
    }

    public virtual Task DisposeAsync()
    {
        return Task.CompletedTask;
    }
}
