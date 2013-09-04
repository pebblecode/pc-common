using System;
using System.Data;

namespace PebbleCode.Monitoring
{
    /// <summary>
    /// Class used to count how many databases are currently on a given machine using the supplied connection string
    /// </summary>
    public abstract class Database
    {
        public string ConnectionString { get; set; }

        public int CountDatabases()
        {
            using (IDbConnection connection = CreateConnection())
            {
                connection.Open();

                using (IDbCommand command = connection.CreateCommand())
                {
                    command.CommandText = GetCommandText();
                    command.CommandType = CommandType.Text;
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        int count = 0;
                        while (reader.Read())
                            count++;

                        return count;
                    }
                }
            }
        }

        protected abstract IDbConnection CreateConnection();
        protected abstract string GetCommandText();

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
