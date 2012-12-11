using System.Collections.ObjectModel;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System;

namespace PebbleCode.Framework.Logging
{
    public class CustomErrorHandlerServiceBehavior<ErrorHandlerType> : IServiceBehavior
        where ErrorHandlerType : IErrorHandler, new()
    {

        public void AddBindingParameters(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase,
            Collection<ServiceEndpoint> endpoints,
             BindingParameterCollection bindingParameters)
        {

        }

        public void ApplyDispatchBehavior(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase)
        {

            foreach (ChannelDispatcher chDisp in serviceHostBase.ChannelDispatchers)
            {
                chDisp.IncludeExceptionDetailInFaults = true;
                if (chDisp.ErrorHandlers.Count > 0)
                {
                    // Remove the System.ServiceModel.Web errorHandler
                    chDisp.ErrorHandlers.Remove(chDisp.ErrorHandlers[0]);
                }
                // Add new custom error handler
                chDisp.ErrorHandlers.Add(new ErrorHandlerType());
            }
        }

        public void Validate(
            ServiceDescription serviceDescription,
            ServiceHostBase serviceHostBase)
        {
        }
    }
}