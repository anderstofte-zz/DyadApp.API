using System.ComponentModel.DataAnnotations;

namespace DyadApp.API.ViewModels
{
    public class AuthenticationUserModel
    {
        private const string RequiredFieldError = "Ikke alle påkrævede felter er udfyldt.";
        private const string EmailRegex = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                          + "@"
                                          + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
        private const string InvalidEmailError = "Indtast en gyldig email-adresse.";

        [Required(ErrorMessage = RequiredFieldError)]
        [RegularExpression(EmailRegex, ErrorMessage = InvalidEmailError)]
        public string Email { get; set; }

        [Required(ErrorMessage = RequiredFieldError)]
        public string Password { get; set; }
    }
}