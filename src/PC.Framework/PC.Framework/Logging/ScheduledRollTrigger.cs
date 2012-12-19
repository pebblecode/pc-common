using PebbleCode.Framework.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PC.Framework.Logging
{
    /// <summary>
    /// Housekeeping task to roll log at specified time, daily
    /// </summary>
    class ScheduledRollTrigger : DailyRepeatingTask
    {
        private readonly ScheduledTimeRollingFlatFileListener _listener;
        public ScheduledRollTrigger(ScheduledTimeRollingFlatFileListener listener, TimeSpan runTime)
            : base(runTime)
        {
            _listener = listener;
        }

        protected override void PerformTask()
        {
            _listener.Roll();
        }
    }
}
