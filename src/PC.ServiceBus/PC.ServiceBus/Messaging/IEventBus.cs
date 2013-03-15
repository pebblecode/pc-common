using PC.ServiceBus.Contracts;
using System.Collections.Generic;

namespace PC.ServiceBus.Messaging
{
    /// <summary>
    /// An event bus that sends serialized object payloads.
    /// </summary>
    public interface IEventBus
    {
        void Publish(IEvent @event);

        void Publish(IEnumerable<IEvent> events);

        void Publish(Envelope<IEvent> @event);

        void Publish(IEnumerable<Envelope<IEvent>> events);
    }
}
