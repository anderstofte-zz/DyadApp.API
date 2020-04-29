using System;
using System.Linq;
using System.Threading.Tasks;
using DyadApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DyadApp.API.Data.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly DyadAppContext _context;

        public AuthenticationRepository(DyadAppContext context)
        {
            _context = context;
        }

        public async Task<Signup> GetSignupByToken(string token)
        {
            return await _context.Signups
                .Where(s => s.Token == token && s.ExpirationDate > DateTime.UtcNow && s.AcceptDate == null)
                .SingleOrDefaultAsync();
        }

        public async Task<User> AuthenticateUser(string email, string password)
        {
            return await _context.Users.Select(u => new User
            {
                UserId = u.UserId,
                Email = u.Email,
                Password = new UserPassword
                {
                    Password = u.Password.Password,
                    Salt = u.Password.Salt
                },
                Verified = u.Verified,
                RefreshTokens = u.RefreshTokens
            }).Where(x => x.Email == email && x.Verified).SingleOrDefaultAsync();
        }

        public async Task CreateTokenAsync<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTokenAsync<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public Task<RefreshToken> GetRefreshToken(int userId, string token)
        {
            return _context.RefreshTokens
                .Where(x => x.UserId == userId && x.Token == token)
                .SingleOrDefaultAsync();
        }

        public async Task<ResetPasswordToken> GetResetPasswordToken(string token)
        {
            return await _context.ResetPasswordTokens
                .Where(x => x.Token == token && x.ExpirationDate > DateTime.UtcNow)
                .SingleOrDefaultAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}