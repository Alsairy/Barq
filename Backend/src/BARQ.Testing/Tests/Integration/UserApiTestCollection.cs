using Xunit;
using BARQ.Testing.Framework;

namespace BARQ.Testing.Tests.Integration;

[CollectionDefinition("UserApiTestCollection")]
public class UserApiTestCollection : ICollectionFixture<ApiTestFramework>
{
}
