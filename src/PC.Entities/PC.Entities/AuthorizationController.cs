using System;
using System.Collections.Generic;
using System.Reflection;

namespace PebbleCode.Entities
{
    /// <summary>
    /// Wraps the string -> string dictionary contained with a ControlledUpdateEntity
    /// class, providing simple functions to establish whether a particular UpdateContext
    /// has sufficient weight to overwrite an entity attribute
    /// </summary>
    /// <typeparam name="UpdateContextConstants"></typeparam>
    [Serializable]
    public class AuthorizationAdapter<UpdateContextConstants> : IAuthorizationController
    {
        private readonly Dictionary<string, string> _authz;
        public AuthorizationAdapter(Dictionary<string, string> authz)
        {
            _authz = authz;
        }

        /// <summary>
        /// Update is possible for upper or equal authorization level,
        /// Level with negative value (e.g. migration) will override any previous levels
        /// </summary>
        /// <param name="contextName"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public bool CanUpdate(string contextName, string propertyName)
        {
            if (_authz == null || !_authz.ContainsKey(propertyName)) return true;

            //Find out level of last updater
            int lastUpdateLevel = GetAuthorisationLevel(_authz[propertyName]);

            //Get level of this user
            int updaterLevel = GetAuthorisationLevel(contextName);

            //Allow Migration (-1) to override any levels
            return updaterLevel >= lastUpdateLevel || updaterLevel < 0;
        }

        /// <summary>
        /// Setting the last updated authorization level
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="contextName"></param>
        public void IndicateUpdated(string propertyName, string contextName)
        {
            if (_authz == null || string.IsNullOrEmpty(propertyName)) return;
            //Skip updating the auth level for negative value context (e.g. migration) 
            if (GetAuthorisationLevel(contextName) < 0) return;

            if (_authz.ContainsKey(propertyName))
                _authz[propertyName] = contextName;
            else
                _authz.Add(propertyName, contextName);
        }

        /// <summary>
        /// Get the authorization level from the UpdateContextConstants configuration
        /// </summary>
        /// <param name="contextName"></param>
        /// <returns></returns>
        private int GetAuthorisationLevel(string contextName)
        {
            FieldInfo fieldInfo = typeof(UpdateContextConstants).GetField(contextName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
            return fieldInfo != null ? (int) fieldInfo.GetValue(null) : 0;
        }
    }
}
