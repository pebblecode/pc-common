using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PebbleCode.Framework.Dates
{
    /// <summary>
    /// Represent a day of week as a flags enum
    /// </summary>
    [Flags]
    public enum Day 
    {
        Sunday = 1,
        Monday = 2,
        Tuesday = 4,
        Wednesday = 8,
        Thursday = 16,
        Friday = 32,
        Saturday = 64,

        WeekDays = Day.Monday | Day.Tuesday | Day.Wednesday | Day.Thursday | Day.Friday,
        All = Day.WeekDays | Day.Sunday | Day.Saturday,
    }

    /// <summary>
    /// Extension methods for Day enum
    /// </summary>
    public static class DayUtilsExt
    {
        /// <summary>
        /// Convert to a DayOfWeek
        /// </summary>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DayOfWeek ToDayOfWeek(this Day input)
        {
            switch (input)
            {
                case Day.Sunday: return DayOfWeek.Sunday;
                case Day.Monday: return DayOfWeek.Monday;
                case Day.Tuesday: return DayOfWeek.Tuesday;
                case Day.Wednesday: return DayOfWeek.Wednesday;
                case Day.Thursday: return DayOfWeek.Thursday;
                case Day.Friday: return DayOfWeek.Friday;
                case Day.Saturday: return DayOfWeek.Saturday;
            }

            throw new InvalidOperationException("Unknown day: " + input.ToString());
        }

        /// <summary>
        /// Convert a DayOfWeek to a Day
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Day ToDay(this DayOfWeek input)
        {
            switch (input)
            {
                case DayOfWeek.Sunday: return Day.Sunday;
                case DayOfWeek.Monday: return Day.Monday;
                case DayOfWeek.Tuesday: return Day.Tuesday;
                case DayOfWeek.Wednesday: return Day.Wednesday;
                case DayOfWeek.Thursday: return Day.Thursday;
                case DayOfWeek.Friday: return Day.Friday;
                case DayOfWeek.Saturday: return Day.Saturday;
            }

            throw new InvalidOperationException("Unknown day: " + input.ToString());
        }
    }
}
