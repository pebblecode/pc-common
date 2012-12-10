using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PebbleCode.Framework.Dates
{
    /// <summary>
    /// Utility functions for working with dates
    /// </summary>
    public static class DateUtils
    {
        public const string DB_DATE_FMT = "yyyyMMdd";
        public const string STD_DATE_FMT = "g";

        /// <summary>
        /// Returns true if one or more dates in the range are week days.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static bool RangeContainsWeekdays(DateTime startDate, DateTime endDate)
        {
            foreach (DateTime date in EnumerateDates(startDate, endDate))
            {
                if (!date.IsWeekend())
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Returns a date time object representing the next time the system clock will hit the specified time
        /// </summary>
        /// <param name="hours"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static DateTime NextRunTime(int hours, int minutes)
        {
            DateTime dt = new DateTime(DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                hours,
                minutes,
                0);
            if (dt < DateTime.Now)
                dt = dt.AddDays(1);

            return dt;
        }

        /// <summary>
        /// Returns a commonly used YYYYMMDD format of this date.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToYYYYMMDD(DateTime? date)
        {
            if (!date.HasValue) return string.Empty;
            return date.Value.ToString(DB_DATE_FMT);
        }

        /// <summary>
        /// Parse a date time from this string
        /// </summary>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static DateTime? FromYYYYMMDD(object pattern)
        {
            if (pattern == null || string.IsNullOrWhiteSpace(pattern.ToString())) return null;

            string str = pattern.ToString();
            if (str.Length > DB_DATE_FMT.Length)
            {
                Regex regex = new Regex(@".*(\d\d\d\d\d\d\d\d).*");
                Match m = regex.Match(str);
                if (!m.Success)
                {
                    return null;
                }
                else
                {
                    str = m.Groups[1].Value;
                }
            }
            return DateTime.ParseExact(str, DB_DATE_FMT, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Centralises all date formatting
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string ToDateString(DateTime? date)
        {
            if (!date.HasValue)
            {
                return "<null>";
            }
            else
            {
                return ToDateString(date.Value);
            }
        }

        public static string ToDateString(DateTime date)
        {
            return date.ToString("d");
        }

        /// <summary>
        /// Returns an array of date objects between the upper and lower bounds, inclusive
        /// </summary>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <returns></returns>
        public static DateTime[] EnumerateDates(DateTime fromDate, DateTime toDate)
        {
            if (toDate < fromDate)
                throw new ArgumentException("toDate must be later than from date");

            List<DateTime> dates = new List<DateTime>();

            while (fromDate <= toDate)
            {
                dates.Add(fromDate);
                fromDate = fromDate.AddDays(1);
            }
            return dates.ToArray();
        }
    }
}
