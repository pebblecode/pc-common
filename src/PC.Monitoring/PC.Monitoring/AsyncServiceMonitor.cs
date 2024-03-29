﻿using System;
using System.Threading;
using Bede.Logging.Models;

namespace PebbleCode.Monitoring
{
    /// <summary>
    /// Async base class for service monitoring providing methods derived classes need to implement to have an asynchronous monitor
    /// </summary>
    public abstract class AsyncServiceMonitor : ServiceMonitor
    {
        private readonly int _timeout;

        protected AsyncServiceMonitor(string name, string host, int timeout, ILoggingService loggingService = null)
            : base(name, host, loggingService)
        {
            this._timeout = timeout;
        }

        protected virtual string ServiceMonitorTypeName
        {
            get { return String.Empty; }
        }

        public override MonitoredServiceStatus GetStatus()
        {
            return IsServiceAvailable() ? MonitoredServiceStatus.CreateStatus(ServiceName) : MonitoredServiceStatus.CreateErrorStatus(ServiceName);
        }

        private bool IsServiceAvailable()
        {
            WaitHandle handle = null;
            try
            {
                IAsyncResult result = BeginServiceCheck();
                handle = result.AsyncWaitHandle;
                if (handle.WaitOne(TimeSpan.FromSeconds(_timeout), false))
                {
                    EndServiceCheck(result);
                    return true;
                }

                CleanupServiceCheck(result);
                return false;
            }
            catch (Exception ex)
            {
                LoggingService.Error(new CommonFormattedLoggingData("AsyncServiceMonitor", null, "{0} error: {1}", ServiceMonitorTypeName, ex));
                return false;
            }
            finally
            {
                if (handle != null)
                    handle.Close();
            }
        }

        protected abstract IAsyncResult BeginServiceCheck();

        protected abstract void EndServiceCheck(IAsyncResult result);

        protected virtual void CleanupServiceCheck(IAsyncResult result)
        {
        }
    }
}
