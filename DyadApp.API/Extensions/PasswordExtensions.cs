using System;
using System.Security.Cryptography;
using DyadApp.API.Models;

namespace DyadApp.API.Extensions
{
    public static class PasswordExtensions
    {
        public static bool ValidatePassword(this UserPassword userPassword, string password)
        {
            var savedPasswordInHashBytes = Convert.FromBase64String(userPassword.Password);
            var salt = Convert.FromBase64String(userPassword.Salt);

            Array.Copy(savedPasswordInHashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            var submittedPasswordInHasBytes = pbkdf2.GetBytes(20);

            var isPasswordValid = true;
            var lengthOfSalt = 16;
            for (var i = 0; i < 20; i++)
            {
                if (savedPasswordInHashBytes[i + lengthOfSalt] != submittedPasswordInHasBytes[i])
                {
                    isPasswordValid = false;
                }
            }

            return isPasswordValid;
        }
    }
}