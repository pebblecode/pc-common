using PebbleCode.Framework.Configuration;
using PebbleCode.Framework.Email;
using PebbleCode.Framework.IoC;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PebbleCode.Framework;
using PebbleCode.Framework.SystemInfo;
using System.Text.RegularExpressions;
using PebbleCode.Framework.Logging;

namespace PC.Framework.Logging
{
    public class ScheduledTimeEmailTraceListener : ScheduledTimeRollingFlatFileListener
    {
        private const string ERROR_REGEX = @"[\d:]+[\s]+[\w]+[\s]*Error";
        private const string WARNING_REGEX = @"[\d:]+[\s]+[\w]+[\s]*Warning";

        public string EmailRecipients
        {
            get
            {
                return Attributes["emailRecipients"];
            }
        }

        public string EmailSubject
        {
            get
            {
                return Attributes["emailSubject"];
            }
        }

        public bool IncludeSystemDetails
        {
            get
            {
                return Attributes.ContainsKey("includeSystemDetails") && Convert.ToBoolean(Attributes["includeSystemDetails"]);
            }
        }

        /// <summary>
        /// Email the Log file on file rolled
        /// </summary>
        /// <param name="filePath"></param>
        protected override void OnFileRolled(string filePath)
        {
            if (!File.Exists(filePath)) return;

            try
            {
                int totalErrors = File.ReadAllLines(filePath).Count(line => Regex.IsMatch(line, ERROR_REGEX));
                int totalWarnings = File.ReadAllLines(filePath).Count(line => Regex.IsMatch(line, WARNING_REGEX));

                string environment = ApplicationSettingsHelper.LoadAppSetting("environment", false, string.Empty);
                if (!string.IsNullOrEmpty(environment))
                    environment = "[{0}]".fmt(environment);

                var info = new SysInfo();
                using (Emailer mailer = Kernel.Get<Emailer>())
                {
                    mailer.AddRecipient(EmailRecipients.Split(';', ','));
                    mailer.AttachFile(filePath);
                    mailer.Send(
                        "[{0} - {1}] {2} {3}"
                            .fmt(EmailSubject,
                                 totalErrors != 0 ? "Error" : (totalWarnings != 0 ? "Warning" : "Information"),
                                 environment,
                                 DateTime.Now.ToString("dd/MM/yyyy")),
                        "{0}\r\n\r\nLog file attached".fmt(info.GetRamDiskUsage()));
                }
            }
            catch (Exception e)
            {
                Logger.WriteUnexpectedException(e, "Failed to email rolled file.", Category.GeneralError);
            }
        }
    }
}
