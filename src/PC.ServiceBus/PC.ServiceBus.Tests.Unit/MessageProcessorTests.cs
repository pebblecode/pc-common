using Bede.Logging.Models;
using Moq;
using NUnit.Framework;
using PC.ServiceBus.Messaging;
using PC.ServiceBus.Messaging.Handling;
using PC.ServiceBus.Serialization;
using System;

namespace PC.ServiceBus.Tests.Unit
{
    [TestFixture]
    public class MessageProcessorFixture
    {
        private readonly ILoggingService _loggingService = new NullLogger();

        [Test]
        public void GivenAMessageProcessor_WhenStartCalledTwice_ThenSecondCallIgnored()
        {
            var receiver = new Mock<IMessageReceiver>();
            var serializer = new Mock<ITextSerializer>();
            var processor = new Mock<MessageProcessor>(receiver.Object, serializer.Object, _loggingService) { CallBase = true }.Object;

            processor.Start();

            processor.Start();
        }

        [Test]
        public void GivenAMessageProcessor_WhenDisposeCalled_ThenStopIsCalled()
        {
            var receiver = new Mock<IMessageReceiver>();
            var serializer = new Mock<ITextSerializer>();
            var processor = new Mock<MessageProcessor>(receiver.Object, serializer.Object, _loggingService) { CallBase = true }.Object;

            processor.Start();
            processor.Dispose();

            Mock.Get(processor).Verify(x => x.Stop());
        }

        [Test]
        public void GivenAMessageProcessor_WhenStartCalledTwice_ThenReceiverDisposedIfDisposable()
        {
            var receiver = new Mock<IMessageReceiver>();
            var disposable = receiver.As<IDisposable>();
            var serializer = new Mock<ITextSerializer>();
            var processor = new Mock<MessageProcessor>(receiver.Object, serializer.Object, _loggingService) { CallBase = true }.Object;

            processor.Dispose();

            disposable.Verify(x => x.Dispose());
        }

        [Test]
        public void GivenAMessageProcessor_WhenStoppingANonStartedProcessor_ThenNothingDone()
        {
            var receiver = new Mock<IMessageReceiver>();
            var serializer = new Mock<ITextSerializer>();
            var processor = new Mock<MessageProcessor>(receiver.Object, serializer.Object, _loggingService) { CallBase = true }.Object;

            processor.Stop();
            receiver.Verify(x => x.Stop(), Times.Never());
        }
    }
}
