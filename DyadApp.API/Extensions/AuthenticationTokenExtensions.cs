using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using DyadApp.API.Models;
using Microsoft.IdentityModel.Tokens;

namespace DyadApp.API.Extensions
{
    public static class AuthenticationTokenExtensions
    {
        public static int GetUserIdFromClaims(this AuthenticationTokens tokens)
        {
            var tokenValidationParameters = GenerateTokenValidationParameters();
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

        private static TokenValidationParameters GenerateTokenValidationParameters()
        {
            var key = Encoding.ASCII.GetBytes(System.IO.File.ReadAllText("key.txt"));
            return new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };
        }
    }
}
