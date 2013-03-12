using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.ServiceBus;
using Microsoft.Practices.TransientFaultHandling;
using PC.ServiceBus.Configuration;
using System;

namespace PC.ServiceBus.Tests.Integration
{
    public class BaseIntegrationTest : IDisposable
    {
        private readonly RetryPolicy<ServiceBusTransientErrorDetectionStrategy> retryPolicy;

        public BaseIntegrationTest()
        {
            ConfigurationManager = new AzureConfigurationManager();
            Topic = "cqrsjourney-test-" + Guid.NewGuid().ToString();
            Subscription = "test-" + Guid.NewGuid().ToString();

            var retryStrategy = new Incremental(3, TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(1));
            retryPolicy = new RetryPolicy<ServiceBusTransientErrorDetectionStrategy>(retryStrategy);

            // Creates the topic too.
            retryPolicy.ExecuteAction(() => ConfigurationManager.CreateSubscription(Topic, Subscription));
        }

        public AzureConfigurationManager ConfigurationManager { get; private set; }

        public string Topic { get; private set; }

        public string Subscription { get; private set; }

        public virtual void Dispose()
        {
            // Deletes subscriptions too.
            retryPolicy.ExecuteAction(() => ConfigurationManager.TryDeleteTopic(Topic));
        }
    }
}
