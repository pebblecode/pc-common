using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PebbleCode.Framework.Dates;
using System.Reflection;
using System.IO;

namespace PebbleCode.Framework
{
    public static class Extensions
    {
        /// <summary>
        /// Get the full filename of the assembly
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        public static string GetFilename(this Assembly assembly)
        {
            string codeBase = assembly.CodeBase;
            UriBuilder uri = new UriBuilder(codeBase);
            return Uri.UnescapeDataString(uri.Path);
        }

        /// <summary>
        /// Constrain a decimal to min/max inclusive
        /// </summary>
        /// <param name="input"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static decimal Constrain(this decimal input, decimal min, decimal max)
        {
            return Math.Max(Math.Min(input, max), min);
        }

        /// <summary>
        /// Is a string null or empty?
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// Remove item from a specified index in a list, returning the item to you
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T RemoveAt<T>(this IList<T> list, int index)
        {
            T result = list[index];
            list.RemoveAt(index);
            return result;
        }

        /// <summary>
        /// Remove all item from a list matching the predicate, returning the removed items to you
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static List<T> RemoveAll<T>(this IList<T> list, Predicate<T> predicate)
        {
            List<T> result = new List<T>();
            for (int index = 0; index < list.Count; )
            {
                if (predicate(list[index]))
                    result.Add(list.RemoveAt<T>(index));
                else
                    index++;
            }
            return result;
        }

        /// <summary>
        /// Determine the number of decimal places present in a decimal value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public static int DecimalPlaces(this decimal number)
        {
            // Convert to a string
            string numberAsString = number.ToString();
            numberAsString = numberAsString.TrimEnd('0');

            // Find the decimal point
            int indexOfDecimalPoint = numberAsString.IndexOf(".");

            // No decimal point, no decimal places
            if (indexOfDecimalPoint < 0)
                return 0;

            // Decimal places is number of chars AFTER dp
            return numberAsString.Length - (indexOfDecimalPoint + 1);
        }

        /// <summary>
        /// Determine the number of decimal places present in a nullable decimal value
        /// </summary>
        /// <param name="number"></param>
        /// <returns>0 if nullable, otherwise number of dps</returns>
        public static int DecimalPlaces(this decimal? number)
        {
            if (number.HasValue)
                return number.Value.DecimalPlaces();
            else
                return 0;
        }

        /// <summary>
        /// Join an array of items together into a string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="sep"></param>
        /// <returns></returns>
        public static string ToString<TInput>(this TInput[] input, string sep)
        {
            return string.Join(" ", Array.ConvertAll<TInput, string>(input, (i) => i.ToString()));
        }

        /// <summary>
        /// As above, but with a generic enumerable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="toJoin"></param>
        /// <param name="sep"></param>
        /// <returns></returns>
        public static string ToString<T>(this IEnumerable<T> toJoin, string sep)
        {
            List<T> list = new List<T>(toJoin);
            return list.ToArray().ToString<T>(sep);
        }

        /// <summary>
        /// Enqueue a range of items 1 by 1.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queue"></param>
        /// <param name="items"></param>
        public static void EnqueueRange<T>(this Queue<T> queue, IEnumerable<T> items)
        {
            foreach (T item in items)
                queue.Enqueue(item);
        }

        public static string Truncate(this string input, int length)
        {
            if (input == null || input.Length <= length)
            {
                return input;
            }
            return input.Substring(0, length);
        }

        public static void TrimStart<T>(this IList<T> input, int length)
        {
            while (input.Count > length)
            {
                input.RemoveAt(0);
            }
        }


        /// <summary>
        /// Update each element in a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="updater"></param>
        public static void UpdateEach<T>(this IList<T> input, UpdateAction<T> updater)
        {
            for (int index = 0; index < input.Count; index++)
                input[index] = updater(input[index]);
        }

        /// <summary>
        /// Neatly converts an integer array to a string array.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string[] ToStringArray(this int[] input)
        {
            return input.Select<int, string>((runnerId) => runnerId.ToString()).ToArray();
        }

        /// <summary>
        /// Join some ints into a string
        /// </summary>
        /// <param name="input"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string ToJoinedString(this int[] input, string separator)
        {
            return string.Join(separator, input.ToStringArray());
        }

        /// <summary>
        /// Split a string and convert each element to an int
        /// </summary>
        /// <param name="input"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static int[] ToIntArray(this string input, string separator)
        {
            string[] strings = input.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            return strings.Select<string, int>((str) => Int32.Parse(str)).ToArray();
        }

        /// <summary>
        /// Get a decimal from a random number generator
        /// </summary>
        /// <param name="rng"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static decimal NextDecimal(this Random rng, decimal min, decimal max)
        {
            decimal fraction = (decimal)rng.NextDouble();
            return min + ((max * fraction) - Math.Abs(min * fraction));
            // N.B. Done this way so that if max is decimal.max and min is decimal.min then we
            // don't get a stack overflow doing the diff of them.
        }

