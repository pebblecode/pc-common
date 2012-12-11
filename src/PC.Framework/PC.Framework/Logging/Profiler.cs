using PebbleCode.Framework.Configuration;
using PebbleCode.Framework.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Timers;

namespace PC.Framework.Logging
{
    /// <summary>
    /// Static class providing public interface to counters
    /// </summary>
    public static class Profiler
    {
        private static Dictionary<string, Dictionary<string, EventProfile>> _eventProfiles = new Dictionary<string, Dictionary<string, EventProfile>>();
        private static System.Threading.Timer _profileLogTimer = null;
        private static int _openConnectionCount = 0;
        private static int _peakOpenConnectionCount = 0;
        private static int _clientConnectionCount = 0;
        private static int _peakClientConnectionCount = 0;
        private static int _logPeriod;
        private static DateTime _startDateTime = DateTime.Now;
        private static bool _statsClearedAfterStartUp = false;
        private static int _sysProfileLinesWithoutHeader = 0;

        /// <summary>
        /// Update the open connection count
        /// </summary>
        /// <param name="openConnectionCount"></param>
        public static void UpdateOpenConnectionCount(int openConnectionCount)
        {
            _openConnectionCount = openConnectionCount;
            if (_openConnectionCount > _peakOpenConnectionCount)
                _peakOpenConnectionCount = _openConnectionCount;
        }

        /// <summary>
        /// Update the client connection count
        /// </summary>
        /// <param name="clientConnectionCount"></param>
        public static void UpdateClientConnectionCount(int clientConnectionCount)
        {
            _clientConnectionCount = clientConnectionCount;
            if (_clientConnectionCount > _peakClientConnectionCount)
                _peakClientConnectionCount = _clientConnectionCount;
        }

        /// <summary>
        /// Clear all existing profile information
        /// </summary>
        public static void ResetEventProfiles()
        {
            foreach (string category in _eventProfiles.Keys)
                ResetEventProfiles(category);
        }

        /// <summary>
        /// Clear existing profile information for a category
        /// </summary>
        public static void ResetEventProfiles(string category)
        {
            LogProfileStats();
            Logger.WriteInfo(string.Format("Clearing {0} event profiles", category), category);
            if (_eventProfiles.ContainsKey(category))
                _eventProfiles[category].Clear();
        }

        /// <summary>
        /// Force the profiler to log to the logger
        /// </summary>
        public static void ForceStatDump()
        {
            LogProfileStats();
        }

        /// <summary>
        /// Get the event profiles for a given category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public static List<EventProfile> GetEventProfiles(string category)
        {
            return new List<EventProfile>(_eventProfiles[category].Values);
        }

        /// <summary>
        /// Static initialiser creates a timer to periodically write 
        /// execution profile to the profile log
        /// </summary>
        static Profiler()
        {
            if (Logger.ShouldLogDebug(Category.DbProfiler) ||
                Logger.ShouldLogDebug(Category.SysProfiler) ||
                Logger.ShouldLogDebug(Category.MsgProfiler))
            {
                // Load the period at which to dump acumulated profile state
                _logPeriod = ProfilerSettings.ProfileLogPeriodMilliseconds;

                //create and start the timer.
                _profileLogTimer = new System.Threading.Timer(
                    new TimerCallback(ProfilerTimeoutCallback),
                    null,
                    _logPeriod,
                    Timeout.Infinite);
            }
        }

        /// <summary>
        /// Import WinAPI method to get accurate ticks reading
        /// </summary>
        /// <param name="ticks"></param>
        [DllImport("Kernel32.dll")]
        private static extern void QueryPerformanceCounter(ref long ticks);

        /// <summary>
        /// Get an accurate measure of current number of ticks
        /// </summary>
        /// <returns></returns>
        public static long AccurateTicksNow()
        {
            //long ticks = 0;
            //QueryPerformanceCounter(ref ticks);
            //return ticks;
            return DateTime.Now.Ticks;
        }

        /// <summary>
        /// Mark the end of an occurence of a counter event, and indicate the start ticks
        /// </summary>
        /// <param name="counter"></param>
        public static void Write(string eventName, long startTicks, string category)
        {
            if (!Logger.ShouldLogDebug(category))
                return;

            long ticksTaken = AccurateTicksNow() - startTicks;
            TimeSpan timeTaken = TimeSpan.FromTicks(ticksTaken);
            //TimeSpan timeTaken = TimeSpan.FromMilliseconds(ticksTaken);
            ProfileEvent(eventName, timeTaken.TotalMilliseconds, category);
        }

