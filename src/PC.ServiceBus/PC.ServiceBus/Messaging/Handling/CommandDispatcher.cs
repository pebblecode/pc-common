using PC.ServiceBus.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PC.ServiceBus.Messaging.Handling
{
    public class CommandDispatcher
    {
        private readonly Dictionary<Type, ICommandHandler> _handlers = new Dictionary<Type, ICommandHandler>();

        /// <summary>
        /// Registers the specified command handler.
        /// </summary>
        public void Register(ICommandHandler commandHandler)
        {
            var genericHandler = typeof(ICommandHandler<>);
            var supportedCommandTypes = commandHandler.GetType()
                .GetInterfaces()
                .Where(iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == genericHandler)
                .Select(iface => iface.GetGenericArguments()[0])
                .ToList();

            if (_handlers.Keys.Any(supportedCommandTypes.Contains))
                throw new ArgumentException("The command handled by the received handler already has a registered handler.");

            // Register this handler for each of he handled types.
            foreach (var commandType in supportedCommandTypes)
            {
                _handlers.Add(commandType, commandHandler);
            }
        }

        /// <summary>
        /// Processes the message by calling the registered handler.
        /// </summary>
        public bool ProcessMessage(string traceIdentifier, ICommand payload, string messageId, string correlationId)
        {
            var commandType = payload.GetType();
            ICommandHandler handler = null;

            if (_handlers.TryGetValue(commandType, out handler))
            {
                ((dynamic)handler).Handle((dynamic)payload);
                return true;
            }
            
            return false;
        }
    }
}
