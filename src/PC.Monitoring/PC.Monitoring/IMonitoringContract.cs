using System.ServiceModel;
using System.ServiceModel.Web;

namespace PebbleCode.Monitoring
{
    [ServiceContract(Name = "MonitoringContract", Namespace = "http://bedegaming.com/monitoring")]
    public interface IMonitoringContract
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/GetStatus", Method = "GET")]
        GetHealthStatusResponse GetStatus();
    }
}
