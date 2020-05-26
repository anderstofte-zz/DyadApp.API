using System.ComponentModel.DataAnnotations;

namespace DyadApp.API.ViewModels
{
    public class PasswordModel
    {
        private const string PasswordComplexityError = "Den nye adgangskode skal består af minimum seks karaktere, bestående af små og store bogstaver samt tal.";
        private const string PasswordPattern = @".*(?=.{6,})(?=(.*[A-Z]){1,})(?=(.*[a-z]){1,})(?=(.*\d){1,}).*";
        private const string RequiredFieldError = "Ikke alle påkrævede felter er udfyldt.";
        private const string PasswordConfirmationError = "Den samme nye adgangskode skal indtastes to gange";

        [Required(ErrorMessage = RequiredFieldError)]
        public string CurrentPassword { get; set; }
        [Required(ErrorMessage = RequiredFieldError)]
        [RegularExpression(PasswordPattern, ErrorMessage = PasswordComplexityError)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = RequiredFieldError)]
        [Compare(nameof(NewPassword), ErrorMessage = PasswordConfirmationError)]
        public string ConfirmNewPassword { get; set; }
    }
}