using System;

namespace DyadApp.API.ViewModels
{
    public class CreateUserModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ProfileImage { get; set; }
    }
}