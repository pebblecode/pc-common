using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.ServiceBus;
using Microsoft.Practices.TransientFaultHandling;
using Microsoft.ServiceBus.Messaging;
using PC.ServiceBus.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using PebbleCode.Framework.Logging;

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
        public void SendAsync(Func<BrokeredMessage> messageFactory)
        {
            SendAsync(messageFactory, () => { }, ex => { });
        }

        public void SendAsync(IEnumerable<Func<BrokeredMessage>> messageFactories)
        {
            foreach (var messageFactory in messageFactories)
            {
                SendAsync(messageFactory);
            }
        }

        public void SendAsync(Func<BrokeredMessage> messageFactory, Action successCallback, Action<Exception> exceptionCallback)
        {
            _retryPolicy.ExecuteAsync(() => Task.Factory.FromAsync(DoBeginSendMessage, DoEndSendMessage, messageFactory(), null))
                        .ContinueWith(t =>
                                          {
                                              if (t.Exception == null)
                                              {
                                                  successCallback();
                                              }
                                              else
                                              {
                                                  Logger.WriteError("An unrecoverable error occurred while trying to send a message:\r\n" + t.Exception, "ServiceBus");
                                                  exceptionCallback(t.Exception);
                                              }
                                          });
        }

        public void Send(Func<BrokeredMessage> messageFactory)
        {
            var resetEvent = new ManualResetEvent(false);
            Exception exception = null;

            SendAsync(
                messageFactory,
                () => resetEvent.Set(),
                ex =>
                {
                    exception = ex;
                    resetEvent.Set();
                });

            resetEvent.WaitOne();
            if (exception != null)
            {
                throw exception;
            }
        }

        protected virtual IAsyncResult DoBeginSendMessage(BrokeredMessage message, AsyncCallback ac, object state)
        {
            try
            {
                return _topicClient.BeginSend(message, ac, state);
            }
            catch
            {
                message.Dispose();
                throw;
            }
        }

        protected virtual void DoEndSendMessage(IAsyncResult ar)
        {
            try
            {
                _topicClient.EndSend(ar);
            }
            finally
            {
                using (ar.AsyncState as IDisposable) { }
            }
        }
    }
}
