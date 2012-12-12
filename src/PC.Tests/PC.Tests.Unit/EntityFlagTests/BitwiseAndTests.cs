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
    public class BitwiseAndTests : BaseUnitTest<TestHelper>
    {
        [TestMethod]
        public void BitwiseAnd_SingleWithEmpty()
        {
            Flags flags = new Flags(1) & Flags.None;

            Assert.IsFalse(flags.Contains(1));
            Assert.IsFalse(flags.Contains(10));
            Assert.IsFalse(flags.Contains(0));
            Assert.IsTrue(flags.IsEmpty);
            Assert.AreEqual(0, flags.Length);
            Assert.AreEqual(0, flags.Split().Count);
            Assert.AreEqual(0, flags.Values.Length);
        }

        [TestMethod]
        public void BitwiseAnd_1With2()
        {
            Flags flags = new Flags(1) & new Flags(2);

            Assert.IsFalse(flags.Contains(1));
            Assert.IsFalse(flags.Contains(2));
            Assert.IsTrue(flags.IsEmpty);
            Assert.AreEqual(0, flags.Length);
            Assert.AreEqual(0, flags.Split().Count);
            Assert.AreEqual(0, flags.Values.Length);
        }

        [TestMethod]
        public void BitwiseAnd_1With1()
        {
            Flags flags = new Flags(1) & new Flags(1);

            Assert.IsTrue(flags.Contains(1));
            Assert.IsFalse(flags.IsEmpty);
            Assert.AreEqual(1, flags.Length);
            Assert.AreEqual(1, flags.Split().Count);
            Assert.AreEqual(1, flags.Split()[0].Values.Length);
            Assert.AreEqual(1, flags.Split()[0].Values[0]);
            Assert.AreEqual(1, flags.Values.Length);
            Assert.AreEqual(1, flags.Values[0]);
        }

        [TestMethod]
        public void BitwiseAnd_SeveralValues()
        {
            Flags flags = new Flags(1, -10) & new Flags(1, -10, int.MaxValue);

            Assert.IsTrue(flags.Contains(1));
            Assert.IsFalse(flags.Contains(2));
            Assert.IsTrue(flags.Contains(-10));
            Assert.IsFalse(flags.Contains(int.MaxValue));

            Assert.IsFalse(flags.Contains(0));
            Assert.IsFalse(flags.Contains(-1));
            Assert.IsFalse(flags.Contains(int.MinValue));
            Assert.IsFalse(flags.IsEmpty);

            Assert.AreEqual(2, flags.Length);
            Assert.AreEqual(2, flags.Split().Count);
            Assert.AreEqual(1, flags.Split()[1].Values.Length);
            Assert.AreEqual(-10, flags.Split()[1].Values[0]);
            Assert.AreEqual(2, flags.Values.Length);
            Assert.AreEqual(1, flags.Values[0]);
        }

        [TestMethod]
        public void BitwiseAnd_MultiInitialiser()
        {
            Flags flags = new Flags(1, 2) & new Flags(2);

            Assert.IsFalse(flags.Contains(1));
            Assert.IsTrue(flags.Contains(2));
            
            Assert.IsFalse(flags.Contains(0));
            Assert.IsFalse(flags.Contains(4));
            Assert.IsFalse(flags.IsEmpty);

            Assert.AreEqual(1, flags.Length);
            Assert.AreEqual(1, flags.Split().Count);
            Assert.AreEqual(1, flags.Split()[0].Values.Length);
            Assert.AreEqual(2, flags.Split()[0].Values[0]);
            Assert.AreEqual(1, flags.Values.Length);
            Assert.AreEqual(2, flags.Values[0]);
        }

        [TestMethod]
        public void BitwiseAnd_Overlap()
        {
            Flags flags = new Flags(1, 2, 3, 4, 5) & new Flags(3, 4, 5, 6, 7);

            Assert.IsFalse(flags.Contains(1));
            Assert.IsFalse(flags.Contains(2));
            Assert.IsTrue(flags.Contains(3));
            Assert.IsTrue(flags.Contains(4));
            Assert.IsTrue(flags.Contains(5));
            Assert.IsFalse(flags.Contains(6));
            Assert.IsFalse(flags.Contains(7));

            Assert.IsFalse(flags.Contains(0));
            Assert.IsFalse(flags.Contains(8));
            Assert.IsFalse(flags.IsEmpty);

            Assert.AreEqual(3, flags.Length);
        }
    }
}
