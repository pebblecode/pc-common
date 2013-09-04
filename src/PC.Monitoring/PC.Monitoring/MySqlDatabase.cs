using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace PebbleCode.Monitoring
{
    /// <summary>
    /// Class used to count how many MySql databases are currently on a given machine
    /// </summary>
    public class MySqlDatabase : Database
    {
        protected override string GetCommandText()
        {
            return "SHOW DATABASES";
        }

        protected override IDbConnection CreateConnection()
        {
            return new MySqlConnection(ConnectionString);
        }
    }
}
