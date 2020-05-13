using System;
using System.ComponentModel.DataAnnotations;

namespace DyadApp.API.ViewModels
{
    public class CreateUserModel
    {
        private const string PasswordPattern = @".*(?=.{6,})(?=(.*[A-Z]){1,})(?=(.*[a-z]){1,})(?=(.*\d){1,}).*";

        public string Name { get; set; }
        public string Email { get; set; }

        [Required]
        [RegularExpression(PasswordPattern)]
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ProfileImage { get; set; }
    }
}