namespace PebbleCode.Framework.Configuration
{
    public class FtpSettings
    {/// <summary>
        /// Number of FTP connection retry attempts
        /// </summary>
        public static int ConnectionRetries
        {
            get
            {
                if (!_connectionRetries.HasValue)
                {
                    _connectionRetries =
                        ApplicationSettingsHelper.LoadAppSetting("FtpSettings.ConnectionRetries", false, DEFAULT_CONNECTION_RETRIES);

                    //Check the value is valid, default if not
                    if (_connectionRetries <= 0)
                        _connectionRetries = DEFAULT_CONNECTION_RETRIES;
                }

                return _connectionRetries.Value;
            }
            set
            {
                _connectionRetries = value;
            }
        }

        private static int? _connectionRetries;
        private const int DEFAULT_CONNECTION_RETRIES = 3;

        /// <summary>
        /// Number of FTP action retry attempts
        /// </summary>
        public static int ActionRetries
        {
            get
            {
                if (!_actionRetries.HasValue)
                {
                    _actionRetries =
                        ApplicationSettingsHelper.LoadAppSetting("FtpSettings.ActionRetries", false, DEFAULT_ACTION_RETRIES);

                    //Check the value is valid, default if not
                    if (_actionRetries <= 0)
                        _actionRetries = DEFAULT_ACTION_RETRIES;
                }

                return _actionRetries.Value;
            }
            set
            {
                _actionRetries = value;
            }
        }

        private static int? _actionRetries;
        private const int DEFAULT_ACTION_RETRIES = 3;
    }
}
