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
        private const string RequiredFieldError = "Ikke alle påkrævede felter er udfyldt.";

        [Required(ErrorMessage = RequiredFieldError)]
        public string Name { get; set; }

        [Required(ErrorMessage = RequiredFieldError)]
        [RegularExpression(EmailRegex, ErrorMessage = InvalidEmailError)]
        public string Email { get; set; }

        [Required(ErrorMessage = RequiredFieldError)]
        [RegularExpression(PasswordPattern, ErrorMessage = PasswordComplexityError)]
        public string Password { get; set; }

        [Required(ErrorMessage = RequiredFieldError)]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = RequiredFieldError)]
        public string ProfileImage { get; set; }
    }
}