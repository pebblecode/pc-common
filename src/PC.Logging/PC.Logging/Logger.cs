using System;
using System.Diagnostics;
using System.Text;

namespace PebbleCode.Framework.Logging
{
    public static class Logger
    {
        private static ILogManager _loggerInstance = new LogManager();
        private static readonly object loggerInstanceLock = new object();

        public const TraceEventType ERROR = TraceEventType.Error;
        public const TraceEventType WARNING = TraceEventType.Warning;
        public const TraceEventType DEBUG = TraceEventType.Verbose;
        public const TraceEventType INFO = TraceEventType.Information;

        public static ILogManager LoggerInstance
        {
            get
            {
                lock (loggerInstanceLock)
                {
                    return _loggerInstance;
                }
            }
            set
            {
                lock (loggerInstanceLock)
                {
                    _loggerInstance = value;
                }
            }
        }

        public static bool ShouldLogInfo(string category)
        {
            return LoggerInstance.ShouldLog(category, INFO);
        }

        public static bool ShouldLogDebug(string category)
        {
            return LoggerInstance.ShouldLog(category, DEBUG);
        }

        public static bool ShouldLogWarning(string category)
        {
            return LoggerInstance.ShouldLog(category, WARNING);
        }

        public static bool ShouldLogError(string category)
        {
            return LoggerInstance.ShouldLog(category, ERROR);
        }


        public static Exception WriteUnexpectedException(Exception exception, string additionalMessage, string category)
        {
            WriteException(exception, additionalMessage, category, TraceEventType.Error);
            if (exception != null)
            {
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                    WriteException(exception, "Inner Exception", category, TraceEventType.Error);
                }
            }
            return new Exception(additionalMessage, exception);
        }

        [DebuggerStepThrough]
        public static Exception WriteError(string message, string category, params object[] args)
        {
            FormatMessage(ref message, args);
            LoggerInstance.WriteLog(message, category, ERROR);

            return new Exception(message);
        }

        public static void WriteWarning(string message, string category, params object[] args)
        {
            FormatMessage(ref message, args);
            LoggerInstance.WriteLog(message, category, WARNING);
        }

        public static void WriteInfo(string message, string category, params object[] args)
        {
            FormatMessage(ref message, args);
            LoggerInstance.WriteLog(message, category, INFO);
        }

        public static void WriteDebug(string message, string category, params object[] args)
        {
            FormatMessage(ref message, args);
            LoggerInstance.WriteLog(message, category, DEBUG);
        }

        /// <summary>
        /// Internal helper to format and write exceptions
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="additionalMessage"></param>
        /// <param name="category"></param>
        /// <param name="level"></param>
        private static void WriteException(Exception exception, string additionalMessage, string category, TraceEventType level)
        {
            if (exception == null)
                return;

            // Build message.
            StringBuilder message = new StringBuilder();
            message.AppendLine(exception.Message);

            // Add other basic info
            message.Append("  Type: ").AppendLine(exception.GetType().Name);
            if (!string.IsNullOrEmpty(additionalMessage))
                message.Append("  Additional: ").AppendLine(additionalMessage);
            message.AppendLine("  Stack Trace: ");
            message.Append(exception.StackTrace);

            // Log
            LoggerInstance.WriteLog(message.ToString(), category, level);
        }

        /// <summary>
        /// Utility method to format a message string if format arguments are specified.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        [DebuggerStepThrough]
        private static void FormatMessage(ref string message, object[] args)
        {
            try
            {
                if (args != null && args.Length > 0)
                {
                    message = string.Format(message, args);
                }
            }
            catch
            {
                //Don't bring application crashing down just because an obscure error message has wrong number of formatting arguments
                WriteError("Incorrectly formatted log message at {0}", Category.General, (new StackTrace()).ToString());
            }
        }
    }
}
