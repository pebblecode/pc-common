using System;
using System.Collections.Generic;
using System.Text;

namespace PebbleCode.Repository.Exceptions
{
    /// <summary>
    /// Represents a collection of keys and BusinessEntityVersionCollection.
    /// </summary>
    [Serializable]
    public class EntityDescriptor<TPrimaryKey>        
        where TPrimaryKey : IComparable
    {
        /// <summary>
        /// The BusinessEntity type
        /// </summary>
        private Type _entityType;

        /// <summary>
        /// The BusinessEntity Id
        /// </summary>
        private TPrimaryKey _entityId;

        /// <summary>
        /// The description
        /// </summary>
        private string _message;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityId">The Id of the BusinessEntity</param>
        /// <param name="entityType">A type derived from BusinessEntity</param>
        public EntityDescriptor(TPrimaryKey entityId, Type entityType) 
            : this (entityId, entityType, String.Empty) 
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entityId">The Id of the BusinessEntity</param>
        /// <param name="entityType">A type derived from BusinessEntity</param>
        /// <param name="message">The description of this BusinessEntity</param>
        public EntityDescriptor(TPrimaryKey entityId, Type entityType, string message)
        {
            _entityId = entityId;
            _entityType = entityType;
            _message = message;
        }

        /// <summary>
        /// The Business Entity's Id
        /// </summary>
        public TPrimaryKey EntityId
        {
            get
            {
                return _entityId;
            }
        }

        /// <summary>
        /// The Business Entity's type
        /// </summary>
        public Type EntityType
        {
            get
            {
                return _entityType;
            }
        }

        /// <summary>
        /// The Business Entity's description
        /// </summary>
        public string Message
        {
            get
            {
                return _message;
            }
        }
    }
}
