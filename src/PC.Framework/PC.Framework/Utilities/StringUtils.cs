using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PebbleCode.Framework.Utilities
{
    /// <summary>
    /// String utility functions
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Parse the future contract delivery date from a Bloomberg ticker.  See
        /// http://commodities.about.com/od/understandingthebasics/ss/futurescontract_3.htm
        /// http://firestone.princeton.edu/econlib/blp/docs/tickers.pdf
        /// </summary>
        /// <param name="ticker"></param>
        /// <returns></returns>
        public static DateTime ReadFuturesDate(string ticker, DateTime? referenceDate = null)
        {
            DateTime futuresDate = DateTime.MinValue;
            
            if (!referenceDate.HasValue)
                referenceDate = DateTime.Now;

            Match match = Regex.Match(ticker, @"\w\d");
            if (!match.Success)
                throw new ArgumentException("Invalid futures code. Expected Bloomberg ticker in the format (ABC1 or ABCD1)");

            try
            {
                //Parse out the month first
                int month = 0;
                switch (char.ToUpper(match.Value[0]))
                {
                    case 'F': month = 1; break;
                    case 'G': month = 2; break;
                    case 'H': month = 3; break;
                    case 'J': month = 4; break;
                    case 'K': month = 5; break;
                    case 'M': month = 6; break;
                    case 'N': month = 7; break;
                    case 'Q': month = 8; break;
                    case 'U': month = 9; break;
                    case 'V': month = 10; break;
                    case 'X': month = 11; break;
                    case 'Z': month = 12; break;
                    default: throw new ArgumentException("Invalid futures code. Expected Bloomberg ticker in the format (ABC1 or ABCD1)");
                }

                //Parse out the year.
                int year = 2000 + int.Parse(match.Value[1].ToString());
                futuresDate = new DateTime(year, month, 1);
                while (futuresDate < referenceDate.Value)
                {
                    futuresDate = futuresDate.AddYears(10);
                }
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch
            {
                throw new ArgumentException("Invalid futures code. Expected Bloomberg ticker in the format (ABC1 or ABCD1)");
            }
            return futuresDate;
        }

        //public static ComputeTokenisedLevenshtein(string str1, string str2)
        //{
        //    //Tokenise both strings
        //    string[] str1Tokens = sanitize(str1).Split(' ');
        //    string[] str2Tokens = sanitize(str2).Split(' ');

        //    //compute a grid of levenshtein distances to work out the most similar tokens
        //    int[,] gridDists = new int[str1Tokens.Length, str2Tokens.Length];

        //    //Build 2 new string of aligned tokens
        //    for (int i = 0; i < str1Tokens.Length; i++)
        //    {
        //        for (int j = 0; j < str1Tokens.Length; j++)
        //        {
        //        }
        //    }

        //    //Compute final levenshtein
        //}

        public static string ToPercentage(decimal x)
        {
            return string.Format("{0:P}", x);
        }

        public static string NumberFormat(decimal x)
        {
            return string.Format("{0:N}", x);
        }

        /// <summary>
        /// Compute the distance between two strings.  Reference: http://dotnetperls.com/levenshtein
        /// </summary>
        public static int ComputeLevenshtein(string str1, string str2)
        {
            str1 = sanitize(str1);
            str2 = sanitize(str2);

            //Re-order tokens
            List<string> str1Tokens = new List<string>(sanitize(str1).Split(' '));
            List<string> str2Tokens = new List<string>(sanitize(str2).Split(' '));
            str1Tokens.Sort();
            str2Tokens.Sort();
            str1 = string.Join(string.Empty, str1Tokens);
            str2 = string.Join(string.Empty, str2Tokens);

            int n = str1.Length;
            int m = str2.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (str2[j - 1] == str1[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        private static string sanitize(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if (char.IsLetterOrDigit(c))
                {
                    sb.Append(char.ToLower(c));
                }
            }
            return sb.ToString();
        }
    }
}
