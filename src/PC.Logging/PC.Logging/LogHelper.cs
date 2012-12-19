using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;

namespace PebbleCode.Framework.Logging
{
	/// <summary>
	/// Description of LogHelper.
	/// </summary>
	public class LogHelper<T>
		where T : class
	{
		private string _category;
		private string _formatPattern = null;
        private T _obj = null;
		private Dictionary<string, PropertyInfo> _propertyInfos = new Dictionary<string, PropertyInfo>();

		private static Regex _propertyNameRegex = new Regex(@"({\w\w+})", RegexOptions.Compiled);
		private const string NULL_STRING = "<null>";

        public LogHelper(string category, string formatPattern)
            : this(category, formatPattern, null)
        {
        }

		/// <summary>
		/// Constructs a log helper. 
		/// </summary>
		/// <param name="category">The logging category to check before logging messages</param>
		/// <param name="formatPattern">Format string to prepend to message, containing 
		/// property names of type T, e.g. "MARKET-ID,BFID:{Id},{BetFairId}"</param>
        public LogHelper(string category, string formatPattern, T obj)
		{
			_category = category;
			_formatPattern = formatPattern;
            _obj = obj;
			
			//Find {PropertyNames} in the format message.
			MatchCollection  colMatches = _propertyNameRegex.Matches(formatPattern);
			
			//Look up and cache the PropertyInfos
			foreach (Match match in colMatches)
			{
				PropertyInfo pi = null;
				string substitutionMarker = match.Value;
				string propertyname = substitutionMarker.TrimStart('{').TrimEnd('}');
				
				if (! _propertyInfos.ContainsKey(substitutionMarker))
				{
					pi = typeof(T).GetProperty(propertyname);
					
					//Store it, null or otherwise
					_propertyInfos.Add(substitutionMarker, pi);
					if (pi == null)
					{
						//TODO: Log warning
					}
				}
				else
				{
					pi = _propertyInfos[substitutionMarker];
				}
			}
		}

        /// <summary>
        /// Get or set the current logged object
        /// </summary>
        public T Current
        {
            get { return _obj; }
            set { _obj = value; }
        }

		/// <summary>
		/// Logs a debug message, if the category is enabled
		/// </summary>
		/// <param name="message"></param>
        public string LogDebug(string message)
		{
            return LogDebug(message, _obj);
		}
		
		/// <summary>
		/// Logs a debug message, if the category is enabled, building a formatted preamble of parameter T
		/// </summary>
		/// <param name="message"></param>
        public string LogDebug(string message, T obj)
		{
            message = FormatMessage(message, obj);
            if (Logger.ShouldLogDebug(_category))
			{
				Logger.WriteDebug(message, _category);
			}
            return message;
        }

        /// <summary>
        /// Logs an error message, if the category is enabled
        /// </summary>
        /// <param name="message"></param>
        public string LogError(string message)
        {
            return LogError(message, _obj);
        }

        /// <summary>
        /// Logs an error message, if the category is enabled, building a formatted preamble of parameter T
        /// </summary>
        /// <param name="message"></param>
        public string LogError(string message, T obj)
        {
            message = FormatMessage(message, obj);
            if (Logger.ShouldLogError(_category))
            {
                Logger.WriteError(message, _category);
            }
            return message;
        }

        /// <summary>
		/// Logs a warning message, if the category is enabled
		/// </summary>
		/// <param name="message"></param>
		public string LogWarning(string message)
		{
            return LogWarning(message, _obj);
		}
		
		/// <summary>
		/// Logs a warning message, if the category is enabled, building a formatted preamble of parameter T
		/// </summary>
		/// <param name="message"></param>
		public string LogWarning(string message, T obj)
		{
            message = FormatMessage(message, obj);
            if (Logger.ShouldLogWarning(_category))
			{
				Logger.WriteWarning(message, _category);
			}
            return message;
        }
		
		/// <summary>
		/// Logs an info message, if the category is enabled
		/// </summary>
		/// <param name="message"></param>
		public string LogInfo(string message)
		{
            return LogInfo(message, _obj);
		}
		
		/// <summary>
		/// Logs an info message, if the category is enabled, building a formatted preamble of parameter T
		/// </summary>
		/// <param name="message"></param>
		public string LogInfo(string message, T obj)
		{
            message = FormatMessage(message, obj);
            if (Logger.ShouldLogInfo(_category))
			{
				Logger.WriteInfo(message, _category);
			}
            return message;
		}

        /// <summary>
        /// Logs a debug message, if the category is enabled, building a formatted preamble of parameter T
        /// </summary>
        /// <param name="message"></param>
        public void LogUnexpectedException(Exception ex, string message, T obj)
        {
            //Format the string with object properties?
            message = FormatMessage(message, obj);
            Logger.WriteUnexpectedException(ex, message, _category);
        }

        /// <summary>
		/// Writes an unexpected exception to the log
		/// </summary>
		/// <param name="message"></param>
		public void LogUnexpectedException(Exception ex, string message)
		{
            LogUnexpectedException(ex, message, _obj);
		}
		
		/// <summary>
		/// Creates preamble formatted string.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="obj"></param>
		/// <returns></returns>
		public string FormatMessage(string message, T obj)
		{
            // No obj, just return message
			if (obj == null)
                return message;

            try
            {
                Dictionary<string, string> substitutionValues = new Dictionary<string, string>();

                foreach (KeyValuePair<string, PropertyInfo> pair in _propertyInfos)
                {
                    //If we found a property, get its value for this object
                    if (pair.Value != null)
                    {
                        //Evaluate property on this object
                        object objValue = pair.Value.GetValue(obj, null);
                        if (objValue != null)
                        {
                            substitutionValues.Add(pair.Key, objValue.ToString());
                        }
                        else
                        {
                            substitutionValues.Add(pair.Key, NULL_STRING);
                        }
                    }
                }

                //Format the string
                StringBuilder formattedMessage = new StringBuilder(_formatPattern);
                foreach (KeyValuePair<string, string> pair in substitutionValues)
                {
                    formattedMessage.Replace(pair.Key, pair.Value);
                }
                formattedMessage.AppendFormat(" {0}", message);

                return formattedMessage.ToString();
            }
            catch (Exception ex)
            {
                return string.Format("Failed to build exception message: " + ex.Message);
            }
		}
	}
}
