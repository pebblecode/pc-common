﻿using System.Threading.Tasks;
using Microsoft.ServiceBus.Messaging;
using System;

namespace PC.ServiceBus.Messaging
{
    /// <summary>
    /// Abstracts the behavior of sending a message.
    /// </summary>
    public interface IMessageSender
    {
        /// <summary>
        /// Sends the specified message synchronously.
        /// </summary>
        Task Send(Func<BrokeredMessage> messageFactory);

        /// <summary>
        /// Sends the specified message asynchronously.
        /// </summary>
        Task SendAsync(Func<BrokeredMessage> messageFactory);

        /// <summary>
        /// Sends the specified message asynchronously.
        /// </summary>
        Task SendAsync(Func<BrokeredMessage> messageFactory, Action successCallback, Action<Exception> exceptionCallback);

        /// <summary>
        /// Notifies that the sender is retrying due to a transient fault.
        /// </summary>
        event EventHandler Retrying;
    }
}
