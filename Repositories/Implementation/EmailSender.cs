using MemberTestAPI.Repositories.Interfaces;
using System.Net;
using System.Net.Mail;

namespace MemberTestAPI.Repositories.Implementation
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IConfiguration configuration, ILogger<EmailSender> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task SendEmail(string recipient, string subject, string body)
        {
            var host = _configuration["EmailSettings:SmtpServer"];
            var port = int.Parse(_configuration["EmailSettings:SmtpPort"]);
            var user = _configuration["EmailSettings:SmtpUser"];
            var pass = _configuration["EmailSettings:SmtpPass"];
            var fromEmail = _configuration["EmailSettings:FromEmail"];

            using var smtpClient = new SmtpClient(host, port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(user, pass)
            };

            using var message = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            message.To.Add(new MailAddress(recipient));

            try
            {
                await smtpClient.SendMailAsync(message);
                _logger.LogInformation("Email sent to {Recipient}", recipient);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Recipient}", recipient);
                throw new Exception("Failed to send email. Please try again later.");
            }
        }
    }
}
