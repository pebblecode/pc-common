using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;

using MyMeta;
using MySql.Data.MySqlClient;

namespace MyMeta.Plugins
{
    public class RedSkyMySqlPlugin : IMyMetaPlugin
    {
        #region IMyMetaPlugin Interface

        private IMyMetaPluginContext context;

        void IMyMetaPlugin.Initialize(IMyMetaPluginContext context)
        {
            this.context = context;
        }

        string IMyMetaPlugin.ProviderName
        {
            get { return @"RedSkyMySql 1.0"; }
        }

        string IMyMetaPlugin.ProviderUniqueKey
        {
            get { return @"REDSKYMYSQL"; }
        }

        string IMyMetaPlugin.ProviderAuthorInfo
        {
            get { return @"RedSkyMySql 1.0MyMeta Plugin Written by Red Sky Software"; }
        }

        Uri IMyMetaPlugin.ProviderAuthorUri
        {
            get { return new Uri(@"http://www.redskysoftware.com/"); }
        }

        bool IMyMetaPlugin.StripTrailingNulls
        {
            get { return false; }
        }

        bool IMyMetaPlugin.RequiredDatabaseName
        {
            get { return false; }
        }

        string IMyMetaPlugin.SampleConnectionString
        {
            get { return @"Database=MyDb;Data Source=MyServer;User Id=MyUsername;Password=MyPassword;"; }
        }

        IDbConnection IMyMetaPlugin.NewConnection
        {
            get
            {
                if (IsIntialized)
                {

                    MySqlConnection cn = new MySqlConnection(this.context.ConnectionString);
                    return cn as IDbConnection;
                }
                else
                    return null;
            }
        }

        string IMyMetaPlugin.DefaultDatabase
        {
            get
            {
                return this.GetDatabaseName();
            }
        }

        /// <summary>
        /// Get all databases
        /// </summary>
        DataTable IMyMetaPlugin.Databases
        {
            get
            {
                DataTable metaData = context.CreateDatabasesDataTable();

                DataSet databases = ExecuteCommand("SHOW DATABASES");

                foreach (DataRow database in databases.Tables[0].Rows)
                {

                    DataRow resultRow = metaData.NewRow();
                    metaData.Rows.Add(resultRow);

                    resultRow["CATALOG_NAME"] = database[0];
                    //resultRow["DESCRIPTION"] = database[0];
                }
                
                return metaData;
            }
        }

        /// <summary>
        /// Get all tables
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        DataTable IMyMetaPlugin.GetTables(string database)
        {
            DataTable metaData = context.CreateTablesDataTable();

            DataSet databases = ExecuteCommand("SHOW TABLES");

            foreach (DataRow table in databases.Tables[0].Rows)
            {

                DataRow resultRow = metaData.NewRow();
                metaData.Rows.Add(resultRow);

                resultRow["TABLE_NAME"] = table[0];
                //resultRow["DESCRIPTION"] = table[0];
            }

            return metaData;
        }

        DataTable IMyMetaPlugin.GetViews(string database)
        {
            return new DataTable();
        }

        /// <summary>
        /// Get all procedures
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        DataTable IMyMetaPlugin.GetProcedures(string database)
        {
            DataTable metaData = context.CreateProceduresDataTable();

            DataSet databases = ExecuteCommand(
                "select " + 
                    "name, " +
                    "returns, " +
                    "created, " +
                    "modified, " +
                    "comment " +
                 "from " +
                    "mysql.proc " +
                 "where " +
                    "db = '" + database + "'");

            foreach (DataRow table in databases.Tables[0].Rows)
            {

                DataRow resultRow = metaData.NewRow();
                metaData.Rows.Add(resultRow);

                resultRow["PROCEDURE_CATALOG"] = database;
                resultRow["PROCEDURE_SCHEMA"] = database;
                resultRow["PROCEDURE_NAME"] = table["name"];
                resultRow["PROCEDURE_TYPE"] = DBNull.Value; // table["returns"];
                resultRow["PROCEDURE_DEFINITION"] = DBNull.Value;
                resultRow["DESCRIPTION"] = table["comment"];
                resultRow["DATE_CREATED"] = table["created"];
                resultRow["DATE_MODIFIED"] = table["modified"];
            }

            return metaData;
        }

        DataTable IMyMetaPlugin.GetDomains(string database)
        {
            return new DataTable();
        }

