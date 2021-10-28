using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csharp_group_homework_64_ivan_kobtsev.service
{
    public class EmailServcie
    {
        public class EmailService
        {

            public void SendEmail(string email, string subject, string message)
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("Моя компания", "admin@mycompany.com")); // Здесь указываем ящик с которого будет происходить отправка
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                {
                    Text = message
                };

                using (var client = new SmtpClient())
                {
                    client.ConnectAsync("smtp.gmail.com", 587, true);
                    client.AuthenticateAsync("vanomc77@gmail.ru", "909323Kobcev");                   
                    client.SendAsync(emailMessage);
                    client.DisconnectAsync(true);
                }

            }

        }
    }
}
