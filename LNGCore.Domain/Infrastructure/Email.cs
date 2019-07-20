using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace LNGCore.Services.Logical
{
    public class Email
    {
        private readonly IConfiguration _config;
        public Email(IConfiguration configuration)
        {
            _config = configuration;
        }
        public string Message { get; set; }
        public string SenderEmail { get; set; }
        public string SenderDisplayName { get; set; }
        public string RecipientEmail { get; set; }
        public string RecipientDisplayName { get; set; }
        public string MailSubject { get; set; }
        public Attachment Attachment { get; set; }

        public string SendEmail()
        {
            try
            {
                var mailerMail = _config.GetSection("SiteConfiguration")["MailerEmail"];
                var mailerPassword = _config.GetSection("SiteConfiguration")["MailerPassword"];

                var senderMail = new MailAddress(SenderEmail, SenderDisplayName);
                var recipientMail = new MailAddress(RecipientEmail, RecipientDisplayName);

                var message = new MailMessage(senderMail, recipientMail)
                {
                    Subject = MailSubject,
                    Body = Message,
                    IsBodyHtml = true
                };

                if (Attachment != null)
                    message.Attachments.Add(Attachment);

                var client = new SmtpClient("smtp.gmail.com", 587)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(mailerMail, mailerPassword),
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                client.Send(message);
                return null;
            }
            catch (Exception ex)
            {
                return $"{ex.Message} ---------------BEGIN-TRACE--------------- {ex.StackTrace}";
            }
        }
    }
}
