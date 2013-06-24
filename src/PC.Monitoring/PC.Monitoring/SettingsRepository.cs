using PebbleCode.Framework.Logging;
using PebbleCode.Framework.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace FB.DataAccess
{
    /// <summary>
    /// Helper class for app settings
    /// </summary>
    public static class SettingsRepository
    {

        /// <summary>
        /// Helper method to get a value from config
        /// </summary>
        public static TReturnType LoadAppSetting<TReturnType>(string name, TReturnType defaultValue)
        {
            // Is it in the config file?
            if (ArrayExt.Contains<string>(ConfigurationManager.AppSettings.AllKeys, name))
            {
                // Read the setting and return it
                AppSettingsReader reader = new AppSettingsReader();
                return (TReturnType)reader.GetValue(name, typeof(TReturnType));
            }

            return defaultValue;
        }
    }
}
