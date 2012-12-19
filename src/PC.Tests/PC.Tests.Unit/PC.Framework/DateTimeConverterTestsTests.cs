using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PebbleCode.Framework.Dates;

namespace PebbleCode.Tests.Unit.PC.Framework
{
    [TestClass]
    public class DateTimeConverterTestsTests 
    {
        [TestMethod]
        public void ParseDateWithSeconds_ValidDateString_ReturnsSameDateAndTime()
        {
            DateTime? date = DateTimeConverter.ParseDateWithSeconds("20120418083058");
            DateTime expectedDate = new DateTime(2012, 4, 18, 08, 30, 58);

            Assert.AreEqual(expectedDate, date.Value);
        }

        [TestMethod]
        public void ParseDate_ValidDateString_ReturnsSameDate()
        {
            DateTime? date = DateTimeConverter.ParseDate("20120418");
            DateTime expectedDate = new DateTime(2012, 4, 18);

            Assert.AreEqual(expectedDate, date.Value);
        }
    }
}
