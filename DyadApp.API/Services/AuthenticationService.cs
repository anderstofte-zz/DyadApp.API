using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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

        public async Task<string> Authenticate(string email, string password)
        {
            var user = await _context.Users.Select(u => new User
            {
                Email = u.Email,
                Password = new UserPassword
                {
                    Password = u.Password.Password,
                    Salt = u.Password.Salt
                },
                Verified = true
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
            var token = tokenHandler.CreateToken(SetUpToken(user));
            return tokenHandler.WriteToken(token);
        }

        private static SecurityTokenDescriptor SetUpToken(User user)
        {
            var key = Encoding.ASCII.GetBytes(System.IO.File.ReadAllText("key.txt"));
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserId.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                IssuedAt = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
        }
    }
}
