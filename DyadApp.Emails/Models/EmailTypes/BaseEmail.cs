namespace DyadApp.Emails.Models.EmailTypes
{
    public class BaseEmail
    {
        public string WebAppAddress { get; set; }
        public string Subject { get; set; }
        public string To { get; set; }
        public string FromAddress { get; set; }
        public string FromName { get; set; }
    }
}