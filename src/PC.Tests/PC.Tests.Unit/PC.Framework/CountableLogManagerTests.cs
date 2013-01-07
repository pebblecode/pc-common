using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using PebbleCode.Framework.Logging;

namespace PebbleCode.Tests.Unit.PC.Framework
{
    [TestFixture]
    public class CountableLogManagerTests
    {
        [Test]
        public void LatestLogs_WhenWriteLogs_CachesLogs()
        {
            //Arrange
            CountableLogManager logManager = SetLogManagerToCountableLogManager();
            
            //Act
            Logger.WriteInfo("Message1", "");
            Logger.WriteWarning("Message2", "");
            List<CountableLogManager.LogEntry> latestLogs = logManager.LatestLogs.ToList();

            //Assert
            Assert.AreEqual(2, latestLogs.Count);
            Assert.AreEqual(1, latestLogs.Count(entry => entry.Message.Equals("Message1")));
            Assert.AreEqual(1, latestLogs.Count(entry => entry.Message.Equals("Message2")));
        }

        [Test]
        public void IsCountChecked_WhenAnyOfWarningOrWarningCountAccessed_SetToFalse()
        {
            //Arrange
            CountableLogManager logManager = SetLogManagerToCountableLogManager();
            
            //Act
            Logger.WriteInfo("Message1", "");

            //Assert
            Assert.IsFalse(logManager.IsCountChecked);
        }

        [Test]
        public void IsCountChecked_WhenWarningCountAccessed_SetToTrue()
        {
            //Arrange
            CountableLogManager logManager = SetLogManagerToCountableLogManager();

            //Act
            Logger.WriteInfo("Message1", "");
            int warningCount = logManager.WarningCount;

            //Assert
            Assert.IsTrue(logManager.IsCountChecked);
        }

        [Test]
        public void IsCountChecked_WhenErrorCountAccessed_SetToTrue()
        {
            //Arrange
            CountableLogManager logManager = SetLogManagerToCountableLogManager();

            //Act
            Logger.WriteInfo("Message1", "");
            int errorCount = logManager.ErrorCount;

            //Assert
            Assert.IsTrue(logManager.IsCountChecked);
        }

        [Test]
        public void ErrorCount_WithErrorMessage_ReturnCountAsExpected()
        {
            //Arrange
            CountableLogManager logManager = SetLogManagerToCountableLogManager();

            //Act
            Logger.WriteError("Message1", "");
            Logger.WriteInfo("Message2", "");
            Logger.WriteWarning("Message3", "");
            Logger.WriteError("Message4", "");

            //Assert
            Assert.AreEqual(2, logManager.ErrorCount);
        }


        [Test]
        public void WarningCount_WithWarningMessage_ReturnCountAsExpected()
        {
            //Arrange
            CountableLogManager logManager = SetLogManagerToCountableLogManager();

            //Act
            Logger.WriteWarning("Message1", "");
            Logger.WriteInfo("Message2", "");
            Logger.WriteWarning("Message3", "");
            Logger.WriteError("Message4", "");
            Logger.WriteWarning("Message4", "");

            //Assert
            Assert.AreEqual(3, logManager.WarningCount);
        }

        private CountableLogManager SetLogManagerToCountableLogManager()
        {

            var logManager = new CountableLogManager(new FakeLogManager {ThrowExceptionOnError = false, ThrowExceptionOnWaring = false});
            Logger.LoggerInstance = logManager;
            return logManager;
        }
    }
}
