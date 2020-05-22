namespace DyadApp.Emails.Models.EmailTypes
{
    public class PasswordRecovery : BaseEmail
    {
        public string ResetPasswordUrl { get; set; }
    }
}