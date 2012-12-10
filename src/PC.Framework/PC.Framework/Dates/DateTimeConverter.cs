using System;
using System.Globalization;

namespace PebbleCode.Framework.Dates
{
    public static class DateTimeConverter
    {
        public const string DATE_FMT = "yyyyMMdd";
        public const string DATETIME_FMT = "yyyyMMddHHmm";
        public const string DATETIME_WITH_SECONDS_FMT = "yyyyMMddHHmmss";

        /// <summary>
        /// Parse a date in the format DATE_FMT to a DateTime 
        /// </summary>
        public static DateTime? ParseDate(string dateStr)
        {
            if (string.IsNullOrWhiteSpace(dateStr))
            {
                return null;
            }
            return ParseDate(dateStr, DATE_FMT);
        }

        private static DateTime? ParseDate(string dateStr, string format)
        {
            DateTime date;
            if (!DateTime.TryParseExact(dateStr, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out date))
                throw new ArgumentException(string.Format("Invalid date format. Expected {0}", format.ToLower()), "date");
            return date;
        }

        /// <summary>
        /// Format a date to the format DATE_FMT
        /// </summary>
        public static string FormatDate(DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToString(DATE_FMT, CultureInfo.InvariantCulture);
            }
            return string.Empty;
        }

        /// <summary>
        /// Format a date to the format DATETIME_FMT
        /// </summary>
        public static string FormatDateTime(DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToString(DATETIME_FMT, CultureInfo.InvariantCulture);
            }
            return string.Empty;
        }

        public static string FormatDateTimeWithSeconds(DateTime? date)
        {
            if (date.HasValue)
            {
                return date.Value.ToString(DATETIME_WITH_SECONDS_FMT, CultureInfo.InvariantCulture);
            }
            return string.Empty;
        }

        public static DateTime? ParseDateWithSeconds(string dateStr)
        {
            if (string.IsNullOrWhiteSpace(dateStr))
            {
                return null;
            }
            return ParseDate(dateStr, DATETIME_WITH_SECONDS_FMT);
        }
    }
}
