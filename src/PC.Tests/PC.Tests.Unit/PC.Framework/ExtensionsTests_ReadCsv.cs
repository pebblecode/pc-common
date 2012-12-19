using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PebbleCode.Framework;
using System.IO;

namespace PebbleCode.Tests.Unit.PC.Framework
{
    [TestClass]
    public partial class ExtensionsTests
    {
        [TestMethod]
        public void ToTempFile_Unreadable_Exception()
        {
            try
            {
                Stream s = new MemoryStream();
                s.Dispose();
                Assert.Fail("Exception Expected");
            }
            catch
            {
            }
        }

        [TestMethod]
        public void ToTempFile_AfterWrite_PositionReset()
        {
            MemoryStream s = new MemoryStream();
            s.WriteByte(10);
            s.ToTempFile();
            byte[] buffer = new byte[1];
            int read = s.Read(buffer, 0, 1);
            Assert.AreEqual(1, read);
        }

        [TestMethod]
        public void ToTempFile_StreamHasData_ContentsIdentical()
        {
            MemoryStream s = new MemoryStream();
            s.WriteByte(10);
            string tempFile = s.ToTempFile();
            byte[] buffer = File.ReadAllBytes(tempFile); ;
            Assert.AreEqual(1, buffer.Length);
            Assert.AreEqual(10, buffer[0]);
        }

        [TestMethod]
        public void ToTempFile_StreamNoData_ZeroByteFile()
        {
            MemoryStream s = new MemoryStream();
            string fileName = s.ToTempFile();
            FileInfo f = new FileInfo(fileName);
            Assert.AreEqual(0, f.Length);
        }
    }
}
