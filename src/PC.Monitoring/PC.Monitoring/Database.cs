using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace PebbleCode.Monitoring
{
    /// <summary>
    /// Class used to count how many databases are currently on a given machine using the supplied connection string
    /// </summary>
    public class Database
    {
        public string ConnectionString { get; set; }

        public Exception LastException { get; private set; }

        public int CountDatabases()
        {
            try
            {
                using (var connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (MySqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = "SHOW DATABASES";
                        command.CommandType = CommandType.Text;
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            int count = 0;
                            while (reader.Read())
                                count++;

                            return count;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LastException = ex;
            }

            return 0;
        }
    }
}
