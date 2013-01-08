using Moq;
using PebbleCode.Repository;

namespace PC.Entities.Tests.Integration.Repository
{
    public class MockTransactionProvider : TransactionProvider
    {
        public override RepositoryTransaction BeginTransaction()
        {
            return new Mock<RepositoryTransaction>().Object;
        }
    }
}
