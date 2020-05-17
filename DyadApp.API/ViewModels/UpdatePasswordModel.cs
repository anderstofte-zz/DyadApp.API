using System.ComponentModel.DataAnnotations;

namespace DyadApp.API.ViewModels
{
    public class UpdatePasswordModel
    {
        private const string PasswordComplexityError = "Den nye adgangskode skal består af minimum seks karaktere, bestående af små og store bogstaver samt tal.";
        private const string PasswordPattern = @".*(?=.{6,})(?=(.*[A-Z]){1,})(?=(.*[a-z]){1,})(?=(.*\d){1,}).*";
        private const string RequiredFieldError = "Ikke alle påkrævede felter er udfyldt.";
        private const string PasswordConfirmationError = "Den samme nye adgangskode skal indtastes to gange";
        private const string EmailRegex = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                          + "@"
                                          + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
        private const string InvalidEmailError = "Indtast en gyldig email-adresse.";

        [Required(ErrorMessage = RequiredFieldError)]
        [RegularExpression(PasswordPattern, ErrorMessage = PasswordComplexityError)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = RequiredFieldError)]
        [Compare(nameof(Models.NewPassword), ErrorMessage = PasswordConfirmationError)]
        public string ConfirmNewPassword { get; set; }

        [Required(ErrorMessage = RequiredFieldError)]
        public string Token { get; set; }

        [Required(ErrorMessage = RequiredFieldError)]
        [RegularExpression(EmailRegex, ErrorMessage = InvalidEmailError)]
        public string Email { get; set; }
    }
}