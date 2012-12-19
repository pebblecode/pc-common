using PebbleCode.Framework.Email;

namespace PebbleCode.Tests.Fakes
{
    public class FakeEmailer : IEmailer
    {
        public int EmailsSent = 0;
        public void SendMail(string recipient, string body, string attachmentPath)
        {
            EmailsSent++;
        }

        public void SendMail(System.Net.Mail.MailMessage message)
        {
            EmailsSent++;
        }
    }
}
