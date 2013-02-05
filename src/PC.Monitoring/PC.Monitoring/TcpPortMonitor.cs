using System;
using System.Net.Sockets;

namespace PebbleCode.Monitoring
{
    /// <summary>
    /// Class for moinotring whether TCP ports are active and accepting connections
    /// </summary>
    public class TcpPortMonitor : AsyncServiceMonitor
    {
        private readonly string _ipAddress;
        private readonly int _port;

        public TcpPortMonitor(ServiceMonitorConfiguration config)
            : base(config.Name, config.Settings["ipAddress"].Value, Int32.Parse(config.Settings["timeout"].Value))
        {
            _ipAddress = config.Settings["ipAddress"].Value;
            _port = Int32.Parse(config.Settings["port"].Value); ;
        }

        protected override IAsyncResult BeginServiceCheck()
        {
            var client = new TcpClient();
            return client.BeginConnect(_ipAddress, _port, null, client);
        }

        protected override void EndServiceCheck(IAsyncResult result)
        {
            var client = (TcpClient)result.AsyncState;
            client.EndConnect(result);
            client.Close();
        }

        protected override void CleanupServiceCheck(IAsyncResult result)
        {
            var client = (TcpClient)result.AsyncState;
            client.Close();
        }

        protected override string ServiceMonitorTypeName
        {
            get { return "TcpPortMonitor"; }
        }
    }
}
