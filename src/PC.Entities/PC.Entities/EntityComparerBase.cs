using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PebbleCode.Tests.EntityComparers
{
    public class EntityComparerBase
    {
        /// <summary>
        /// Compare two nullable date-times
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="propertyName"></param>
        protected static void Compare(DateTime? expected, DateTime? actual, string propertyName)
        {
            // Check for nulls
            if (expected == null && actual == null) return;
            if (expected == null) Assert.Fail(propertyName + ": Expected null, got byte[]");
            if (actual == null) Assert.Fail(propertyName + ": Expected byte[], got null");

            // Compare the values
            Compare(expected.Value, actual.Value, propertyName);
        }

        /// <summary>
        /// Compare two date-times
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="propertyName"></param>
        protected static void Compare(DateTime expected, DateTime actual, string propertyName)
        {
            // Remove millis from the values as these will be db date times which doesn't store
            // to the right accuracy
            expected = ComparisonSafeDate(expected);
            actual = ComparisonSafeDate(actual);
            Assert.AreEqual(expected, actual, propertyName + " not equal");
        }

        /// <summary>
        /// Compare two byte arrays
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        protected static void Compare(byte[] expected, byte[] actual, string propertyName)
        {
            // Check for nulls
            if (expected == null && actual == null) return;
            if (expected == null) Assert.Fail(propertyName + ": Expected null, got byte[]");
            if (actual == null) Assert.Fail(propertyName + ": Expected byte[], got null");

            // Check lengths
            Assert.AreEqual(expected.Length, actual.Length, propertyName + ": byte[] lengths not equal");

            // Check each byte
            for (int i = 0; i < expected.Length; i++)
                Assert.AreEqual(expected[i], actual[i], string.Format(propertyName + ": byte[]s not equal at position {0}", i));
        }

        /// <summary>
        /// Drop the millis from a date time to make it comparison safe. DateTimes coming out of the db
        /// have all lost there milli seconds as db doesn't store them.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected static DateTime ComparisonSafeDate(DateTime input)
        {
            return new DateTime(input.Year, input.Month, input.Day, input.Hour, input.Minute, input.Second);
        }
    }
}
