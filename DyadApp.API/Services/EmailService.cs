using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DyadApp.API.Data;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DyadApp.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly SmtpOptions _smtpOptions;

        public EmailService(IOptions<SmtpOptions> smtpOptions, SmtpClient smtpClient)
        {
            _smtpOptions = smtpOptions.Value;
            _smtpClient = smtpClient;
        }

        public async Task SendAsync()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("DyadApp support", "support@dyadapp.com"));
            message.To.Add(new MailboxAddress("andersrta@gmail.com"));
            message.Subject = "Email verification - DyadApp";


            var bodyBuilder = new BodyBuilder();
            bodyBuilder.TextBody = "Sådan!";
            message.Body = bodyBuilder.ToMessageBody();

            try
            {

                _smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await _smtpClient.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, _smtpOptions.SecureSocketOptions);
                await _smtpClient.AuthenticateAsync(_smtpOptions.UserName, _smtpOptions.Password);
                await _smtpClient.SendAsync(message);
                await _smtpClient.DisconnectAsync(true);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }
    }
}
