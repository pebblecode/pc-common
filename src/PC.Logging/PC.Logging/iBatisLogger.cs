using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IBatisNet.Common.Logging;

namespace PebbleCode.Framework.Logging
{
    /// <summary>
    /// iBatis logger factory adapter to interface with iBatis logging
    /// </summary>
    class iBatisLoggerFactoryAdapter : ILoggerFactoryAdapter
    {
        /// <summary>
        /// Get a logger by class name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ILog GetLogger(string name)
        {
            switch (name)
            {
                case "IBatisNet.DataMapper.Commands.DefaultPreparedCommand":
                    return new iBatisLog(Category.iBatisSql);

                case "IBatisNet.DataMapper.Configuration.Statements.PreparedStatementFactory":
                case "IBatisNet.DataMapper.Commands.IPreparedCommand":
                    return new iBatisLog(Category.iBatisInit);

                case "IBatisNet.DataMapper.Configuration.Cache.CacheModel":
                case "IBatisNet.DataMapper.LazyLoadList":
                case "IBatisNet.DataMapper.SqlMapSession":
                case "IBatisNet.Common.FeeTransaction.TransactionScope":
                case "IBatisNet.DataAccess.DaoSession":
                case "IBatisNet.DataAccess.Configuration.DaoProxy":
                case "IBatisNet.DataMapper.Configuration.DomSqlMapBuilder":
                case "IBatisNet.Common.Utilities.Objects.ObjectFactory":
                case "IBatisNet.Common.Utilities.Objects.DelegateObjectFactory":
                case "IBatisNet.DataMapper.TypeHandlers.TypeHandlerFactory":
                case "IBatisNet.DataMapper.Configuration.ParameterMapping.ParameterMap":
                case "IBatisNet.Common.Utilities.Resources":
                case "IBatisNet.Common.Utilities.Objects.FactoryLogAdapter":
                default:
                    return new iBatisLog(Category.iBatis);
            }
        }

        /// <summary>
        /// Get a logger by type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ILog GetLogger(Type type)
        {
            return GetLogger(type.FullName);
        }
    }

    /// <summary>
    /// iBatis ILog implementation
    /// </summary>
    class iBatisLog : ILog
    {
        string _category = Category.iBatis;

        /// <summary>
        /// Construct with a category
        /// </summary>
        /// <param name="category"></param>
        public iBatisLog(string category)
        {
            _category = category;
        }

        #region ILog Members

        public bool IsDebugEnabled
        {
            get { return Logger.ShouldLogDebug(_category); }
        }

        public bool IsErrorEnabled
        {
            get { return Logger.ShouldLogError(_category); }
        }

        public bool IsFatalEnabled
        {
            get { return Logger.ShouldLogError(_category); }
        }

        public bool IsInfoEnabled
        {
            get { return Logger.ShouldLogInfo(_category); }
        }

        public bool IsWarnEnabled
        {
            get { return Logger.ShouldLogWarning(_category); }
        }

        public void Debug(object message, Exception exception)
        {
            Logger.WriteDebug(message.ToString(), _category);
        }

        public void Debug(object message)
        {
            Logger.WriteDebug(message.ToString(), _category);
        }

        public void Error(object message, Exception exception)
        {
            Logger.WriteError(message.ToString(), _category);
        }

        public void Error(object message)
        {
            Logger.WriteError(message.ToString(), _category);
        }

        public void Fatal(object message, Exception exception)
        {
            Logger.WriteError("Fatal:" + message.ToString(), _category);
        }

        public void Fatal(object message)
        {
            Logger.WriteError("Fatal:" + message.ToString(), _category);
        }

        public void Info(object message, Exception exception)
        {
            Logger.WriteInfo(message.ToString(), _category);
        }

        public void Info(object message)
        {
            Logger.WriteInfo(message.ToString(), _category);
        }

        public void Warn(object message, Exception exception)
        {
            Logger.WriteWarning(message.ToString(), _category);
        }

        public void Warn(object message)
        {
            Logger.WriteWarning(message.ToString(), _category);
        }

        #endregion
    }
}
