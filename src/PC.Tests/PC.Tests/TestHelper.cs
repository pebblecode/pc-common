using System;
using NUnit.Framework;
using PebbleCode.Framework;

namespace PebbleCode.Tests
{
    /// <summary>
    /// A helper class for unit tests
    /// </summary>
    public class TestHelper
    {
        private readonly Random _rng = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// Get or set the expected error count for a test. Inits to zero
        /// </summary>
        public int ExpectedErrorCount { get; set; }

        /// <summary>
        /// Called by BaseIntegrationTest and BaseUnitTest during TestInitialise
        /// </summary>
        
        [ExpectedException(typeof(Exception))]
        internal virtual void TestInitialise()
        {
        }

        /// <summary>
        /// Called by BaseIntegrationTest and BaseUnitTest during TestCleanup
        /// </summary>
        internal virtual void TestCleanup()
        {
        }

        /// <summary>
        /// Get a random number generator prepared for action
        /// </summary>
        public Random Rng { get { return _rng; } }

        /// <summary>
        /// Get a random decimal between decimal.MinValue and decimal.MaxValue
        /// </summary>
        public decimal RandomDecimal
        {
            get { return Rng.NextDecimal(decimal.MinValue, decimal.MaxValue); }
        }

        /// <summary>
        /// Get a random int between int.MinValue and int.MaxValue
        /// </summary>
        public int RandomInt
        {
            get { return Rng.Next(int.MinValue, int.MaxValue); }
        }
    }
}
