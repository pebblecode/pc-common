using Bede.Logging.Models;
using Microsoft.ServiceBus.Messaging;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using PC.ServiceBus.Messaging;
using PC.ServiceBus.Messaging.Handling;
using PC.ServiceBus.Serialization;
using System;
using System.IO;
using System.Text;
using System.Threading;

namespace PC.ServiceBus.Tests.Integration
{
    public class MessageProcessorTests : BaseIntegrationTest
    {
        private readonly ILoggingService _loggingService = new NullLogger();

        [Test]
        public void GivenAMessageProcessor_WhenMessageReceived_ThenProcessMessageCalled()
        {
            var waiter = new ManualResetEventSlim();
            var sender = new TopicSender(Topic, _loggingService);
            var processor = new FakeProcessor(waiter, new SubscriptionReceiver(Topic, Subscription, _loggingService), new JsonTextSerializer());

            processor.Start();

            var messageId = Guid.NewGuid().ToString();
            var correlationId = Guid.NewGuid().ToString();
            var stream = new MemoryStream();
            new JsonTextSerializer().Serialize(new StreamWriter(stream), "Foo");
            stream.Position = 0;
            sender.SendAsync(() => new BrokeredMessage(stream, true) { MessageId = messageId, CorrelationId = correlationId });

            waiter.Wait(5000);

            Assert.NotNull(processor.Payload);
            Assert.AreEqual(messageId, processor.MessageId);
            Assert.AreEqual(correlationId, processor.CorrelationId);

            processor.Stop();
        }

        [Test]
        public void GivenAMessageProcessor_WhenProcessingThrowsException_ThenMessageSentToDeadLetter()
        {
            var failCount = 0;
            var waiter = new ManualResetEventSlim();
            var sender = new TopicSender(Topic, _loggingService);
            var processor = new Mock<MessageProcessor>(
                new SubscriptionReceiver(Topic, Subscription, _loggingService), new JsonTextSerializer(), _loggingService) { CallBase = true };

            processor.Protected()
                .Setup("ProcessMessage", ItExpr.IsAny<string>(), ItExpr.IsAny<object>(), ItExpr.IsAny<string>(), ItExpr.IsAny<string>())
                .Callback(() =>
                {
                    failCount++;
                    if (failCount == 5)
                        waiter.Set();

                    throw new ArgumentException();
                });

            processor.Object.Start();

            var stream = new MemoryStream();
            new JsonTextSerializer().Serialize(new StreamWriter(stream), "Foo");
            stream.Position = 0;
            sender.SendAsync(() => new BrokeredMessage(stream, true));

            waiter.Wait(5000);

            var deadReceiver = ConfigurationManager.CreateMessageReceiver(Topic, Subscription);

            var deadMessage = deadReceiver.Receive(TimeSpan.FromSeconds(5));

            processor.Object.Dispose();

            Assert.NotNull(deadMessage);
            var data = new JsonTextSerializer().Deserialize(new StreamReader(deadMessage.GetBody<Stream>()));

            Assert.AreEqual("Foo", data);
        }

        [Test]
        public void GivenAMessageProcessor_WhenMessageFailsToDeserialize_ThenMessageSentToDeadLetter()
        {
            var waiter = new ManualResetEventSlim();
            var sender = new TopicSender(Topic, _loggingService);
            var processor = new FakeProcessor(
                waiter,
                new SubscriptionReceiver(Topic, Subscription, _loggingService),
                new JsonTextSerializer());

            processor.Start();

            var data = new JsonTextSerializer().Serialize(new Data());
            data = data.Replace(typeof(Data).FullName, "Some.TypeName.Cannot.Resolve");
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(data));
            stream.Position = 0;

            sender.SendAsync(() => new BrokeredMessage(stream, true));

            waiter.Wait(5000);

            var deadReceiver = ConfigurationManager.CreateMessageReceiver(Topic, Subscription);
            var deadMessage = deadReceiver.Receive(TimeSpan.FromSeconds(5));

            processor.Dispose();

            Assert.NotNull(deadMessage);
            var payload = new StreamReader(deadMessage.GetBody<Stream>()).ReadToEnd();

            Assert.IsTrue(payload.Contains("Some.TypeName.Cannot.Resolve"));
        }
    }

    public class FakeProcessor : MessageProcessor
    {
        private readonly ManualResetEventSlim waiter;

        public FakeProcessor(ManualResetEventSlim waiter, IMessageReceiver receiver, ITextSerializer serializer)
            : base(receiver, serializer, new NullLogger())
        {
            this.waiter = waiter;
        }

        public object Payload { get; private set; }

        public string MessageId { get; private set; }

        public string CorrelationId { get; private set; }

        protected override void ProcessMessage(string traceIdentifier, object payload, string messageId, string correlationId)
        {
            Payload = payload;
            MessageId = messageId;
            CorrelationId = correlationId;

            waiter.Set();
        }
    }

    [Serializable]
    internal class Data
    {
    }
}
