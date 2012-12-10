using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PebbleCode.Framework.Utilities
{
    /// <summary>
    /// Extensions to the Math class.  Would be nice to be able to write static extension methods
    /// </summary>
    public static class MathUtils
    {
        /// <summary>
        /// Rounds the leadst significant digit up, if there is a successive fractional part
        /// </summary>
        /// <param name="number"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static decimal RoundUp(decimal number, int digits)
        {
            return Math.Ceiling(number * (decimal)Math.Pow(10, digits)) / (decimal)Math.Pow(10, digits);
        }

        /// <summary>
        /// Rounds the least successive digit down
        /// </summary>
        /// <param name="number"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static decimal RoundDown(decimal number, int digits)
        {
            return Math.Floor(number * (decimal)Math.Pow(10, digits)) / (decimal)Math.Pow(10, digits);
        }

        /// <summary>
        /// Trucate the least successive digit down
        /// </summary>
        /// <param name="number"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static decimal Truncate(decimal number, int digits)
        {
            return Math.Truncate(number * (decimal)Math.Pow(10, digits)) / (decimal)Math.Pow(10, digits);
        }

        /// <summary>
        /// Performs the default rounding
        /// </summary>
        /// <param name="number"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        public static decimal Round(decimal number, int digits)
        {
            return Math.Round(number, digits);
        }
    }
}
