using PebbleCode.Framework.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace PebbleCode.Entities
{
    /// <summary>
    /// Static decorator class for the Logger framework, which formats entity messages to add context.
    /// </summary>
    public class EntityLogger<TPrimaryKey>
        where TPrimaryKey : IComparable
    {
        /// <summary>
        /// Writes to the debug log is so configured
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="entities"></param>
        public static void WriteDebug(string message, string category, params Entity<TPrimaryKey>[] entities)
        {
            Logger.WriteDebug(FormatEntityMessage(message, entities), category, null);
        }

        /// <summary>
        /// Writes to the info log is so configured
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="args">Arguments for the message format string - pass null if not required</param>
        /// <param name="entities"></param>
        public static void WriteInfo(string message, string category, params Entity<TPrimaryKey>[] entities)
        {
            Logger.WriteInfo(FormatEntityMessage(message, entities), category, null);
        }

        /// <summary>
        /// Writes to the warning log is so configured
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="entities"></param>
        public static void WriteWarning(string message, string category, params Entity<TPrimaryKey>[] entities)
        {
            Logger.WriteWarning(FormatEntityMessage(message, entities), category, null);
        }

        /// <summary>
        /// Writes to the Error log 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="category"></param>
        /// <param name="entities"></param>
        public static Exception WriteError(string message, string category, params Entity<TPrimaryKey>[] entities)
        {
            return Logger.WriteError(FormatEntityMessage(message, entities), category, null);
        }

        /// <summary>
        /// Wraps a new system exception around the core exception, formats a message based on the passed in entities.
        /// </summary>
        /// <param name="exception"></param>
        /// <param name="additionalMessage"></param>
        /// <param name="category"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static Exception WriteUnexpectedException(Exception exception, string additionalMessage, string category, params Entity<TPrimaryKey>[] entities)
        {
            return Logger.WriteUnexpectedException(exception, FormatEntityMessage(additionalMessage, entities), category);
        }

        /// <summary>
        /// Formats a message to contain details of each of the entities
        /// </summary>
        /// <param name="message"></param>
        /// <param name="entities"></param>
        /// <returns></returns>
        private static string FormatEntityMessage(string message, Entity<TPrimaryKey>[] entities)
        {
            List<string> entityMessages = new List<string>();
            if (entities != null && entities.Length > 0)
            {
                foreach (Entity<TPrimaryKey> e in entities)
                {
                    entityMessages.Add(e.ToLogString());
                }
                return message + string.Format(" {0}", string.Join(",", entityMessages));
            }
            else
            {
                return message;
            }
        }
    }
}
