using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PebbleCode.Framework;
using System.IO;

namespace PebbleCode.Tests.Unit.PC.Framework
{
    public partial class ExtensionsTests
    {
        const string multiLineValidData =
@"alex,is,32
james,is,21";

        [TestMethod]
        public void ReadCsv_ValidMultiLineData_TokensRead()
        {
            MemoryStream stream = new MemoryStream(
                System.Text.UTF8Encoding.UTF8.GetBytes(multiLineValidData));
            AssertRowsCols(stream, 2, 3);
        }

        [TestMethod]
        public void ReadCsv_ValidMultiLineDataExtraComma_TokensRead()
        {
            MemoryStream stream = new MemoryStream(
                System.Text.UTF8Encoding.UTF8.GetBytes(multiLineValidData + ","));
            AssertRowsCols(stream, 2, null);
        }

        [TestMethod]
        public void ReadCsv_ValidMultiLineDataExtraLineFeed_TokensRead()
        {
            MemoryStream stream = new MemoryStream(
                System.Text.UTF8Encoding.UTF8.GetBytes(multiLineValidData + "\n"));
            AssertRowsCols(stream, 2, 3);
        }

        private void AssertRowsCols(Stream stream, int expectedRow, int? expectedColumns)
        {
            int rows = 0;
            foreach (var tokens in stream.ReadCsv())
            {
                rows++;
                if (expectedColumns != null)
                    Assert.AreEqual(expectedColumns.Value, tokens.Length);
            }
            Assert.AreEqual(expectedRow, rows);
        }
    }
}
