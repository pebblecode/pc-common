using Bede.Logging.Models;
using Microsoft.Practices.TransientFaultHandling;
using Microsoft.ServiceBus.Messaging;
using NUnit.Framework;
using PC.ServiceBus.Configuration;
using PC.ServiceBus.Messaging;
using System;
using System.Collections.Generic;

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
    }

    public class TestableTopicSender : TopicSender
    {
        public TestableTopicSender(AzureConfigurationManager configurationManager, string topic, RetryStrategy retryStrategy)
            : base(configurationManager, topic, retryStrategy, new NullLogger())
        {
        }

        public Func<BrokeredMessage, AsyncCallback, object, IAsyncResult> DoBeginSendMessageDelegate;
        public Action<IAsyncResult> DoEndSendMessageDelegate;
    }
}