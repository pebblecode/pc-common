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
    public class WidgetComparer : EntityComparerBase
    {
        /// <summary>
        /// Compare two Widget entities
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void Compare(Widget expected, Widget actual)
        {
            // Check for nulls
            if (expected == null && actual == null) return;
            if (expected == null) Assert.Fail("Expected null, got Widget");
            if (actual == null) Assert.Fail("Expected Widget, got null");

            // Compare simple properties
			Assert.AreEqual(expected.Id, actual.Id, "Widget.Id not equal");
			Assert.AreEqual(expected.ThingId, actual.ThingId, "Widget.ThingId not equal");
			Assert.AreEqual(expected.Description, actual.Description, "Widget.Description not equal");
			
			// Compare FieldTestListUsingForeignKeyField
			if (expected.FieldTestListUsingForeignKeyFieldPopulated && actual.FieldTestListUsingForeignKeyFieldPopulated)
				FieldTestComparer.Compare(expected.FieldTestListUsingForeignKeyField, actual.FieldTestListUsingForeignKeyField);
			
			// Compare FieldTestListUsingForeignKeyFieldNullable
			if (expected.FieldTestListUsingForeignKeyFieldNullablePopulated && actual.FieldTestListUsingForeignKeyFieldNullablePopulated)
				FieldTestComparer.Compare(expected.FieldTestListUsingForeignKeyFieldNullable, actual.FieldTestListUsingForeignKeyFieldNullable);
			
			// Compare Thing
			if (expected.ThingPopulated && actual.ThingPopulated)
				ThingComparer.Compare(expected.Thing, actual.Thing);
		}

        /// <summary>
        /// Compare lists of Widget entities
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void Compare(WidgetList expected, WidgetList actual)
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