using Microsoft.ServiceBus.Messaging;
using PC.ServiceBus.Messaging;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace PC.ServiceBus.Tests.Unit
{
    class MessageSenderMock : IMessageSender
    {
        public readonly AutoResetEvent SendSignal = new AutoResetEvent(false);
        public readonly ConcurrentBag<BrokeredMessage> Sent = new ConcurrentBag<BrokeredMessage>();
        public readonly ConcurrentBag<Action> AsyncSuccessCallbacks = new ConcurrentBag<Action>();

        public bool ShouldWaitForCallback { get; set; }

        void IMessageSender.Send(Func<BrokeredMessage> messageFactory)
        {
            Sent.Add(messageFactory.Invoke());
            SendSignal.Set();
        }

        void IMessageSender.SendAsync(Func<BrokeredMessage> messageFactory)
        {
            throw new NotImplementedException();
        }

        void IMessageSender.SendAsync(Func<BrokeredMessage> messageFactory, Action successCallback, Action<Exception> exceptionCallback)
        {
            Task.Factory.StartNew(
                () =>
                {
                    Sent.Add(messageFactory.Invoke());
                    SendSignal.Set();
                    if (!ShouldWaitForCallback)
                    {
                        successCallback();
                    }
                    else
                    {
                        AsyncSuccessCallbacks.Add(successCallback);
                    }
                },
                TaskCreationOptions.AttachedToParent);
        }

        public event EventHandler Retrying;
    }
}
