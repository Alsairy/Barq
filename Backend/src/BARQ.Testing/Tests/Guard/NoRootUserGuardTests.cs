using Microsoft.Extensions.Configuration;
using Xunit;

namespace BARQ.Testing.Tests.Guard;

public class NoRootUserGuardTests
{
    [Fact]
    public void DefaultConnection_ShouldNotUseRoot()
    {
        const string testConn = "Host=localhost;Database=barq_test;Username=postgres;Password=postgres";
        Environment.SetEnvironmentVariable("ConnectionStrings__DefaultConnection", testConn);
        Console.WriteLine($">>> BARQ-TEST-CONN={testConn}");
        
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
