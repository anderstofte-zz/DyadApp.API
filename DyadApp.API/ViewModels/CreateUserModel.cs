using System;
using System.ComponentModel.DataAnnotations;

namespace DyadApp.API.ViewModels
{
    public class CreateUserModel
    {
        private const string InvalidEmailError = "Indtast en gyldig email-adresse.";

        private const string EmailRegex = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                          + "@"
                                          + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
        private const string PasswordComplexityError = "Adgangskoden skal være minimum 6 tegn og indeholde både store og små bogstaver samt tal";
        private const string PasswordPattern = @".*(?=.{6,})(?=(.*[A-Z]){1,})(?=(.*[a-z]){1,})(?=(.*\d){1,}).*";

        public string Name { get; set; }

        [RegularExpression(EmailRegex, ErrorMessage = InvalidEmailError)]
        public string Email { get; set; }

        [Required]
        [RegularExpression(PasswordPattern, ErrorMessage = PasswordComplexityError)]
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ProfileImage { get; set; }
    }
}