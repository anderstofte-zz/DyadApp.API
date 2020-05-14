using System;
using System.Threading.Tasks;
using DyadApp.API.Data;
using DyadApp.API.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using DyadApp.Emails;
using DyadApp.Emails.Models.EmailTypes;
using Microsoft.AspNetCore.Mvc;

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

        public async Task<ActionResult> SendEmail<T>(T model, EmailTypeEnum emailType)
        {
            BaseEmail emailConfiguration = emailType switch
            {
                EmailTypeEnum.Verification => ToVerificationEmail(model as User),
                EmailTypeEnum.PasswordRecovery => ToRecoveryEmail(model as ResetPasswordRequest),
                _ => throw new ArgumentOutOfRangeException(nameof(emailType), emailType, null)
            };

            emailConfiguration.WebAppAddress = _configuration.GetSection("WebAppBaseAddress").Value;
            var email = EmailGenerator.GenerateEmail(emailConfiguration, emailType);

            try
            {
                _smtpClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await _smtpClient.ConnectAsync(_smtpOptions.Host, _smtpOptions.Port, _smtpOptions.SecureSocketOptions);
                await _smtpClient.AuthenticateAsync(_smtpOptions.UserName, _smtpOptions.Password);
                await _smtpClient.SendAsync(email);
                await _smtpClient.DisconnectAsync(true);
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }

            return new OkResult();
        }

        private PasswordRecovery ToRecoveryEmail(ResetPasswordRequest model)
        {
            return new PasswordRecovery
            {
                To = model.Email,
                Subject = "Gendan adgangskode - Dyad",
                ResetPasswordUrl = _configuration.GetSection("WebAppBaseAddress").Value + "resetpassword?token=" + model.Token
            };
        }

        public Verification ToVerificationEmail(User model)
        {
            return new Verification
            {
                Name = model.Name,
                To = model.Email,
                Subject = "Email verificering - Dyad",
                VerifySignupUrl = _configuration.GetSection("WebAppBaseAddress").Value + "email-verified?token=" + model.Signups[0].Token
            };
        }
    }
}
