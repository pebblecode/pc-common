using Bede.Logging.Models;
using System;
using System.Threading;

namespace PebbleCode.Framework.Threading
{
    /// <summary>
    /// Base class for classes requiring repeated callbacks
    /// </summary>
    public abstract class RepeatingTask : SimpleTask, IDisposable
    {
        private readonly TimeSpan _callbackInterval;
        private readonly ILoggingService _loggingService;

        private Timer _timer;
        private readonly object[] _timerLock = new object[0];
        public DateTime NextRunTime { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        public RepeatingTask(TimeSpan callbackInterval, ILoggingService loggingService)
        {
            _callbackInterval = callbackInterval;
            _loggingService = loggingService;
        }

        /// <summary>
        /// Start the updater. Will run every X seconds, starting with a call now.
        /// </summary>
        public virtual void Start()
        {
            Start(0);
        }

        /// <summary>
        /// Starts the task, but delays the first run until the indicated time
        /// </summary>
        /// <param name="nextRunTime"></param>
        public void Start(DateTime nextRunTime)
        {
            if (nextRunTime < DateTime.Now)
            {
                nextRunTime = nextRunTime.AddDays(1);
            }
            TimeSpan diff = nextRunTime - DateTime.Now;
            Start((long)diff.TotalMilliseconds);
        }

        /// <summary>
        /// Starts the task with a delay
        /// </summary>
        /// <param name="nextRunTime"></param>
        public void Start(TimeSpan nextRunTime)
        {
            Start(DateTime.Now.Date.Add(nextRunTime));
        }

        /// <summary>
        /// Start the updater. Will run every X seconds
        /// </summary>
        /// <param name="delayMillis">Number of milliseconds to delay before repeating for the first time</param>
        public void Start(long delayMillis)
        {
            if (delayMillis < 0)
                throw new ArgumentOutOfRangeException("delayMillis", "Must be non-negative");

            lock (_timerLock)
            {
                if (_timer != null)
                    throw new InvalidOperationException("Only call start once!");

                // Create a timer with an immediate callback
                _timer = new Timer(OnTimerExpired, null, delayMillis, Timeout.Infinite);
            }

            //Cache the next system clock run time
            NextRunTime = DateTime.Now.AddMilliseconds(delayMillis);

            OnStarted();
        }

        /// <summary>
        /// Stop the timer doing the updates
        /// </summary>
        public void Stop()
        {
            lock (_timerLock)
            {
                if (_timer != null)
                {
                    _timer.Change(Timeout.Infinite, Timeout.Infinite);
                    _timer.Dispose();
                    _timer = null;
                }
            }
            OnStopped();
        }

        /// <summary>
        /// When timer expires, call derived class to do work
        /// </summary>
        /// <param name="state"></param>
        private void OnTimerExpired(object state)
        {
            // Call derived class
            try
            {
                PerformTask();
            }
            catch(Exception ex)
            {
                _loggingService.Error(new CommonLoggingData("Error in RepeatingTask.PerformTask", "General"), ex);
            }

            // Reset the timer
            lock (_timerLock)
            {
                if (_timer != null)
                {
                    _timer.Change(_callbackInterval, TimeSpan.FromMilliseconds(-1));
                }
            }
        }

        /// <summary>
        /// Virtual method to allow derived classes to perform work after the timer is stopped.
        /// </summary>
        protected virtual void OnStopped()
        {
        }

        /// <summary>
        /// Virtual method to allow derived classes to perform work after the timer is started
        /// </summary>
        protected virtual void OnStarted()
        {
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
