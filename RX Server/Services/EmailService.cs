using System.Net;
using System.Net.Mail;

namespace RX_Server.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var host = _config["EmailSettings:SmtpHost"];
            var port = int.Parse(_config["EmailSettings:SmtpPort"] ?? "587");
            var email = _config["EmailSettings:SmtpUser"];
            var pass = _config["EmailSettings:SmtpPass"];

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(pass) || email.Contains("your_email"))
            {
                throw new Exception("Vui lòng cấu hình Email trong appsettings.json chưa chính xác.");
            }

            using (var client = new SmtpClient(host, port))
            {
                client.EnableSsl = true;
                client.Credentials = new NetworkCredential(email, pass);
                
                var mailMessage = new MailMessage
                {
                    From = new MailAddress(email),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                await client.SendMailAsync(mailMessage);
            }
        }
    }
}
