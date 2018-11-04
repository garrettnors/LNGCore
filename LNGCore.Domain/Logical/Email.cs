﻿using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace LNGCore.Domain.Logical
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

        public void SendEmail()
        {
            var mailerMail = _config["SiteConfiguration:MailerEmail"];
            var mailerPassword = _config["SiteConfiguration:MailerPassword"];

            var senderMail = new MailAddress(SenderEmail, SenderDisplayName);
            var recipientMail = new MailAddress(RecipientEmail, RecipientDisplayName);

            var message = new MailMessage(senderMail, recipientMail)
            {
                Subject = MailSubject,
                Body = Message
            };

            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(mailerMail, mailerPassword),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            client.Send(message);
        }
    }
}