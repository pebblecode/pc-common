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
using PebbleCode.Framework.Dates;
using PebbleCode.Tests.Entities;

namespace PebbleCode.Tests.EntityComparers
{
    public class FieldTestComparer : EntityComparerBase
    {
        /// <summary>
        /// Compare two FieldTest entities
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void Compare(FieldTest expected, FieldTest actual)
        {
            // Check for nulls
            if (expected == null && actual == null) return;
            if (expected == null) Assert.Fail("Expected null, got FieldTest");
            if (actual == null) Assert.Fail("Expected FieldTest, got null");

            // Compare simple properties
			Assert.AreEqual(expected.Id, actual.Id, "FieldTest.Id not equal");
			Assert.AreEqual(expected.IntField, actual.IntField, "FieldTest.IntField not equal");
			Assert.AreEqual(expected.IntFieldNullable, actual.IntFieldNullable, "FieldTest.IntFieldNullable not equal");
			Assert.AreEqual(expected.DecimalField, actual.DecimalField, "FieldTest.DecimalField not equal");
			Assert.AreEqual(expected.DecimalFieldNullable, actual.DecimalFieldNullable, "FieldTest.DecimalFieldNullable not equal");
			Assert.AreEqual(expected.StringField, actual.StringField, "FieldTest.StringField not equal");
			Assert.AreEqual(expected.StringFieldNullable, actual.StringFieldNullable, "FieldTest.StringFieldNullable not equal");
			Assert.AreEqual(expected.TextField, actual.TextField, "FieldTest.TextField not equal");
			Assert.AreEqual(expected.TextFieldNullable, actual.TextFieldNullable, "FieldTest.TextFieldNullable not equal");
			Compare(expected.DatetimeField, actual.DatetimeField, "FieldTest.DatetimeField");
			Compare(expected.DatetimeFieldNullable, actual.DatetimeFieldNullable, "FieldTest.DatetimeFieldNullable");
			Assert.AreEqual(expected.TinyintField, actual.TinyintField, "FieldTest.TinyintField not equal");
			Assert.AreEqual(expected.TinyintFieldNullable, actual.TinyintFieldNullable, "FieldTest.TinyintFieldNullable not equal");
			Compare(expected.TimestampField, actual.TimestampField, "FieldTest.TimestampField");
			Compare(expected.TimestampFieldNullable, actual.TimestampFieldNullable, "FieldTest.TimestampFieldNullable");
			Assert.AreEqual(expected.EnumField, actual.EnumField, "FieldTest.EnumField not equal");
			Assert.AreEqual(expected.EnumFieldNullable, actual.EnumFieldNullable, "FieldTest.EnumFieldNullable not equal");
			Assert.AreEqual(expected.ForeignKeyField, actual.ForeignKeyField, "FieldTest.ForeignKeyField not equal");
			Assert.AreEqual(expected.ForeignKeyFieldNullable, actual.ForeignKeyFieldNullable, "FieldTest.ForeignKeyFieldNullable not equal");
			Assert.AreEqual(DbConvert.ToDateInt(expected.IntDateField), DbConvert.ToDateInt(actual.IntDateField), "FieldTest.IntDateField not equal");
			Assert.AreEqual(DbConvert.ToDateInt(expected.IntDateFieldNullable), DbConvert.ToDateInt(actual.IntDateFieldNullable), "FieldTest.IntDateFieldNullable not equal");
			Assert.AreEqual(expected.IndexedField, actual.IndexedField, "FieldTest.IndexedField not equal");
			Assert.AreEqual(expected.IndexedFieldNullable, actual.IndexedFieldNullable, "FieldTest.IndexedFieldNullable not equal");
			Assert.AreEqual(expected.NodeIdField, actual.NodeIdField, "FieldTest.NodeIdField not equal");
			Assert.AreEqual(expected.NodeIdFieldNullable, actual.NodeIdFieldNullable, "FieldTest.NodeIdFieldNullable not equal");
			Assert.AreEqual(expected.DefaultValueIsTwo, actual.DefaultValueIsTwo, "FieldTest.DefaultValueIsTwo not equal");
			
			// Compare ForeignKeyFieldWidget
			if (expected.ForeignKeyFieldWidgetPopulated && actual.ForeignKeyFieldWidgetPopulated)
				WidgetComparer.Compare(expected.ForeignKeyFieldWidget, actual.ForeignKeyFieldWidget);
			
			// Compare ForeignKeyFieldNullableWidget
			if (expected.ForeignKeyFieldNullableWidgetPopulated && actual.ForeignKeyFieldNullableWidgetPopulated)
				WidgetComparer.Compare(expected.ForeignKeyFieldNullableWidget, actual.ForeignKeyFieldNullableWidget);
		}

        /// <summary>
        /// Compare lists of FieldTest entities
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        public static void Compare(FieldTestList expected, FieldTestList actual)
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