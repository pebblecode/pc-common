using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PebbleCode.Repository
{
    public class TransactionProvider
    {
        public virtual RepositoryTransaction BeginTransaction()
        {
            return EntityRepository.BeginTransaction();
        }
    }
}
