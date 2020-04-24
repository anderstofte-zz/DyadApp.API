using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DyadApp.API.Data;
using DyadApp.API.Extensions;
using DyadApp.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DyadApp.API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly DyadAppContext _context;

        public AuthenticationService(DyadAppContext context)
        {
            _context = context;
        }

        public async Task<AuthenticationTokens> Authenticate(string email, string password)
        {
            var user = await _context.Users.Select(u => new User
            {
                UserId = u.UserId,
                Email = u.Email,
                Password = new UserPassword
                {
                    Password = u.Password.Password,
                    Salt = u.Password.Salt
                },
                Verified = true,
                RefreshTokens = u.RefreshTokens
            }).Where(x => x.Email == email && x.Verified).SingleOrDefaultAsync();


            if (user == null)
            {
                return null;
            }

            var isSubmittedPasswordValid = user.Password.ValidatePassword(password);
            if (!isSubmittedPasswordValid)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var accessToken = tokenHandler.CreateToken(SetUpToken(user.UserId));

            var refreshToken = GenerateRefreshToken(user.UserId);
            _context.RefreshTokens.Add(refreshToken);

            await _context.SaveChangesAsync();

            var something = new AuthenticationTokens
            {
                AccessToken = tokenHandler.WriteToken(accessToken),
                RefreshToken = refreshToken.Token
            };

            return something;
        }

        public async Task<AuthenticationTokens> GenerateTokens(int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var accessToken = tokenHandler.CreateToken(SetUpToken(userId));

            var refreshToken = GenerateRefreshToken(userId);
            
            _context.RefreshTokens.Add(refreshToken);

            await _context.SaveChangesAsync();

            var something = new AuthenticationTokens
            {
                AccessToken = tokenHandler.WriteToken(accessToken),
                RefreshToken = refreshToken.Token
            };

            return something;
        }

        private static SecurityTokenDescriptor SetUpToken(int userId)
        {
            var key = Encoding.ASCII.GetBytes(System.IO.File.ReadAllText("key.txt"));
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(2),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
        }

        private RefreshToken GenerateRefreshToken(int userId)
        {
            var randomNumber = new byte[32];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);

            var currentDateTime = DateTime.UtcNow;
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = Convert.ToBase64String(randomNumber),
                ExpirationDate = DateTime.UtcNow.AddDays(31),
                Modified = currentDateTime,
                ModifiedBy = 0,
                Created = currentDateTime,
                CreatedBy = 0
            };

            return refreshToken;
        }
    }
}
