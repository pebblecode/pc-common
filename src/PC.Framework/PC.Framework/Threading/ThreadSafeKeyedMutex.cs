using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PebbleCode.Framework.Collections.ThreadSafe;
using System.Threading;

namespace PebbleCode.Framework.Threading
{
    /// <summary>
    /// Thread safe approach to holding and releasing locks asychronously accross
    /// a range of ids. Adapted from AcquireRegisterPlayerMutex/ReleaseRegisterPlayerMutex
    /// in FB.Engine.Room (FreeBingo project)
    /// </summary>
    public class ThreadSafeKeyedMutex
    {
        private ThreadSafeDictionary<int, Mutex> _mutexes = new ThreadSafeDictionary<int, Mutex>();

        /// <summary>
        /// Get the mutex for a given mutexId
        /// </summary>
        /// <param name="mutexId"></param>
        public void AcquireMutex(int mutexId)
        {
            Mutex mutex = null;

            // Create a mutex for this mutexId, and immediately own it
            lock (_mutexes)
            {
                // No mutex for this mutex id yet
                if (!_mutexes.SafeContainsKey(mutexId))
                {
                    _mutexes.SafeAdd(mutexId, new Mutex(true));
                    return;
                }

                // More complicated.
                //
                // There is already a mutex for this id. 
                // 
                // Wait on the mutex, and then try adding again. But we MUST wait
                // on the mutex OUTSIDE of this lock, otherwise we hold up all
                // other acquires
                mutex = _mutexes[mutexId];
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
            AcquireMutex(mutexId);
        }

        /// <summary>
        /// Release the mutex for a given player
        /// </summary>
        /// <param name="mutexId"></param>
        public void ReleaseMutex(int mutexId)
        {
            Mutex mutex = _mutexes[mutexId];
            mutex.ReleaseMutex();

            lock (_mutexes)
            {
                _mutexes.SafeRemove(mutexId);
            }

            mutex.Close();
            mutex.Dispose();
        }
    }
}
