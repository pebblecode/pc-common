using System;
using System.Collections.Generic;
using System.Text;
using PebbleCode.Framework.Utilities;

namespace PebbleCode.Framework.Collections.ThreadSafe
{
    /// <summary>
    /// Thread safe dictionary
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class ThreadSafeDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        /// <summary>
        /// The sync object used to lock internally
        /// </summary>
        private object _syncObject = new object();

        /// <summary>
        /// Add safely
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SafeAdd(TKey key, TValue value)
        {
            lock (_syncObject)
            {
                base.Add(key, value);
            }
        }

        /// <summary>
        /// Remove safely
        /// </summary>
        /// <param name="key"></param>
        public void SafeRemove(TKey key)
        {
            lock (_syncObject)
            {
                base.Remove(key);
            }
        }

        /// <summary>
        /// Check for a key safely
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool SafeContainsKey(TKey key)
        {
            lock (_syncObject)
            {
                return base.ContainsKey(key);
            }
        }

        /// <summary>
        /// Get the count, safelty
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
        /// <param name="key"></param>
        /// <returns></returns>
        public new TValue this[TKey key] 
        {
            get
            {
                lock (_syncObject)
                {
                    return base[key];
                }
            }
            set
            {
                lock (_syncObject)
                {
                    base[key] = value;
                }
            }
        }

        /// <summary>
        /// Safe count of keys
        /// </summary>
        public int SafeKeyCount
        {
            get
            {
                lock (_syncObject)
                {
                    return base.Keys.Count;
                }
            }
        }

        /// <summary>
        /// Check if it has a key, if not add the default value for that key
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        public void SafeEnsureHasKey(TKey key, TValue defaultValue)
        {
            lock (_syncObject)
            {
                if (!base.ContainsKey(key))
                    base.Add(key, defaultValue);
            }
        }

        /// <summary>
        /// Delegate used for callbacks in SafeEnsureHasKey
        /// </summary>
        /// <returns></returns>
        public delegate TValue GetDefaultCallback();

        /// <summary>
        /// Check if it has a key, if not, callback to get the default value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        public void SafeEnsureHasKey(TKey key, GetDefaultCallback callback)
        {
            lock (_syncObject)
            {
                if (!base.ContainsKey(key))
                    base.Add(key, callback());
            }
        }

        /// <summary>
        /// Safe conversion to array of keys
        /// </summary>
        /// <returns></returns>
        public TKey[] SafeGetKeyArray()
        {
            lock (_syncObject)
            {
                return ArrayExt.ToArray<TKey>(base.Keys);
            }
        }

        /// <summary>
        /// Safe conversion to array of values
        /// </summary>
        /// <returns></returns>
        public TValue[] SafeGetValueArray()
        {
            lock (_syncObject)
            {
                return ArrayExt.ToArray<TValue>(base.Values);
            }
        }

        /// <summary>
        /// Safe get if key exists, default otherwise
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public TValue SafeGetIfContainsKey(TKey key)
        {
            lock (_syncObject)
            {
                if (base.ContainsKey(key))
                    return base[key];
                return default(TValue);
            }
        }
    }
}
