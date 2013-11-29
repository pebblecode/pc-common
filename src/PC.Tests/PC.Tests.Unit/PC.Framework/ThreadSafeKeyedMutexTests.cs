
using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using PebbleCode.Framework.Threading;

namespace PebbleCode.Tests.Unit.PC.Framework
{
    [TestFixture]
    public class ThreadSafeKeyedMutexTests
    {
        [Test]
        public void StackoverflowTest()
        {
            var threadSafeKeyedMutex = new ThreadSafeKeyedMutex();
            int numThreads = 10;
            var threads = new List<Thread>();
            for (int i = 0; i < numThreads; i++)
            {
                var newThread = new Thread(() => LockMutex(threadSafeKeyedMutex));
                newThread.Start();
                threads.Add(newThread);
            }
            
            foreach (Thread thread in threads)
            {
                thread.Join();
            }
        }
        
        private void LockMutex(ThreadSafeKeyedMutex threadSafeKeyedMutex)
        {
            int id = 10;
            int iterations = 100;
            for (int i = 0; i < iterations; i++)
            {
                threadSafeKeyedMutex.AcquireMutex(id);
                Thread.Sleep(50);
                threadSafeKeyedMutex.ReleaseMutex(id);
                Thread.Sleep(50);
            }
        }
    }
}
