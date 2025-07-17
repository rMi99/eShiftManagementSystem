using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using eShiftManagementSystem.Business.Interfaces;

namespace eShiftManagementSystem.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly string _fromEmail;

        public EmailService(IConfiguration configuration)
        {
            var smtpSettings = configuration.GetSection("SmtpSettings");

            _fromEmail = smtpSettings["FromEmail"];

            _smtpClient = new SmtpClient(smtpSettings["Host"])
            {
                Port = int.Parse(smtpSettings["Port"]),
                EnableSsl = bool.Parse(smtpSettings["EnableSsl"]),
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"]),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(toEmail))
                {
                    throw new ArgumentException("Recipient email address cannot be null or empty.", nameof(toEmail));
                }

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                await _smtpClient.SendMailAsync(mailMessage);

                // Optional: UI feedback (can be removed in production)
                MessageBox.Show("Email sent successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send email: {ex.Message}", "Email Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
