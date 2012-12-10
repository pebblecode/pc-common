using System.Net.Mail;

namespace PebbleCode.Framework.Email
{
    public interface IEmailer
    {
        void SendMail(MailMessage message);
    }
}
