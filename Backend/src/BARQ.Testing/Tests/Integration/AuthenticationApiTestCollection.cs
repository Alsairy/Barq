using Xunit;
using BARQ.Testing.Framework;

namespace BARQ.Testing.Tests.Integration;

[CollectionDefinition("AuthenticationApiTestCollection")]
public class AuthenticationApiTestCollection : ICollectionFixture<ApiTestFramework>
{
}
