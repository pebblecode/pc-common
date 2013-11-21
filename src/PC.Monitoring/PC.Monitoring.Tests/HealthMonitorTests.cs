using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bede.Logging.Models;
using NUnit.Framework;

using PebbleCode.Monitoring;

namespace PC.Monitoring.Tests
{
    [TestFixture]
    public class HealthMonitorTests
    {
        [Test]
        public void GivenValidConfiguration_WhenGetStatusCalled_ThenStatusOkReturned()
        {
            HealthMonitor monitor = new HealthMonitor();
            monitor.AddMonitoredServices("FB.BingoService", new NullLogger());
            var result = monitor.GetStatus();
            Assert.That(result.Status == MonitoredServiceStatusCode.OK);
        }
    }
}
