using System.ServiceModel;
using System.ServiceModel.Web;

namespace PebbleCode.Monitoring
{
    [ServiceContract(Name = "MonitoringContract", Namespace = "http://bedegaming.com/monitoring")]
    public interface IMonitoringContract
    {
        [OperationContract]
        [XmlSerializerFormat]
        [WebInvoke(UriTemplate = "/GetStatus", Method = "GET", ResponseFormat = WebMessageFormat.Xml, RequestFormat = WebMessageFormat.Xml)]
        GetHealthStatusResponse GetStatus();
    }
}
