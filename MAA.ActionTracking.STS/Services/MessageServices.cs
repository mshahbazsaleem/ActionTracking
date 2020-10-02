using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using MAA.ActionTracking.WebHost.Infrastructures.Constants;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MAA.ActionTracking.WebHost.Services
{
    // This class is used by the application to send Email and SMS
    // when you turn on two-factor authentication in ASP.NET Identity.
    // For more details see this link https://go.microsoft.com/fwlink/?LinkID=532713
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _environment;

        public AuthMessageSender(IConfiguration configuration, IHostingEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            //Make sure all emails are blocked except dartican during development
            if (_environment.IsDevelopment() &&
               !email.EndsWith(_configuration[StringConstants.DarticanDomain]))
            {
                email = $"{email}-x";
            }

            var apiKey = _configuration[StringConstants.SendGridAPIKey];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(_configuration[StringConstants.FromEmail], _configuration[StringConstants.FromName]);
            var to = new EmailAddress(email);
            var plainTextContent = Regex.Replace(message, "<[^>]*>", "");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            var response = await client.SendEmailAsync(msg);
            // Plug in your email service here to send an email.
            //return Task.FromResult(0);
        }

        public async Task SendEmailAsync(string fromEmail, string fromName, string toEmail, string subject, string message)
        {
            //Make sure all emails are blocked except dartican during development
            if (_environment.IsDevelopment() &&
               !toEmail.EndsWith(_configuration[StringConstants.DarticanDomain]))
            {
                toEmail = $"{toEmail}-x";
            }

            var apiKey = _configuration[StringConstants.SendGridAPIKey];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, fromName);
            var to = new EmailAddress(toEmail);
            var plainTextContent = Regex.Replace(message, "<[^>]*>", "");
            var msg = MailHelper.CreateSingleEmail(from, to, subject, message, message);

            var response = await client.SendEmailAsync(msg);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }
}
