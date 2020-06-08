using System;
using System.Security.Cryptography;
using DyadApp.API.Models;

namespace DyadApp.API.Extensions
{
    public static class PasswordExtensions
    {
        public static bool ValidatePassword(this User user, string password)
        {
            var savedPasswordInHashBytes = ConvertBase64StringToBytes(user.Password);
            var salt = ConvertBase64StringToBytes(user.Salt);

            Array.Copy(savedPasswordInHashBytes, 0, salt, 0, 16);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            var submittedPasswordInHasBytes = pbkdf2.GetBytes(20);

            var isPasswordValid = true;
            const int lengthOfSalt = 16;
            for (var i = 0; i < 20; i++)
            {
                if (savedPasswordInHashBytes[i + lengthOfSalt] != submittedPasswordInHasBytes[i])
                {
                    isPasswordValid = false;
                }
            }

            return isPasswordValid;
        }

        private static byte[] ConvertBase64StringToBytes(string password)
        {
            return Convert.FromBase64String(password);
        }
    }
}
