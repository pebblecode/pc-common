using System;
using System.Text;
using System.IO;
using System.Reflection;

namespace PebbleCode.Framework.Logging
{
    /// <summary>
    /// Emergency exception logging
    /// </summary>
    public class ExceptionDump
    {
        public const string DEFAULT_DUMP_FILENAME = "exceptionDump.txt";

        /// <summary>
        /// Dump an exception to the default file in the apps main dir
        /// </summary>
        /// <param name="e">Exception to log</param>
        /// <param name="message">Additional message to write along with exception</param>
        public static void Dump(Exception e, string message)
        {
            string runDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            Dump(e, message, Path.Combine(runDir, DEFAULT_DUMP_FILENAME));
        }

        /// <summary>
        /// Public method to dump an exception
        /// </summary>
        /// <param name="e">Exception to log</param>
        /// <param name="message">Additional message to write along with exception</param>
        /// <param name="filename"></param>
        public static void Dump(Exception e, string message, string filename)
        {
            // Build up the exception text
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Exception details. ");
            sb.AppendLine("Date/Time:   " + DateTime.Now.ToString("dd-MM-yyyy HH:mm"));
            sb.AppendLine("Additional:  " + message);
            sb.AppendLine("Dump follows");
            sb.AppendLine();

            // Recurse through the exception stack
            DumpExceptionStack(sb, e);

            // Now output to file
            using (StreamWriter sw = new StreamWriter(filename, false))
            {
                sw.Write(sb.ToString());
                sw.Close();
            }
        }

        /// <summary>
        /// Private helper
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="e"></param>
        private static void DumpExceptionStack(StringBuilder sb, Exception e)
        {
            if (e == null)
                return;

            DumpExceptionDetails(sb, e);
            DumpExceptionStack(sb, e.InnerException);
        }

        /// <summary>
        /// Private helper method
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="e"></param>
        private static void DumpExceptionDetails(StringBuilder sb, Exception e)
        {
            sb.AppendLine("Message:     " + e.Message);
            sb.AppendLine("Type:        " + e.GetType().FullName);
            sb.AppendLine("Stack Trace: ");
            sb.AppendLine(e.StackTrace);
        }
    }
}
