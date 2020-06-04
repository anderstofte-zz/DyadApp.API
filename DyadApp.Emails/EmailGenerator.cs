using DyadApp.Emails.Models;
using MimeKit;

namespace DyadApp.Emails
{
    public static class EmailGenerator
    {
        public static MimeMessage GenerateEmail(EmailData data, string webAppBaseUrl)
        {
            switch (data.Type)
            {
                case EmailTypeEnum.Verification:
                    return EmailFactory.CreateVerification(data, webAppBaseUrl);
                case EmailTypeEnum.DataInsight:
                    return EmailFactory.CreateDataInsight(data);
            }
            return data.Type == EmailTypeEnum.Verification ? 
                EmailFactory.CreateVerification(data, webAppBaseUrl) : 
                EmailFactory.CreateResetPassword(data, webAppBaseUrl);
        }
    }
}
