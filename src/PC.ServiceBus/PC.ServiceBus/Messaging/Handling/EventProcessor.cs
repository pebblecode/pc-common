using PC.ServiceBus.Contracts;
using PC.ServiceBus.Serialization;

namespace PC.ServiceBus.Messaging.Handling
{
    /// <summary>
    /// Processes incoming events from the bus and routes them to the appropriate 
    /// handlers.
    /// </summary>
    public class EventProcessor : MessageProcessor, IEventProcessor
    {
        private readonly EventDispatcher eventDispatcher;

        public EventProcessor(IMessageReceiver receiver, ITextSerializer serializer)
            : base(receiver, serializer)
        {
            this.eventDispatcher = new EventDispatcher();
        }

        public void Register(IEventHandler eventHandler)
        {
            this.eventDispatcher.Register(eventHandler);
        }

        protected override void ProcessMessage(string traceIdentifier, object payload, string messageId, string correlationId)
        {
            var @event = payload as IEvent;
            if (@event != null)
            {
                this.eventDispatcher.DispatchMessage(@event, messageId, correlationId, traceIdentifier);
            }
        }
    }
}
