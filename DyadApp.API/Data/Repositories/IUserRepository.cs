using System.Threading.Tasks;
using DyadApp.API.Models;

namespace DyadApp.API.Data.Repositories
{
    public interface IUserRepository
    {
        Task<bool> DoesUserExists(string email);
        Task CreateAsync(User user);
        Task<User> GetUserById(int id);
        Task<UserPassword> GetUserPasswordByUserId(int id);
        Task<User> GetByEmail(string email);
        Task SaveChangesAsync();
    }
}