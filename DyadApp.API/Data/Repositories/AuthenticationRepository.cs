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
            return await _context.Signups.Where(x => x.Token == token).SingleOrDefaultAsync();
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.Where(x => x.Email == email).Include(x => x.RefreshTokens)
                .SingleOrDefaultAsync();
        }

        public async Task CreateToken<T>(T entity) where T : class
        {
            _context.Set<T>().Add(entity);
            await SaveChangesAsync();
        }

        public async Task DeleteToken<T>(T entity) where T : class
        {
            _context.Set<T>().Remove(entity);
            await SaveChangesAsync();
        }

        public Task<RefreshToken> GetRefreshToken(int userId, string token)
        {
            return _context.RefreshTokens
                .Where(x => x.UserId == userId && x.Token == token)
                .SingleOrDefaultAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
