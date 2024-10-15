namespace QualiPro_Recruitment_Web_Api.Services
{
    using Microsoft.Extensions.Configuration;
    using MimeKit;
    using System.Threading.Tasks;
    using MailKit.Net.Smtp;

    public class EmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // Create the email message
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Your App", "nawres@gmail.com"));  // Updated sender email
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = body };
            email.Body = builder.ToMessageBody();

            // Use fully qualified MailKit SmtpClient to avoid conflict
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.CheckCertificateRevocation = false; // Disable certificate revocation checking
            await smtp.ConnectAsync(_config["MailSettings:SMTPHost"],
                                    int.Parse(_config["MailSettings:SMTPPort"]),
                                    MailKit.Security.SecureSocketOptions.Auto); // Try using Auto or SslOnConnect
            await smtp.AuthenticateAsync(_config["MailSettings:SMTPUser"],
                                         _config["MailSettings:SMTPPass"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);   
        }
    }
}
