using System;
using System.Collections.Generic;
using System.Text;

namespace PebbleCode.Repository.Exceptions
{
    /// <summary>
    /// Exception for when business entities are not able to be returned when expected.
    /// </summary>
    [Serializable]
    public class EntityNotFoundException<TPrimaryKey> : EntityCollectionException<TPrimaryKey>
        where TPrimaryKey : IComparable
    {
        private const string SYSTEM_MESSAGE = "Entity '{0}' with an id of '{1}' can not be found.";

        /// <summary>
        /// Constructor
        /// </summary>
        public EntityNotFoundException()
            : this(null, null)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entity">An entity descriptor to prime the collection with. Useful when there is only one entity. 
        /// If there is more than one entity then add them via the Entities property</param>
        public EntityNotFoundException(EntityDescriptor<TPrimaryKey> entity)
            : this(entity, null)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="entity">An entity descriptor to prime the collection with. Useful when there is only one entity. 
        /// If there is more than one entity then add them via the Entities property</param>
        /// <param name="innerException">The lower level exception</param>
        public EntityNotFoundException(EntityDescriptor<TPrimaryKey> entity, Exception innerException)
            : base(SYSTEM_MESSAGE, entity, innerException)
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info">Serialization Info</param>
        /// <param name="context">Streaming Context</param>
        protected EntityNotFoundException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        { }
    }
}
