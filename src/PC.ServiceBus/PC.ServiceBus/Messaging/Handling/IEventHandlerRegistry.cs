namespace PC.ServiceBus.Messaging.Handling
{
    public interface IEventHandlerRegistry : IProcessor
    {
        void Register(IEventHandler handler);
    }
}
