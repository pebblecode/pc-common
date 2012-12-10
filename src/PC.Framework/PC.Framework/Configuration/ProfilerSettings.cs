using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading;

namespace PebbleCode.Framework.Configuration
{
    /// <summary>
    /// Encapsulated access to database application settings
    /// </summary>
    public static class ProfilerSettings
    {
        /// <summary>
        /// Get the max number of worker threads
        /// </summary>
        public static int SecondsBeforeClearingProfileStats
        {
            get
            {
                if (_secondsBeforeClearingProfileStats == Int32.MinValue)
                    _secondsBeforeClearingProfileStats = ApplicationSettingsHelper.LoadAppSetting<int>(
                        "secondsBeforeClearingProfileStats",
                        false,
                        DEFAULT_SECONDS_BEFORE_CLEARING_PROFILE_STATS);
                return _secondsBeforeClearingProfileStats;
            }
        }

        private static int _secondsBeforeClearingProfileStats = Int32.MinValue;
        private const int DEFAULT_SECONDS_BEFORE_CLEARING_PROFILE_STATS = 0;

        /// <summary>
        /// Get the number of milliseconds to wait between event profile calls
        /// </summary>
        public static int ProfileLogPeriodMilliseconds
        {
            get
            {
                if (_profileLogPeriodMilliseconds == 0)
                {
                    _profileLogPeriodMilliseconds = ApplicationSettingsHelper.LoadAppSetting<int>(
                        "profileLogPeriod",
                        false,
                        DEFAULT_PROFILE_LOG_PERIOD_MILLISECONDS);

                    // Ensure its a sensible value
                    if (_profileLogPeriodMilliseconds <= 100)
                        _profileLogPeriodMilliseconds = DEFAULT_PROFILE_LOG_PERIOD_MILLISECONDS;
                }

                return _profileLogPeriodMilliseconds;
            }
        }

        private static int _profileLogPeriodMilliseconds;
        private const int DEFAULT_PROFILE_LOG_PERIOD_MILLISECONDS = 5000;
    }
}
