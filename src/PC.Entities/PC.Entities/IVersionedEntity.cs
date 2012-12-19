using System;
using System.Collections.Generic;
using System.Text;

namespace PebbleCode.Entities
{
    /// <summary>
    /// Interface for all versioned business entities
    /// </summary>
    public interface IVersionedEntity : IEntity
    {
        /// <summary>
        /// Public accessor for _versionNo
        /// </summary>		
        int VersionNo { get; }
        
        /// <summary>
        /// Public accessor for _versionDate
        /// </summary>
        DateTime VersionDate { get; }

        /// <summary>
        /// Resets the Deleted and Changed flags, and also the version and datestamp
        /// </summary>
        void Reset(VersionedEntityInfo info);

    }
}
