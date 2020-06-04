using DyadApp.Emails.Models;
using HandlebarsDotNet;
using MimeKit;

namespace DyadApp.Emails
{
    public static class EmailFactory
    {
        public static MimeMessage CreateVerification(EmailData data, string webAppBaseUrl)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Dyad support", "support@dyadapp.com"));
            message.To.Add(new MailboxAddress(data.Email));
            message.Subject = "Verificer konto";
            message.Body = GenerateEmailBody(data, webAppBaseUrl);

            return message;
        }

        public static MimeMessage CreateResetPassword(EmailData data, string webAppBaseUrl)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Dyad support", "support@dyadapp.com"));
            message.To.Add(new MailboxAddress(data.Email));
            message.Subject = "Ny adgangskode";
            message.Body = GenerateEmailBody(data, webAppBaseUrl);

            return message;
        }

        private static MimeEntity GenerateEmailBody(EmailData data, string webAppBaseUrl)
        {
            string template;
            if (data.Type == EmailTypeEnum.Verification)
            {
                template = System.IO.File.ReadAllText("wwwroot\\EmailTemplates\\EmailVerification.html");
                Handlebars.RegisterHelper("signupLink", (writer, context, parameters) =>
                {
                    writer.WriteSafeString($"{webAppBaseUrl}email-verified?token={data.Token}");
                });
            }
            else
            {
                template = System.IO.File.ReadAllText("wwwroot\\EmailTemplates\\PasswordRecovery.html");
                Handlebars.RegisterHelper("resetPasswordUrl", (writer, context, parameters) =>
                {
                    writer.WriteSafeString($"{webAppBaseUrl}reset-password?token={data.Token}");
                });
            }
            
            var compiledTemplate = Handlebars.Compile(template);

            var builder = new BodyBuilder { HtmlBody = compiledTemplate(data) };
            return builder.ToMessageBody();
        }

        public static MimeMessage CreateDataInsight(EmailData data)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Dyad support", "support@dyadapp.com"));
            message.To.Add(new MailboxAddress(data.Email));
            message.Subject = "Ny adgangskode";

            var template = System.IO.File.ReadAllText("wwwroot\\EmailTemplates\\DataInsight.html");

            var compiledTemplate = Handlebars.Compile(template);

            var builder = new BodyBuilder { HtmlBody = compiledTemplate(data) };

            builder.Attachments.Add("user-data.json", System.Text.Encoding.UTF8.GetBytes(data.UserData), new ContentType("application", "json"));

            message.Body = builder.ToMessageBody();

            return message;
        }
    }
}
