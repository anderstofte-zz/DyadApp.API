using DyadApp.Emails.Models.EmailTypes;
using HandlebarsDotNet;
using MimeKit;

namespace DyadApp.Emails
{
    public class EmailGenerator
    {
        public static MimeMessage GenerateEmail<T>(T model, EmailTypeEnum emailType) where T : BaseEmail
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Dyad support", "support@dyadapp.com"));
            message.To.Add(new MailboxAddress(model.To));
            message.Subject = model.Subject;
            message.Body = GenerateEmailBody(model, emailType);

            return message;
        }
        private static MimeEntity GenerateEmailBody<T>(T templateData, EmailTypeEnum emailType) where T : BaseEmail
        {
            var template = emailType switch
            {
                EmailTypeEnum.Verification => System.IO.File.ReadAllText(
                    "wwwroot\\EmailTemplates\\EmailVerification.html"),
                EmailTypeEnum.PasswordRecovery => System.IO.File.ReadAllText(
                    "wwwroot\\EmailTemplates\\PasswordRecovery.html")
            };

            var compiledTemplate = Handlebars.Compile(template);

            var builder = new BodyBuilder { HtmlBody = compiledTemplate(templateData) };
            return builder.ToMessageBody();
        }
    }
}
