using System.Diagnostics;

namespace PebbleCode.Framework.Logging
{
    public class FakeLogManager : ILogManager
    {
        /// <summary>
        /// Default to true
        /// </summary>
        public bool LogError { get; set; }

        /// <summary>
        /// Default to true
        /// </summary>
        public bool LogWarning { get; set; }

        /// <summary>
        /// Default to true
        /// </summary>
        public bool LogInfo { get; set; }
        
        /// <summary>
        /// Default to true
        /// </summary>
        public bool LogDebug { get; set; }

        /// <summary>
        /// Default to true
        /// </summary>
        public bool ThrowExceptionOnError { get; set; }

        /// <summary>
        /// Default to false
        /// </summary>
        public bool ThrowExceptionOnWaring { get; set; }

        public FakeLogManager()
        {
            LogError = true;
            LogWarning = true;
            LogInfo = true;
            LogDebug = true;
            ThrowExceptionOnError = true;
        }

        public bool ShouldLog(string category, TraceEventType level)
        {
            switch (level)
            {
                case Logger.ERROR:
                    return LogError;
                case Logger.WARNING:
                    return LogWarning;
                case Logger.INFO:
                    return LogInfo;
                case Logger.DEBUG:
                    return LogDebug;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Throws exception when ThrowExceptionOnError|ThrowExceptionOnWaring is set
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="level"></param>
        public void WriteLog(string message, string category, TraceEventType level)
        {
            if((ThrowExceptionOnError && level == TraceEventType.Error) ||
                (ThrowExceptionOnWaring && level == TraceEventType.Warning))
            {
                throw new FakeLogManagerLogWrittenException(message, category, level);
            }
        }
    }
}
