using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging;

namespace PebbleCode.Framework.Logging
{
    /// <summary>
    /// Custom trace listener to output to debug window
    /// </summary>
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class DebugTraceListener : CustomTraceListener
    {
        public DebugTraceListener()
        {

        }

        /// <summary>
        /// Override TraceData to format correctly
        /// </summary>
        /// <param name="eventCache"></param>
        /// <param name="source"></param>
        /// <param name="eventType"></param>
        /// <param name="id"></param>
        /// <param name="data"></param>
        [DebuggerStepThrough]
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            string message;

            // Get formatted message
            LogEntry logEntry = data as LogEntry;
            if (logEntry != null && logEntry.CategoriesStrings.Length > 0)
                message = string.Format("{0,-12}  {1,-12}  {2}", GetCategory(logEntry.CategoriesStrings[0]), GetLevel(logEntry.Severity), logEntry.Message);
            else
                message = data.ToString();

            // Write out each line
            string[] lines = message.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
            foreach (string line in lines)
                this.WriteLine(line);
        }

        /// <summary>
        /// Write to debug
        /// </summary>
        /// <param name="message"></param>
        [DebuggerStepThrough]
        public override void Write(string message)
        {
            Trace.Write(string.Format("<CT>  {0}", message));
        }

        /// <summary>
        /// Write to debug
        /// </summary>
        /// <param name="message"></param>
        [DebuggerStepThrough]
        public override void WriteLine(string message)
        {
            Trace.WriteLine(string.Format("<CT>  {0}", message));
        }

        /// <summary>
        /// Get text for a category
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        private static string GetCategory(string categoryString)
        {
            return "<" + categoryString.ToUpper() + ">";
        }

        /// <summary>
        /// Get text for a level
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        private static string GetLevel(TraceEventType level)
        {
            switch (level)
            {
                case TraceEventType.Critical: return "<CRITICAL>";
                case TraceEventType.Error: return "<ERROR>";
                case TraceEventType.Information: return "<INFO>";
                case TraceEventType.Verbose: return "<DEBUG>";
                case TraceEventType.Warning: return "<WARNING>";
                default: return "<" + level.ToString().ToUpper() + ">";
            }
        }
    }
}
