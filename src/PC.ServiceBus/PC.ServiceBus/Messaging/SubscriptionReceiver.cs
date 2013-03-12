using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.ServiceBus;
using Microsoft.Practices.TransientFaultHandling;
using Microsoft.ServiceBus.Messaging;
using PC.ServiceBus.Configuration;
using PC.ServiceBus.Utils;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using PebbleCode.Framework.Logging;

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
        private readonly SubscriptionClient client;

        public SubscriptionReceiver(string topic, string subscription, bool processInParallel = false)
            : this(
                topic,
                subscription,
                processInParallel,
                new ExponentialBackoff(10, TimeSpan.FromMilliseconds(100), TimeSpan.FromSeconds(15), TimeSpan.FromSeconds(1)),
                new AzureConfigurationManager())
        {
        }

        protected SubscriptionReceiver(string topic, string subscription, bool processInParallel, RetryStrategy backgroundRetryStrategy, AzureConfigurationManager configurationManager)
        {
            _subscription = subscription;
            _processInParallel = processInParallel;

            client = configurationManager.MessagingFactory.CreateSubscriptionClient(topic, subscription);
            client.PrefetchCount = 30;

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
                Logger.WriteWarning(string.Format(
                    "An error occurred in attempt number {1} to receive a message from subscription {2}: {0}",
                    e.LastException.Message,
                    e.CurrentRetryCount,
                    _subscription), "ServiceBus"); 
            };
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
                Task.Factory.StartNew(() =>
                    ReceiveMessages(_cancellationSource.Token),
                    _cancellationSource.Token);
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
                        .FromAsync(client.BeginReceive, client.EndReceive, ReceiveLongPollingTimeout, null)
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
                Logger.WriteError(string.Format("An unrecoverable error occurred while trying to receive a new message from subscription {1}:\r\n{0}", ex, _subscription), "ServiceBus");
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
                        releaseAction.DeadLetterDescription);
                    break;
            }
        }
    }
}
