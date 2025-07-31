using Microsoft.AspNetCore.Mvc;
using TarifarioBackend.Services;
using System.Net.Mail;

namespace TarifarioBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController : ControllerBase
    {
        private readonly EmailService _emailService;

        public EmailController(EmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail(
            [FromQuery] string to,
            [FromQuery] string subject,
            [FromQuery] string body,
            [FromQuery] string? attachmentPath = null)
        {
            try
            {
                await _emailService.SendEmailAsync(to, subject, body, attachmentPath);
                return Ok("Email sent successfully.");
            }
            catch (SmtpException ex)
            {
                return StatusCode(500, $"Error sending email: {ex.Message}. Check SMTP settings and credentials.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
