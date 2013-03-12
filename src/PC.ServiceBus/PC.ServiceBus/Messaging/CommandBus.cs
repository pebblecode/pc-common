using Microsoft.ServiceBus.Messaging;
using PC.ServiceBus.Contracts;
using PC.ServiceBus.Serialization;
using System;
using System.Collections.Generic;
using System.IO;

namespace PC.ServiceBus.Messaging
{
    /// <summary>
    /// A command bus that sends serialized object payloads through a <see cref="IMessageSender"/>.
    /// </summary>
    public class CommandBus : ICommandBus
    {
        private readonly IMessageSender _sender;
        private readonly IMetadataProvider _metadataProvider;
        private readonly ITextSerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandBus"/> class.
        /// </summary>
        public CommandBus(IMessageSender sender, IMetadataProvider metadataProvider, ITextSerializer serializer)
        {
            _sender = sender;
            _metadataProvider = metadataProvider;
            _serializer = serializer;
        }

        /// <summary>
        /// Sends the specified command.
        /// </summary>
        public void Send(Envelope<ICommand> command)
        {
            _sender.Send(() => BuildMessage(command));
        }

        public void Send(IEnumerable<Envelope<ICommand>> commands)
        {
            foreach (var command in commands)
            {
                Send(command);
            }
        }

        private BrokeredMessage BuildMessage(Envelope<ICommand> command)
        {
            var stream = new MemoryStream();
            try
            {
                var writer = new StreamWriter(stream);
                _serializer.Serialize(writer, command.Body);
                stream.Position = 0;

                var message = new BrokeredMessage(stream, true);

                if (!string.IsNullOrWhiteSpace(command.MessageId))
                {
                    message.MessageId = command.MessageId;
                }
                else if (!default(Guid).Equals(command.Body.Id))
                {
                    message.MessageId = command.Body.Id.ToString();
                }

                if (!string.IsNullOrWhiteSpace(command.CorrelationId))
                {
                    message.CorrelationId = command.CorrelationId;
                }

                var metadata = _metadataProvider.GetMetadata(command.Body);
                if (metadata != null)
                {
                    foreach (var pair in metadata)
                    {
                        message.Properties[pair.Key] = pair.Value;
                    }
                }

                if (command.Delay > TimeSpan.Zero)
                {
                    message.ScheduledEnqueueTimeUtc = DateTime.UtcNow.Add(command.Delay);
                }

                if (command.TimeToLive > TimeSpan.Zero)
                {
                    message.TimeToLive = command.TimeToLive;
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
