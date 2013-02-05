using System.Xml.Serialization;

namespace PebbleCode.Monitoring
{
    /// <summary>
    /// Response back from a call to get a healthcheck of running services
    /// </summary>
    [XmlRootAttribute("Services")]
    public class GetHealthStatusResponse
    {
        [XmlAttribute("serverStatus")]
        public string ServerStatus { get; set; }

        [XmlElement("Service")]
        public MonitoredService[] Services { get; set; }
    }
}
