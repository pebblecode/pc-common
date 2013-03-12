using System.Linq;
using PC.ServiceBus.Contracts;
using System.Collections.Generic;

namespace PC.ServiceBus.Messaging
{
    /// <summary>
    /// Provides usability overloads for <see cref="ICommandBus"/>
    /// </summary>
    public static class CommandBusExtensions
    {
        public static void Send(this ICommandBus bus, ICommand command)
        {
            bus.Send(new Envelope<ICommand>(command));
        }

        public static void Send(this ICommandBus bus, IEnumerable<ICommand> commands)
        {
            bus.Send(commands.Select(x => new Envelope<ICommand>(x)));
        }
    }
}
