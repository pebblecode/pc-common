using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PebbleCode.Framework.Configuration
{
    public class ApplicationSettings
    {
        /// <summary>
        /// Get the name of the Environment (e.g. Dev,Prod,QA,Staging)
        /// </summary>
        public static string Environment
        {
            get
            {
                return _environment ?? (_environment =
                    ApplicationSettingsHelper.LoadAppSetting(
                        "app.environment",
                        false,
                        DEFAULT_ENVIRONMENT));
            }
        }

        private static string _environment;
        private const string DEFAULT_ENVIRONMENT = "development";

        /// <summary>
        /// Get version of the app
        /// </summary>
        public static string Version
        {
            get
            {
                return _version ?? (_version =
                    ApplicationSettingsHelper.LoadAppSetting(
                        "app.version",
                        false,
                        DEFAULT_VERSION));
            }
        }

        private static string _version;
        private const string DEFAULT_VERSION = "DEV_BUILD";
    }
}
