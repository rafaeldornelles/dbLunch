using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TestesInterface.Fixtures
{
    [CollectionDefinition("Chrome Driver")]
    public class CollectionFixture: ICollectionFixture<TestFixture>
    {
    }
}
