using NUnit.Framework;
using PebbleCode.Framework.Collections;

namespace PebbleCode.Tests.Unit.EntityFlagTests
{
    [TestFixture]
    public class BasicTests : BaseUnitTest<TestHelper>
    {
        [Test]
        public void None()
        {
            Flags flags = Flags.None;
            Assert.IsFalse(flags.Contains(1), "should be no flags");
            Assert.IsTrue(flags.IsEmpty, "should be empty");
            Assert.AreEqual(0, flags.Length, "should be zero length");
            Assert.AreEqual(0, flags.Split().Count, "should be nothing split");
            Assert.AreEqual(0, flags.Values.Length, "values should be empty");
        }

        [Test]
        public void Single()
        {
            Flags flags = new Flags(99);
            Assert.IsFalse(flags.Contains(1), "should be no flags");
            Assert.IsTrue(flags.Contains(99), "flag missing");
            Assert.IsFalse(flags.IsEmpty, "should not be empty");
            Assert.AreEqual(1, flags.Length, "should be length 1");
            Assert.AreEqual(1, flags.Split().Count, "should be 1 split");
            Assert.AreEqual(99, flags.Split()[0].Values[0], "should be 99 after split");
            Assert.AreEqual(1, flags.Values.Length, "values should be 1 item");
            Assert.AreEqual(99, flags.Values[0], "values should contain only 99");
        }

        [Test]
        public void Multiple()
        {
            Flags flags = new Flags(1,10,5);
            
            Assert.IsFalse(flags.Contains(0));
            Assert.IsFalse(flags.Contains(2));

            Assert.IsTrue(flags.Contains(1));
            Assert.IsTrue(flags.Contains(10));
            Assert.IsTrue(flags.Contains(5));

            Assert.IsFalse(flags.IsEmpty, "should not be empty");
            Assert.AreEqual(3, flags.Length, "should be length 3");
            Assert.AreEqual(3, flags.Split().Count, "should be 3 split");
            Assert.AreEqual(1, flags.Split()[0].Values[0]);
            Assert.AreEqual(10, flags.Split()[1].Values[0]);
            Assert.AreEqual(5, flags.Split()[2].Values[0]);

            Assert.AreEqual(3, flags.Values.Length, "values should have 3 items");
            Assert.AreEqual(1, flags.Values[0]);
            Assert.AreEqual(10, flags.Values[1]);
            Assert.AreEqual(5, flags.Values[2]);
        }

        [Test]
        public void Repeat()
        {
            Flags flags = new Flags(1, 1);

            Assert.IsFalse(flags.Contains(2));
            Assert.IsTrue(flags.Contains(1));

            Assert.IsFalse(flags.IsEmpty, "should not be empty");

            Assert.AreEqual(1, flags.Length, "should be length 1");
            Assert.AreEqual(1, flags.Split().Count, "should be 1 split");
            Assert.AreEqual(1, flags.Split()[0].Values[0], "should be 1 after split");
            Assert.AreEqual(1, flags.Values.Length, "values should be 1 item");
            Assert.AreEqual(1, flags.Values[0], "values should contain only 1");
        }
    }
}
