using System;
using System.Data;
using System.Data.SqlClient;

using Microsoft.Practices.EnterpriseLibrary.WindowsAzure.TransientFaultHandling.SqlAzure;

namespace PebbleCode.Monitoring
{
    /// <summary>
    /// Class used to count how many databases are currently on a given machine using the supplied connection string
    /// </summary>
    public class Database
    {
        public string ConnectionString { get; set; }

        public int CountDatabases()
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT database_id FROM sys.databases";
                    command.CommandType = CommandType.Text;
                    using (var reader = command.ExecuteReader())
                    {
                        int count = 0;
                        while (reader.Read())
                            count++;

                        return count;
                    }
                }
            }
        }

        public void TryCountDatabases()
        {
            try
            {
                CountDatabases();
            }
            catch (Exception ex)
            {
                LastException = ex;
            }
        }

        public Exception LastException { get; private set; }
    }
}
