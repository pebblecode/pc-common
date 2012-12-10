using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PebbleCode.Framework.Dates
{
    public class DateRange
    {
        public static readonly DateRange EmptyDateRange = new DateRange();

        public DateRange(DateTime startDate, DateTime endDate)
        {
            this.StartDate = startDate;
            this.EndDate = endDate;
        }

        private DateRange()
        {
        }

        public static DateRange CreateDateRangeWithStartDate(DateTime startDate)
        {
            return new DateRange
            {
                StartDate = startDate
            };
        }

        public static DateRange CreateDateRangeWithEndDate(DateTime? endDate)
        {
            return new DateRange
            {
                EndDate = endDate
            };
        }

        public static DateRange CreateDateRangeWithEndDate(DateTime endDate)
        {
            DateTime? nullableEndDate = endDate;
            return CreateDateRangeWithEndDate(nullableEndDate);
        }

        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }

        public bool IsInRange(DateTime date)
        {
            return IsOnOrAfterStartDate(date) && IsOnOrBeforeEndDate(date);
        }

        private bool IsOnOrAfterStartDate(DateTime date)
        {
            if (StartDate.HasValue)
            {
                return date >= StartDate;
            }
            return true;
        }

        private bool IsOnOrBeforeEndDate(DateTime date)
        {
            if (EndDate.HasValue)
            {
                return date <= EndDate;
            }
            return true;
        }
    }
}
