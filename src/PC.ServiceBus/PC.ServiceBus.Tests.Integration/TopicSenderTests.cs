using Microsoft.Practices.TransientFaultHandling;
using Microsoft.ServiceBus.Messaging;
using NUnit.Framework;
using PC.ServiceBus.Configuration;
using PC.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace PC.ServiceBus.Tests.Integration
{
    [TestFixture]
    public class TopicSenderTests : BaseIntegrationTest
    {
        private SubscriptionClient _subscriptionClient;
        private TestableTopicSender _sut;

        [SetUp]
        public void Setup()
        {
            _sut = new TestableTopicSender(ConfigurationManager, Topic, new Incremental(1, TimeSpan.Zero, TimeSpan.Zero));
            var subscriptionName = Guid.NewGuid().ToString();
            ConfigurationManager.NamespaceManager.CreateSubscription(Topic, subscriptionName);

            _subscriptionClient = ConfigurationManager.MessagingFactory.CreateSubscriptionClient(Topic, subscriptionName);
        }

        [Test]
        public void GivenATopicSender_WhenSendingMessageAsync_ThenMessageGetSentSuccessfully()
        {
            var payload = Guid.NewGuid().ToString();

            _sut.SendAsync(() => new BrokeredMessage(payload));

            var message = _subscriptionClient.Receive(TimeSpan.FromSeconds(5));
            Assert.AreEqual(payload, message.GetBody<string>());
        }

        [Test]
        public void GivenATopicSender_WhenSendingBatchMessageAsync_ThenMessageGetSentSuccessfully()
        {
            var payload1 = Guid.NewGuid().ToString();
            var payload2 = Guid.NewGuid().ToString();

            _sut.SendAsync(new Func<BrokeredMessage>[] { () => new BrokeredMessage(payload1), () => new BrokeredMessage(payload2) });

            var messages = new List<string>
            {
                _subscriptionClient.Receive(TimeSpan.FromSeconds(5)).GetBody<string>(),
                _subscriptionClient.Receive(TimeSpan.FromSeconds(2)).GetBody<string>()
            };

            Assert.Contains(payload1, messages);
            Assert.Contains(payload2, messages);
        }

        [Test]
        public void GivenATopicSender_WhenSendingMessageSync_ThenMessageGetSentSuccessfully()
        {
            var payload = Guid.NewGuid().ToString();
            _sut.Send(() => new BrokeredMessage(payload));

            var message = _subscriptionClient.Receive();
            Assert.AreEqual(payload, message.GetBody<string>());
        }

        [Test]
        public void GivenATopicSender_WhenSendingMessageFailsTransientlyOnce_ThenSenderRetriesSending()
        {
            var payload = Guid.NewGuid().ToString();

            var attempt = 0;
            var signal = new AutoResetEvent(false);
            var currentDelegate = _sut.DoBeginSendMessageDelegate;
            _sut.DoBeginSendMessageDelegate =
                (mf, ac, obj) =>
                {
                    if (attempt++ == 0) throw new TimeoutException();
                    var result = currentDelegate(mf, ac, obj);
                    signal.Set();

                    return result;
                };

            _sut.SendAsync(() => new BrokeredMessage(payload));

            var message = _subscriptionClient.Receive(TimeSpan.FromSeconds(5));
            Assert.True(signal.WaitOne(TimeSpan.FromSeconds(5)), "Test timed out");
            Assert.AreEqual(payload, message.GetBody<string>());
            Assert.AreEqual(2, attempt);
        }

        [Test]
        public void GivenATopicSender_WhenSendingMessageFailsTransientlyMultipleTimes_ThenSendingMessageFails()
        {
            var payload = Guid.NewGuid().ToString();

            var currentDelegate = _sut.DoBeginSendMessageDelegate;
            _sut.DoBeginSendMessageDelegate =
                (mf, ac, obj) =>
                {
                    throw new TimeoutException();
                };

            _sut.SendAsync(() => new BrokeredMessage(payload));

            var message = _subscriptionClient.Receive(TimeSpan.FromSeconds(5));
            Assert.Null(message);
        }
    }

    public class TestableTopicSender : TopicSender
    {
        public TestableTopicSender(AzureConfigurationManager configurationManager, string topic, RetryStrategy retryStrategy)
            : base(configurationManager, topic, retryStrategy)
        {
            DoBeginSendMessageDelegate = base.DoBeginSendMessage;
            DoEndSendMessageDelegate = base.DoEndSendMessage;
        }

        public Func<BrokeredMessage, AsyncCallback, object, IAsyncResult> DoBeginSendMessageDelegate;
        public Action<IAsyncResult> DoEndSendMessageDelegate;

        protected override IAsyncResult DoBeginSendMessage(BrokeredMessage messageFactory, AsyncCallback ac, object state)
        {
            return DoBeginSendMessageDelegate(messageFactory, ac, state);
        }

        protected override void DoEndSendMessage(IAsyncResult ar)
        {
            DoEndSendMessageDelegate(ar);
        }
    }
}