using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PebbleCode.Framework.Threading
{
    /// <summary>
    /// A self starting, 24 hour period task
    /// </summary>
    public abstract class DailyRepeatingTask : RepeatingTask
    {
        public DailyRepeatingTask(TimeSpan timeToRun)
            : base(TimeSpan.FromHours(24))
        {
            Start(timeToRun);
        }
    }
}
