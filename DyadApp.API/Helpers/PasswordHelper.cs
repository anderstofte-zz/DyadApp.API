using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DyadApp.API.Models;
using Microsoft.AspNetCore.Identity;

namespace DyadApp.API.Helpers
{
    public class PasswordHelper
    {
        public static string GenerateRandomPassword()
        {
            var opts = new PasswordOptions()
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = false,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",
                "abcdefghijkmnopqrstuvwxyz",
                "0123456789"
            };

            var random = new Random();
            var chars = new List<char>();

            if (opts.RequireUppercase)
            {
                chars.Insert(random.Next(0, chars.Count), 
                    randomChars[0][random.Next(0, randomChars[0].Length)]);
            }

            if (opts.RequireLowercase)
            {
                chars.Insert(random.Next(0, chars.Count),
                    randomChars[1][random.Next(0, randomChars[1].Length)]);
            }

            if (opts.RequireDigit)
            {
                chars.Insert(random.Next(0, chars.Count),
                    randomChars[2][random.Next(0, randomChars[2].Length)]);
            }

            for (var i = chars.Count; i < opts.RequiredLength
                                      || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                var rcs = randomChars[random.Next(0, randomChars.Length)];
                chars.Insert(random.Next(0, chars.Count),
                    rcs[random.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

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
