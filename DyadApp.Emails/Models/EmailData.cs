namespace DyadApp.Emails.Models
{
    public class EmailData
    {
        public EmailData(string token, string email, EmailTypeEnum type)
        {
            Token = token;
            Email = email;
            Type = type;
        }

        public EmailData()
        {
        }

        public string Token { get; set; }
        public string Email { get; set; }
        public string UserData { get; set; }
        public EmailTypeEnum Type { get; set; }
    }
}