        /// <summary>
        /// Batch adds a collection of T to this hashset
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="set">The set to which Ts should be added</param>
        /// <param name="toAdd">The collectio of Ts to add</param>
        public static void AddAll<T>(this HashSet<T> set, IEnumerable<T> toAdd)
        {
            foreach (T t in toAdd)
            {
                set.Add(t);
            }
        }

        /// <summary>
        /// Clone a dictionary of values
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> Clone<TKey, TValue>(this Dictionary<TKey, TValue> source)
        {
            Dictionary<TKey, TValue> target = new Dictionary<TKey, TValue>();
            foreach (TKey key in source.Keys)
                target.Add(key, source[key]);
            return target;
        }

        /// <summary>
        /// Get multiple values from a dictionary
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="source"></param>
        /// <param name="keys">The keys to get matching values for</param>
        /// <returns></returns>
        public static List<TValue> Get<TKey, TValue>(this Dictionary<TKey, TValue> source, IEnumerable<TKey> keys)
        {
            List<TValue> result = new List<TValue>();
            foreach (TKey key in keys)
                result.Add(source[key]);
            return result;
        }

        /// <summary>
        /// Shortcut to format a string message without all the mess of "String.Format"
        /// </summary>
        /// <param name="str">A format string</param>
        /// <param name="args">Some format string arguments</param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string fmt(this String str, params object[] args)
        {
            if (str == null) return string.Empty;

            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    if (args[i] is DateTime || args[i] is DateTime?)
                    {
                        args[i] = ((DateTime?)args[i]).fmt();
                    }
                }
            }

            return String.Format(str, args);
        }

        /// <summary>
        /// Shortcut extension to format a date using project standard format string
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string fmt(this DateTime? dt)
        {
            return DateUtils.ToDateString(dt);
        }

        /// <summary>
        /// Shortcut extension to format a date using project standard format string
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string fmt(this DateTime dt)
        {
            return DateUtils.ToDateString(dt);
        }

        /// <summary>
        /// Adds the useful ForEach() function to IEnumerable collections
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumerable"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T t in enumerable)
            {
                action(t);
            }
        }

        /// <summary>
        /// Merge the values from source into target. Returns target for convenience and use in F# List.reduce
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="target"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> target, Dictionary<TKey, TValue> source)
        {
            foreach (TKey key in source.Keys)
                target[key] = source[key];
            return target;
        }


        /// <summary>
        /// Returns true if the parameter represents a weekend date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsWeekend(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// Similar to String.GetHashCode but returns the same
        /// as the x86 version of String.GetHashCode for x64 and x86 frameworks.
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static unsafe int GetHashCode32(this string s)
        {
            fixed (char* str = s.ToCharArray())
            {
                char* chPtr = str;
                int num = 0x15051505;
                int num2 = num;
                int* numPtr = (int*)chPtr;
                for (int i = s.Length; i > 0; i -= 4)
                {
                    num = (((num << 5) + num) + (num >> 0x1b)) ^ numPtr[0];
                    if (i <= 2)
                    {
                        break;
                    }
                    num2 = (((num2 << 5) + num2) + (num2 >> 0x1b)) ^ numPtr[1];
                    numPtr += 2;
                }
                return (num + (num2 * 0x5d588b65));
            }
        }

   
        public static bool Defines<AttributeType>(this Type target)
            where AttributeType : Attribute
        {
            return target.GetCustomAttributes(typeof(AttributeType), true).Length > 0;
        }

        /// <summary>
        /// Dumps this stream to disk, in a temporary file, returning the full path to the file, 
        /// resetting the stream position to zero on completion, if the stream is seekable
        /// </summary>
        /// <param name="dataStream">The source data to serialise to disk</param>
        /// <returns>Full path to temporary file</returns>
        public static string ToTempFile(this Stream dataStream, string fileExtension = null)
        {
            const int bufferSize = 10 * 1024;
            byte[] buffer = new byte[bufferSize];
            int read = 0;

            if (!dataStream.CanRead) throw new Exception("Can't render stream to file - unable to read from stream");
            if (!dataStream.CanSeek) throw new Exception("Can't render stream to file - unable to seek to begining of stream");

            dataStream.Position = 0;
            string tempPath = Path.GetTempFileName();
            if (!string.IsNullOrEmpty(fileExtension))
            {
                tempPath = tempPath + "." + fileExtension.TrimStart('.');
            }
            using (FileStream output = File.OpenWrite(tempPath))
            {
                while ((read = dataStream.Read(buffer, 0, bufferSize)) > 0)
                    output.Write(buffer, 0, read);
            }
            dataStream.Position = 0;
            return tempPath;
        }

        /// <summary>
        /// Lazily reads a stream passing back tokens
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static IEnumerable<string[]> ReadCsv(this Stream stream)
        {
            StreamReader reader = new StreamReader(stream);
            string line = null;
            while ((line = reader.ReadLine()) != null)
            {
                yield return line.Split(',');
            }
        }
    }

    /// <summary>
    /// Delegate which takes and input and returns an updated version of it
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="input"></param>
    /// <returns></returns>
    public delegate T UpdateAction<T>(T input);
}
