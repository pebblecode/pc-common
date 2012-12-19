using System.Diagnostics;

namespace PebbleCode.Framework.Logging
{
    public interface ILogManager
    {
        bool ShouldLog(string category, TraceEventType level);
        void WriteLog(string message, string category, TraceEventType level);
    }
}
