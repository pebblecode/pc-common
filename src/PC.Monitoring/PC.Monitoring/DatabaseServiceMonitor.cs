using System;
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
            _connectionString = DatabaseSettings.ConnectionString;
        }


        protected override string ServiceMonitorTypeName
        {
            get { return "DatabaseServiceMonitor"; }
        }

        protected override IAsyncResult BeginServiceCheck()
        {
            var db = new Database { ConnectionString = _connectionString };
            Action<Database> serviceCheck = CheckMySqlIsAvailable;
            return serviceCheck.BeginInvoke(db, null, db);
        }

        private void CheckMySqlIsAvailable(Database db)
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
