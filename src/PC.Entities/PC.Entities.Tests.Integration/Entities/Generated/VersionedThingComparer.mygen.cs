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

namespace PC.Entities.Tests.Integration.Tests.EntityComparers
{
    public class VersionedThingComparer : EntityComparerBase
    {
        /// <summary>
        /// Compare two VersionedThing entities
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void Compare(VersionedThing expected, VersionedThing actual)
        {
            // Check for nulls
            if (expected == null && actual == null) return;
            if (expected == null) Assert.Fail("Expected null, got VersionedThing");
            if (actual == null) Assert.Fail("Expected VersionedThing, got null");

            // Compare simple properties
			Assert.AreEqual(expected.Id, actual.Id, "VersionedThing.Id not equal");
			Assert.AreEqual(expected.Name, actual.Name, "VersionedThing.Name not equal");
		}

        /// <summary>
        /// Compare lists of VersionedThing entities
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void Compare(VersionedThingList expected, VersionedThingList actual)
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