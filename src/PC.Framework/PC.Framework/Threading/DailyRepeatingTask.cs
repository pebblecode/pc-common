using PebbleCode.Framework.Logging;
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
        public DailyRepeatingTask(TimeSpan timeToRun, ILogger logger)
            : base(TimeSpan.FromHours(24), logger)
        {
            Start(timeToRun);
        }
    }
}
