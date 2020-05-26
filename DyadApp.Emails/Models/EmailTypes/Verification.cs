using System.Reflection.Metadata;

namespace DyadApp.Emails.Models.EmailTypes
{
    public class Verification : BaseEmail
    {
        public string Name { get; set; }
        public string Token { get; set; }
        public string VerifySignupUrl { get; set; }
    }
}