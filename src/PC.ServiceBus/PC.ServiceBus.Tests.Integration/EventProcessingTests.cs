using Bede.Logging.Models;
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
    [TestFixture]
    public class EventProcessingTests : BaseIntegrationTest
    {
        private const int TimeoutPeriod = 20000;
        private readonly ILoggingService _loggingService = new NullLogger();

        [Test]
        public void GivenAnEventBus_WhenReceivingAnEvent_ThenCallsRegisteredEventHandler()
        {
            var processor = new EventProcessor(new SubscriptionReceiver(Topic, Subscription, _loggingService), new JsonTextSerializer(), _loggingService);
            var bus = new EventBus(new TopicSender(Topic, _loggingService), new StandardMetadataProvider(), new JsonTextSerializer());

            var e = new ManualResetEventSlim();
            var handler = new FooEventHandler(e);

            processor.Register(handler);

            processor.Start();

            try
            {
                bus.Publish(new FooEvent());

                e.Wait(TimeoutPeriod);

                Assert.True(handler.Called);
            }
            finally
            {
                processor.Stop();
            }
        }

        [Test]
        public void GivenAnEventBus_WhenReceivingAnEvent_ThenCallsRegisteredEventHandlerWithEnvelope()
        {
            var processor = new EventProcessor(new SubscriptionReceiver(Topic, Subscription, _loggingService), new JsonTextSerializer(), _loggingService);
            var bus = new EventBus(new TopicSender(Topic, _loggingService), new StandardMetadataProvider(), new JsonTextSerializer());

            var e = new ManualResetEventSlim();
            var handler = new FooEnvelopedEventHandler(e);

            processor.Register(handler);

            processor.Start();

            try
            {
                bus.Publish(new FooEvent());

                e.Wait(TimeoutPeriod);

                Assert.True(handler.Called);
            }
            finally
            {
                processor.Stop();
            }
        }

        [Test]
        public void GivenAnEventBus_WhenReceivingAnEventWithMessageAndCorrelationIds_ThenCallsRegisteredEventHandlerWithEnvelope()
        {
            var processor = new EventProcessor(new SubscriptionReceiver(Topic, Subscription, _loggingService), new JsonTextSerializer(), _loggingService);
            var bus = new EventBus(new TopicSender(Topic, _loggingService), new StandardMetadataProvider(), new JsonTextSerializer());

            var e = new ManualResetEventSlim();
            var handler = new FooEnvelopedEventHandler(e);

            processor.Register(handler);

            processor.Start();

            try
            {
                bus.Publish(new Envelope<IEvent>(new FooEvent()) { CorrelationId = "correlation", MessageId = "message" });

                e.Wait(TimeoutPeriod);

                Assert.True(handler.Called);
                Assert.AreEqual("correlation", handler.CorrelationId);
                Assert.AreEqual("message", handler.MessageId);
            }
            finally
            {
                processor.Stop();
            }
        }

        [Test]
        public void GivenAnEventBus_WhenReceivingAnEventWithNoRegisteredHandlers_ThenNoEventHandlerCalled()
        {
            var receiverMock = new Mock<SubscriptionReceiver>(Topic, Subscription, _loggingService, false);
            var processor = new EventProcessor(receiverMock.Object, new JsonTextSerializer(), _loggingService);
            var bus = new EventBus(new TopicSender(Topic, _loggingService), new StandardMetadataProvider(), new JsonTextSerializer());

            var e = new ManualResetEventSlim();
            var handler = new FooEventHandler(e);

            receiverMock.Protected().Setup("InvokeMessageHandler", ItExpr.IsAny<BrokeredMessage>()).Callback(e.Set);

            processor.Register(handler);

            processor.Start();

            try
            {
                bus.Publish(new BarEvent());

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
        public void GivenAnEventBus_WhenSendingMultipleEvents_ThenAllEventHandlersCalled()
        {
            var processor = new EventProcessor(new SubscriptionReceiver(Topic, Subscription, _loggingService), new JsonTextSerializer(), _loggingService);
            var bus = new EventBus(new TopicSender(Topic, _loggingService), new StandardMetadataProvider(), new JsonTextSerializer());

            var fooEvent = new ManualResetEventSlim();
            var fooHandler = new FooEventHandler(fooEvent);

            var barEvent = new ManualResetEventSlim();
            var barHandler = new BarEventHandler(barEvent);

            processor.Register(fooHandler);
            processor.Register(barHandler);

            processor.Start();

            try
            {
                bus.Publish(new IEvent[] { new FooEvent(), new BarEvent() });

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

        [Serializable]
        public class FooEvent : IEvent
        {
            public FooEvent()
            {
                this.Id = Guid.NewGuid();
            }
            public Guid Id { get; set; }
        }

        [Serializable]
        public class BarEvent : IEvent
        {
            public BarEvent()
            {
                this.Id = Guid.NewGuid();
            }
            public Guid Id { get; set; }
        }

        public class FooEventHandler : IEventHandler<FooEvent>
        {
            private ManualResetEventSlim e;

            public FooEventHandler(ManualResetEventSlim e)
            {
                this.e = e;
            }

            public void Handle(FooEvent command)
            {
                this.Called = true;
                e.Set();
            }

            public bool Called { get; set; }
        }

        public class FooEnvelopedEventHandler : IEnvelopedEventHandler<FooEvent>
        {
            private ManualResetEventSlim e;

            public FooEnvelopedEventHandler(ManualResetEventSlim e)
            {
                this.e = e;
            }

            public void Handle(Envelope<FooEvent> command)
            {
                this.Called = true;
                this.MessageId = command.MessageId;
                this.CorrelationId = command.CorrelationId;
                e.Set();
            }

            public bool Called { get; set; }

            public string MessageId { get; set; }

            public string CorrelationId { get; set; }
        }

        public class BarEventHandler : IEventHandler<BarEvent>
        {
            private ManualResetEventSlim e;

            public BarEventHandler(ManualResetEventSlim e)
            {
                this.e = e;
            }

            public void Handle(BarEvent command)
            {
                this.Called = true;
                e.Set();
            }

            public bool Called { get; set; }
        }
    }
}
