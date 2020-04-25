using System;
using System.Diagnostics;
using System.Threading.Tasks;
using DyadApp.API.Data;
using DyadApp.API.ViewModels;
using HandlebarsDotNet;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;

namespace DyadApp.API.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;
        private readonly SmtpOptions _smtpOptions;
        private readonly IConfiguration _configuration;

        public EmailService(IOptions<SmtpOptions> smtpOptions, SmtpClient smtpClient, IConfiguration configuration)
        {
            _smtpOptions = smtpOptions.Value;
            _smtpClient = smtpClient;
            _configuration = configuration;
        }

        public async Task<string> SendAsync(string signupToken, CreateUserModel model)
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
            catch (Exception ex)
            {
                return ex.Message;
            }

            return "";
        }

        private MimeEntity GenerateEmaiLBody(string signupToken, CreateUserModel model)
        {
            var template = System.IO.File.ReadAllText("wwwroot\\EmailTemplates\\EmailVerification.html");
            var compiledTemplate = Handlebars.Compile(template);
            var webAppAddress = _configuration.GetSection("WebAppBaseAddress").Value;
            var templateData = new
            {
                name = model.Name,
                verifyEmailUrl = $"{webAppAddress}emailverified?token=" + signupToken
            };
            var builder = new BodyBuilder {HtmlBody = compiledTemplate(templateData)};
            return builder.ToMessageBody();
        }
    }
}
