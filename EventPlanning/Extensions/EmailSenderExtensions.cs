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
            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"<h2>Пожалуйста подтвердите свою учетную запись, нажав на эту ссылку: <a href='{HtmlEncoder.Default.Encode(link)}' style='color:#E3F2FD;'>Event Planning App</a></h2>");
        }
    }
}
