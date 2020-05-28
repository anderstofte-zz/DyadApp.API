using System.Threading.Tasks;
using DyadApp.Emails;
using Microsoft.Extensions.Configuration;
using EmailData = DyadApp.Emails.Models.EmailData;

namespace DyadApp.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailHandler _emailHandler;
        public EmailService(IConfiguration configuration, IEmailHandler emailHandler)
        {
            _configuration = configuration;
            _emailHandler = emailHandler;
        }

        public async Task SendEmail(EmailData data)
        {
            var webAppBaseUrl = _configuration.GetSection("WebAppBaseAddress").Value;
            var email = Emails.EmailGenerator.GenerateEmail(data, webAppBaseUrl);
            await _emailHandler.Send(email);
        }
    }
}
