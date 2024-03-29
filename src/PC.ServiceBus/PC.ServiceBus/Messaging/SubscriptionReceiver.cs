﻿using Bede.Logging.Models;
using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.ServiceBus;
using Microsoft.Practices.TransientFaultHandling;
using Microsoft.ServiceBus.Messaging;
using PC.ServiceBus.Configuration;
using PC.ServiceBus.Utils;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace PC.ServiceBus.Messaging
{
    /// <summary>
    /// Implements an asynchronous receiver of messages from a Windows Azure 
    /// Service Bus topic subscription.
    /// </summary>
    public class SubscriptionReceiver : IMessageReceiver, IDisposable
    {
        private static readonly TimeSpan ReceiveLongPollingTimeout = TimeSpan.FromMinutes(1);
        private readonly string _subscription;
        private readonly object _lockObject = new object();
        private readonly RetryPolicy _receiveRetryPolicy;
        private readonly bool _processInParallel;
        private readonly DynamicThrottling _dynamicThrottling;
        private CancellationTokenSource _cancellationSource;
        private readonly SubscriptionClient _client;
        private readonly ILoggingService _loggingService;

        /// <summary>
        /// This will create a new Receiver for the specified topic / subscription. 
        /// </summary>
        /// <param name="topic">The name of the topic</param>
        /// <param name="subscription">The name of the subscription</param>
        /// <param name="loggingService">The service that will be used to log in the class</param>
        /// <param name="processInParallel">Whether to kick off a new Task to start receiving messages whilst processing the current message</param>
        public SubscriptionReceiver(string topic, string subscription, ILoggingService loggingService, bool processInParallel = false)
            : this(
                topic,
                subscription,
                true,
                processInParallel,
                null,
                new ExponentialBackoff(10, TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(1)),
                new AzureConfigurationManager(),
                loggingService)
        {
        }

        /// <summary>
        /// This will create a new Receiver for the specific topic / subscription. 
        /// </summary>
        /// <param name="topic">The name of the topic</param>
        /// <param name="subscription">The name of the subscription</param>
        /// <param name="filter">The Filter to apply to the subscription</param>
        /// <param name="processInParallel">Whether to kick off a new Task to start receiving messages whilst processing the current message</param>
        public SubscriptionReceiver(string topic, string subscription, Filter filter, ILoggingService loggingService, bool processInParallel = false)
            : this(
                topic,
                subscription,
                true,
                processInParallel,
                filter,
                new ExponentialBackoff(10, TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(1)),
                new AzureConfigurationManager(),
                loggingService)
        {
        }

        protected SubscriptionReceiver(string topic, string subscription, bool createIfNotExists, bool processInParallel, Filter filter, RetryStrategy backgroundRetryStrategy, AzureConfigurationManager configurationManager, ILoggingService loggingService)
        {
            _subscription = subscription;
            _processInParallel = processInParallel;
            _loggingService = loggingService;

            _client = configurationManager.MessagingFactory.CreateSubscriptionClient(topic, subscription);
            _client.PrefetchCount = 30;

            _dynamicThrottling =
                new DynamicThrottling(
                    maxDegreeOfParallelism: 100,
                    minDegreeOfParallelism: 50,
                    penaltyAmount: 3,
                    workFailedPenaltyAmount: 5,
                    workCompletedParallelismGain: 1,
                    intervalForRestoringDegreeOfParallelism: 8000);

            _receiveRetryPolicy = new RetryPolicy<ServiceBusTransientErrorDetectionStrategy>(backgroundRetryStrategy);
            _receiveRetryPolicy.Retrying += (s, e) =>
            {
                _dynamicThrottling.Penalize();
                _loggingService.Warning(
                    "An error occurred in attempt number {1} to receive a message from subscription {2}: {0}",
                    "ServiceBus",
                    e.LastException.Message,
                    e.CurrentRetryCount,
                    _subscription);
            };

            if (createIfNotExists && !configurationManager.NamespaceManager.SubscriptionExists(topic, subscription))
            {
                if (filter != null)
                {
                    configurationManager.NamespaceManager.CreateSubscription(topic, subscription, filter);
                }
                else
                {
                    configurationManager.NamespaceManager.CreateSubscription(topic, subscription);
                }
            }
        }

        /// <summary>
        /// Handler for incoming messages. The return value indicates whether the message should be disposed.
        /// </summary>
        protected Func<BrokeredMessage, MessageReleaseAction> MessageHandler { get; private set; }

        /// <summary>
        /// Starts the listener.
        /// </summary>
        public void Start(Func<BrokeredMessage, MessageReleaseAction> messageHandler)
        {
            lock (_lockObject)
            {
                MessageHandler = messageHandler;
                _cancellationSource = new CancellationTokenSource();
                Task.Factory.StartNew(() => ReceiveMessages(_cancellationSource.Token), _cancellationSource.Token);
                _dynamicThrottling.Start(_cancellationSource.Token);
            }
        }

        /// <summary>
        /// Stops the listener.
        /// </summary>
        public void Stop()
        {
            lock (_lockObject)
            {
                using (_cancellationSource)
                {
                    if (_cancellationSource != null)
                    {
                        _cancellationSource.Cancel();
                        _cancellationSource = null;
                        MessageHandler = null;
                    }
                }
            }
        }

        /// <summary>
        /// Stops the listener if it was started previously.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            Stop();

            if (disposing)
            {
                _dynamicThrottling.Dispose();
            }
        }

        protected virtual MessageReleaseAction InvokeMessageHandler(BrokeredMessage message)
        {
            return MessageHandler != null ? MessageHandler(message) : MessageReleaseAction.AbandonMessage;
        }

        ~SubscriptionReceiver()
        {
            Dispose(false);
        }

        /// <summary>
        /// Receives the messages in an endless asynchronous loop.
        /// </summary>
        private void ReceiveMessages(CancellationToken cancellationToken)
        {
            // Declare an action to receive the next message in the queue or end if cancelled.
            Action receiveNext = null;

            // Declare an action acting as a callback whenever a non-transient exception occurs while receiving or processing messages.
            Action<Exception> recoverReceive = null;

            // Declare an action responsible for the core operations in the message receive loop.
            Action receiveMessage = (() =>
            {
                // Use a retry policy to execute the Receive action in an asynchronous and reliable fashion.
                _receiveRetryPolicy.ExecuteAsync(() => Task<BrokeredMessage>.Factory
                        .FromAsync(_client.BeginReceive, _client.EndReceive, ReceiveLongPollingTimeout, null)
                        .ContinueWith(t =>
                        {
                            if (t.Exception == null || t.Exception is TimeoutException)
                            {
                                var msg = t.Result;

                                if (_processInParallel)
                                {
                                    // Continue receiving and processing new messages asynchronously
                                    Task.Factory.StartNew(receiveNext);
                                }

                                // Check if we actually received any messages.
                                if (t.Result != null)
                                {
                                    var roundtripStopwatch = Stopwatch.StartNew();

                                    Task.Factory.StartNew(() =>
                                    {
                                        var releaseAction = MessageReleaseAction.AbandonMessage;

                                        try
                                        {
                                            // Make sure the process was told to stop receiving while it was waiting for a new message.
                                            if (!cancellationToken.IsCancellationRequested)
                                            {
                                                try
                                                {

                                                    string messageType =
                                                        msg.Properties.ContainsKey(StandardMetadata.FullName)
                                                            ? msg.Properties[StandardMetadata.FullName].ToString()
                                                            : "Unknown type, metadata noin message properties";

                                                    _loggingService.Information("Received message [{0}:{1}] on subscription {2}", "ServiceBus", messageType, msg.MessageId, _subscription);
                                                    
                                                    releaseAction = InvokeMessageHandler(msg);
                                                }
                                                finally
                                                {
                                                    if (roundtripStopwatch.Elapsed > TimeSpan.FromSeconds(45))
                                                    {
                                                        _dynamicThrottling.Penalize();
                                                    }
                                                }
                                            }
                                        }
                                        finally
                                        {
                                            // Ensure that any resources allocated by a BrokeredMessage instance are released.
                                            ReleaseMessage(msg, releaseAction);
                                        }

                                        if (!_processInParallel)
                                        {
                                            // Continue receiving and processing new messages until told to stop.
                                            receiveNext.Invoke();
                                        }
                                    });
                                }
                                else
                                {
                                    _dynamicThrottling.NotifyWorkCompleted();
                                    if (!_processInParallel)
                                    {
                                        // Continue receiving and processing new messages until told to stop.
                                        receiveNext.Invoke();
                                    }
                                }
                            }
                            else
                            {
                                recoverReceive.Invoke(t.Exception);
                            }
                        }));
            });

            // Initialize an action to receive the next message in the queue or end if cancelled.
            receiveNext = () =>
            {
                _dynamicThrottling.WaitUntilAllowedParallelism(cancellationToken);
                if (!cancellationToken.IsCancellationRequested)
                {
                    _dynamicThrottling.NotifyWorkStarted();
                    // Continue receiving and processing new messages until told to stop.
                    receiveMessage.Invoke();
                }
            };

            // Initialize a custom action acting as a callback whenever a non-transient exception occurs while receiving or processing messages.
            recoverReceive = ex =>
            {
                // Just log an exception. Do not allow an unhandled exception to terminate the message receive loop abnormally.
                _loggingService.Error(ex, "An unrecoverable error occurred while trying to receive a new message from subscription {1}:\r\n{0}", ex, _subscription);
                _dynamicThrottling.NotifyWorkCompletedWithError();

                if (!cancellationToken.IsCancellationRequested)
                {
                    // Continue receiving and processing new messages until told to stop regardless of any exceptions.
                    TaskEx.Delay(10000).ContinueWith(t => receiveMessage.Invoke());
                }
            };

            // Start receiving messages asynchronously.
            receiveNext.Invoke();
        }

        private void ReleaseMessage(BrokeredMessage msg, MessageReleaseAction releaseAction)
        {
            switch (releaseAction.Kind)
            {
                case MessageReleaseActionKind.Complete:
                    msg.SafeComplete();
                    break;
                case MessageReleaseActionKind.Abandon:
                    msg.SafeAbandon();
                    break;
                case MessageReleaseActionKind.DeadLetter:
                    msg.SafeDeadLetter(
                        releaseAction.DeadLetterReason,
                        releaseAction.DeadLetterDescription,
                        _loggingService);
                    break;
            }
        }
    }
}
