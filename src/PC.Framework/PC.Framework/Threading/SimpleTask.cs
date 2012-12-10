using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PebbleCode.Framework.Threading
{
    public abstract class SimpleTask
    {
        /// <summary>
        /// Run task only once
        /// </summary>
        public void RunOnce()
        {
            PerformTask();
        }

        /// <summary>
        /// Implemented by derived classes to do work on timer expiry
        /// </summary>
        protected abstract void PerformTask();

    }
}
