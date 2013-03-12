using System;

namespace PC.ServiceBus.Contracts
{
    /// <summary>
    /// Represents an event message.
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// Gets the event identifier.
        /// </summary>
        Guid Id { get; }
    }
}
