using System;
using System.Configuration;
using System.Linq;
using System.Xml;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Bede.Logging.Models;
using Bede.Logging.Providers;
using IBatisNet.DataMapper;
using IBatisNet.DataMapper.Configuration;

using PebbleCode.Entities;
using PebbleCode.Framework;
using PebbleCode.Framework.Configuration;
using PebbleCode.Framework.Utilities;
using log4net;
using PC.Entities;


namespace PebbleCode.Repository
{
    public abstract class EntityRepository
    {
        // Private members
        private static ISqlMapper _mapper = null;
        private static bool _isInitialized;
        private static object _initializeLock = new object();
        private static ILoggingService _loggingService = new Log4NetLoggingService(LogManager.GetLogger("EntitiesLogger"));

        /// <summary>
        /// Private initialisation routine. Only runs once.
        /// </summary>
        private static void Initialize()
        {
            // Use if/lock/if pattern for best performance around the lock
            if (_isInitialized)
                return;

            lock (_initializeLock)
            {
                if (_isInitialized)
                    return;

                // Load the assembly for the map
                Assembly assembly = null;
                try { assembly = Assembly.Load(DatabaseSettings.SqlMap_Assembly); }
                catch { throw new Exception(string.Format("Error loading SQL Map Assembly: {0}", DatabaseSettings.SqlMap_Assembly)); }

                // Load the map from the named resource
                Stream sqlMapFileStream = assembly.GetManifestResourceStream(DatabaseSettings.SqlMap_Resource);
                if (sqlMapFileStream == null)
                    throw new Exception(string.Format("Couldn't load SQL Map Resource {0} from Assembly {1}", DatabaseSettings.SqlMap_Resource, DatabaseSettings.SqlMap_Assembly));

                // Load the SQL map configuration XML
                XmlDocument docSqlMap = new XmlDocument();
                docSqlMap.Load(sqlMapFileStream);

                // Inject connection string if set
                string connectionString = DatabaseSettings.ConnectionString;
                if (!string.IsNullOrWhiteSpace(connectionString))
                {
                    // Select the connection string attribute
                    XmlNodeList dataSourceNodes = docSqlMap.GetElementsByTagName("dataSource");
                    if (dataSourceNodes.Count != 1)
                    {
                        throw new XmlException(string.Format("No dataSource node found in {0}", DatabaseSettings.SqlMap_Resource));
                    }
                    XmlElement dataSource = (XmlElement)dataSourceNodes[0];
                    dataSource.SetAttribute("connectionString", connectionString);
                }

                //Ensure we're not using a prod connection string in a
                if (connectionString.Contains("prod") && ReflectionUtils.InUnitTest)
                {
                    throw new Exception("You can't run tests against a production database! Check app.config");
                }

                // Mapper
                DomSqlMapBuilder builder = new DomSqlMapBuilder();
                _mapper = builder.Configure(docSqlMap);

                // Set flag
                _isInitialized = true;
            }
        }

        /// <summary>
        /// Begin a new transaction on the current thread.
        /// Dispose of the RepositoryTransaction when you want to commit transaction (i.e. put it in a using)
        /// </summary>
        /// <returns></returns>
        public static RepositoryTransaction BeginTransaction()
        {
            return new RepositoryTransaction(GetMapper());
        }

        /// <summary>
        /// Get the one and only mapper instance.
        /// </summary>
        /// <returns></returns>
        private static ISqlMapper GetMapper()
        {
            Initialize();
            return _mapper;
        }

        /// <summary>
        /// Get the mapper
        /// </summary>
        protected ISqlMapper Mapper
        {
            get { return GetMapper(); }
        }

        /// <summary>
        /// Log a message to the logger. Repo logging is always debug logging, category=Repository;
        /// </summary>
        /// <param name="message"></param>
        [DebuggerStepThrough]
        protected void Log(string message)
        {
            _loggingService.Warning(new CommonLoggingData(message, LogCategories.EntityFramework));
        }

        /// <summary>
        /// Build a set of parameters
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected Dictionary<string, object> Params(params Param[] parameters)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (Param parameter in parameters)
                result.Add(parameter.Name, parameter.Value);
            return result;
        }

        /// <summary>
        /// Adds in standard Params to lookup a particular page of the result set
        /// </summary>
        /// <param name="pageInfo"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        protected Param[] AddPageInfoParams(PageInfo pageInfo, params Param[] parameters)
        {
            if (pageInfo == null) return parameters;

            List<Param> newParameters = new List<Param>();
            newParameters.AddRange(parameters);
            newParameters.AddRange(pageInfo.ToParams());
            return newParameters.ToArray();
        }
    }
}

