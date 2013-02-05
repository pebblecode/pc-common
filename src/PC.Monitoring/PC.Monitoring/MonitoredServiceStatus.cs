namespace PebbleCode.Monitoring
{
    public enum MonitoredServiceStatusCode
    {
        OK = 0,
        Error = 1
    };

    /// <summary>
    /// Wrapper around the status of a service being monitored
    /// </summary>
    public class MonitoredServiceStatus
    {
        public MonitoredServiceStatus(string serviceName)
            : this(serviceName, MonitoredServiceStatusCode.OK)
        {
        }

        public MonitoredServiceStatus(string serviceName, MonitoredServiceStatusCode monitoredServiceStatusCode)
        {
            this.ServiceName = serviceName;
            this.Status = monitoredServiceStatusCode;
        }

        public string ServiceName { get; set; }
        public MonitoredServiceStatusCode Status { get; set; }

        public bool IsOK
        {
            get { return Status == MonitoredServiceStatusCode.OK; }
        }

        public static MonitoredServiceStatus CreateStatus(string serviceName)
        {
            return new MonitoredServiceStatus(serviceName);
        }

        public static MonitoredServiceStatus CreateErrorStatus(string serviceName)
        {
            return new MonitoredServiceStatus(serviceName, MonitoredServiceStatusCode.Error);
        }

        public MonitoredService ToMonitoredService()
        {
            return new MonitoredService
            {
                Status = Status.ToString(),
                Name = ServiceName
            };
        }
    }
}
