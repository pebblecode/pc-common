using NUnit.Framework;
using PebbleCode.Framework.Logging;

namespace PebbleCode.Tests
{
    /// <summary>
    /// Integration tests touch the database, and therefore it should be reset between tests
    /// </summary>
    [TestFixture]
    public abstract class BaseIntegrationTest<THelper> : BaseTest<CountableLogManager,THelper>
        where THelper : TestHelper, new()
    {
        [SetUp]
        public override void TestInitialise()
        {
            base.TestInitialise();
            _loggerInstance = new CountableLogManager(new LogManager());
            Logger.LoggerInstance = _loggerInstance;
        }

        [TearDown]
        public override void TestCleanup()
        {
            base.TestCleanup();
            Helper.AssertLoggerError(Logger.LoggerInstance as CountableLogManager);
        }

        private CountableLogManager _loggerInstance;
        protected override CountableLogManager LoggerInstance
        {
            get { return _loggerInstance; }
        }
    }
}
