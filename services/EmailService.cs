using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace csharp_group_homework_64_ivan_kobtsev.services
{
    public class EmailService
    {
        private readonly ILogger<EmailService> logger;

        public EmailService(ILogger<EmailService> logger)
        {
            this.logger = logger;
        }
        public void SendEmail(string email, string subject, string message)
            {
                try
                {
                    var emailMessage = new MimeMessage();
                    emailMessage.From.Add(new MailboxAddress("Администрация", "admin@mycompany.com"));
                    emailMessage.To.Add(new MailboxAddress("", email));
                    emailMessage.Subject = subject;
                    emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
                    {
                        Text = message
                    };

                    using (var client = new SmtpClient())
                    {
                        client.Connect("smtp.gmail.com", 465, true);
                        client.Authenticate("vanomc77@gmail.com", "909323Kobcev");
                        client.Send(emailMessage);
                        client.Disconnect(true);
                        logger.LogInformation("Сообщение отправлено успешно!");
                    }
                }
                catch (Exception e)
                {

                    logger.LogError(e.GetBaseException().Message);
                }
               

        }
    }
}