        /// <summary>
        /// Records an execution time against the averages and counts for this event.
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="millis"></param>
        private static void ProfileEvent(string eventName, double millis, string category)
        {
            // Log it
            Logger.WriteDebug(string.Format("{0} {1}", eventName, millis), category);

            // Update the profile
            try
            {
                // Add the category if needed
                if (!_eventProfiles.ContainsKey(category))
                    _eventProfiles.Add(category, new Dictionary<string, EventProfile>());

                // Add the event if needed
                if (!_eventProfiles[category].ContainsKey(eventName))
                    _eventProfiles[category].Add(eventName, new EventProfile(eventName, category));

                // Log this occurence
                _eventProfiles[category][eventName].ProfileEvent(millis);
            }
            catch (Exception ex)
            {
                Logger.WriteWarning("Failed to update profile summary: " + ex.Message, category);
            }
        }

        private static bool _systemInformationLogged = false;

        /// <summary>
        /// Writes a summary log message containing total and average executions for
        /// all events
        /// </summary>
        private static void ProfilerTimeoutCallback(object state)
        {
            // Log sys info on first pass
            try
            {
                if (!_systemInformationLogged)
                {
                    _systemInformationLogged = true;
                    LogSystemInformation();
                }
            }
            catch (Exception e)
            {
                Logger.WriteUnexpectedException(e, "Error logging system information", Category.ServerError);
            }

            // Log the new stats
            try
            {
                LogProfileStats();
            }
            catch (Exception e)
            {
                Logger.WriteUnexpectedException(e, "Error logging profile stats", Category.ServerError);
            }

            // Clear stats before dumping?
            if (!_statsClearedAfterStartUp &&
                ProfilerSettings.SecondsBeforeClearingProfileStats > 0 &&
                (DateTime.Now - _startDateTime).TotalSeconds > ProfilerSettings.SecondsBeforeClearingProfileStats)
            {
                ResetEventProfiles();
                _statsClearedAfterStartUp = true;
            }

            // Reset the timer to callback again soon
            _profileLogTimer.Change(_logPeriod, Timeout.Infinite);
        }

        /// <summary>
        /// Log interesting system information at start up
        /// </summary>
        private static void LogSystemInformation()
        {
            Logger.WriteDebug(string.Format("OS Version: {0}", System.Environment.OSVersion.ToString()), Category.SysProfiler);
            Logger.WriteDebug(string.Format("CLR Version: {0}", System.Environment.Version.ToString()), Category.SysProfiler);
            Logger.WriteDebug(string.Format("Processors: {0}", System.Environment.ProcessorCount), Category.SysProfiler);
        }

        /// <summary>
        /// Write the current stats out to log file
        /// </summary>
        private static void LogProfileStats()
        {
            // Write out all the event profiles (E.g. DbProfiles and MessageProfiles)
            try
            {
                foreach (string category in _eventProfiles.Keys)
                {
                    //Write a header
                    Logger.WriteInfo(
                        string.Format("{0} MAvr    Avr     Count   Min     Max     Total",
                        new string(' ', EventProfile.EVENT_NAME_CHAR_LENGTH)), category);

                    ArrayList al = new ArrayList(_eventProfiles[category].Keys);
                    al.Sort();
                    double totalMillis = 0;
                    int count = 0;
                    foreach (string eventName in al)
                    {
                        Logger.WriteInfo(_eventProfiles[category][eventName].ToString(), category);
                        count += _eventProfiles[category][eventName].Count;
                        totalMillis += _eventProfiles[category][eventName].TotalMillis;
                    }

                    Logger.WriteInfo(
                        string.Format("TOTAL Average:{0:0.00}   Count:{1}   total:{2:0}   Clients:{3}",
                            totalMillis / count,
                            count,
                            totalMillis,
                            _clientConnectionCount),
                        category);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteUnexpectedException(ex, "Failed to write profile summaries", Category.ServerError);
            }

            // Special hack to write out the number of db connections currently open.
            Logger.WriteDebug(
                string.Format("Connections. Open:{0}  Peak:{1}", _openConnectionCount, _peakOpenConnectionCount),
                Category.DbProfiler);

            // Now write the system profile
            if (Logger.ShouldLogDebug(Category.SysProfiler))
            {
                try
                {
                    // Write a header every 10th visit here
                    if (_sysProfileLinesWithoutHeader % 10 == 0)
                        Logger.WriteDebug("Clients  Handles  Threads  Used/Max  Used/Max(IO)  WorkingSet  Paged", Category.SysProfiler);
                    _sysProfileLinesWithoutHeader++;

                    // Get thread pool stats
                    int workerThreads;
                    int completionPortThreads;
                    int maxWorkerThreads;
                    int maxCompletionPortThreads;
                    ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);
                    ThreadPool.GetMaxThreads(out maxWorkerThreads, out maxCompletionPortThreads);

                    // Get process stats
                    Process process = Process.GetCurrentProcess();

                    // Log all stats
                    Logger.WriteDebug(
                        string.Format("{6:0000}     {0:0000}     {1:000}      {2:000}/{3:000}   {4:000}/{5:000}       {6:000.00}      {7:000.00}",
                            process.HandleCount,
                            process.Threads.Count,
                            maxWorkerThreads - workerThreads,
                            maxWorkerThreads,
                            maxCompletionPortThreads - completionPortThreads,
                            maxCompletionPortThreads,
                            process.WorkingSet64 / (double)(1024 * 1024),
                            process.PagedMemorySize64 / (double)(1024 * 1024),
                            _clientConnectionCount),
                        Category.SysProfiler);
                }
                catch (Exception ex)
                {
                    Logger.WriteUnexpectedException(ex, "Failed to write process summary", Category.ServerError);
                }
            }
        }
    }

