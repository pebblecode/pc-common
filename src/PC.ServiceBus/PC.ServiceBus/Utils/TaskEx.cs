using System;
using System.Threading;
using System.Threading.Tasks;

namespace PC.ServiceBus.Utils
{
    internal class TaskEx
    {
        /// <summary>
        /// Starts a Task that will complete after the specified due time.
        /// </summary>
        /// <param name="dueTime">The delay in milliseconds before the returned task completes.</param>
        /// <returns>
        /// The timed Task.
        /// </returns>
        public static Task Delay(int dueTime)
        {
            if (dueTime <= 0) throw new ArgumentOutOfRangeException("dueTime");

            var tcs = new TaskCompletionSource<bool>();
            var timer = new Timer(self =>
            {
                ((Timer)self).Dispose();
                tcs.TrySetResult(true);
            });
            timer.Change(dueTime, Timeout.Infinite);
            return tcs.Task;
        }
    }
}
