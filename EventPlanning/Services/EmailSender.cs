using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using SendGrid;
using SendGrid.Helpers.Mail;


namespace EventPlanning.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class EmailSender : IEmailSender
    {
        public EmailSender(IConfiguration configuration)
        {
            _options = configuration;
        }

        public IConfiguration _options { get; }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(_options["SendGridKey"], subject, message, email);
        }

        private Task Execute(string sendGridKey, string subject, string message, string email)
        {
            var client = new SendGridClient(sendGridKey);

            // from
            var msg = new SendGridMessage
            {
                From = new EmailAddress("develop.with.heart@gmail.com", "MOD DEVELOPER"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };

            // to
            msg.AddTo(new EmailAddress(email));

            // send
            return client.SendEmailAsync(msg);
        }
    }
}
