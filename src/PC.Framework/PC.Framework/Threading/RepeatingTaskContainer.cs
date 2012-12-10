using System.Collections.Generic;

namespace PebbleCode.Framework.Threading
{
    /// <summary>
    /// Container for registering, starting and stopping repeating tasks
    /// </summary>
    public class RepeatingTaskContainer
    {
        private readonly List<RepeatingTask> _tasks = new List<RepeatingTask>();

        public void RegisterTask(RepeatingTask task)
        {
            _tasks.Add(task);
        }

        public void StartAll()
        {
            foreach (var task in _tasks)
                task.Start();
        }

        public void StopAll()
        {
            foreach (var task in _tasks)
                task.Stop();
        }
    }
}
