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
    public class BitwiseXorTests : BaseUnitTest<TestHelper>
    {
        [TestMethod]
        public void BitwiseXor_SingleWithEmpty()
        {
            Flags flags = new Flags(1) ^ Flags.None;

            Assert.IsTrue(flags.Contains(1));
            Assert.IsFalse(flags.Contains(10));
            Assert.IsFalse(flags.Contains(0));
            Assert.IsFalse(flags.IsEmpty);
            Assert.AreEqual(1, flags.Length);
            Assert.AreEqual(1, flags.Split().Count);
            Assert.AreEqual(1, flags.Values.Length);
        }

        [TestMethod]
        public void BitwiseXor_1With2()
        {
            Flags flags = new Flags(1) ^ new Flags(2);

            Assert.IsTrue(flags.Contains(1));
            Assert.IsTrue(flags.Contains(2));
            Assert.IsFalse(flags.IsEmpty);
            Assert.AreEqual(2, flags.Length);
            Assert.AreEqual(2, flags.Split().Count);
            Assert.AreEqual(2, flags.Values.Length);
        }

        [TestMethod]
        public void BitwiseXor_1With1()
        {
            Flags flags = new Flags(1) ^ new Flags(1);

            Assert.IsFalse(flags.Contains(1));
            Assert.IsTrue(flags.IsEmpty);
            Assert.AreEqual(0, flags.Length);
            Assert.AreEqual(0, flags.Split().Count);
            Assert.AreEqual(0, flags.Values.Length);
        }

        [TestMethod]
        public void BitwiseXor_SeveralValues()
        {
            Flags flags = new Flags(1, -10) ^ new Flags(1, -10, int.MaxValue);

            Assert.IsFalse(flags.Contains(1));
            Assert.IsFalse(flags.Contains(-10));
            Assert.IsTrue(flags.Contains(int.MaxValue));

            Assert.IsFalse(flags.Contains(0));
            Assert.IsFalse(flags.Contains(-1));
            Assert.IsFalse(flags.Contains(int.MinValue));
            Assert.IsFalse(flags.IsEmpty);

            Assert.AreEqual(1, flags.Length);
            Assert.AreEqual(1, flags.Split().Count);
            Assert.AreEqual(1, flags.Split()[0].Values.Length);
            Assert.AreEqual(int.MaxValue, flags.Split()[0].Values[0]);
            Assert.AreEqual(1, flags.Values.Length);
            Assert.AreEqual(int.MaxValue, flags.Values[0]);
        }

        [TestMethod]
        public void BitwiseXor_MultiInitialiser()
        {
            Flags flags = new Flags(1, 2) ^ new Flags(2);

            Assert.IsTrue(flags.Contains(1));
            Assert.IsFalse(flags.Contains(2));
            
            Assert.IsFalse(flags.Contains(0));
            Assert.IsFalse(flags.Contains(4));
            Assert.IsFalse(flags.IsEmpty);

            Assert.AreEqual(1, flags.Length);
            Assert.AreEqual(1, flags.Split().Count);
            Assert.AreEqual(1, flags.Split()[0].Values.Length);
            Assert.AreEqual(1, flags.Split()[0].Values[0]);
            Assert.AreEqual(1, flags.Values.Length);
            Assert.AreEqual(1, flags.Values[0]);
        }

        [TestMethod]
        public void BitwiseXor_Overlap()
        {
            Flags flags = new Flags(1, 2, 3, 4, 5) ^ new Flags(3, 4, 5, 6, 7);

            Assert.IsTrue(flags.Contains(1));
            Assert.IsTrue(flags.Contains(2));
            Assert.IsFalse(flags.Contains(3));
            Assert.IsFalse(flags.Contains(4));
            Assert.IsFalse(flags.Contains(5));
            Assert.IsTrue(flags.Contains(6));
            Assert.IsTrue(flags.Contains(7));

            Assert.IsFalse(flags.Contains(0));
            Assert.IsFalse(flags.Contains(8));
            Assert.IsFalse(flags.IsEmpty);

            Assert.AreEqual(4, flags.Length);
        }
    }
}
