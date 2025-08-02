using Microsoft.Extensions.Configuration;
using Xunit;

namespace BARQ.Testing.Tests.Guard;

public class NoRootUserGuardTests
{
    [Fact]
    public void DefaultConnection_ShouldNotUseRoot()
    {
        var cfg = new ConfigurationBuilder().AddEnvironmentVariables().Build();
        var cs = cfg.GetConnectionString("DefaultConnection")
              ?? cfg["ConnectionStrings__DefaultConnection"];
        Assert.DoesNotContain("Username=root", cs, StringComparison.OrdinalIgnoreCase);
    }
}
