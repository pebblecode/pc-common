using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PC.ServiceBus.Messaging.Handling
{
    public interface ICommandProcessor
    {
        void Start();

        void Stop();

        void Register(ICommandHandler handler);
    }
}