    /// <summary>
    /// Summary class to store information regarding number of calls and average duration for a particular event name
    /// </summary>
    public class EventProfile : IComparable
    {
        public const int EVENT_NAME_CHAR_LENGTH = 32;
        private const int MOVING_AVERAGE_EVENT_COUNT = 500;

        private string _category;
        private int _count;
        private double _totalMillis;
        private double _minMillis;
        private double _maxMillis;
        private string _eventName = null;
        private Queue<double> _eventTimes = new Queue<double>(MOVING_AVERAGE_EVENT_COUNT);

        /// <summary>
        /// Construct with an event key
        /// </summary>
        /// <param name="eventName"></param>
        public EventProfile(string eventName, string category)
        {
            _eventName = eventName;
            _category = category;
        }

        /// <summary>
        /// Get the name of this event
        /// </summary>
        public string EventName
        {
            get { return _eventName; }
        }

        /// <summary>
        /// Get number of occurences
        /// </summary>
        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// Get average millis for the event
        /// </summary>
        public double AverageMillis
        {
            get { return _totalMillis / _count; }
        }

        /// <summary>
        /// Get total millis for the event
        /// </summary>
        public double TotalMillis
        {
            get { return _totalMillis; }
        }

        /// <summary>
        /// Get the category
        /// </summary>
        public string Category
        {
            get { return _category; }
        }

        /// <summary>
        /// Get the moving average of event times
        /// </summary>
        public double MovingAverage
        {
            get
            {
                try
                {
                    return _eventTimes.Sum() / _eventTimes.Count;
                }
                catch
                {
                    // Ignore errors. Could be threading issues around this.
                    // But don't want to take the hit to make it thread safe
                    return 0;
                }
            }
        }


        /// <summary>
        /// Get the number of items used in the moving average of event times
        /// </summary>
        public double MovingAverageCount
        {
            get { return _eventTimes.Count; }
        }

        /// <summary>
        /// Appends this event time to the average and counts
        /// </summary>
        /// <param name="ticks"></param>
        public void ProfileEvent(double milliseconds)
        {
            if (_count == 0)
            {
                _minMillis = milliseconds;
                _maxMillis = milliseconds;
                _totalMillis = milliseconds;
            }
            else
            {
                _minMillis = Math.Min(_minMillis, milliseconds);
                _maxMillis = Math.Max(_maxMillis, milliseconds);
                _totalMillis += milliseconds;
            }

            _count++;

            // Add to moving average
            try
            {
                _eventTimes.Enqueue(milliseconds);
                while (_eventTimes.Count > MOVING_AVERAGE_EVENT_COUNT)
                    _eventTimes.Dequeue();
            }
            catch
            {
                // Ignore errors. Could be threading issues around this.
                // But don't want to take the hit to make it thread safe
            }
        }

        public override int GetHashCode()
        {
            return _eventName.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this._eventName.Equals(((EventProfile)obj)._eventName);
        }

        public override string ToString()
        {
            return string.Format("{0} {6:00000}   {1:00000}   {2:00000}   {4:00000}   {5:00000}   {3:0000000}",
                _eventName.PadRight(EventProfile.EVENT_NAME_CHAR_LENGTH, ' ').Substring(0, EventProfile.EVENT_NAME_CHAR_LENGTH),
                AverageMillis,
                _count,
                _totalMillis,
                _minMillis,
                _maxMillis,
                MovingAverage);
        }

        public int CompareTo(object obj)
        {
            return this._eventName.CompareTo(((EventProfile)obj)._eventName);
        }
    }
}
