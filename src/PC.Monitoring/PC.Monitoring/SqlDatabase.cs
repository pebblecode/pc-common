
using System;
using System.Data.SqlClient;

namespace PebbleCode.Monitoring
{
    /// <summary>
    /// Class used to count how many Sql Server databases are currently on a given machine
    /// </summary>
    public class SqlDatabase : Database
    {
        protected override string GetCommandText()
        {
            return "SELECT database_id FROM sys.databases";
        }

        protected override System.Data.IDbConnection CreateConnection()
        {
 	        return new SqlConnection(ConnectionString);
        } 
    }
}
