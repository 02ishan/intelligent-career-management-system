using MailKit.Net.Smtp;
using MimeKit;

namespace ICMS.API.Services
{
    public class EmailService
    {
        public async Task SendEmailAsync(
            string toEmail,
            string subject,
            string body)
        {
            var email = new MimeMessage();

            email.From.Add(
                MailboxAddress.Parse("YOUR_EMAIL@gmail.com"));

            email.To.Add(
                MailboxAddress.Parse(toEmail));

            email.Subject = subject;

            email.Body = new TextPart("plain")
            {
                Text = body
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                "smtp.gmail.com",
                587,
                MailKit.Security.SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                "YOUR_EMAIL@gmail.com",
                "YOUR_APP_PASSWORD");

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}