using NUnit.Framework;
using PebbleCode.Framework;

namespace PebbleCode.Tests.Unit.PC.Framework
{
    [TestFixture]
    public class EnumHelpers_PascalCaseToReadableNameTests
    {
        [Test]
        public void ToStringEmptyString()
        {
            var pascalCased = string.Empty;
            var expected = string.Empty;
            var actual = EnumHelpers.ToReadableName(pascalCased);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToStringSingleCharacter()
        {
            var pascalCased = "A";
            var expected = "A";
            var actual = EnumHelpers.ToReadableName(pascalCased);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToStringSingleWord()
        {
            var pascalCased = "Word";
            var expected = "Word";
            var actual = EnumHelpers.ToReadableName(pascalCased);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToStringTwoWords()
        {
            var pascalCased = "TwoWords";
            var expected = "Two Words";
            var actual = EnumHelpers.ToReadableName(pascalCased);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToStringFirstWordAccronym()
        {
            var pascalCased = "TWOWords";
            var expected = "TWO Words";
            var actual = EnumHelpers.ToReadableName(pascalCased);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToStringSecondWordAccronym()
        {
            var pascalCased = "TwoWORDS";
            var expected = "Two WORDS";
            var actual = EnumHelpers.ToReadableName(pascalCased);
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToStringMiddleWordAccronym()
        {
            var pascalCased = "ThreeDIFFERENTWords";
            var expected = "Three DIFFERENT Words";
            var actual = EnumHelpers.ToReadableName(pascalCased);
            Assert.AreEqual(expected, actual);
        }
    }
}
