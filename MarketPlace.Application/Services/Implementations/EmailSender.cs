using MailKit.Net.Smtp;
using MailKit.Security;
using MarketPlace.Application.Services.Interfaces;
using MimeKit;
using MimeKit.Text;

namespace MarketPlace.Application.Services.Implementations
{
    public class EmailSender : IEmailSender
    {
        public async Task SendMail(string to, string code, string subject, string text)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("سایت فروشگاه ساز", "EmailSender@hotmail.com"));
            message.To.Add(new MailboxAddress("کاربر", to));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Plain)
            {
                Text = $"{text} : {code}"
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp-mail.outlook.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync("EmailSender@hotmail.com", "Password");
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }

        }
    }
}
