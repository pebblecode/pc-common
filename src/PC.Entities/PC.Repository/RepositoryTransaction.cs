using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBatisNet.DataMapper;

namespace PebbleCode.Repository
{

    /// <summary>
    /// Wraps all proceeding calls, on current thread, to repo methods in a transaction until Commit or 
    /// Rollback is called or until RepositoryTransaction is disposed.
    /// </summary>
    public class RepositoryTransaction : IDisposable
    {
        private ISqlMapper _mapper;
        private bool _finished = false;

        public RepositoryTransaction()
        {
        }

        /// <summary>
        /// Called from EntityRepository.BeginTransaction
        /// </summary>
        /// <param name="mapper"></param>
        internal RepositoryTransaction(ISqlMapper mapper)
        {
            // Check the state of the mapper
            if (mapper.IsSessionStarted)
                throw new InvalidOperationException("Dispose all other RepositoryTransactions first");

            _mapper = mapper;
            _mapper.BeginTransaction();
        }

        /// <summary>
        /// Rollback data
        /// </summary>
        public virtual void Rollback()
        {
            if (_finished)
                throw new InvalidOperationException("Transaction already rolled back or committed");

            _mapper.RollBackTransaction();
            _finished = true;
        }

        /// <summary>
        /// Commit data
        /// </summary>
        public virtual void Commit()
        {
            if (_finished)
                throw new InvalidOperationException("Transaction already rolled back or committed");

            _mapper.CommitTransaction();
            _finished = true;
        }

        /// <summary>
        /// Commit data
        /// </summary>
        public virtual void Dispose()
        {
            if (!_finished)
                Rollback();
        }
    }
}
