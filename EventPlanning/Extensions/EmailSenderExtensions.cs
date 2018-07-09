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
            return emailSender.SendEmailAsync(email, "Подтвердить email",
                $"<h3>Пожалуйста подтвердите свою учетную запись, нажав на эту ссылку: <a href='{HtmlEncoder.Default.Encode(link)}' style='color:#41adfc;'>Event Planning App</a></h3>");
        }

        public static Task SendEmailToSubscriberAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Подписка на событие",
                $"<h3>Вы подписаны на событие: <a href='{HtmlEncoder.Default.Encode(link)}' style='color:#41adfc;'>Event Planning App</a></h3>");
        }
    }
}
