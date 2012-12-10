using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using PebbleCode.Framework.Configuration;
using PebbleCode.Framework.Email;

namespace PebbleCode.Framework.Email
{
    public class Emailer : IEmailer, IDisposable
    {
        private readonly SmtpClient _client;
        private readonly MailMessage _message;

        public string SenderEmail { get; set; }
        public string SenderDisplayName { get; set; }

        public MailAddress Sender
        {
            get
            {
                return new MailAddress(SenderEmail, SenderDisplayName);
            }
        }

        public Emailer()
        {
            _client =
                new SmtpClient
                    {
                        Host = EmailSettings.Host,
                        Port = EmailSettings.Port,
                        EnableSsl = EmailSettings.SslEnabled,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(EmailSettings.Username, EmailSettings.Password)
                    };
            _message = new MailMessage();
            SenderEmail = EmailSettings.Username;
            SenderDisplayName = EmailSettings.DisplayName;
        }

        public void AddRecipient(params string[] recipients)
        {
            foreach (var recipient in recipients.Where(recipient => !string.IsNullOrWhiteSpace(recipient)))
                _message.To.Add(recipient);
        }

        public void AttachFile(params string[] filePaths)
        {
            foreach (var filePath in filePaths.Where(File.Exists))
                _message.Attachments.Add(new Attachment(filePath));
        }

        public virtual bool Send(string subject, string body)
        {
            if (_message.To.Count > 0)
            {
                _message.From = Sender;
                _message.Subject = subject;
                _message.Body = body;

                _client.Send(_message);
                return true;
            }
            return false;
        }

        public void Dispose()
        {
            if(_client != null)
                _client.Dispose();
        }

        public void SendMail(MailMessage message)
        {
            message.From = Sender;
            _client.Send(message);
        }
    }
}
