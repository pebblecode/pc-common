﻿using System;
using System.Configuration;
using PebbleCode.Framework.Utilities;
using System.Collections.Specialized;
using System.Reflection;

namespace PebbleCode.Framework.Configuration
{
    /// <summary>
    /// Helper class for app settings
    /// </summary>
    public static class ApplicationSettingsHelper
    {
        /// <summary>
        /// Helper method to get a value from config
        /// </summary>
        /// <typeparam name="TReturnType"></typeparam>
        /// <param name="name"></param>
        /// <param name="required"></param>
        /// <param name="defaultValue"></param>
        /// <param name="lockObject"></param>
        /// <returns></returns>
        public static TReturnType LoadAppSetting<TReturnType>(string name, bool required, TReturnType defaultValue)
        {
            // Check for missing settings
            if (!ArrayExt.Contains<string>(ConfigurationManager.AppSettings.AllKeys, name))
            {
                if (required)
                    throw new ConfigurationErrorsException(string.Format("Required setting missing: {0}", name));
                else
                    return defaultValue;
            }

            // Read the setting and return it
            AppSettingsReader reader = new AppSettingsReader();
            return (TReturnType)reader.GetValue(name, typeof(TReturnType));
        }


        public static NameValueCollection ListSettings(Type staticSettingsType)
        {
            NameValueCollection output = new NameValueCollection();

            PropertyInfo[] props = staticSettingsType.GetProperties(BindingFlags.Static | BindingFlags.Public | BindingFlags.GetProperty);
            foreach (PropertyInfo pi in props)
            {
                //Execute the static property
                object val = pi.GetValue(null, null);
                output.Add(pi.Name, (val == null) ? "<null>" : val.ToString());
            }
            return output;
        }
    }
}
