using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace PebbleCode.Framework.Logging
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DontLogAttribute : Attribute
    {
    }

    /// <summary>
    /// Creats instances of DataContractLogHelper on the fly so you can log many different types of object
    /// without having to construct a new instance each time.
    /// </summary>
    public class DataContractLogger
    {
        private Dictionary<string, DataContractLogHelper> _loggers = new Dictionary<string, DataContractLogHelper>();
        private string _category;
        private string _typeNamespacePrefix = string.Empty;

        /// <summary>
        /// Construct with a category
        /// </summary>
        public DataContractLogger(string category, string typeNamespacePrefix)
        {
            _category = category;
            if (!string.IsNullOrEmpty(typeNamespacePrefix))
                _typeNamespacePrefix = typeNamespacePrefix + ".";
        }

        /// <summary>
        /// Log a contract
        /// </summary>
        public void Log(object dataContract)
        {
            Log(dataContract, _category);
        }

        /// <summary>
        /// Log a contract, overriding the category
        /// </summary>
        public void Log(object dataContract, string category)
        {
            Type type = dataContract.GetType();

            // Only required types please.
            if (!IsRequiredType(type))
                throw new InvalidOperationException(string.Format("Only types in the namespace '{0}' are valid here", _typeNamespacePrefix));

            // Make sure we have a logger for this type.
            if (!_loggers.ContainsKey(type.Name))
                _loggers.Add(type.Name, new DataContractLogHelper(this, type, _category, 0));

            // Log it
            _loggers[type.Name].Log(dataContract, category);
        }

        public bool IsRequiredType(Type type)
        {
            return type.IsClass && type.Namespace.StartsWith(_typeNamespacePrefix);
        }
    }

    /// <summary>
    /// Log objects based on properties that have ObjectLoggerAttribute attribute
    /// </summary>
    public class DataContractLogHelper
    {
        private Dictionary<string, DataContractLogHelper> _nestedHelpers = new Dictionary<string, DataContractLogHelper>();
        private IEnumerable<PropertyInfo> _propertys;
        private string _category;
        private string _contractName;
        private string _indent;
        private int _level;
        private DataContractLogger _logger;

        /// <summary>
        /// Constructs a log helper, based on attributes in the type T
        /// </summary>
        /// <param name="category">The logging category to check before logging messages</param>
        public DataContractLogHelper(DataContractLogger logger, Type contractType, string category, int level)
        {
            _logger = logger;
            _level = level;
            _indent = new string('\t', level);
            _category = category;
            _contractName = contractType.Name;
            _propertys = contractType
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty)
                .Where(pi => !pi.IsDefined(typeof(DontLogAttribute), false));

            // Build nested LogHelpers to log object trees
            foreach (PropertyInfo pi in _propertys)
            {
                if (_logger.IsRequiredType(pi.PropertyType))
                {
                    _nestedHelpers.Add(pi.Name, new DataContractLogHelper(_logger, pi.PropertyType, category, level + 1));
                }
            }
        }

        /// <summary>
        /// Log a contract
        /// </summary>
        public void Log(object dataContract)
        {
            Log(dataContract, _category);
        }

        /// <summary>
        /// Log a contract
        /// </summary>
        public void Log(object dataContract, string category)
        {
            // Log the top level item. We use INFO if we're level 0, otherwise
            // we're nested info and therefor debug level
            string message = string.Format("{0}{1}:", _indent, _contractName);
            if (_level == 0)
                Logger.WriteInfo(message, category);
            else
                Logger.WriteDebug(message, category);

            // If we have a value, log some more details...
            if (dataContract != null)
            {
                foreach (PropertyInfo property in _propertys)
                {
                    string propName = property.Name;
                    try
                    {
                        object value = property.GetValue(dataContract, null);
                        if (_nestedHelpers.ContainsKey(property.Name))
                            _nestedHelpers[property.Name].Log(value, category);
                        else if (IsEnumerable(property.PropertyType))
                            LogEnumerable(category, property, value);
                        else
                            LogProperty(category, property, value);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("Error rendering {0}: {1}", new {propName, ex.Message}));
                    }
                }
            }
        }

        private bool IsEnumerable(Type type)
        {
            return typeof(string) != type && typeof(IEnumerable).IsAssignableFrom(type);
        }

        private void LogEnumerable(string category, PropertyInfo property, object value)
        {
            if (value != null)
            {
                var list = value as IEnumerable;
                Type type = property.PropertyType.GetGenericArguments()[0];
                if (_logger.IsRequiredType(type))
                {
                    var logger = new DataContractLogHelper(_logger, type, category, _level + 1);
                    foreach (object item in list)
                        logger.Log(item, category);
                }
                else
                {
                    foreach (object item in list)
                        LogProperty(category, property, item);
                }
            }
            else
            {
                LogProperty(category, property, null);
            }
        }

        private void LogProperty(string category, PropertyInfo property, object value)
        {
            Logger.WriteDebug("{0}\t{1}: {2}", category, _indent, property.Name, value);
        }
    }

}
