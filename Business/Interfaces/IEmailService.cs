using System.Threading.Tasks;

namespace eShiftManagementSystem.Business.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="toEmail">Recipient email address.</param>
        /// <param name="subject">Email subject.</param>
        /// <param name="body">HTML email body.</param>
        /// <returns>A Task representing the async operation.</returns>
        Task SendEmailAsync(string toEmail, string subject, string body);
    }
}
