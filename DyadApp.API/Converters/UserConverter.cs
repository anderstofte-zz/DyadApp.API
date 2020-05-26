using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using DyadApp.API.Models;
using DyadApp.API.ViewModels;

namespace DyadApp.API.Converters
{
    public static class UserConverter
    {
        public static User ToUser(this CreateUserModel model)
        {
            var password = GenerateHashedPassword(model.Password);
            return new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = password.Password,
                Salt = password.Salt,
                ProfileImage = Convert.FromBase64String(model.ProfileImage),
                DateOfBirth = model.DateOfBirth,
                Verified = false,
                Active = true,
                Signups = new List<Signup>()
            };
        }

        public static User ToUser(this UserProfileModel model)
        {
            return new User
            {
                Name = model.Name,
                Email = model.Email,
                ProfileImage = Convert.FromBase64String(model.ProfileImage),
            };
        }

        public static UserProfileModel ToUserProfileModel(this User user)
        {
            return new UserProfileModel
            {
                Name = user.Name,
                Email = user.Email,
                ProfileImage = Convert.ToBase64String(user.ProfileImage) 
            };
        }

        private static UserPassword GenerateHashedPassword(string password)
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
