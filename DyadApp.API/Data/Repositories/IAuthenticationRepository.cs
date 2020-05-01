using System.Threading.Tasks;
using DyadApp.API.Models;

namespace DyadApp.API.Data.Repositories
{
    public interface IAuthenticationRepository
    {
        Task<Signup> GetSignupByToken(string token);
        Task<RefreshToken> GetRefreshToken(int userId, string token);
        Task<ResetPasswordToken> GetResetPasswordToken(string token);
        Task<User> AuthenticateUser(string email, string password);
        Task<User> GetUserCredentialsByEmail(string email);
        Task CreateTokenAsync<T>(T entity) where T : class;
        Task DeleteTokenAsync<T>(T entity) where T : class;
        Task SaveChangesAsync();

    }
}