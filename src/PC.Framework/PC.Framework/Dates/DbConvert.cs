using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PebbleCode.Framework.Dates
{
    /// <summary>
    /// Conversion methods for converting between database value types and entity value types
    /// </summary>
    public static class DbConvert
    {
        /// <summary>
        /// Convert a date stored as an int into a DateTime
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime FromDateInt(int input)
        {
            return DateTime.ParseExact(input.ToString(), "yyyyMMdd", null);
        }

        public static DateTime? FromDateInt(int? input)
        {
            if (input.HasValue) return FromDateInt(input.Value);
            return null;
        }

        /// <summary>
        /// Convert a date to an int for safe storage without the time
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int ToDateInt(DateTime input)
        {
            return int.Parse(input.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// Convert a date to an int for safe storage without the time
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int? ToDateInt(DateTime? input)
        {
            if (input.HasValue) return ToDateInt(input.Value);
            return null;
        }
    }
}
