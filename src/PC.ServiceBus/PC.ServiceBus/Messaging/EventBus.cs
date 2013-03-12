using Microsoft.ServiceBus.Messaging;
using PC.ServiceBus.Contracts;
using PC.ServiceBus.Serialization;
using System.Collections.Generic;
using System.IO;

namespace PC.ServiceBus.Messaging
{
    /// <summary>
    /// An event bus that sends serialized object payloads through a <see cref="IMessageSender"/>.
    /// </summary>
    public class EventBus : IEventBus
    {
        private readonly IMessageSender _sender;
        private readonly IMetadataProvider _metadataProvider;
        private readonly ITextSerializer _serializer;

        public EventBus(IMessageSender sender, IMetadataProvider metadataProvider, ITextSerializer serializer)
        {
            _sender = sender;
            _metadataProvider = metadataProvider;
            _serializer = serializer;
        }

        /// <summary>
        /// Sends the specified event.
        /// </summary>
        public void Publish(Envelope<IEvent> @event)
        {
            _sender.Send(() => BuildMessage(@event));
        }

        /// <summary>
        /// Publishes the specified events.
        /// </summary>
        public void Publish(IEnumerable<Envelope<IEvent>> events)
        {
            foreach (var @event in events)
            {
                Publish(@event);
            }
        }

        private BrokeredMessage BuildMessage(Envelope<IEvent> envelope)
        {
            var @event = envelope.Body;

            var stream = new MemoryStream();
            try
            {
                var writer = new StreamWriter(stream);
                _serializer.Serialize(writer, @event);
                stream.Position = 0;

                var message = new BrokeredMessage(stream, true) { SessionId = @event.Id.ToString() };

                if (!string.IsNullOrWhiteSpace(envelope.MessageId))
                {
                    message.MessageId = envelope.MessageId;
                }

                if (!string.IsNullOrWhiteSpace(envelope.CorrelationId))
                {
                    message.CorrelationId = envelope.CorrelationId;
                }

                var metadata = _metadataProvider.GetMetadata(@event);
                if (metadata != null)
                {
                    foreach (var pair in metadata)
                    {
                        message.Properties[pair.Key] = pair.Value;
                    }
                }

                return message;
            }
            catch
            {
                stream.Dispose();
                throw;
            }
        }
    }
}
