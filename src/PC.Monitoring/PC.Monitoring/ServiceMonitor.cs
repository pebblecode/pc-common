using System;
using System.Text.RegularExpressions;

namespace PC.Monitoring
{
    /// <summary>
    /// Base class for the synchronous version of service monitors
    /// </summary>
    public abstract class ServiceMonitor
    {
        private readonly string _name;
        private readonly string _host;

        protected ServiceMonitor(string name, string host)
        {
            _name = name;
            _host = host;
        }

        public string ServiceName { get { return string.Format("[{0}] {1}", _name, GetMachineName()); } }

        public abstract MonitoredServiceStatus GetStatus();

        public static ServiceMonitor Create(ServiceMonitorConfiguration config)
        {
            switch (config.Type)
            {
                case "DummyServiceMonitor":
                    return new DummyServiceMonitor(config.Name);
                case "TcpPortServiceMonitor":
                    return new TcpPortMonitor(config);
                case "DatabaseServiceMonitor":
                    return new DatabaseServiceMonitor(config);
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
