using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using System.IO;
using PebbleCode.Framework.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
namespace PC.Framework.Logging
{
    /// <summary>
    /// Decorates a RollingFlatFileTraceListener, rolling the file at a scheduled time
    /// </summary>
    [ConfigurationElementType(typeof(CustomTraceListenerData))]
    public class ScheduledTimeRollingFlatFileListener : CustomTraceListener, IDisposable
    {
        private ScheduledRollTrigger _rollTrigger;

        /// <summary>
        /// Decorated member - the listener which will perform actual writes
        /// </summary>
        private RollingFlatFileTraceListener _rollingListener;
        private bool _listenerInitialised;
        private readonly object[] _syncObject = new object[0];

        public string FileName
        {
            get
            {
                return Attributes["fileName"];
            }
        }

        public string TimeStampPattern
        {
            get
            {
                return Attributes["timeStampPattern"];
            }
        }

        public string RollTime
        {
            get
            {
                return Attributes["rollTime"];
            }
        }

        /// <summary>
        /// Event log level
        /// </summary>
        public TraceEventType LogLevel
        {
            get
            {
                if (!_logLevel.HasValue)
                    _logLevel = (TraceEventType)Enum.Parse(typeof(TraceEventType), Attributes["logLevel"]);
                return _logLevel.Value;
            }
        }

        private TraceEventType? _logLevel;

        /// <summary>
        /// Performs initialisation of underlying rolling flat file listener, and
        /// sets up automated roll trigger
        /// </summary>
        private void Initialise()
        {
            lock (_syncObject)
            {
                if (_listenerInitialised) return;
                _listenerInitialised = true;
            }

            _rollingListener = new RollingFlatFileTraceListener(this.FileName,
                string.Empty,
                string.Empty,
                this.Formatter,
                rollSizeKB: 0,
                timeStampPattern: this.TimeStampPattern,
                rollFileExistsBehavior: RollFileExistsBehavior.Increment,
                rollInterval: RollInterval.None);

            //Auto roll the log on a timer
            _rollTrigger = new ScheduledRollTrigger(this, TimeSpan.Parse(this.RollTime));
            this.WriteLine("Next log file roll time: " + _rollTrigger.NextRunTime, Category.General);
        }

        /// <summary>
        /// Rolls the log file
        /// </summary>
        public void Roll()
        {
            this.WriteLine("Rolling log file", Category.General);
            DateTime currentTime = DateTime.Now;
            string logFileName = ((FileStream)((StreamWriter)_rollingListener.Writer).BaseStream).Name;
            string archiveFilePath =
                Path.Combine(new FileInfo(FileName).Directory.FullName, _rollingListener.RollingHelper.ComputeArchiveFileName(logFileName, currentTime));

            _rollingListener.RollingHelper.PerformRoll(currentTime);

            OnFileRolled(archiveFilePath);
        }

        protected virtual void OnFileRolled(string filePath)
        {
        }

        public override void Fail(string message)
        {
            Initialise();
            _rollingListener.Fail(message);
            _rollingListener.Flush();
        }

        public override void Fail(string message, string detailMessage)
        {
            Initialise();
            _rollingListener.Fail(message, detailMessage);
            _rollingListener.Flush();
        }

        private bool ShouldLog(TraceEventCache cache, string source, TraceEventType eventType, int id, string formatOrMessage, object[] args, object data1, object[] data)
        {
            return this.Filter == null || this.Filter.ShouldTrace(cache, source, eventType, id, null, null, data, null);
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (!ShouldLog(eventCache, source, eventType, id, null, null, data, null)) return;
            Initialise();
            _rollingListener.TraceData(eventCache, source, eventType, id, data);
            _rollingListener.Flush();
        }

        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, params object[] data)
        {
            if (!ShouldLog(eventCache, source, eventType, id, null, null, data, null)) return;
            Initialise();
            _rollingListener.TraceData(eventCache, source, eventType, id, data);
            _rollingListener.Flush();
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id)
        {
            if (!ShouldLog(eventCache, source, eventType, id, null, null, null, null)) return;
            Initialise();
            _rollingListener.TraceEvent(eventCache, source, eventType, id);
            _rollingListener.Flush();
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string format, params object[] args)
        {
            if (!ShouldLog(eventCache, source, eventType, id, null, null, format, null)) return;
            Initialise();
            _rollingListener.TraceEvent(eventCache, source, eventType, id, format, args);
            _rollingListener.Flush();
        }

        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            if (!ShouldLog(eventCache, source, eventType, id, null, null, message, null)) return;
            Initialise();
            _rollingListener.TraceEvent(eventCache, source, eventType, id, message);
            _rollingListener.Flush();
        }

        public override void TraceTransfer(TraceEventCache eventCache, string source, int id, string message, Guid relatedActivityId)
        {
            Initialise();
            _rollingListener.TraceTransfer(eventCache, source, id, message, relatedActivityId);
            _rollingListener.Flush();
        }

        public override void Write(string message)
        {
            Initialise();
            _rollingListener.Write(message);
            _rollingListener.Flush();
        }

        public override void WriteLine(string message)
        {
            Initialise();
            _rollingListener.WriteLine(message);
            _rollingListener.Flush();
        }

        public override void Write(object o)
        {
            Initialise();
            _rollingListener.Write(o);
            _rollingListener.Flush();
        }

        public override void Write(object o, string category)
        {
            Initialise();
            _rollingListener.Write(o, category);
            _rollingListener.Flush();
        }

        public override void Write(string message, string category)
        {
            Initialise();
            _rollingListener.Write(message, category);
            _rollingListener.Flush();
        }

        public override void WriteLine(object o)
        {
            Initialise();
            _rollingListener.WriteLine(o);
            _rollingListener.Flush();
        }

        public override void WriteLine(object o, string category)
        {
            Initialise();
            _rollingListener.WriteLine(o, category);
            _rollingListener.Flush();
        }

        public override void WriteLine(string message, string category)
        {
            Initialise();
            _rollingListener.WriteLine(message, category);
            _rollingListener.Flush();
        }


        void IDisposable.Dispose()
        {
            if (_rollingListener != null)
            {
                _rollingListener.Flush();
                _rollingListener.Dispose();
                if (_rollTrigger != null)
                {
                    _rollTrigger.Dispose();
                }
            }
        }
    }
}
