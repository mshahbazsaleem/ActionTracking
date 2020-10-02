using System.Threading.Tasks;

namespace MAA.ActionTracking.WebHost.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);

        Task SendEmailAsync(string fromEmail, string fromName, string toEmail, string subject, string message);
    }
}
