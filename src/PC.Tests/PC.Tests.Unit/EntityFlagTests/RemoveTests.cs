using NUnit.Framework;
using PebbleCode.Framework.Collections;

namespace PebbleCode.Tests.Unit.EntityFlagTests
{
    [TestFixture]
    public class RemoveTests : BaseUnitTest<TestHelper>
    {
        [Test]
        public void Remove_SingleWithEmpty()
        {
            Flags flags = new Flags(1).Remove(Flags.None);

            Assert.IsTrue(flags.Contains(1));
            Assert.IsFalse(flags.Contains(10));
            Assert.IsFalse(flags.Contains(0));
            Assert.IsFalse(flags.IsEmpty);
            Assert.AreEqual(1, flags.Length);
            Assert.AreEqual(1, flags.Split().Count);
            Assert.AreEqual(1, flags.Values.Length);
        }

        [Test]
        public void Remove_1With2()
        {
            Flags flags = new Flags(1).Remove(new Flags(2));

            Assert.IsTrue(flags.Contains(1));
            Assert.IsFalse(flags.Contains(2));
            Assert.IsFalse(flags.IsEmpty);
            Assert.AreEqual(1, flags.Length);
            Assert.AreEqual(1, flags.Split().Count);
            Assert.AreEqual(1, flags.Values.Length);
        }

        [Test]
        public void Remove_1With1()
        {
            Flags flags = new Flags(1).Remove(new Flags(1));

            Assert.IsFalse(flags.Contains(1));
            Assert.IsTrue(flags.IsEmpty);
            Assert.AreEqual(0, flags.Length);
            Assert.AreEqual(0, flags.Split().Count);
            Assert.AreEqual(0, flags.Values.Length);
        }

        [Test]
        public void Remove_SeveralValues()
        {
            Flags flags = new Flags(1, -10).Remove(new Flags(1, -10, int.MaxValue));

            Assert.IsFalse(flags.Contains(1));
            Assert.IsFalse(flags.Contains(-10));
            Assert.IsFalse(flags.Contains(int.MaxValue));
            Assert.IsTrue(flags.IsEmpty);
            Assert.AreEqual(0, flags.Length);
            Assert.AreEqual(0, flags.Split().Count);
            Assert.AreEqual(0, flags.Values.Length);
        }

        [Test]
        public void Remove_MultiInitialiser()
        {
            Flags flags = new Flags(1, 2).Remove(new Flags(2));

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

        [Test]
        public void Remove_Overlap()
        {
            Flags flags = new Flags(1, 2, 3, 4, 5).Remove( new Flags(3, 4, 5, 6, 7));

            Assert.IsTrue(flags.Contains(1));
            Assert.IsTrue(flags.Contains(2));
            Assert.IsFalse(flags.Contains(3));
            Assert.IsFalse(flags.Contains(4));
            Assert.IsFalse(flags.Contains(5));
            Assert.IsFalse(flags.Contains(6));
            Assert.IsFalse(flags.Contains(7));

            Assert.IsFalse(flags.Contains(0));
            Assert.IsFalse(flags.Contains(8));
            Assert.IsFalse(flags.IsEmpty);

            Assert.AreEqual(2, flags.Length);
            Assert.AreEqual(1, flags.Values[0]);
            Assert.AreEqual(2, flags.Values[1]);
        }
    }
}
