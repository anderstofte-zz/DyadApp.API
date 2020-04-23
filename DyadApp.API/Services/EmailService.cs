using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DyadApp.API.Data;
using DyadApp.API.ViewModels;
using HandlebarsDotNet;
using MailKit.Net.Smtp;
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

        public async Task SendAsync(string signupToken, CreateUserModel model)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("DyadApp support", "support@dyadapp.com"));
            message.To.Add(new MailboxAddress(model.Email));
            message.Subject = "Email verification - DyadApp";
            message.Body = GenerateEmaiLBody(signupToken, model);

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

        private MimeEntity GenerateEmaiLBody(string signupToken, CreateUserModel model)
        {
            var template = System.IO.File.ReadAllText("EmailTemplates\\EmailVerification.html");
            var compiledTemplate = Handlebars.Compile(template);
            var templateData = new
            {
                name = model.Name,
                verifyEmailUrl = "https://localhost:5002/emailverified?token=" + signupToken
            };
            var builder = new BodyBuilder();
            builder.HtmlBody = compiledTemplate(templateData);
            return builder.ToMessageBody();
        }
    }
}
