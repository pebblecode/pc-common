using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PebbleCode.Framework.Configuration
{
    public class EmailSettings
    {
        /// <summary>
        /// Get email SMTP host address
        /// </summary>
        public static string Host
        {
            get
            {
                return _host ?? (_host =
                    ApplicationSettingsHelper.LoadAppSetting(
                        "EmailSettings.Host",
                        false,
                        DEFAULT_HOST));
            }
        }

        private static string _host;
        private const string DEFAULT_HOST = "smtp.gmail.com";

        /// <summary>
        /// Get email SMTP port
        /// </summary>
        public static int Port
        {
            get
            {
                return _port ?? (_port =
                    ApplicationSettingsHelper.LoadAppSetting(
                        "EmailSettings.Port",
                        false,
                        DEFAULT_PORT)).Value;
            }
        }

        private static int? _port;
        private const int DEFAULT_PORT = 587;

        /// <summary>
        /// Get email SMTP SSL enabled
        /// </summary>
        public static bool SslEnabled
        {
            get
            {
                return _sslEnabled ?? (_sslEnabled =
                    ApplicationSettingsHelper.LoadAppSetting(
                        "EmailSettings.SslEnabled",
                        false,
                        DEFAULT_SSL_ENABLED)).Value;
            }
        }

        private static bool? _sslEnabled;
        private const bool DEFAULT_SSL_ENABLED = true;

        /// <summary>
        /// Get email SMTP username
        /// </summary>
        public static string Username
        {
            get
            {
                return _username ?? (_username =
                    ApplicationSettingsHelper.LoadAppSetting(
                        "EmailSettings.Username",
                        false,
                        DEFAULT_USERNAME));
            }
        }

        private static string _username;
        private const string DEFAULT_USERNAME = "admin@pebblecode.com";


        /// <summary>
        /// Get email SMTP password
        /// </summary>
        public static string Password
        {
            get
            {
                return _password ?? (_password =
                    ApplicationSettingsHelper.LoadAppSetting(
                        "EmailSettings.Password",
                        false,
                        DEFAULT_PASSWORD));
            }
        }

        private static string _password;
        private const string DEFAULT_PASSWORD = "rebble85";

        /// <summary>
        /// Get email sender display name
        /// </summary>
        public static string DisplayName
        {
            get
            {
                return _displayName ?? (_displayName =
                    ApplicationSettingsHelper.LoadAppSetting(
                        "EmailSettings.Host",
                        false,
                        DEFAULT_DISPLAY_NAME));
            }
        }

        private static string _displayName;
        private const string DEFAULT_DISPLAY_NAME = "PebbleCode";

        public static bool RedirectAllMailToNetDev
        {
            get
            {
                if (_redirectAllMailToNetDev == null)
                {
                    _redirectAllMailToNetDev =
                    ApplicationSettingsHelper.LoadAppSetting(
                        "EmailSettings.RedirectAllMailToNetDev",
                        false,
                        DEFAULT_REDIRECT_MAIL_TO_NETDEV);
                }
                return _redirectAllMailToNetDev.Value;
            }
        }

        private static bool? _redirectAllMailToNetDev = null;
#if DEBUG
        private const bool DEFAULT_REDIRECT_MAIL_TO_NETDEV = true;
#else
        private const bool DEFAULT_REDIRECT_MAIL_TO_NETDEV = false;
#endif
    }
}
