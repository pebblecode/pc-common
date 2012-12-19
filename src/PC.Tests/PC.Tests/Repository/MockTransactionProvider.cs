using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq;
using PebbleCode.Repository;

namespace PebbleCode.Tests
{
    public class MockTransactionProvider : TransactionProvider
    {
        public override RepositoryTransaction BeginTransaction()
        {
            return new Mock<RepositoryTransaction>().Object;
        }
    }
}
