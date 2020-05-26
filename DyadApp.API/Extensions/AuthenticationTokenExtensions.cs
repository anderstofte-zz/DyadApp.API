using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using DyadApp.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace DyadApp.API.Extensions
{
    public static class AuthenticationTokenExtensions
    {
        public static int GetUserIdFromClaims(this AuthenticationTokens tokens, string key)
        {
            var tokenValidationParameters = GenerateTokenValidationParameters(key);
            var tokenHandler = new JwtSecurityTokenHandler();

            var claims = tokenHandler.ValidateToken(tokens.AccessToken, tokenValidationParameters, out var validatedToken);

            if (!(validatedToken is JwtSecurityToken jwtSecurityToken) ||
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return int.Parse(claims.Identity.Name);
        }

        private static TokenValidationParameters GenerateTokenValidationParameters(string key)
        {
            var encodedKey = Encoding.ASCII.GetBytes(key);
            return new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(encodedKey),
                ValidateLifetime = false
            };
        }
    }
}
