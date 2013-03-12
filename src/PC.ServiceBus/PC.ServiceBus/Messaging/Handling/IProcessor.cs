namespace PC.ServiceBus.Messaging.Handling
{
    public interface IProcessor
    {
        void Start();

        void Stop();
    }
}
