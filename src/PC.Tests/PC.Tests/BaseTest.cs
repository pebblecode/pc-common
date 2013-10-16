using System;
using NUnit.Framework;

namespace PebbleCode.Tests
{
    /// <summary>
    /// Integration tests touch the database, and therefore it should be reset between tests
    /// </summary>
    [TestFixture]
    public abstract class BaseTest<THelper>
        where THelper : TestHelper, new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        protected BaseTest()
        {
            Helper = new THelper();
        }

        /// <summary>
        /// Get a handle on the unit test helper
        /// </summary>
        protected virtual THelper Helper { get; private set; }

        /// <summary>
        /// Initialise for unit tests
        /// </summary>
        [SetUp]
        public virtual void TestInitialise()
        {
            Helper.TestInitialise();
        }

        /// <summary>
        /// CleanUp for unit tests
        /// </summary>
        [TearDown]
        public virtual void TestCleanup()
        {
            Helper.TestCleanup(); 
        }

        /// <summary>
        /// Checks for the indicated type in the exception and all of its inner exceptions
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="innerExceptionType"></param>
        /// <returns></returns>
        protected void AssertHasExceptionOfTypeInChain(Exception ex, Type innerExceptionType)
        {
            while (ex != null)
            {
                if (ex.GetType().Equals(innerExceptionType)) return;
                ex = ex.InnerException;
            }
            Assert.Fail("Failed to find exception of type {0} in exception chain", innerExceptionType.FullName);
        }
    }
}
