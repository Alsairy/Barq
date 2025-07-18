using Xunit;
using BARQ.Testing.Framework;

namespace BARQ.Testing.Tests.Integration;

[CollectionDefinition("ProjectApiTestCollection")]
public class ProjectApiTestCollection : ICollectionFixture<ApiTestFramework>
{
}
