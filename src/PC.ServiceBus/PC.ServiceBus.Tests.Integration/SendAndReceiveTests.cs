using Microsoft.ServiceBus.Messaging;
using NUnit.Framework;
using PC.ServiceBus.Messaging;
using System;
using System.Runtime.Serialization;
using System.Threading;

namespace PC.ServiceBus.Tests.Integration
{
    [TestFixture]
    public class SendAndReceiveTests : BaseIntegrationTest
    {
        [Test]
        public void when_sending_message_then_can_receive_it()
        {
            var sender = new TopicSender(Topic);
            var data = new Data { Id = Guid.NewGuid(), Title = "Foo" };
            Data received = null;
            using (var receiver = new SubscriptionReceiver(Topic, Subscription))
            {
                var signal = new ManualResetEventSlim();

                receiver.Start(
                    m =>
                    {
                        received = m.GetBody<Data>();
                        signal.Set();
                        return MessageReleaseAction.CompleteMessage;
                    });

                sender.SendAsync(() => new BrokeredMessage(data));

                signal.Wait();
            }

            Assert.NotNull(received);
            Assert.AreEqual(data.Id, received.Id);
            Assert.AreEqual(data.Title, received.Title);
        }

        [DataContract]
        private class Data
        {
            [DataMember]
            public Guid Id { get; set; }

            [DataMember]
            public string Title { get; set; }
        }
    }
}
