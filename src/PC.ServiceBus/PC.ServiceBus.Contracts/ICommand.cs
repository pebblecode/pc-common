using System;

namespace PC.ServiceBus.Contracts
{
    public interface ICommand
    {
        /// <summary>
        /// Gets the command identifier.
        /// </summary>
        Guid Id { get; }
    }
}
