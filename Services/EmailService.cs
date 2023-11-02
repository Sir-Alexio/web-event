using MailKit.Net.Smtp;
using MimeKit;
using WebEvent.API.Services.Abstract;

namespace WebEvent.API.Services
{
    public class EmailService :IEmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService()
        {
            _smtpClient = new SmtpClient();
        }

        public async Task SendVerificationEmailAsync(string verificationLink)
        {
            string str = "freda.luettgen20@ethereal.email";
            var email = new MimeMessage();

            email.From.Add(MailboxAddress.Parse(str));
            email.To.Add(MailboxAddress.Parse(str));
            email.Subject = "Event email verification";

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<p>Click the link to verify your email: <a href='{verificationLink}'>Verify Email</a></p>";

            email.Body = bodyBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.email", 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(str, "46TtKWyZYEguVa2dhC");
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
