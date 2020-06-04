using System.Threading.Tasks;
using DyadApp.API.Models;

namespace DyadApp.API.Data.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<Signup> GetSignupByToken(string token);
        Task<RefreshToken> GetRefreshToken(int userId, string token);
        Task<User> GetUserByEmail(string email);
        Task CreateToken<T>(T entity) where T : class;
        Task DeleteToken<T>(T entity) where T : class;
        Task SaveChangesAsync();

    }
}