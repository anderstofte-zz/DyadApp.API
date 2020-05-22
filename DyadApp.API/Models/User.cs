using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DyadApp.API.Models
{
    public class User : EntityBase
    {
        private const string InvalidEmailError = "Indtast en gyldig email-adresse.";

        private const string EmailRegex = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                          + "@"
                                          + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";
        private const string PasswordComplexityError = "Adgangskoden skal være minimum 6 tegn og indeholde både store og små bogstaver samt tal";
        private const string PasswordPattern = @".*(?=.{6,})(?=(.*[A-Z]){1,})(?=(.*[a-z]){1,})(?=(.*\d){1,}).*";
        private const string RequiredFieldError = "Feltet er påkrævet.";

        public int UserId { get; set; }

        [Required(ErrorMessage = RequiredFieldError)]
        public string Name { get; set; }

        [Required(ErrorMessage = RequiredFieldError)]
        [RegularExpression(EmailRegex, ErrorMessage = InvalidEmailError)]
        public string Email { get; set; }

        [Required(ErrorMessage = RequiredFieldError)]
        [RegularExpression(PasswordPattern, ErrorMessage = PasswordComplexityError)]
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte[] ProfileImage { get; set; }
        public bool Verified { get; set; }
        public bool Active { get; set; }
        public List<Signup> Signups { get; set; }
        public List<RefreshToken> RefreshTokens { get; set; }
        public List<ResetPasswordToken> ResetPasswordTokens { get; set; }
    }
}
