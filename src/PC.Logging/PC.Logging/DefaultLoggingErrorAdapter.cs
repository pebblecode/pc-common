using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;
using System.Web;
using System.ServiceModel;

namespace PebbleCode.Framework.Logging
{
    public class DefaultLoggingErrorAdapter : CustomErrorHandlerServiceBehavior<ErrorHandlerLoggingAdapter>
    {
        public static void ConnectToServiceHost(ServiceHost host)
        {
            host.Description.Behaviors.Add(new DefaultLoggingErrorAdapter());
        }
    }

    public class ErrorHandlerLoggingAdapter : IErrorHandler
    {
        public void ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            Logger.WriteUnexpectedException(error, string.Empty, Category.ServerError);
        }

        public bool HandleError(Exception error)
        {
            return true;
        }
    }
}