        /// <summary>
        /// Get params for a procedure
        /// </summary>
        /// <param name="database"></param>
        /// <param name="procedure"></param>
        /// <returns></returns>
        DataTable IMyMetaPlugin.GetProcedureParameters(string database, string procedure)
        {
            DataTable metaData = context.CreateParametersDataTable();

            DataSet parameterDs = ExecuteCommand(
                "select " +
                    "param_list " +
                 "from " +
                    "mysql.proc " +
                 "where " +
                    "db = '" + database + "' " +
                    "AND " +
                    "name = '" + procedure + "'");

            
            
            // Grab the params
            byte[] paramBlob = parameterDs.Tables[0].Rows[0][0] as byte[];
            string paramString = Encoding.ASCII.GetString(paramBlob);
            //paramString = paramString.Replace("\r", string.Empty);
            //paramString = paramString.Replace("\n", string.Empty);
            //paramString = paramString.Replace("\t", string.Empty);
            string[] parameters = paramString.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            int position = 1;

            foreach (string param in parameters)
            {
                string parameter = param.Trim();

                // From MySQL Docs:
                // proc_parameter:
                //     [ IN | OUT | INOUT ] param_name type

                // Split param into parts
                string[] parts = parameter.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length < 2)
                    continue;

                // Process parts
                int partIndex = 0;

                // Direction?
                string direction = "IN";
                if (parts[partIndex] == "IN" || parts[partIndex] == "OUT" || parts[partIndex] == "INOUT")
                    direction = parts[partIndex++];

                // Next is name
                string paramName = parts[partIndex++].Trim();

                // All the rest is the paramType.
                // Add them all together to get rid of spaces
                string paramType = parts[partIndex++];
                for ( ; partIndex < parts.Length; partIndex++)
                    paramType = parts[partIndex];

                // Now process the name, direction and type...

                // Create a result row
                DataRow resultRow = metaData.NewRow();
                metaData.Rows.Add(resultRow);

                // Populate the result row
                resultRow["PROCEDURE_CATALOG"] = DBNull.Value;
                resultRow["PROCEDURE_SCHEMA"] = database;
                resultRow["PROCEDURE_NAME"] = procedure;
                resultRow["PARAMETER_NAME"] = paramName;
                resultRow["ORDINAL_POSITION"] = position++;
                resultRow["PARAMETER_TYPE"] =  ConvertDirectionToParamDirection(direction);
                resultRow["PARAMETER_HASDEFAULT"] = false;
                resultRow["PARAMETER_DEFAULT"] = DBNull.Value;
                resultRow["IS_NULLABLE"] = false;
                resultRow["DATA_TYPE"] = ConvertParameterTypeToDbType(paramType);
                resultRow["CHARACTER_MAXIMUM_LENGTH"] = DBNull.Value;
                resultRow["CHARACTER_OCTET_LENGTH"] = DBNull.Value;
                resultRow["NUMERIC_PRECISION"] = DBNull.Value;
                resultRow["NUMERIC_SCALE"] = DBNull.Value;
                resultRow["DESCRIPTION"] = DBNull.Value;
                resultRow["TYPE_NAME"] = ConvertParameterTypeToTypeName(paramType);
                resultRow["LOCAL_TYPE_NAME"] = paramType;
            }

            return metaData;
        }

        private DbType ConvertParameterTypeToDbType(string paramType)
        {
            switch (paramType.ToLower())
            {
                case "int": return DbType.Int32;
                case "double": return DbType.Double;
                case "float": return DbType.Double;
                case "bit": return DbType.Boolean;
                case "bool": return DbType.Boolean;
                case "tinyblob": return DbType.Binary;
                default:
                    return DbType.Object;
            }
        }

        private string ConvertParameterTypeToTypeName(string paramType)
        {
            if (paramType.ToLower().StartsWith("char("))
                return "CHAR";
            else if (paramType.ToLower().StartsWith("varchar("))
                return "VARCHAR";
            return paramType;
        }

        private ParamDirection ConvertDirectionToParamDirection(string direction)
        {
            switch (direction.ToUpper())
            {
                case "IN": return ParamDirection.Input;
                case "OUT": return ParamDirection.Output;
                case "INOUT": return ParamDirection.InputOutput;
                default:
                    return ParamDirection.Input;
            }
        }

        DataTable IMyMetaPlugin.GetProcedureResultColumns(string database, string procedure)
        {
            return new DataTable();
        }

        DataTable IMyMetaPlugin.GetViewColumns(string database, string view)
        {
            return new DataTable();
        }

        DataTable IMyMetaPlugin.GetTableColumns(string database, string table)
        {
            return new DataTable();
        }

        List<string> IMyMetaPlugin.GetPrimaryKeyColumns(string database, string table)
        {
            return new List<string>();
        }

        List<string> IMyMetaPlugin.GetViewSubViews(string database, string view)
        {
            return new List<string>();
        }

        List<string> IMyMetaPlugin.GetViewSubTables(string database, string view)
        {
            return new List<string>();
        }

        DataTable IMyMetaPlugin.GetTableIndexes(string database, string table)
        {
            return new DataTable();
        }

        DataTable IMyMetaPlugin.GetForeignKeys(string database, string tableName)
        {
            return new DataTable();
        }

        public object GetDatabaseSpecificMetaData(object myMetaObject, string key)
        {
            return null;
        }

        #endregion

        #region Internal Methods

        private bool IsIntialized
        {
            get
            {
                return (context != null);
            }
        }

        public string GetDatabaseName()
        {
            string[] connectionParts = this.context.ConnectionString.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string connectionPart in connectionParts)
            {
                string[] nameAndValue = connectionPart.Split(new string[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (nameAndValue.Length == 2 && nameAndValue[0].Trim().ToLower() == "database")
                {
                    return nameAndValue[1].Trim();
                }
            }

            return "";
        }

        public string GetFullDatabaseName()
        {
            return GetDatabaseName();
        }

        private DataSet ExecuteCommand(string command)
        {
            using (MySqlConnection connection = Connect())
            {
                using (MySqlDataAdapter adapter = new MySqlDataAdapter(command, connection))
                {
                    DataSet ds = new DataSet();
                    adapter.Fill(ds);
                    return ds;
                }
            }

        }

        private MySqlConnection Connect()
        {
            return new MySqlConnection(this.context.ConnectionString);
        }

        #endregion
    }
}
