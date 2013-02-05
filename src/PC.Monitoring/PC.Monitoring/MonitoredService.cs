using System.Xml.Serialization;

namespace PC.Monitoring
{
    /// <summary>
    /// Data contract containing information about the status of a service
    /// </summary>
    [XmlRootAttribute("Service")]
    public class MonitoredService
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("status")]
        public string Status { get; set; }
    }
}
