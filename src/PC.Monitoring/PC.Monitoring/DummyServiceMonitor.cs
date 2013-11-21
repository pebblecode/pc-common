using Bede.Logging.Models;

namespace PebbleCode.Monitoring
{
    /// <summary>
    /// A fake service monitor that represents the currently running process.
    /// </summary>
    public class DummyServiceMonitor : ServiceMonitor
    {
        public DummyServiceMonitor(string name)
            : base(name, @"localhost", new NullLogger())
        {
        }

        /// <summary>
        /// Always returns a successful status.
        /// </summary>
        public override MonitoredServiceStatus GetStatus()
        {
            return MonitoredServiceStatus.CreateStatus(ServiceName);
        }
    }
}
