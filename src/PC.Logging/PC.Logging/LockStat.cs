using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace PebbleCode.Framework.Logging
{
    public static class LockStat
    {
        public static void Acquiring(string lockId)
        {
            if (Logger.ShouldLogDebug(Category.Locking))
            {
                string message = string.Format("Aqcuiring lock: {0}", GenerateMessageData(lockId));
                Logger.WriteDebug(message, Category.Locking);
            }
        }

        public static void Acquired(string lockId)
        {
            if (Logger.ShouldLogDebug(Category.Locking))
            {
                string message = string.Format("Aqcuired lock: {0}", GenerateMessageData(lockId));
                Logger.WriteDebug(message, Category.Locking);
            }
        }

        private static string GenerateMessageData(string lockId)
        {
            string message = string.Format("Lock:{0}  Thread-Id:{1}  Thread-Name:{2}", 
                lockId, 
                Thread.CurrentThread.ManagedThreadId,
                Thread.CurrentThread.Name);
            return message;
        }
    }
}
