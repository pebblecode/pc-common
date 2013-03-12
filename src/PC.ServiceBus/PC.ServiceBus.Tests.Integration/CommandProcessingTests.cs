using Microsoft.ServiceBus.Messaging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using PC.ServiceBus.Contracts;
using PC.ServiceBus.Messaging;
using PC.ServiceBus.Messaging.Handling;
using PC.ServiceBus.Serialization;
using System;
using System.Threading;

namespace PC.ServiceBus.Tests.Integration
{
    public class CommandProcessingTests : BaseIntegrationTest
    {
        private const int TimeoutPeriod = 20000;

        [Test]
        public void GiveACommandBus_WhenReceivingACommand_ThenHandlerCalled()
        {
            var processor = new CommandProcessor(new SubscriptionReceiver(Topic, Subscription), new JsonTextSerializer());
            var bus = new CommandBus(new TopicSender(Topic), new StandardMetadataProvider(), new JsonTextSerializer());

            var e = new ManualResetEventSlim();
            var handler = new FooCommandHandler(e);

            processor.Register(handler);

            processor.Start();

            try
            {
                bus.Send(new FooCommand());

                e.Wait(TimeoutPeriod);

                Assert.True(handler.Called);
            }
            finally
            {
                processor.Stop();
            }
        }

        [Test]
        public void GiveACommandBus_WhenHandlerhandlesMultipleCommands_ThenHandlerGetsCalledForAllCommands()
        {
            var processor = new CommandProcessor(new SubscriptionReceiver(Topic, Subscription), new JsonTextSerializer());
            var bus = new CommandBus(new TopicSender(Topic), new StandardMetadataProvider(), new JsonTextSerializer());

            var fooWaiter = new ManualResetEventSlim();
            var barWaiter = new ManualResetEventSlim();
            var handler = new MultipleHandler(fooWaiter, barWaiter);

            processor.Register(handler);

            processor.Start();

            try
            {
                bus.Send(new FooCommand());
                bus.Send(new BarCommand());

                fooWaiter.Wait(TimeoutPeriod);
                barWaiter.Wait(TimeoutPeriod);

                Assert.True(handler.HandledFooCommand);
                Assert.True(handler.HandledBarCommand);
            }
            finally
            {
                processor.Stop();
            }
        }

        [Test]
        public void GiveACommandBus_WhenReceivingANotRegisteredCommand_ThenNoHandlerCalled()
        {
            var receiverMock = new Mock<SubscriptionReceiver>(Topic, Subscription, false);
            var processor = new CommandProcessor(receiverMock.Object, new JsonTextSerializer());
            var bus = new CommandBus(new TopicSender(Topic), new StandardMetadataProvider(), new JsonTextSerializer());

            var e = new ManualResetEventSlim();
            var handler = new FooCommandHandler(e);

            receiverMock.Protected().Setup("InvokeMessageHandler", ItExpr.IsAny<BrokeredMessage>()).Callback(e.Set);

            processor.Register(handler);

            processor.Start();

            try
            {
                bus.Send(new BarCommand());

                e.Wait(TimeoutPeriod);
                // Give the other event handler some time.
                Thread.Sleep(100);

                Assert.False(handler.Called);
            }
            finally
            {
                processor.Stop();
            }
        }

        [Test]
        public void GiveACommandBus_WhenSendingMultipleCommands_ThenAllHandlersCalled()
        {
            var processor = new CommandProcessor(new SubscriptionReceiver(Topic, Subscription), new JsonTextSerializer());
            var bus = new CommandBus(new TopicSender(Topic), new StandardMetadataProvider(), new JsonTextSerializer());

            var fooEvent = new ManualResetEventSlim();
            var fooHandler = new FooCommandHandler(fooEvent);

            var barEvent = new ManualResetEventSlim();
            var barHandler = new BarCommandHandler(barEvent);

            processor.Register(fooHandler);
            processor.Register(barHandler);

            processor.Start();

            try
            {
                bus.Send(new ICommand[] { new FooCommand(), new BarCommand() });

                fooEvent.Wait(TimeoutPeriod);
                barEvent.Wait(TimeoutPeriod);

                Assert.True(fooHandler.Called);
                Assert.True(barHandler.Called);
            }
            finally
            {
                processor.Stop();
            }
        }

