﻿using System.Collections.Generic;

namespace PC.Monitoring
{
    /// <summary>
    /// This class is responsible for getting the response back from all registered service monitors
    /// </summary>
    public class HealthMonitor
    {
        private readonly List<ServiceMonitor> _monitors = new List<ServiceMonitor>();

        public void AddMonitoredServices(string groupName)
        {
            foreach (ServiceMonitorConfiguration config in GetServiceMonitorConfigs(groupName))
            {
                _monitors.Add(ServiceMonitor.Create(config));
            }
        }

        private ServiceMonitors GetServiceMonitorConfigs(string groupName)
        {
            return MonitoringConfiguration.Instance.serviceMonitorGroups[groupName].ServiceMonitors;
        }

        public MonitoredServiceGroupStatus GetStatus()
        {
            var overallServiceStatus = new MonitoredServiceGroupStatus();
            foreach (ServiceMonitor monitor in _monitors)
            {
                overallServiceStatus.Add(monitor.GetStatus());
            }
            return overallServiceStatus;
        }

        public GetHealthStatusResponse GetHealthStatusResponse()
        {
            return GetStatus().ToHealthStatusResponse();
        }
    }
}
