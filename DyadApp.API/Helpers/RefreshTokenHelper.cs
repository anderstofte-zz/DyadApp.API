using System;
using System.Security.Cryptography;
using DyadApp.API.Models;

namespace DyadApp.API.Helpers
{
    public class RefreshTokenHelper
    {
        public static RefreshToken Generate(int userId)
        {
            var randomNumber = new byte[32];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);

            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = Convert.ToBase64String(randomNumber),
                ExpirationDate = DateTime.UtcNow.AddDays(31)
            };

            return refreshToken;
        }
    }
}