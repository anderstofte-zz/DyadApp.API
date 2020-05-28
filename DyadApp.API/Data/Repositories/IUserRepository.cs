using System.Threading.Tasks;
using DyadApp.API.Models;

namespace DyadApp.API.Data.Repositories
{
    public interface IUserRepository
    {
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task<User> GetUserById(int id);
        Task<User> GetUserWithResetTokensByEmail(string email);
        Task<User> GetUserByEmail(string email);
        Task SaveChangesAsync();
    }
}
