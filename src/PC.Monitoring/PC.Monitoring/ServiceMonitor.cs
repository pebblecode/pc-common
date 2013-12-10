using Bede.Logging.Models;
using System;
using System.Text.RegularExpressions;
using Bede.Logging.Providers;
using log4net;

namespace PebbleCode.Monitoring
{
    /// <summary>
    /// Base class for the synchronous version of service monitors
    /// </summary>
    /// <param name="loggingService">The service that will do the logging. If null or not passed - a new default instance will be created</param>
    public abstract class ServiceMonitor
    {
        private readonly string _name;
        private readonly string _host;
        
        protected ILoggingService LoggingService { get; set; }

        protected ServiceMonitor(string name, string host, ILoggingService loggingService = null)
        {
            _name = name;
            _host = host;
            LoggingService = loggingService ?? new Log4NetLoggingService(LogManager.GetLogger("MonitoringLogger"));
        }

        public string ServiceName { get { return string.Format("[{0}] {1}", _name, GetMachineName()); } }

        public abstract MonitoredServiceStatus GetStatus();

        public static ServiceMonitor Create(ServiceMonitorConfiguration config, ILoggingService loggingService = null)
        {
            switch (config.Type)
            {
                case "DummyServiceMonitor":
                    return new DummyServiceMonitor(config.Name);
                case "TcpPortServiceMonitor":
                    return new TcpPortMonitor(config, loggingService);
                case "DatabaseServiceMonitor":
                    return new DatabaseServiceMonitor(config, loggingService);
                default:
                    throw new NotImplementedException("Unknown service monitor type " + config.Type);
            }
        }

        public string GetMachineName()
        {
            var localhostRegex = new Regex(@"^(127\.[\d.]+|[0:]+1|localhost)$");
            if (localhostRegex.Match(_host).Success)
                return Environment.MachineName;

            return _host;
        }
    }
}
