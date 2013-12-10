/*****************************************************************************/
/***                                                                       ***/
/***    This is an automatically generated file. It is generated using     ***/
/***    MyGeneration in conjunction with IBatisBusinessObject template.    ***/
/***                                                                       ***/
/***    DO NOT MODIFY THIS FILE DIRECTLY!                                  ***/
/***                                                                       ***/
/***    If you need to make changes either modify the template and         ***/
/***    regenerate or derive a class from this class and override.         ***/
/***                                                                       ***/
/*****************************************************************************/

using NUnit.Framework;
using PC.Tests;
using PebbleCode.Framework.Dates;
using PebbleCode.Entities.Tests.Integration.Entities;

namespace PebbleCode.Tests.EntityComparers
{
    public class NodeBuilderTestComparer : EntityComparerBase
    {
        /// <summary>
        /// Compare two NodeBuilderTest entities
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void Compare(NodeBuilderTest expected, NodeBuilderTest actual)
        {
            // Check for nulls
            if (expected == null && actual == null) return;
            if (expected == null) Assert.Fail("Expected null, got NodeBuilderTest");
            if (actual == null) Assert.Fail("Expected NodeBuilderTest, got null");

            // Compare simple properties
			Assert.AreEqual(expected.Id, actual.Id, "NodeBuilderTest.Id not equal");
			Assert.AreEqual(expected.Field1, actual.Field1, "NodeBuilderTest.Field1 not equal");
			Assert.AreEqual(expected.Field2, actual.Field2, "NodeBuilderTest.Field2 not equal");
			Assert.AreEqual(expected.NodeIdField, actual.NodeIdField, "NodeBuilderTest.NodeIdField not equal");
		}

        /// <summary>
        /// Compare lists of NodeBuilderTest entities
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void Compare(NodeBuilderTestList expected, NodeBuilderTestList actual)
        {
            // Check for nulls
            if (expected == null && actual == null)  return;
            if (expected == null) Assert.Fail("Expected null, got list");
            if (actual == null) Assert.Fail("Expected list, got null");

            // Check counts
            Assert.AreEqual(expected.Count, actual.Count, "List counts not equal");

            // Now compare each entity in the list
            expected.SortById();
            actual.SortById();
            for (int index = 0; index < expected.Count; index++)
				Compare(expected[index], actual[index]);
        }
    }
}