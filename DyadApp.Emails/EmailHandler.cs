using System;
using System.Diagnostics;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DyadApp.Emails
{
    public class EmailHandler : IEmailHandler
    {
        private readonly SmtpOptions _smtpOptions;
        private SmtpClient SmtpClient { get; }

        public EmailHandler(IOptions<SmtpOptions> options, SmtpClient smtpClient)
        {
            var smtpOptions = options;
            this.SmtpClient = smtpClient;
            _smtpOptions = new SmtpOptions
            {
                Host = smtpOptions.Value.Host,
                Port = smtpOptions.Value.Port,
                SecureSocketOptions = smtpOptions.Value.SecureSocketOptions,
                UserName = smtpOptions.Value.UserName,
                Password = smtpOptions.Value.Password
            };
        }

        public async Task Send(MimeMessage email)
        {
            try
            {
                SmtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await SmtpClient.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, _smtpOptions.SecureSocketOptions);
                await SmtpClient.AuthenticateAsync(_smtpOptions.UserName, _smtpOptions.Password);
                await SmtpClient.SendAsync(email);
                await SmtpClient.DisconnectAsync(true);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }
        }
    }
}
