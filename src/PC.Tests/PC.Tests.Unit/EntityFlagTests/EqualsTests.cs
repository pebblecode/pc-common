using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PebbleCode.Entities;
using PebbleCode.Framework.Collections;

namespace PebbleCode.Tests.Unit.EntityFlagTests
{
    [TestClass]
    public class EqualsTests : BaseUnitTest<TestHelper>
    {
        [TestMethod]
        public void Equals_None()
        {
            Assert.AreEqual(Flags.None, new Flags());
            Assert.AreNotEqual(new Flags(1), Flags.None);
            Assert.AreNotEqual(new Flags(2), Flags.None);
            Assert.AreNotEqual(Flags.None, new Flags(-1));
        }

        [TestMethod]
        public void Equals_Single()
        {
            Assert.AreEqual(new Flags(1), new Flags(1));
            Assert.AreNotEqual(new Flags(1), new Flags(2));
            Assert.AreNotEqual(new Flags(-1), new Flags(1));
        }

        [TestMethod]
        public void Equals_Multiple()
        {
            Assert.AreEqual(new Flags(1,2), new Flags(1,2));
            Assert.AreEqual(new Flags(1, 2), new Flags(2, 1));
            Assert.AreEqual(new Flags(1, 2, 3), new Flags(2, 3, 1));
            Assert.AreNotEqual(new Flags(1, 2), new Flags(2));
            Assert.AreNotEqual(new Flags(1, 2), new Flags(1, 2, 3));
            Assert.AreNotEqual(new Flags(1, 2, 3), new Flags(1, 2));
            Assert.AreNotEqual(new Flags(1, 2, 3), new Flags(1, 2));
            Assert.AreNotEqual(new Flags(1, 2, 3), new Flags(4, 5, 6));
        }

        [TestMethod]
        public void Equals_Repeat()
        {
            Assert.AreEqual(new Flags(1, 2), new Flags(1, 2, 2));
            Assert.AreEqual(new Flags(1, 2), new Flags(2, 1, 2));
            Assert.AreEqual(new Flags(1, 1, 2, 3, 3), new Flags(2, 3, 1));
            Assert.AreNotEqual(new Flags(1, 2), new Flags(2, 2));
            Assert.AreNotEqual(new Flags(1, 2), new Flags(1, 2, 2, 3));
            Assert.AreNotEqual(new Flags(1, 2, 2, 3), new Flags(1, 2));
            Assert.AreNotEqual(new Flags(1, 1, 2, 3), new Flags(1, 1, 1, 2));
            Assert.AreNotEqual(new Flags(1, 2, 3, 3), new Flags(4, 5, 6, 6));
        }


        [TestMethod]
        public void Equals_Operators()
        {
            Assert.IsTrue(new Flags(1, 2) == new Flags(1, 2));
            Assert.IsFalse(new Flags(1, 2) != new Flags(1, 2));
            Assert.IsTrue(new Flags(1, 2).Equals(new Flags(1, 2)));

            Assert.IsFalse(new Flags(1, 2) == new Flags(4, 5));
            Assert.IsTrue(new Flags(1, 2) != new Flags(4, 5));
            Assert.IsFalse(new Flags(1, 2).Equals(new Flags(6, 2)));
        }
    }
}
