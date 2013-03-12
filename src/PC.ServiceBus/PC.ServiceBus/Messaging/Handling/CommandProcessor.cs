using PC.ServiceBus.Contracts;
using PC.ServiceBus.Serialization;

namespace PC.ServiceBus.Messaging.Handling
{
    /// <summary>
    /// Processes incoming commands from the bus and routes them to the appropriate 
    /// handlers.
    /// </summary>
    public class CommandProcessor : MessageProcessor, ICommandHandlerRegistry
    {
        private readonly CommandDispatcher _commandDispatcher;

        public CommandProcessor(IMessageReceiver receiver, ITextSerializer serializer)
            : base(receiver, serializer)
        {
            _commandDispatcher = new CommandDispatcher();
        }

        /// <summary>
        /// Registers the specified command handler.
        /// </summary>
        public void Register(ICommandHandler commandHandler)
        {
            _commandDispatcher.Register(commandHandler);
        }

        /// <summary>
        /// Processes the message by calling the registered handler.
        /// </summary>
        protected override void ProcessMessage(string traceIdentifier, object payload, string messageId, string correlationId)
        {
            _commandDispatcher.ProcessMessage(traceIdentifier, (ICommand)payload, messageId, correlationId);
        }
    }
}
