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
    public class BitwiseOrTests : BaseUnitTest<TestHelper>
    {
        [TestMethod]
        public void BitwiseOr_SingleWithEmpty()
        {
            Flags flag1 = new Flags(1);
            Flags flags = flag1 | Flags.None;
            Assert.IsTrue(flags.Contains(1));
            Assert.IsFalse(flags.Contains(10));
            Assert.IsFalse(flags.Contains(0));
            Assert.IsFalse(flags.IsEmpty);
            Assert.AreEqual(1, flags.Length);
            Assert.AreEqual(1, flags.Split().Count);
            Assert.AreEqual(1, flags.Split()[0].Values.Length);
            Assert.AreEqual(1, flags.Split()[0].Values[0]);
            Assert.AreEqual(1, flags.Values.Length);
            Assert.AreEqual(1, flags.Values[0]);
        }

        [TestMethod]
        public void BitwiseOr_1With2()
        {
            Flags flag1 = new Flags(1);
            Flags flag2 = new Flags(2);
            Flags flags = flag1 | flag2;
            Assert.IsTrue(flags.Contains(1), "flag1 missing");
            Assert.IsTrue(flags.Contains(2), "flag2 missing");
            Assert.IsFalse(flags.Contains(10));
            Assert.IsFalse(flags.Contains(-1));
            Assert.IsFalse(flags.IsEmpty);
            Assert.AreEqual(2, flags.Length);
            Assert.AreEqual(2, flags.Split().Count);
            Assert.AreEqual(1, flags.Split()[0].Values.Length);
            Assert.AreEqual(1, flags.Split()[0].Values[0]);
            Assert.AreEqual(2, flags.Values.Length);
            Assert.AreEqual(1, flags.Values[0]);
        }

        [TestMethod]
        public void BitwiseOr_SeveralValues()
        {
            Flags flags = new Flags(1) | new Flags(2) | new Flags(-10) | new Flags(45) | new Flags(int.MaxValue);

            Assert.IsTrue(flags.Contains(1), "1 missing");
            Assert.IsTrue(flags.Contains(2), "2 missing");
            Assert.IsTrue(flags.Contains(-10), "-10 missing");
            Assert.IsTrue(flags.Contains(45), "45 missing");
            Assert.IsTrue(flags.Contains(int.MaxValue), "int.MaxValue missing");

            Assert.IsFalse(flags.Contains(0));
            Assert.IsFalse(flags.Contains(-1));
            Assert.IsFalse(flags.Contains(int.MinValue));
            Assert.IsFalse(flags.IsEmpty);

            Assert.AreEqual(5, flags.Length);
            Assert.AreEqual(5, flags.Split().Count);
            Assert.AreEqual(1, flags.Split()[3].Values.Length);
            Assert.AreEqual(-10, flags.Split()[2].Values[0]);
            Assert.AreEqual(5, flags.Values.Length);
            Assert.AreEqual(int.MaxValue, flags.Values[4]);
        }

        [TestMethod]
        public void BitwiseOr_MultiInitialiser()
        {
            Flags flags = new Flags(1, 2) | new Flags(3);

            Assert.IsTrue(flags.Contains(1), "1 missing");
            Assert.IsTrue(flags.Contains(2), "2 missing");
            Assert.IsTrue(flags.Contains(3), "3 missing");

            Assert.IsFalse(flags.Contains(0));
            Assert.IsFalse(flags.Contains(4));
            Assert.IsFalse(flags.IsEmpty);

            Assert.AreEqual(3, flags.Length);
            Assert.AreEqual(3, flags.Split().Count);
            Assert.AreEqual(1, flags.Split()[2].Values.Length);
            Assert.AreEqual(3, flags.Split()[2].Values[0]);
            Assert.AreEqual(3, flags.Values.Length);
            Assert.AreEqual(2, flags.Values[1]);
        }

        [TestMethod]
        public void BitwiseOr_1With1()
        {
            Flags flags = new Flags(1) | new Flags(1);

            Assert.IsTrue(flags.Contains(1), "1 missing");
            Assert.IsFalse(flags.Contains(0));
            Assert.IsFalse(flags.Contains(2));
            Assert.IsFalse(flags.IsEmpty);

            Assert.AreEqual(1, flags.Length);
            Assert.AreEqual(1, flags.Split().Count);
            Assert.AreEqual(1, flags.Split()[0].Values.Length);
            Assert.AreEqual(1, flags.Split()[0].Values[0]);
            Assert.AreEqual(1, flags.Values.Length);
            Assert.AreEqual(1, flags.Values[0]);
        }
    }
}
