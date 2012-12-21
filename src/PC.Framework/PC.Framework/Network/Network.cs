using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace PebbleCode.Framework.Network
{
    public static class Helper
    {
        /// <summary>
        /// Get my ip address
        /// </summary>
        /// <returns></returns>
        public static string GetIpAddress()
        {
            string hostName = Dns.GetHostName();
            if (string.IsNullOrEmpty(hostName))
                throw new InvalidOperationException("No host name. Cannot determine node");

            // Get my IP address list
            IPHostEntry ipEntry = Dns.GetHostEntry(hostName);
            if (ipEntry == null)
                throw new InvalidOperationException(string.Format("No host entry for '{0}'. Cannot determine node", hostName));

            // Get my IP address
            IPAddress ipAddress = ipEntry.AddressList.First<IPAddress>((ip) => ip.AddressFamily == AddressFamily.InterNetwork);
            if (ipAddress == null)
                throw new InvalidOperationException(string.Format("No ipv4 address for '{0}'. Cannot determine node", hostName));
            return ipAddress.ToString();
        }
    }
}
