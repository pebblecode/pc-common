using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bede.Logging.Models;

namespace PebbleCode.Framework.Threading
{
    /// <summary>
    /// A self starting, 24 hour period task
    /// </summary>
    public abstract class DailyRepeatingTask : RepeatingTask
    {
        private readonly TimeSpan _timeToRun;

        protected DailyRepeatingTask(TimeSpan timeToRun, ILoggingService loggingService)
            : base(TimeSpan.FromHours(24), loggingService)
        {
            _timeToRun = timeToRun;
        }

        public override void Start()
        {
            Start(_timeToRun);
        }
    }
}
