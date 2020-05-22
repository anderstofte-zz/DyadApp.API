using System.Threading.Tasks;
using DyadApp.API.Models;

namespace DyadApp.API.Data.Repositories
{
    public interface IUserRepository
    {
        Task<bool> DoesUserExists(string email);
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task<User> GetUserById(int id);
        Task<User> GetUserForPasswordUpdate(string token, string email);
        Task<User> GetByEmail(string email);
        Task SaveChangesAsync();
    }
}