using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System.Collections.Specialized;
using System.Diagnostics;

namespace PebbleCode.Framework.Logging
{
    /// <summary>
    /// Log formatter for logging to file
    /// </summary>
    [ConfigurationElementType(typeof(CustomFormatterData))]
    public class FileFormatter : LogFormatter
    {
        /// <summary>
        /// Constructor. Required for use in ELAB
        /// </summary>
        /// <param name="nameValueCollection"></param>
        public FileFormatter(NameValueCollection nameValueCollection)
        {
        }

        /// <summary>
        /// Do the formatting
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public override string Format(LogEntry log)
        {
            string date = log.TimeStamp.ToString("dd-MM HH:mm:ss"); 
            string message = log.Message.Replace(Environment.NewLine, "///   ");
            string category = log.CategoriesStrings[0];
            string severity = ToLogString(log.Severity);

            return string.Format("{0,-18}{1,-16}{2,-10}{3}", date, category, severity, message);
        }

        /// <summary>
        /// Override the names of some logging
        /// </summary>
        /// <param name="severity"></param>
        /// <returns></returns>
        private string ToLogString(TraceEventType severity)
        {
            switch (severity)
            {
                case TraceEventType.Information: return "Info";
                case TraceEventType.Verbose: return "Debug";
                default: return severity.ToString();
            }
        }
    }
}
