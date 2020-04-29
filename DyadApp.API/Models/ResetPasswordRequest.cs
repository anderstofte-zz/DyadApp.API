namespace DyadApp.API.Models
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}