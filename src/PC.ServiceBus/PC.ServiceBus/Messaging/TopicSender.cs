using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.ServiceBus;
using Microsoft.Practices.TransientFaultHandling;
using Microsoft.ServiceBus.Messaging;
using PC.ServiceBus.Configuration;
using PebbleCode.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PC.ServiceBus.Messaging
{
    /// <summary>
    /// Implements an asynchronous sender of messages to a Windows Azure Service Bus topic.
    /// </summary>
    public class TopicSender : IMessageSender
    {
        private readonly string _topic;
        private readonly RetryPolicy _retryPolicy;
        private readonly TopicClient _topicClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="TopicSender"/> class, 
        /// automatically creating the given topic if it does not exist.
        /// </summary>
        public TopicSender(string topic)
            : this(new AzureConfigurationManager(), topic, new ExponentialBackoff(10, TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(1)))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TopicSender"/> class, 
        /// automatically creating the given topic if it does not exist.
        /// </summary>
        protected TopicSender(AzureConfigurationManager configurationManager, string topic, RetryStrategy retryStrategy)
        {
            _topic = topic;

            _retryPolicy = new RetryPolicy<ServiceBusTransientErrorDetectionStrategy>(retryStrategy);
            _retryPolicy.Retrying +=
                (s, e) =>
                {
                    var handler = Retrying;
                    if (handler != null)
                    {
                        handler(this, EventArgs.Empty);
                    }

                    Logger.WriteWarning(string.Format("An error occurred in attempt number {1} to send a message: {0}", e.LastException.Message, e.CurrentRetryCount), "ServiceBus");
                };

            _topicClient = configurationManager.MessagingFactory.CreateTopicClient(_topic);
        }

        /// <summary>
        /// Notifies that the sender is retrying due to a transient fault.
        /// </summary>
        public event EventHandler Retrying;

        /// <summary>
        /// Asynchronously sends the specified message.
        /// </summary>
        public Task SendAsync(Func<BrokeredMessage> messageFactory)
        {
            return SendAsync(messageFactory, () => { }, ex => { });
        }

        public void SendAsync(IEnumerable<Func<BrokeredMessage>> messageFactories)
        {
            foreach (var messageFactory in messageFactories)
            {
                SendAsync(messageFactory);
            }
        }

        public Task SendAsync(Func<BrokeredMessage> messageFactory, Action successCallback, Action<Exception> exceptionCallback)
        {
            Logger.WriteInfo("Sending message with id {0} to topic: {1}", "ServiceBus", messageFactory().MessageId, _topic);

            return _retryPolicy.ExecuteAsync(() => _topicClient.SendAsync(messageFactory()))
                        .ContinueWith(t =>
                        {
                            if (t.Exception == null)
                            {
                                successCallback();
                            }
                            else
                            {
                                Logger.WriteError(
                                    "An unrecoverable error occurred while trying to send message {0} to topic {1}:\r\n{2}", 
                                    "ServiceBus", 
                                    messageFactory().MessageId, 
                                    _topic, 
                                    t.Exception);

                                exceptionCallback(t.Exception);
                            }
                        });
        }

        public async void Send(Func<BrokeredMessage> messageFactory)
        {
            Exception exception = null;
            await SendAsync(messageFactory);

            if (exception != null)
            {
                throw exception;
            }
        }
    }
}
