using System;
using FB.DataAccess;
using PebbleCode.Framework.Configuration;
using PebbleCode.Framework.Logging;

namespace PebbleCode.Monitoring
{
    /// <summary>
    /// Asynchronous monitor of a database which tries to count how many databases are on a machine as a healthcheck
    /// </summary>
    public class DatabaseServiceMonitor : ServiceMonitor
    {
        private readonly string _connectionString;
        private bool _useSqlServer;

        public DatabaseServiceMonitor(ServiceMonitorConfiguration config)
            : base(config.Name, DatabaseSettings.DatabaseHost)
        {
            var connectionString = SettingsRepository.LoadAppSetting("connectionString", "Server={0};Database={1};User Id={2};Password={3};");
            var server = SettingsRepository.LoadAppSetting("db.host", "localhost");
            var databaseName = SettingsRepository.LoadAppSetting("db.name", "fb_dev");
            var user = SettingsRepository.LoadAppSetting("db.user", "gambler");
            var password = SettingsRepository.LoadAppSetting("db.password", "g@mbl3r");
            _useSqlServer = UseSqlServer(config);
            _connectionString = string.Format(connectionString, server, databaseName, user, password);
        }

        private bool UseSqlServer(ServiceMonitorConfiguration config)
        {
            Add providerName = config.Settings["providerName"];
            if (providerName != null && !String.IsNullOrEmpty(providerName.Value))
            {
                if (providerName.Value.Equals("MySqlClient", StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }
            }
            return true;
        }

        public override MonitoredServiceStatus GetStatus()
        {
            Database db = CreateDatabase();
            db.TryCountDatabases();

            if (db.LastException == null)
            {
                return MonitoredServiceStatus.CreateStatus(ServiceName);
            }
            else
            {
                Logger.WriteError("{0} error: {1}", Category.Service, ServiceName, db.LastException);
                return MonitoredServiceStatus.CreateErrorStatus(ServiceName);
            }
        }

        private Database CreateDatabase()
        {
            if (_useSqlServer)
            {
                return new SqlDatabase { ConnectionString = _connectionString };
            }
            return new MySqlDatabase { ConnectionString = _connectionString };
        }
    }
}
