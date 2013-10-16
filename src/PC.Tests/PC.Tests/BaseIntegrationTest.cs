using NUnit.Framework;

namespace PebbleCode.Tests
{
    /// <summary>
    /// Integration tests touch the database, and therefore it should be reset between tests
    /// </summary>
    [TestFixture]
    public abstract class BaseIntegrationTest<THelper> : BaseTest<THelper>
        where THelper : TestHelper, new()
    {
    }
}
