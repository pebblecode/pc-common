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
    /// Custom trace listener to output to console
    /// </summary>
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class ConsoleTraceListener : DebugTraceListener
    {
        /// <summary>
        /// Write to debug
        /// </summary>
        /// <param name="message"></param>
        public override void Write(string message)
        {
            Console.Write(message);
        }

        /// <summary>
        /// Write to debug
        /// </summary>
        /// <param name="message"></param>
        public override void WriteLine(string message)
        {
            Console.WriteLine(message);
        }
    }
}
