using System;
using System.Diagnostics;
using System.IO;
using ELAB = Microsoft.Practices.EnterpriseLibrary.Logging;

namespace PebbleCode.Framework.Logging
{
    /// <summary>
    /// Central class for all log messages to pass through. Abstracts out the use of ELAB
    /// </summary>
    public class LogManager : ILogManager
    {
        public bool ShouldLog(string category, TraceEventType severity)
        {
            try
            {
                ELAB.LogEntry logEntry = new ELAB.LogEntry();
                logEntry.Categories.Add(category);
                logEntry.Severity = severity;
                return ELAB.Logger.ShouldLog(logEntry);

            }
            catch (Exception e)
            {
                DumpException(e);
                return false;
            }
        }

        public void WriteLog(string message, string category, TraceEventType level)
        {
            if (level == TraceEventType.Error)
                category = Category.GeneralError;

            try
            {
                ELAB.LogEntry logEntry = new ELAB.LogEntry();
                logEntry.Categories.Add(category);
                logEntry.Message = message;
                logEntry.Priority = 1;
                logEntry.Severity = level;
                ELAB.Logger.Write(logEntry);

            }
            catch (Exception e)
            {
                DumpException(e);
            }
        }



        /// <summary>
        /// Dump an exception out to last resort log. Do not throw exceptions out of here.
        /// </summary>
        /// <param name="e"></param>
        private static void DumpException(Exception e)
        {
            try
            {
                string exDumpFile = Path.Combine(AppDomain.CurrentDomain.SetupInformation.ApplicationBase, "exceptionDump.txt");
                ExceptionDump.Dump(e, "LoggingError", exDumpFile);
            }
            catch (Exception)
            {
                // Argh! This is really bad news. Nothing we can do now.
                // Simply ignore it.
            }
        }
    }
}
