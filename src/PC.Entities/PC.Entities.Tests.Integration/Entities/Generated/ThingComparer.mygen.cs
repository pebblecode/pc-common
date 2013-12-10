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
    public class ThingComparer : EntityComparerBase
    {
        /// <summary>
        /// Compare two Thing entities
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void Compare(Thing expected, Thing actual)
        {
            // Check for nulls
            if (expected == null && actual == null) return;
            if (expected == null) Assert.Fail("Expected null, got Thing");
            if (actual == null) Assert.Fail("Expected Thing, got null");

            // Compare simple properties
			Assert.AreEqual(expected.Id, actual.Id, "Thing.Id not equal");
			Assert.AreEqual(expected.Name, actual.Name, "Thing.Name not equal");
			Assert.AreEqual(expected.Corners, actual.Corners, "Thing.Corners not equal");
			Assert.AreEqual(expected.Edges, actual.Edges, "Thing.Edges not equal");
			Assert.AreEqual(expected.Test, actual.Test, "Thing.Test not equal");
			
			// Compare WidgetList
			if (expected.WidgetListPopulated && actual.WidgetListPopulated)
				WidgetComparer.Compare(expected.WidgetList, actual.WidgetList);
		}

        /// <summary>
        /// Compare lists of Thing entities
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void Compare(ThingList expected, ThingList actual)
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