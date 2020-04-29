using DyadApp.API.Models;

namespace DyadApp.API.ViewModels
{
    public class UpdatePasswordModel
    {
        public string Password { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
        public User User { get; set; }
    }
}