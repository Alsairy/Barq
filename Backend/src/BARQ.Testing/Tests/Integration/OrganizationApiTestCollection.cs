using BARQ.Testing.Framework;
using Xunit;

namespace BARQ.Testing.Tests.Integration;

[CollectionDefinition("OrganizationApiTestCollection")]
public class OrganizationApiTestCollection : ICollectionFixture<ApiTestFramework>
{
}
