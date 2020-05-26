using MailKit.Security;

namespace DyadApp.API.Data
{
    public class SmtpOptions
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public SecureSocketOptions SecureSocketOptions { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
