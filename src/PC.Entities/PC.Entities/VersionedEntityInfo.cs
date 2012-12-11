using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Diagnostics.CodeAnalysis;

namespace PebbleCode.Entities
{
    /// <summary>
    /// Mini data class containing version info
    /// </summary>
    [Serializable]
    public class VersionedEntityInfo
    {
        private int _versionNo = 1;
        private DateTime _versionDate = DateTime.Now;

        /// <summary>
        /// Constructor
        /// </summary>
        public VersionedEntityInfo()
        { }

        /// <summary>
        /// Public accessor for _versionNo
        /// </summary>		
        public int VersionNo
        {
            get { return _versionNo; }
            set { _versionNo = value; }
        }

        /// <summary>
        /// Public accessor for _versionDate
        /// </summary>		
        public DateTime VersionDate
        {
            get { return _versionDate; }
            set { _versionDate = value; }
        }
    }
}
