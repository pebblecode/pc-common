using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace PebbleCode.Entities
{
    /// <summary>
    /// Base class for all versioned Business Entities
    /// </summary>
    [Serializable]
    public abstract class VersionedEntity<TPrimaryKey> : Entity<TPrimaryKey>, IVersionedEntity<TPrimaryKey>
        where TPrimaryKey : IComparable
    {
        private int _versionNo = 0;                         // Not saved yet
        private DateTime _versionDate = DateTime.MinValue;  // Not saved yet

        /// <summary>
        /// Create a new Entity loosely based on the supplied template Entity.
        /// </summary>
        /// <param name="template">Existing Entity to use as a template</param>
        protected VersionedEntity(Entity<TPrimaryKey> template)
            : base(template)
        {
            if (template == null)
                throw new ArgumentNullException("template");
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected VersionedEntity()
        { }

        /// <summary>
        /// Public accessor for _versionNo
        /// </summary>		
        public int VersionNo
        {
            get { return _versionNo; }
        }

        /// <summary>
        /// Public accessor for _versionDate
        /// </summary>
        public DateTime VersionDate
        {
            get { return _versionDate; }
        }

        /// <summary>
        /// Database accessor for _versionNo. Only used by IBatis.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal int DbVersionNo
        {
            get { return _versionNo; }
            set { _versionNo = value; }
        }

        /// <summary>
        /// Database accessor for _versionDate. Only used by IBatis.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal DateTime DbVersionDate
        {
            get { return _versionDate; }
            set { _versionDate = value; }
        }

        /// <summary>
        /// Resets the Deleted and Changed flags, and also the version and datestamp
        /// </summary>
        public void Reset(VersionedEntityInfo info)
        {
            _versionNo = info.VersionNo;
            _versionDate = info.VersionDate;
            base.Reset();
        }

        /// <summary>
        /// Method provided so that derived classes can request that base members be copied to the new 
        /// clone object. Maybe overridden by child classes where instances of collections etc need to be cloned
        /// </summary>
        /// <param name="clone">The new clone</param>
        protected override void OnClone(Entity<TPrimaryKey> clone)
        {
            if (clone == null)
                throw new ArgumentNullException("clone");

            VersionedEntity<TPrimaryKey> versionedEntity = clone as VersionedEntity<TPrimaryKey>;
            if (versionedEntity == null)
                throw new ArgumentException("clone is not VersionedEntity", "clone");

            versionedEntity._versionNo = this._versionNo;
            versionedEntity._versionDate = this._versionDate;
        }
    }
}
