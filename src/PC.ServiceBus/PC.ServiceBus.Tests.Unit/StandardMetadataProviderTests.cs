using NUnit.Framework;
using PC.ServiceBus.Messaging.Handling;

namespace PC.ServiceBus.Tests.Unit
{
    [TestFixture]
    public class StandardMetadataProviderTests
    {
        [Test]
        public void GivenAStandardMetadataProvider_WhenGettingMetadata_ThenTypeNamePresent()
        {
            var provider = new StandardMetadataProvider();
            var expected = typeof(StandardMetadataProviderTests).Name;

            var metadata = provider.GetMetadata(this);

            Assert.IsTrue(metadata.Values.Contains(expected));
            Assert.IsTrue(metadata.Keys.Contains(StandardMetadata.TypeName));
        }

        [Test]
        public void GivenAStandardMetadataProvider_WhenGettingMetadata_ThenTypeFullNamePresent()
        {
            var provider = new StandardMetadataProvider();
            var expected = typeof(StandardMetadataProviderTests).FullName;

            var metadata = provider.GetMetadata(this);

            Assert.IsTrue(metadata.Values.Contains(expected));
            Assert.IsTrue(metadata.Keys.Contains(StandardMetadata.FullName));
        }

        [Test]
        public void GivenAStandardMetadataProvider_WhenGettingMetadata_ThenAssemblyFullNamePresent()
        {
            var provider = new StandardMetadataProvider();
            var expected = typeof(StandardMetadataProviderTests).Assembly.GetName().Name;

            var metadata = provider.GetMetadata(this);

            Assert.IsTrue(metadata.Values.Contains(expected));
            Assert.IsTrue(metadata.Keys.Contains(StandardMetadata.AssemblyName));
        }

        [Test]
        public void GivenAStandardMetadataProvider_WhenGettingMetadata_ThenANamespacePresent()
        {
            var provider = new StandardMetadataProvider();
            var expected = typeof(StandardMetadataProviderTests).Namespace;

            var metadata = provider.GetMetadata(this);

            Assert.IsTrue(metadata.Values.Contains(expected));
            Assert.IsTrue(metadata.Keys.Contains(StandardMetadata.Namespace));
        }
    }
}
