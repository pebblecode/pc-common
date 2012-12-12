using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PebbleCode.Framework.Logging;

namespace PebbleCode.Tests.Unit.PC.Framework
{
    [TestClass]
    public class DataContractLogHelperTests
    {
        [TestMethod]
        public void Log_WithEnumerablePropertyContainsItems_LogAllItemsOfEnumerableProperty()
        {
            //Arrange
            var logManager = SetLogManagerToCountableLogManager();
            var helper = GetDataContractLogger(Category.Service);
            var items = new List<string> {"Item1", "Item2", "Item3"};
            var dataContract = new TestContaract { Property1 = "Value1", Property2 = 1, Property3 = items };

            //Act
            helper.Log(dataContract, Category.Service);

            //Assert
            foreach (var item in items)
            {
                Assert.IsTrue(logManager.LatestLogs.Any(l => l.Message.Contains(item)));
            }
        }

        [TestMethod]
        public void Log_WithEnumerablePropertyWithNullValue_LogEnumerableProperty()
        {
            //Arrange
            var logManager = SetLogManagerToCountableLogManager();
            var helper = GetDataContractLogger(Category.Service);
            var dataContract = new TestContaract { Property1 = "Value1", Property2 = 1, Property3 = null };

            //Act
            helper.Log(dataContract, Category.Service);

            //Assert
            Assert.IsTrue(logManager.LatestLogs.Any(l => l.Message.Contains("Property3")));
        }

        private DataContractLogHelper GetDataContractLogger(string category)
        {
            var logger = new DataContractLogger(category, "Test");
            var helper = new DataContractLogHelper(logger, typeof(TestContaract), Category.Service, 0);
            return helper;
        }
    
        private CountableLogManager SetLogManagerToCountableLogManager()
        {
            var logManager = new CountableLogManager(new FakeLogManager());
            Logger.LoggerInstance = logManager;
            return logManager;
        }
    }

    public class TestContaract
    {
        public string Property1 { get; set; }
        public int Property2 { get; set; }
        public IEnumerable<string> Property3 { get; set; }
    }
}
