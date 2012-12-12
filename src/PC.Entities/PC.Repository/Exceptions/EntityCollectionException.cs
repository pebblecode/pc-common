using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Text;
using System.Collections.ObjectModel;

namespace PebbleCode.Repository.Exceptions
{
    /// <summary>
    /// Base class for any exception which requires a list of errors to be associated with it
    /// </summary>
    [Serializable]
    public abstract class EntityCollectionException : Exception
    {
        private Collection<EntityDescriptor> _entities = new Collection<EntityDescriptor>();

        /// <summary>
        /// The current list of entities associated with this exception
        /// </summary>
        public Collection<EntityDescriptor> Entities { get { return _entities; } }

        /// <summary>
        /// Contsructor
        /// </summary>
        /// <param name="message">The system message to use on the exception</param>
        protected EntityCollectionException(string message)
            : this(message, null, null)
        { }

        /// <summary>
        /// Contsructor
        /// </summary>
        /// <param name="message">The system message to use on the exception</param>
        /// <param name="innerException">The lower level exception</param>
        protected EntityCollectionException(string message, Exception innerException)
            : this(message, null, innerException)
        { }

        /// <summary>
        /// Contsructor
        /// </summary>
        /// <param name="message">The system message to use on the exception</param>
        /// <param name="entity">An entity descriptor to prime the collection with. Useful when there is only one entity. 
        /// If there is more than one entity then add them via the Entities property</param>
        protected EntityCollectionException(string message, EntityDescriptor entity)
            : this(message, entity, null)
        { }

        /// <summary>
        /// Contsructor
        /// </summary>
        /// <param name="message">The system message to use on the exception</param>
        /// <param name="entity">An entity descriptor to prime the collection with. Useful when there is only one entity. 
        /// If there is more than one entity then add them via the Entities property</param>
        /// <param name="innerException">The lower level exception</param>
        protected EntityCollectionException(string message, EntityDescriptor entity, Exception innerException)
            : base(string.Format(message, entity.EntityType, entity.EntityId), innerException)
        {
            if (entity != null)
                _entities.Add(entity);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="info">Serialization Info</param>
        /// <param name="context">Streaming Context</param>
        protected EntityCollectionException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            // Deserialize any field is required
            _entities = (Collection<EntityDescriptor>)info.GetValue("Entities", typeof(Collection<EntityDescriptor>));
        }

        /// <summary>
        /// Serialization method
        /// </summary>
        /// <param name="info">Serialization Info</param>
        /// <param name="context">Streaming Context</param>
        [System.Security.Permissions.SecurityPermissionAttribute(System.Security.Permissions.SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            if (info == null)
                throw new ArgumentNullException("info");

            // Let the base type serialize its fields
            base.GetObjectData(info, context);

            // Serialize each field
            info.AddValue("Entities", _entities, typeof(Collection<EntityDescriptor>));
        }
    }
}
