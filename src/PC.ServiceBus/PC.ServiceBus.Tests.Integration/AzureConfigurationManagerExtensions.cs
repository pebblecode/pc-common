using Microsoft.ServiceBus.Messaging;
using PC.ServiceBus.Configuration;

namespace PC.ServiceBus.Tests.Integration
{
    internal static class AzureConfigurationManagerExtensions
    {
        public static MessageReceiver CreateMessageReceiver(this AzureConfigurationManager configurationManager , string topic, string subscription)
        {
            return configurationManager.MessagingFactory.CreateMessageReceiver(SubscriptionClient.FormatDeadLetterPath(topic, subscription));
        }

        public static SubscriptionClient CreateSubscriptionClient(this AzureConfigurationManager configurationManager, string topic, string subscription, ReceiveMode mode = ReceiveMode.PeekLock)
        {
            return configurationManager.MessagingFactory.CreateSubscriptionClient(topic, subscription, mode);
        }

        public static TopicClient CreateTopicClient(this AzureConfigurationManager configurationManager, string topic)
        {
            return configurationManager.MessagingFactory.CreateTopicClient(topic);
        }


        public static void CreateTopic(this AzureConfigurationManager configurationManager, string topic)
        {
            configurationManager.NamespaceManager.CreateTopic(topic);
        }

        public static void CreateSubscription(this AzureConfigurationManager configurationManager, string topic, string subscription)
        {
            CreateTopic(configurationManager, topic);

            configurationManager.NamespaceManager.CreateSubscription(topic, subscription);
        }

        public static void CreateSubscription(this AzureConfigurationManager configurationManager, SubscriptionDescription description)
        {
            CreateTopic(configurationManager, description.TopicPath);

            configurationManager.NamespaceManager.CreateSubscription(description);
        }

        public static void TryDeleteSubscription(this AzureConfigurationManager configurationManager, string topic, string subscription)
        {
            try
            {
                configurationManager.NamespaceManager.DeleteSubscription(topic, subscription);
            }
            catch { }
        }

        public static void TryDeleteTopic(this AzureConfigurationManager configurationManager, string topic)
        {
            try
            {
                configurationManager.NamespaceManager.DeleteTopic(topic);
            }
            catch { }
        }
    }
}
