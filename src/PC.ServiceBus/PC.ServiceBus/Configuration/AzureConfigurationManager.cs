using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace PC.ServiceBus.Configuration
{
    /// <summary>
    /// This manager creates the azure service bus helper classes according to the settings in the configuration. Will ensure topics and subscriptions exist
    /// </summary>
    public class AzureConfigurationManager
    {
        private readonly string _azureNamespace;
        private readonly string _issuer;
        private readonly string _key;

        public AzureConfigurationManager()
        {
            _azureNamespace = AzureConfiguration.Instance.Settings["azureNamespace"].Value;
            _issuer = AzureConfiguration.Instance.Settings["issuer"].Value;
            _key = AzureConfiguration.Instance.Settings["key"].Value;
            Topics = AzureConfiguration.Instance.Topics.OfType<topic>().ToList();

            TokenProvider credentials = TokenProvider.CreateSharedSecretTokenProvider(_issuer, _key);
            Uri serviceBusUri = ServiceBusEnvironment.CreateServiceUri("sb", _azureNamespace, string.Empty);

            MessagingFactory = MessagingFactory.Create(serviceBusUri, credentials);
            NamespaceManager = new NamespaceManager(serviceBusUri, credentials);
        }

        public MessagingFactory MessagingFactory { get; private set; }

        public NamespaceManager NamespaceManager { get; private set; }

        public List<topic> Topics { get; private set; }
    }
}
