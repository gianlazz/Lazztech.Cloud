using Lazztech.Standard.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Lazztech.Standard.Services
{
    public class EmailService : IEmailService
    {
        public string FromAddress { get; private set; }

        private SmtpClient _client;

        public EmailService(string fromAddress)
        {
            FromAddress = fromAddress;

            _client = new SmtpClient();
            _client.Port = 25;
            _client.DeliveryMethod = SmtpDeliveryMethod.Network;
            _client.UseDefaultCredentials = false;
            _client.Host = "smtp.gmail.com";
        }

        public virtual void SendEmail(string toAddress, string subject, string emailBody)
        {
            var mail = new MailMessage(FromAddress, toAddress, subject, emailBody);
            _client.Send(mail);
        }
    }
}
