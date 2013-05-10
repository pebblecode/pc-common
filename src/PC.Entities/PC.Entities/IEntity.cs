using System;
using System.Collections.Generic;
using System.Text;

namespace PebbleCode.Entities
{
    /// <summary>
    /// Interface for all Business Entities
    /// </summary>
    public interface IEntity<TPrimaryKey>
        where TPrimaryKey : IComparable
    {
        /// <summary>
        /// Identity is the xxxxID of the business entity as used in the database
        /// It uniquely identifies the objects from any other. The PK of the business
        /// entity usually maps to this field. Derived classes must implement the 
        /// GetIdentity method to allow the base class to manage IsNew?
        /// </summary>
        TPrimaryKey Identity { get; }

        /// <summary>
        /// Specifies if the Business Entity is new.
        /// </summary>
        bool IsNew { get; }

        /// <summary>
        /// Specifies if the Business Entity has been altered since retrieval from the data
        /// store.
        /// </summary>
        bool IsChanged { get; }

        /// <summary>
        /// Specifies if the Business Entity is flagged for deletion.
        /// </summary>
        bool IsDeleted { get; }

        /// <summary>
        /// override object.Equals
        /// </summary>
        /// <param name="obj">The object to compare to this</param>
        /// <returns>True if equal otherwise false</returns>
        bool Equals(object obj);

        /// <summary>
        /// override object.GetHashCode
        /// </summary>
        /// <returns>The hash code of the inner object</returns>
        int GetHashCode();

        /// <summary>
        /// Flags the Business Entity for deletion from the underlying data store.
        /// </summary>
        void MarkForDeletion();

        /// <summary>
        /// Explicitly flag the Business Entity as new.
        /// </summary>
        void MarkAsNew();

        /// <summary>
        /// Explicitly flags the Business Entity as altered.
        /// </summary>
        void MarkAsChanged();

        /// <summary>
        /// Resets the Deleted and Changed flags.
        /// </summary>
        void Reset();

        /// <summary>
        /// Clone method. Force all business entities to be clonable
        /// </summary>
        /// <returns></returns>
        object Clone();
    }
}
