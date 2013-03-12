using Microsoft.ServiceBus.Messaging;
using PC.ServiceBus.Serialization;
using PebbleCode.Framework.Logging;
using System;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;

namespace PC.ServiceBus.Messaging.Handling
{
    /// <summary>
    /// Provides basic common processing code for components that handle 
    /// incoming messages from a receiver.
    /// </summary>
    public abstract class MessageProcessor : IProcessor, IDisposable
    {
        private const int MAX_PROCESSING_RETRIES = 5;
        private bool _disposed;
        private bool _started = false;
        private readonly IMessageReceiver _receiver;
        private readonly ITextSerializer _serializer;
        private readonly object _lockObject = new object();

        protected MessageProcessor(IMessageReceiver receiver, ITextSerializer serializer)
        {
            _receiver = receiver;
            _serializer = serializer;
        }

        protected ITextSerializer Serializer { get { return _serializer; } }

        /// <summary>
        /// Starts the listener.
        /// </summary>
        public virtual void Start()
        {
            ThrowIfDisposed();
            lock (_lockObject)
            {
                if (!_started)
                {
                    _receiver.Start(OnMessageReceived);
                    _started = true;
                }
            }
        }

        /// <summary>
        /// Stops the listener.
        /// </summary>
        public virtual void Stop()
        {
            lock (_lockObject)
            {
                if (_started)
                {
                    _receiver.Stop();
                    _started = false;
                }
            }
        }

        /// <summary>
        /// Disposes the resources used by the processor.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Processes the message.
        /// </summary>
        /// <param name="traceIdentifier">The identifier that can be used to track the source message in the logs.</param>
        /// <param name="payload">The typed message payload.</param>
        /// <param name="messageId">The message id.</param>
        /// <param name="correlationId">The message correlation id.</param>
        protected abstract void ProcessMessage(string traceIdentifier, object payload, string messageId, string correlationId);

        /// <summary>
        /// Disposes the resources used by the processor.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Stop();
                    _disposed = true;

                    using (_receiver as IDisposable)
                    {
                        // Dispose receiver if it's disposable.
                    }
                }
            }
        }

        ~MessageProcessor()
        {
            Dispose(false);
        }

        private MessageReleaseAction OnMessageReceived(BrokeredMessage message)
        {
            // NOTE: type information does not belong here. It's a responsibility 
            // of the serializer to be self-contained and put any information it 
            // might need for rehydration.

            object payload;
            using (var stream = message.GetBody<Stream>())
            using (var reader = new StreamReader(stream))
            {
                try
                {
                    payload = _serializer.Deserialize(reader);
                }
                catch (SerializationException e)
                {
                    return MessageReleaseAction.DeadLetterMessage(e.Message, e.ToString());
                }
            }

            string traceIdentifier = BuildTraceIdentifier(message);

            try
            {
                ProcessMessage(traceIdentifier, payload, message.MessageId, message.CorrelationId);
            }
            catch (Exception e)
            {
                return HandleProcessingException(message, traceIdentifier, e);
            }

            return CompleteMessage();
        }

        private MessageReleaseAction CompleteMessage()
        {
            return MessageReleaseAction.CompleteMessage;
        }

        private MessageReleaseAction HandleProcessingException(BrokeredMessage message, string traceIdentifier, Exception e)
        {
            if (message.DeliveryCount > MAX_PROCESSING_RETRIES)
            {
                Logger.WriteError("An error occurred while processing the message" + traceIdentifier + " and will be dead-lettered:\r\n" + e, "ServiceBus");
                return MessageReleaseAction.DeadLetterMessage(e.Message, e.ToString());
            }

            Logger.WriteError("An error occurred while processing the message" + traceIdentifier + " and will be abandoned:\r\n" + e, "ServiceBus");
            return MessageReleaseAction.AbandonMessage;
        }

        private static string BuildTraceIdentifier(BrokeredMessage message)
        {
            try
            {
                var messageId = message.MessageId;
                return string.Format(CultureInfo.InvariantCulture, " (MessageId: {0})", messageId);
            }
            catch (ObjectDisposedException)
            {
                // if there is any kind of exception trying to build a trace identifier, ignore, as it is not important.
            }

            return string.Empty;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException("MessageProcessor");
        }
    }
}
