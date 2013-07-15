using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
namespace PebbleCode.Framework.Configuration
{
    /// <summary>
    /// Encapsulated access to database application settings
    /// </summary>
    public static class DatabaseSettings
    {
        /// <summary>
        /// Get the DB ConnectionString
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                if (_connectionString == null)
                {
                    _connectionString =
                        string.Format(
                        ApplicationSettingsHelper.LoadAppSetting<string>(
                            "connectionString",
                            true,
                            string.Empty),
                            DatabaseHost,
                            DatabaseName,
                            DatabaseUsername,
                            DatabasePassword);
                }
                return _connectionString;
            }
        }

        private static string _connectionString = null;

        /// <summary>
        /// Get the DB hostname
        /// </summary>
        public static string DatabaseHost
        {
            get
            {
                if (_databaseHost == null)
                {
                    _databaseHost =
                        ApplicationSettingsHelper.LoadAppSetting<string>(
                            "db.host",
                            false,
                            "localhost");
                }
                return _databaseHost;
            }
        }
        private static string _databaseHost = null;

        /// <summary>
        /// Get the catalog name
        /// </summary>
        public static string DatabaseName
        {
            get
            {
                if (_databaseName == null)
                {
                    _databaseName =
                        ApplicationSettingsHelper.LoadAppSetting<string>(
                            "db.name",
                            false,
                            "contrarius");
                }
                return _databaseName;
            }
            set
            {
                _databaseName = value;
            }
        }
        private static string _databaseName = null;

        /// <summary>
        /// Get the port to connect on
        /// </summary>
        public static string DatabasePort
        {
            get
            {
                if (_databasePort == null)
                {
                    _databasePort =
                        ApplicationSettingsHelper.LoadAppSetting<string>(
                            "db.port",
                            false,
                            "3306");
                }
                return _databasePort;
            }
        }
        private static string _databasePort = null;

        /// <summary>
        /// Get the user account name
        /// </summary>
        public static string DatabaseUsername
        {
            get
            {
                if (_databaseUsername == null)
                {
                    _databaseUsername =
                        ApplicationSettingsHelper.LoadAppSetting<string>(
                            "db.user",
                            false,
                            "contrarius");
                }
                return _databaseUsername;
            }
        }
        private static string _databaseUsername = null;

        /// <summary>
        /// Whether to use compression
        /// </summary>
        public static string DatabaseCompression
        {
            get
            {
                if (_databaseCompression == null)
                {
                    _databaseCompression =
                        ApplicationSettingsHelper.LoadAppSetting<string>(
                            "db.compression",
                            false,
                            "false");
                }
                return _databaseCompression;
            }
        }
        private static string _databaseCompression = null;

        /// <summary>
        /// Get the user account name
        /// </summary>
        public static string DatabasePassword
        {
            get
            {
                if (_databasePassword == null)
                {
                    _databasePassword =
                        ApplicationSettingsHelper.LoadAppSetting<string>(
                            "db.password",
                            false,
                            "c0ntrar1us");
                }
                return _databasePassword;
            }
        }
        private static string _databasePassword = null;






        /// <summary>
        /// Get assembly in which the SqlMap lives
        /// </summary>
        public static string SqlMap_Assembly
        {
            get
            {
                return _sqlMap_Assembly ?? 
                    (_sqlMap_Assembly = ApplicationSettingsHelper.LoadAppSetting(
                        "SqlMap.Assembly",
                        true,
                        (string)null));
            }
        }

        private static string _sqlMap_Assembly;

        /// <summary>
        /// Get name of the SqlMap
        /// </summary>
        public static string SqlMap_Resource
        {
            get
            {
                return _sqlMap_Resource ??
                    (_sqlMap_Resource = ApplicationSettingsHelper.LoadAppSetting(
                        "SqlMap.Resource",
                        true,
                        (string)null));
            }
        }

        private static string _sqlMap_Resource;
    }
}
