using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;

namespace PebbleCode.Framework.Utilities
{
    /// <summary>
    /// A utility class for synchronising asynchrouns calls.  Essentially a stateful wrapper 
    /// for a manual reset wait handle, allowing exceptions and result data to be returned to the waiting thread
    /// </summary>
    public class AsyncState : IDisposable
    {
        private const string THROW_MESSAGE = "Timed out waiting for {0} to complete.  Timeout={1}";
        private ManualResetEvent _handle = new ManualResetEvent(false);
        private TimeSpan? _defaultWaitTimesout = null;
        private bool _throwExceptions = true;
        private string _operationName = null;

        /// <summary>
        /// Default costructor
        /// </summary>
        public AsyncState(bool throwExceptions = false, string operationName = null)
        {
            _throwExceptions = throwExceptions;
            _operationName = operationName;
        }

        /// <summary>
        /// Creates this wait object ready to wait for the specified period
        /// </summary>
        /// <param name="waitTimesout"></param>
        /// <param name="throwExceptions">Whether to bubble exceptions from the async thread</param>
        public AsyncState(TimeSpan waitTimesout, bool throwExceptions = false, string operationName = null)
            : this(throwExceptions, operationName)
        {
            _defaultWaitTimesout = waitTimesout;
        }
        
        /// <summary>
        /// Arbitrary result object - can be used to communicate results to caller
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// Optional Exception object - if set will be throw in the invoker's thread
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Signals the wait handle
        /// </summary>
        public void Set() { _handle.Set(); }

        /// <summary>
        /// Resets the handle, allowing this object to be reused
        /// </summary>
        public void Reset() { _handle.Reset(); }

        [DebuggerStepThrough]
        public bool Wait()
        {
            bool signalled = _handle.WaitOne();
            ThrowExceptionIfSet();
            Debug.Assert(signalled);
            return signalled;
        }

        /// <summary>
        /// Wait for the operation to complete, until the specified timeout
        /// </summary>
        /// <param name="timeout">Length of time to wait for signal</param>
        /// <returns>True if a signal was received, false if timed out</returns>
        [DebuggerStepThrough]
        public bool Wait(TimeSpan timeout)
        { 
            bool signalled = _handle.WaitOne(timeout);
            ThrowExceptionIfSet();
            return signalled;
        }

        /// <summary>
        /// Wait for signal
        /// </summary>
        /// <param name="timeout">Length of time to wait for signal</param>
        /// <param name="operationName">Name of operation being executed</param>
        /// <returns>True if signal received, otherwise exception thrown</returns>
        [DebuggerStepThrough]
        public bool WaitAndThrow(TimeSpan timeout, string operationName)
        {
            bool signalled = _handle.WaitOne(timeout);
            if (!signalled)
                throw new Exception(string.Format(THROW_MESSAGE, operationName, timeout));
            ThrowExceptionIfSet();
            return signalled;
        }

        /// <summary>
        /// Throws the exception property if it is set.
        /// </summary>
        [DebuggerStepThrough]
        private void ThrowExceptionIfSet()
        {
            if (Exception != null)
                throw Exception;
        }

        /// <summary>
        /// Waits on the handle
        /// </summary>
        public void Dispose()
        {
            if (_defaultWaitTimesout.HasValue)
            {
                if (_throwExceptions)
                {
                    WaitAndThrow(_defaultWaitTimesout.Value, _operationName);
                }
                else
                {
                    Wait(_defaultWaitTimesout.Value);
                }
            }
            else
            {
                Wait();
            }
        }
    }
}
