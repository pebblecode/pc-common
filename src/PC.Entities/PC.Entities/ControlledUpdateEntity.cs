using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Bede.Logging.Providers;
using PebbleCode.Framework;
using PebbleCode.Framework.Utilities;

namespace PebbleCode.Entities
{
    /// <summary>
    /// Base class for all controlled update Business Entities
    /// UpdateContextConstants is a generic class which has update context constants are 
    /// context level with negative value will override any records (e.g. migration is an override for all)
    /// </summary>
    /// <typeparam name="UpdateContextConstants"></typeparam>
    [Serializable]
    public abstract class ControlledUpdateEntity<UpdateContextConstants, TPrimaryKey> : VersionedEntity<TPrimaryKey>
        where TPrimaryKey : IComparable
    {
        private Dictionary<string, string> _propertyAuthorization;

        [NonSerialized]
        private IAuthorizationController _autorizationAdapter;

        [NonSerialized]
        private UpdateContext<UpdateContextConstants, TPrimaryKey> _updateContext;
        public UpdateContext<UpdateContextConstants, TPrimaryKey> UpdateContext
        {
            get { return _updateContext; }
            set { _updateContext = value; }
        }

        /// <summary>
        /// Database accessor for _propertyAuthorization. Only used by IBatis.
        /// </summary>
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public string DbPropertyAuthorization
        {
            get { return SerialisationUtils.ToXml(_propertyAuthorization); }
            set
            {
                _propertyAuthorization = value != null ? SerialisationUtils.FromXml<Dictionary<string, string>>(value) : new Dictionary<string, string>();
                _autorizationAdapter = new AuthorizationAdapter<UpdateContextConstants>(_propertyAuthorization);
            }
        }

        private IAuthorizationController PropertyAuthorization
        {
            get
            {
                if (_propertyAuthorization == null)
                {
                    _propertyAuthorization = new Dictionary<string, string>();
                    _autorizationAdapter = new AuthorizationAdapter<UpdateContextConstants>(_propertyAuthorization);
                }
                return _autorizationAdapter;
            }
        }

        /// <summary>
        /// Checking the property is updatable
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        private bool CanUpdateProperty(string propertyName, object newValue)
        {
            if (string.IsNullOrEmpty(propertyName) || UpdateContext == null || _autorizationAdapter == null)
                return true;

            if (!PropertyAuthorization.CanUpdate(UpdateContext.Name, propertyName))
            {
                FieldInfo field = GetType().GetField(propertyName);
                object oldValue = field != null ? field.GetValue(this) : null;
                EntityLogger<TPrimaryKey>.WriteWarning("{0} property cannot be overriden from {1} to {2} by {3} context".fmt(propertyName, oldValue,
                                                                                          newValue, UpdateContext.Name),
                    "EntityUpdate", null);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Set the last updated authorization level on property changed
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (UpdateContext == null)
                throw new Exception("Missing update context");

            PropertyAuthorization.IndicateUpdated(e.PropertyName, UpdateContext.Name);
            base.OnPropertyChanged(e);
        }

        /// <summary>
        /// Checking property is updatable 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        protected override bool PropertyValueChanging(string propertyName, object newValue)
        {
            return CanUpdateProperty(propertyName, newValue) &&
                base.PropertyValueChanging(propertyName, newValue);
        }
    }
}
