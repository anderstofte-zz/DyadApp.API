using DyadApp.Emails.Models;
using MimeKit;

namespace DyadApp.Emails
{
    public static class EmailGenerator
    {
        public static MimeMessage GenerateEmail(EmailData data, string webAppBaseUrl)
        {
            return data.Type == EmailTypeEnum.Verification ? 
                EmailFactory.CreateVerification(data, webAppBaseUrl) : 
                EmailFactory.CreateResetPassword(data, webAppBaseUrl);
        }
    }
}
