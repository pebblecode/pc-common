using System.Collections.Generic;
using System.Diagnostics;

namespace PebbleCode.Framework.Logging
{
    public class CountableLogManager : ILogManager
    {
        private readonly ILogManager _logManager;
        public List<LogEntry> LatestLogs { get; private set; }

        public CountableLogManager(ILogManager logManager)
        {
            LatestLogs = new List<LogEntry>();
            _logManager = logManager;
        }

        public bool IsCountChecked { get; set; }

        private int _errorCount;
        public int ErrorCount
        {
            get
            {
                IsCountChecked = true;
                return _errorCount;
            }
        }

        private int _warningCount;
        public int WarningCount
        {
            get
            {
                IsCountChecked = true;
                return _warningCount;
            }
        }

        public bool ShouldLog(string category, TraceEventType severity)
        {
            return _logManager.ShouldLog(category, severity);
        }

        public void WriteLog(string message, string category, TraceEventType level)
        {
            switch (level)
            {
                case Logger.ERROR:
                    _errorCount++;
                    break;
                case Logger.WARNING:
                    _warningCount++;
                    break;
            }
            LatestLogs.Add(new LogEntry {Message = message, Category = category, Level = level});

            _logManager.WriteLog(message,category,level);
        }

        public class LogEntry
        {
            public string Message { get; set; }
            public string Category { get; set; }
            public TraceEventType Level { get; set; }
        }
    }
}
