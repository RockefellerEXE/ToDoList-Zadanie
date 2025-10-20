using MailKit.Net.Smtp;
using MimeKit;
using MailKit;

namespace ToDoList.Services
{
    public class EmailSender
    {
        private readonly string smtpServer;
        private readonly int smtpPort;
        private readonly string smtpUsername;
        private readonly string smtpPassword;

        public EmailSender(IConfiguration configuration)
        {
            smtpServer = configuration.GetValue<string>("SMTP:HOST", "");
            smtpPort = configuration.GetValue<int>("SMTP:PORT", 0);
            smtpUsername = configuration.GetValue<string>("SMTP:USERNAME", "");
            smtpPassword = configuration.GetValue<string>("SMTP:PASSWORD", "");
        }
        public void SendEmail(string email, string subject, string context)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("todoSupport", smtpUsername));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = context
            };

            using (var client = new SmtpClient())
            {
              
                client.Connect(smtpServer, smtpPort, true);
                client.Authenticate(smtpUsername, smtpPassword);
                client.Send(message);
                client.Disconnect(true);
            }
        }
    }
}
