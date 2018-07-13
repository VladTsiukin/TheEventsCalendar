using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EventPlanning.Services;

namespace EventPlanning.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm email",
                $"<h1>Hellow:)</h1><h2>Please confirm your account by clicking on this link: <a href='{HtmlEncoder.Default.Encode(link)}' style='color:#41adfc;'>Event Planning App</a></h2>");
        }

        public static Task SendEmailToSubscriberAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Event Subscription",
                $"<h1>Hellow:)</h1><h2>You are subscribed to an event: <a href='{HtmlEncoder.Default.Encode(link)}' style='color:#41adfc;'>Event Planning App</a></h2>");
        }
    }
}
