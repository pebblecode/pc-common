using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml;
using System.IO;
using System.Runtime.Serialization;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;

using PebbleCode.Framework.Collections;

namespace PebbleCode.Entities
{
    /// <summary>
    /// Base class for all Business Entities
    /// </summary>
    [Serializable]
    public abstract class Entity<TPrimaryKey> : ICloneable, IEntity<TPrimaryKey>, INotifyPropertyChanged
        where TPrimaryKey : IComparable
    {
        /// <summary>
        /// Create a new Entity loosely based on the supplied template
        /// Entity.
        /// </summary>
        /// <param name="template">Existing Entity to use as a template</param>
        protected Entity(Entity<TPrimaryKey> template)
        {
            if (template == null)
                throw new ArgumentNullException("template");

            this._isChanged = template._isChanged;
            this._isDeleted = template._isDeleted;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        protected Entity()
        { }

        /// <summary>
        /// Get the entity type (as a flags)
        /// </summary>
        public abstract Flags EntityFlag { get; }

        /// <summary>
        /// Identity is the xxxxID of the business entity as used in the database
        /// It uniquely identifies the objects from any other. The PK of the business
        /// entity usually maps to this field. Derived classes must implement the 
        /// GetIdentity method to allow the base class to manage IsNew?
        /// </summary>
        public abstract TPrimaryKey Identity { get; }

        /// <summary>
        /// IsNew generally just looks at the Identity to see if it has been set or
        /// not. If not, then the entity is assumed to be new. It is also possible to
        /// specifically mark something as new. This is used during replication to
        /// retain server side Ids while still treating an entity as "new" to the offline
        /// database.
        /// </summary>
        protected bool _isNew;

        /// <summary>
        /// Specifies if the Business Entity is new.
        /// </summary>
        public virtual bool IsNew
        {
            get { return (Convert.ToInt64(Identity) <= 0 || _isNew); }
        }

        /// <summary>
        /// Explicitly flag the Business Entity as new.
        /// </summary>
        public virtual void MarkAsNew()
        {
            _isNew = true;
        }

        /// <summary>
        /// Changed flag indicates if any properties of the object have been set
        /// since it was loaded from the database. Derived classes call the MarkAsChanged
        /// method to indicate that a property has been set.
        /// </summary>
        private bool _isChanged;

        /// <summary>
        /// Specifies if the Business Entity has been altered since retrieval from the data
        /// store.
        /// </summary>
        public virtual bool IsChanged
        {
            get { return _isChanged; }
        }

        /// <summary>
        /// Explicitly flags the Business Entity as altered.
        /// </summary>
        public virtual void MarkAsChanged()
        {
            _isChanged = true;
        }

        protected virtual bool PropertyValueChanging(string propertyName, object newValue)
        {
            return true;
        }

        /// <summary>
        /// Called by all generated value properties to indicate a value has changed.
        /// Allows us to mark entity as changed and fire notifier event
        /// </summary>
        /// <param name="propertyName"></param>
        protected void PropertyValueChanged(string propertyName)
        {
            MarkAsChanged();
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// This MUST be split off as a virtual method in order that RadGridView can do its
        /// thing and intercept the call. Not entirely sure how it works but if this is not
        /// a virtual method the sorting after edits stops working
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        /// <summary>
        /// Provides an opportunity for non-generated code to perform
        /// setup logic at the end of object construction
        /// </summary>
        protected virtual void OnCreated()
        {
        }

        /// <summary>
        /// Event fired when a property value changes
        /// </summary>
        /// <remarks>Part of INotifyPropertyChanged interface</remarks>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Deleted flag is used to control deleting entities from the database. For certain
        /// types of entities you can set the deleted flag and then call Updated on the
        /// data layer and it will delete the object rather than updating it
        /// </summary>
        private bool _isDeleted;

        /// <summary>
        /// Specifies if the Business Entity is flagged for deletion.
        /// </summary>
        public virtual bool IsDeleted
        {
            get { return _isDeleted; }
        }

        /// <summary>
        /// Flags the Business Entity for deletion from the underlying data store.
        /// </summary>
        public virtual void MarkForDeletion()
        {
            _isDeleted = true;
        }

        /// <summary>
        /// Resets the Deleted and Changed flags.
        /// </summary>
        public virtual void Reset()
        {
            _isDeleted = false;
            ResetChanged();
        }

        /// <summary>
        /// Resets the IsChanged flag on this object
        /// </summary>
        public virtual void ResetChanged()
        {
            _isChanged = false;
        }

        /// <summary>
        /// Method provided so that derived classes can request that
        /// base members be copied to the new clone object.
        /// Maybe overridden by child classes where instances of collections etc need to be cloned
        /// </summary>
        /// <param name="clone">The new clone</param>
        protected virtual void OnClone(Entity<TPrimaryKey> clone)
        {
            if (clone == null)
                throw new ArgumentNullException("clone");

            clone._isChanged = this._isChanged;
            clone._isDeleted = this._isDeleted;
        }

        /// <summary>
        /// Abstract clone method. Force all business entities to be clonable
        /// </summary>
        /// <returns></returns>
        public abstract object Clone();

        /// <summary>
        /// Returns a collection of property names to be used in a log message
        /// format string of this object
        /// </summary>
        protected virtual string[] LogProperties
        {
            get
            {
                return new string[] { "Identity" };
            }
        }

        /// <summary>
        /// Adding common attributes
        /// </summary>
        /// <param name="container"></param>
        protected virtual void SetCustomAttributes(IAttributeContainer container) { }

        /// <summary>
        /// Formats a logging string detailing this object.  Override LogProperties to add or remove from this.
        /// </summary>
        /// <returns></returns>
        public string ToLogString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("{0}: [", this.GetType().Name);
            List<string> propertyFormatStrings = new List<string>();
            List<string> propertyNames = new List<string>(LogProperties);

            //Ensure Id (and Version if versioned) are there
            if (!propertyNames.Contains("Identity") && !propertyNames.Contains("Id"))
            {
                propertyNames.Add("Identity");
            }
            if (this is VersionedEntity<TPrimaryKey>)
            {
                if (!propertyNames.Contains("VersionNo"))
                    propertyNames.Add("VersionNo");
                if (!propertyNames.Contains("VersionDate"))
                    propertyNames.Add("VersionDate");
            }

            propertyNames.Sort();
            propertyNames.ForEach(
                (propName) =>
                {
                    //Get the property, and execute it on this object
                    PropertyInfo property = this.GetType().GetProperty(propName, BindingFlags.Public | BindingFlags.Instance);
                    if (property != null)
                    {
                        try
                        {
                            object value = property.GetValue(this, null);
                            propertyFormatStrings.Add(string.Format("{0}:{1}", propName, value));
                        }
                        catch { }
                    }
                });
            sb.Append(string.Join(",", propertyFormatStrings));
            sb.Append("]");

            return sb.ToString();
        }

        /// <summary>
        /// Creates a default ToString implementation using the ToLogString function.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return ToLogString();
        }

        /// <summary>
        /// Default equality comparison relies on hash code implementation
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != this.GetType())
                return false;
            return this.Identity.Equals(((Entity<TPrimaryKey>)obj).Identity);
        }

        /// <summary>
        /// Default hashcode implementation relies on type and entity identifier
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (this.GetType().FullName + Identity.ToString()).GetHashCode();
        }
    }
}