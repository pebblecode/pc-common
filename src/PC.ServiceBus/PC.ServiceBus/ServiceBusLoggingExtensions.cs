using Bede.Logging.Models;
using System;
namespace PC.ServiceBus
{
    public static class ServiceBusLoggingExtensions
    {
        private const string ServiceBusCategory = "ServiceBus";

        public static void Warning(this ILoggingService loggingService, string messageFormat, params object[] messageArguments)
        {
            var loggingData = new CommonFormattedLoggingData(ServiceBusCategory, null, null, messageFormat, messageArguments);
            loggingService.Warning(loggingData);
        }

        public static void Information(this ILoggingService loggingService, string messageFormat, params object[] messageArguments)
        {
            var loggingData = new CommonFormattedLoggingData(ServiceBusCategory, null, null, messageFormat, messageArguments);
            loggingService.Information(loggingData);
        }

        public static void Error<TTargat>(this ILoggingService loggingService, TTargat target, Exception exception, string messageFormat, params object[] messageArguments)
            where TTargat : class
        {
            var loggingData = new CommonFormattedLoggingData(ServiceBusCategory, null, null, messageFormat, messageArguments);
            loggingService.Error(loggingData, exception, target);
        }

        public static void Error(this ILoggingService loggingService, Exception exception, string messageFormat, params object[] messageArguments)
        {
            var loggingData = new CommonFormattedLoggingData(ServiceBusCategory, null, null, messageFormat, messageArguments);
            loggingService.Error(loggingData, exception);
        }
    }
}