        [Test]
        public void GiveACommandBus_WhenSendingCommandWithDelay_ThenMessageEnqueueTimeSet()
        {
            var sender = new Mock<IMessageSender>();
            var bus = new CommandBus(sender.Object, new StandardMetadataProvider(), new JsonTextSerializer());

            BrokeredMessage message = null;
            sender.Setup(x => x.Send(It.IsAny<Func<BrokeredMessage>>()))
                .Callback<Func<BrokeredMessage>>(mf => message = mf());

            bus.Send(new Envelope<ICommand>(new FooCommand()) { Delay = TimeSpan.FromMinutes(5) });

            Assert.NotNull(message);
            Assert.True(message.ScheduledEnqueueTimeUtc > DateTime.UtcNow.Add(TimeSpan.FromMinutes(4)));
        }

        [Test]
        public void GiveACommandBus_WhenSendingMultipleCommandsWithDelay_ThenMessageEnqueueTimeSet()
        {
            var sender = new Mock<IMessageSender>();
            var bus = new CommandBus(sender.Object, new StandardMetadataProvider(), new JsonTextSerializer());

            BrokeredMessage message = null;
            sender.Setup(x => x.Send(It.IsAny<Func<BrokeredMessage>>()))
                .Callback<Func<BrokeredMessage>>(mf =>
                {
                    var m = mf();
                    if (m.ScheduledEnqueueTimeUtc > DateTime.UtcNow.Add(TimeSpan.FromMinutes(4))) message = m;
                });

            bus.Send(new[] 
            {
                new Envelope<ICommand>(new FooCommand()) { Delay = TimeSpan.FromMinutes(5) }, 
                new Envelope<ICommand>(new BarCommand())
            });

            Assert.NotNull(message);
        }

        [Serializable]
        public class FooCommand : ICommand
        {
            public FooCommand()
            {
                Id = Guid.NewGuid();
            }
            public Guid Id { get; set; }
        }

        [Serializable]
        public class BarCommand : ICommand
        {
            public BarCommand()
            {
                Id = Guid.NewGuid();
            }
            public Guid Id { get; set; }
        }

        public class MultipleHandler : ICommandHandler<FooCommand>, ICommandHandler<BarCommand>
        {
            private readonly ManualResetEventSlim fooWaiter;
            private readonly ManualResetEventSlim barWaiter;

            public MultipleHandler(ManualResetEventSlim fooWaiter, ManualResetEventSlim barWaiter)
            {
                this.fooWaiter = fooWaiter;
                this.barWaiter = barWaiter;
            }

            public bool HandledBarCommand { get; private set; }
            public bool HandledFooCommand { get; private set; }

            public void Handle(BarCommand command)
            {
                HandledBarCommand = true;
                barWaiter.Set();
            }

            public void Handle(FooCommand command)
            {
                HandledFooCommand = true;
                fooWaiter.Set();
            }
        }

        public class FooCommandHandler : ICommandHandler<FooCommand>
        {
            private ManualResetEventSlim e;

            public FooCommandHandler(ManualResetEventSlim e)
            {
                this.e = e;
            }

            public void Handle(FooCommand command)
            {
                Called = true;
                e.Set();
            }

            public bool Called { get; set; }
        }

        public class BarCommandHandler : ICommandHandler<BarCommand>
        {
            private ManualResetEventSlim e;

            public BarCommandHandler(ManualResetEventSlim e)
            {
                this.e = e;
            }

            public void Handle(BarCommand command)
            {
                Called = true;
                e.Set();
            }

            public bool Called { get; set; }
        }
    }
}
