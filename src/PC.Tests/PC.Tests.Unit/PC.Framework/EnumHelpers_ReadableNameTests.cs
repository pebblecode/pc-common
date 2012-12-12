using Microsoft.VisualStudio.TestTools.UnitTesting;
using PebbleCode.Framework;

namespace PebbleCode.Tests.Unit.PC.Framework
{
    [TestClass]
    public class EnumHelpers_ReadableNameTests
    {
        [TestMethod]
        public void ReadableName_OnNotAttributedEnum_ReturnReadableNamesAsExpected()
        {
            //ARRANGE
            const NotAttributedEnum first = NotAttributedEnum.FirstValue;
            const NotAttributedEnum second = NotAttributedEnum.SecondValue;

            //ACT
            string firstReadableName = first.ReadableName();
            string secondReadableName = second.ReadableName();
            
            //ASSERT
            Assert.AreEqual("First Value", firstReadableName);
            Assert.AreEqual("Second Value", secondReadableName);
        }

        [TestMethod]
        public void ReadableName_OnPartiallyAttributedEnum_ReturnReadableNamesAsExpected()
        {
            //ARRANGE
            const PartiallyAttributedEnum first = PartiallyAttributedEnum.FirstValue;
            const PartiallyAttributedEnum second = PartiallyAttributedEnum.SecondValue;

            //ACT
            string firstReadableName = first.ReadableName();
            string secondReadableName = second.ReadableName();

            //ASSERT
            Assert.AreEqual("First Value", firstReadableName);
            Assert.AreEqual("Second Value From Attribute", secondReadableName);
        }

        [TestMethod]
        public void ReadableName_OnFullyAttributedEnum_ReturnReadableNamesAsExpected()
        {
            //ARRANGE
            const FullyAttributedEnum first = FullyAttributedEnum.FirstValue;
            const FullyAttributedEnum second = FullyAttributedEnum.SecondValue;

            //ACT
            string firstReadableName = first.ReadableName();
            string secondReadableName = second.ReadableName();

            //ASSERT
            Assert.AreEqual("First Value From Attribute", firstReadableName);
            Assert.AreEqual("Second Value From Attribute", secondReadableName);
        }

        private enum NotAttributedEnum
        {
            FirstValue,
            SecondValue,
        }

        private enum PartiallyAttributedEnum
        {
            FirstValue,
            [ReadableName("Second Value From Attribute")]
            SecondValue,
        }

        private enum FullyAttributedEnum
        {
            [ReadableName("First Value From Attribute")]
            FirstValue,
            [ReadableName("Second Value From Attribute")]
            SecondValue,
        }
    }
}
