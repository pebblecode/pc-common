using System;
using System.Threading;
using PebbleCode.Framework.Collections.ThreadSafe;

namespace PebbleCode.Framework.Threading
{
    /// <summary>
    /// Thread safe approach to holding and releasing locks asychronously accross
    /// a range of ids. Adapted from AcquireRegisterPlayerMutex/ReleaseRegisterPlayerMutex
    /// in FB.Engine.Room (FreeBingo project)
    /// </summary>
    public class ThreadSafeKeyedMutex<T>
    {
        private ThreadSafeDictionary<T, Mutex> _mutexes = new ThreadSafeDictionary<T, Mutex>();

        /// <summary>
        /// Get the mutex for a given mutexId
        /// </summary>
        /// <param name="mutexKey"></param>
        public void AcquireMutex(T mutexKey)
        {
            Mutex mutex = null;

            // Create a mutex for this mutexId, and immediately own it
            lock (_mutexes)
            {
                // No mutex for this mutex id yet
                if (!_mutexes.SafeContainsKey(mutexKey))
                {
                    _mutexes.SafeAdd(mutexKey, new Mutex(true));
                    return;
                }

                // More complicated.
                //
                // There is already a mutex for this id. 
                // 
                // Wait on the mutex, and then try adding again. But we MUST wait
                // on the mutex OUTSIDE of this lock, otherwise we hold up all
                // other acquires
                mutex = _mutexes[mutexKey];
            }

            // Wait on the mutex now, but then release immediately, we don't need
            // it. Just need to know once the other acquire has finished.
            try
            {
                mutex.WaitOne();
                mutex.ReleaseMutex();
            }
            catch (ObjectDisposedException)
            {
                // Ignore this. Happens if we Close the mutex elsewhere
                // before we get chance to release it here. Its not a problem. 
                // Either way we no longer have the mutex;
            }

            // Go back and try to get the mutex again. We can't just take it
            // incase others are waiting
            AcquireMutex(mutexKey);
        }

        /// <summary>
        /// Release the mutex for a given player
        /// </summary>
        /// <param name="mutexKey"></param>
        public void ReleaseMutex(T mutexKey)
        {
            Mutex mutex = null;

            lock (_mutexes)
            {
                mutex = _mutexes[mutexKey];
                mutex.ReleaseMutex();
                _mutexes.SafeRemove(mutexKey);
            }

            mutex.Close();
            mutex.Dispose();
        }
    }
}
