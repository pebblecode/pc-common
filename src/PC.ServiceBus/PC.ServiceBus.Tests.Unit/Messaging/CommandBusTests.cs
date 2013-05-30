using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using PC.ServiceBus.Contracts;
using PC.ServiceBus.Messaging;
using PC.ServiceBus.Serialization;

namespace PC.ServiceBus.Tests.Unit.Messaging
{
    [TestFixture]
    public class CommandBusFixture
    {
        [Test]
        public void GivenACommandBus_WhenSendingACommand_ThenMessageIdGetsSetToCommandId()
        {
            var sender = new MessageSenderMock();
            var sut = new CommandBus(sender, Mock.Of<IMetadataProvider>(), new JsonTextSerializer());

            var command = new FooCommand { Id = Guid.NewGuid() };
            sut.Send(command);

            Assert.AreEqual(command.Id.ToString(), sender.Sent.Single().MessageId);
        }

        [Test]
        public void GivenACommandBus_WhenSpecifyingTimeToLive_ThenMessageContainsTimeToLive()
        {
            var sender = new MessageSenderMock();
            var sut = new CommandBus(sender, Mock.Of<IMetadataProvider>(), new JsonTextSerializer());

            var command = new Envelope<ICommand>(new FooCommand { Id = Guid.NewGuid() })
            {
                TimeToLive = TimeSpan.FromMinutes(15)
            };
            sut.Send(command);

            Assert.GreaterOrEqual(sender.Sent.Single().TimeToLive, TimeSpan.FromMinutes(14.9));
            Assert.LessOrEqual(sender.Sent.Single().TimeToLive, TimeSpan.FromMinutes(15.1));
        }

        [Test]
        public void GivenACommandBus_WhenSpecifyingDeplay_ThenMessageContainsDelay()
        {
            var sender = new MessageSenderMock();
            var sut = new CommandBus(sender, Mock.Of<IMetadataProvider>(), new JsonTextSerializer());

            var command = new Envelope<ICommand>(new FooCommand { Id = Guid.NewGuid() })
            {
                Delay = TimeSpan.FromMinutes(15)
            };
            sut.Send(command);

            Assert.GreaterOrEqual(sender.Sent.Single().ScheduledEnqueueTimeUtc, DateTime.UtcNow.AddMinutes(14.9));
            Assert.LessOrEqual(sender.Sent.Single().ScheduledEnqueueTimeUtc, DateTime.UtcNow.AddMinutes(15.1));
        }

        class FooCommand : ICommand
        {
            public Guid Id { get; set; }
        }
    }
}
