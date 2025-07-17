using Xunit;

namespace BARQ.Testing.Tests.Integration;

[CollectionDefinition("AuthenticationApiTestCollection")]
public class AuthenticationApiTestCollection : ICollectionFixture<ApiTestFramework>
{
}
