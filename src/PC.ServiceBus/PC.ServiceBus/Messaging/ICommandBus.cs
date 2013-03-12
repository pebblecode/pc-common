using PC.ServiceBus.Contracts;
using System.Collections.Generic;

namespace PC.ServiceBus.Messaging
{
    /// <summary>
    /// A command bus that sends serialized object payloads.
    /// </summary>
    public interface ICommandBus
    {
        void Send(Envelope<ICommand> command);

        void Send(IEnumerable<Envelope<ICommand>> commands);
    }
}
