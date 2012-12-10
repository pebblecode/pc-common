using System;
using System.Collections.Generic;
using System.Text;
using PebbleCode.Framework.Utilities;

namespace PebbleCode.Framework.Collections.ThreadSafe
{
    /// <summary>
    /// Thread safe list
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ThreadSafeList<T> : List<T>
    {
        /// <summary>
        /// The sync object used to lock internally
        /// </summary>
        private object _syncObject = new object();

        /// <summary>
        /// Add safely
        /// </summary>
        /// <param name="value"></param>
        public void SafeAdd(T value)
        {
            lock (_syncObject)
            {
                base.Add(value);
            }
        }

        /// <summary>
        /// Safe remove
        /// </summary>
        /// <param name="value"></param>
        public void SafeRemove(T value)
        {
            lock (_syncObject)
            {
                base.Remove(value);
            }
        }

        /// <summary>
        /// Get count, safely
        /// </summary>
        public int SafeCount
        {
            get
            {
                lock (_syncObject)
                {
                    return base.Count;
                }
            }
        }

        /// <summary>
        /// Safe indexer
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public new T this[int index] 
        {
            get
            {
                lock (_syncObject)
                {
                    return base[index];
                }
            }
            set
            {
                lock (_syncObject)
                {
                    base[index] = value;
                }
            }
        }

        /// <summary>
        /// Safe conversion to simple array
        /// </summary>
        /// <returns></returns>
        public T[] SafeToArray()
        {
            lock (_syncObject)
            {
                return base.ToArray();
            }
        }
    }
}
