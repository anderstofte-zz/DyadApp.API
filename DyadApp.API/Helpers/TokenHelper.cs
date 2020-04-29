using System;
using System.Security.Cryptography;

namespace DyadApp.API.Helpers
{
    public class TokenHelper
    {
        public static string GenerateSignupToken()
        {
            var bytes = new byte[40];
            using (var provider = new RNGCryptoServiceProvider())
                provider.GetBytes(bytes);

            var token = BitConverter.ToString(bytes, 0).Replace("-", "");

            return token;
        }

        public static string GenerateResetPasswordToken()
        {
            var bytes = new byte[40];
            using (var provider = new RNGCryptoServiceProvider())
                provider.GetBytes(bytes);

            var token = BitConverter.ToString(bytes, 0).Replace("-", "");
            return token;
        }
    }
}