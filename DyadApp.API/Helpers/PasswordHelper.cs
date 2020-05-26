using System;
using System.Security.Cryptography;
using DyadApp.API.Models;

namespace DyadApp.API.Helpers
{
    public class PasswordHelper
    {
        public static UserPassword GenerateHashedPassword(string password)
        {
            var salt = new byte[16];
            using (var provider = new RNGCryptoServiceProvider())
                provider.GetBytes(salt);

            var hashedPasswordAndSalt = new Rfc2898DeriveBytes(password, salt, 10000);

            var hash = hashedPasswordAndSalt.GetBytes(20);

            var hashBytes = new byte[36];

            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            return new UserPassword
            {
                Password = Convert.ToBase64String(hashBytes),
                Salt = Convert.ToBase64String(salt),
            };
        }
    }
}
