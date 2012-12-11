using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace PebbleCode.Framework.Logging
{
    public class FakeLogManagerLogWrittenException : Exception
    {
        public FakeLogManagerLogWrittenException()
        {   
        }

        public FakeLogManagerLogWrittenException(string message) : base(message)
        {
        }

        public FakeLogManagerLogWrittenException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public FakeLogManagerLogWrittenException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {   
        }

        public FakeLogManagerLogWrittenException(string message, string category, TraceEventType level) : 
            base(FormatMessage(message, category, level))
        {
        }

        private static string FormatMessage(string message, string category, TraceEventType level)
        {
            return String.Format("Attempt to write log.\n\rMessage: {0}\r\nCategory: {1}\r\nLevel:{2}", message, category, level);
        }
    }
}