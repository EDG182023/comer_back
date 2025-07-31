using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using TarifarioBackend.Models; // Assuming EmailSettings is here

namespace TarifarioBackend.Helpers
{
    // This class is no longer needed as EmailService handles email sending directly.
    // If you had custom email sending logic here, you might want to integrate it into EmailService
    // or keep it if it serves other purposes. For now, I'm assuming it's redundant.
    public class EmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string message)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress(_emailSettings.SenderEmail);
                mail.To.Add(toEmail);
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
                {
                    smtp.Credentials = new NetworkCredential(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
                    smtp.EnableSsl = _emailSettings.EnableSsl;
                    await smtp.SendMailAsync(mail);
                }
            }
        }
    }
}
