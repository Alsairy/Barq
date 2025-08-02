using Microsoft.Extensions.Configuration;
using Xunit;

namespace BARQ.Testing.Tests.Guard;

public class NoRootUserGuardTests
{
    [Fact]
    public void DefaultConnection_ShouldNotUseRoot()
    {
        var cfg = new ConfigurationBuilder().AddEnvironmentVariables().Build();
        var cs =
            cfg.GetConnectionString("DefaultConnection") ??
            cfg["ConnectionStrings__DefaultConnection"] ??
            Environment.GetEnvironmentVariable("PGCONNSTRING") ??
            "";

        Console.WriteLine($">>>> GUARD_CONN={cs}");

        Assert.DoesNotContain("Username=root", cs, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("User Id=root", cs, StringComparison.OrdinalIgnoreCase);
    }
}
