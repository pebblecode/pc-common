using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PebbleCode.Framework;

namespace PebbleCode.Tests.Unit.PC.Framework
{
    [TestClass]
    public class EnumHelpers_PascalCaseToReadableNameTests
    {
        [TestMethod]
        public void ToStringEmptyString()
        {
            var pascalCased = string.Empty;
            var expected = string.Empty;
            var actual = EnumHelpers.ToReadableName(pascalCased);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToStringSingleCharacter()
        {
            var pascalCased = "A";
            var expected = "A";
            var actual = EnumHelpers.ToReadableName(pascalCased);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToStringSingleWord()
        {
            var pascalCased = "Word";
            var expected = "Word";
            var actual = EnumHelpers.ToReadableName(pascalCased);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToStringTwoWords()
        {
            var pascalCased = "TwoWords";
            var expected = "Two Words";
            var actual = EnumHelpers.ToReadableName(pascalCased);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToStringFirstWordAccronym()
        {
            var pascalCased = "TWOWords";
            var expected = "TWO Words";
            var actual = EnumHelpers.ToReadableName(pascalCased);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToStringSecondWordAccronym()
        {
            var pascalCased = "TwoWORDS";
            var expected = "Two WORDS";
            var actual = EnumHelpers.ToReadableName(pascalCased);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void ToStringMiddleWordAccronym()
        {
            var pascalCased = "ThreeDIFFERENTWords";
            var expected = "Three DIFFERENT Words";
            var actual = EnumHelpers.ToReadableName(pascalCased);
            Assert.AreEqual(expected, actual);
        }
    }
}
