using Bede.Logging.Models;
using Moq;
using NUnit.Framework;
using PC.ServiceBus.Contracts;
using PC.ServiceBus.Messaging;
using PC.ServiceBus.Messaging.Handling;
using PC.ServiceBus.Serialization;
using System;

namespace PC.ServiceBus.Tests.Unit
{
    [TestFixture]
    public class CommandProcessorFixture
    {
        private readonly ILoggingService _loggingService = new NullLogger();

        [Test]
        public void GivenACommandProcessor_WhenDisposingPorcessor_ThenReceiverDisposedIfDisposable()
        {
            var receiver = new Mock<IMessageReceiver>();
            var disposable = receiver.As<IDisposable>();

            var processor = new CommandProcessor(receiver.Object, Mock.Of<ITextSerializer>(),_loggingService);

            processor.Dispose();

            disposable.Verify(x => x.Dispose());
        }

        [Test]
        public void GivenACommandProcessor_WhenRegisteringAHandlerThatIsAlreadyRegistered_THenArgumentExceptionThrown()
        {
            var processor = new CommandProcessor(Mock.Of<IMessageReceiver>(), Mock.Of<ITextSerializer>(),_loggingService);
            var handler1 = Mock.Of<ICommandHandler<FooCommand>>();
            var handler2 = Mock.Of<ICommandHandler<FooCommand>>();

            processor.Register(handler1);

            Assert.Throws<ArgumentException>(() => processor.Register(handler2));
        }

        public class FooCommand : ICommand
        {
            public Guid Id { get; set; }
        }

    }
}
