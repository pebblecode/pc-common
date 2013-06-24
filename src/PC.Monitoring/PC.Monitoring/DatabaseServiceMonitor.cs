using System;

using FB.DataAccess;

using PebbleCode.Framework.Configuration;

namespace PebbleCode.Monitoring
{
    /// <summary>
    /// Asynchronous monitor of a database which tries to count how many databases are on a machine as a healthcheck
    /// </summary>
    public class DatabaseServiceMonitor : AsyncServiceMonitor
    {
        private readonly string _connectionString;

        public DatabaseServiceMonitor(ServiceMonitorConfiguration config)
            : base(config.Name, DatabaseSettings.DatabaseHost, Int32.Parse(config.Settings["timeout"].Value))
        {
            var connectionString = SettingsRepository.LoadAppSetting("connectionString", "Server={0};Database={1};User Id={2};Password={3};");
            var server = SettingsRepository.LoadAppSetting("db.host", "localhost");
            var databaseName = SettingsRepository.LoadAppSetting("db.name", "fb_dev");
            var user = SettingsRepository.LoadAppSetting("db.user", "gambler");
            var password = SettingsRepository.LoadAppSetting("db.password", "g@mbl3r");
            _connectionString = string.Format(connectionString, server, databaseName, user, password);
        }


        protected override string ServiceMonitorTypeName
        {
            get { return "DatabaseServiceMonitor"; }
        }

        protected override IAsyncResult BeginServiceCheck()
        {
            var db = new Database { ConnectionString = _connectionString };
            Action<Database> serviceCheck = CheckDatabaseIsAvailable;
            return serviceCheck.BeginInvoke(db, null, db);
        }

        private void CheckDatabaseIsAvailable(Database db)
        {
            db.TryCountDatabases();
        }

        protected override void EndServiceCheck(IAsyncResult result)
        {
            var db = (Database)result.AsyncState;
            if (db.LastException != null)
                throw db.LastException;
        }
    }
}
