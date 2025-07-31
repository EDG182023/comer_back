using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using TarifarioBackend.Models; // Asegúrate de que EmailSettings esté aquí

namespace TarifarioBackend.Services
{
    public class EmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, string? attachmentPath = null)
        {
            using (var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort))
            {
                client.EnableSsl = _emailSettings.EnableSsl;
                client.Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password);

                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName);
                    message.To.Add(toEmail);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = true; // Si el cuerpo es HTML

                    if (!string.IsNullOrEmpty(attachmentPath) && System.IO.File.Exists(attachmentPath))
                    {
                        try
                        {
                            message.Attachments.Add(new Attachment(attachmentPath));
                            _logger.LogInformation($"Adjunto '{attachmentPath}' añadido al correo.");
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, $"Error al adjuntar el archivo '{attachmentPath}'.");
                            // Puedes decidir si lanzar la excepción o simplemente loguear y continuar sin el adjunto
                        }
                    }
                    else if (!string.IsNullOrEmpty(attachmentPath))
                    {
                        _logger.LogWarning($"El archivo adjunto '{attachmentPath}' no existe o la ruta es inválida.");
                    }

                    try
                    {
                        await client.SendMailAsync(message);
                        _logger.LogInformation($"Email enviado a {toEmail} con asunto '{subject}'.");
                    }
                    catch (SmtpException ex)
                    {
                        _logger.LogError(ex, $"Error SMTP al enviar correo a {toEmail}: {ex.Message}");
                        throw; // Re-lanzar para que el controlador pueda manejarlo
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error inesperado al enviar correo a {toEmail}: {ex.Message}");
                        throw;
                    }
                }
            }
        }
    }
}
