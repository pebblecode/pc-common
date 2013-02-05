using System.Collections.Generic;
using System.Linq;

namespace PC.Monitoring
{
    /// <summary>
    /// Contains all the statuses of services that are being monitored
    /// </summary>
    public class MonitoredServiceGroupStatus
    {
        private readonly List<MonitoredServiceStatus> _statuses = new List<MonitoredServiceStatus>();

        public MonitoredServiceStatusCode Status { get; private set; }

        public void Add(MonitoredServiceStatus status)
        {
            _statuses.Add(status);

            if (!status.IsOK)
            {
                Status = MonitoredServiceStatusCode.Error;
            }
        }

        public GetHealthStatusResponse ToHealthStatusResponse()
        {
            return new GetHealthStatusResponse
            {
                ServerStatus = Status.ToString(),
                Services = GetMonitoredServiceStatuses()
            };
        }

        private MonitoredService[] GetMonitoredServiceStatuses()
        {
            return _statuses.Select(status => status.ToMonitoredService()).ToArray();
        }
    }
}
