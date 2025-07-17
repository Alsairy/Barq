using Xunit;

namespace BARQ.Testing.Tests.Integration;

[CollectionDefinition("UserApiTestCollection")]
public class UserApiTestCollection : ICollectionFixture<ApiTestFramework>
{
}
