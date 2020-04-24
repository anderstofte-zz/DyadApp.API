using System.Threading.Tasks;
using DyadApp.API.Models;

namespace DyadApp.API.Services
{
    public interface IAuthenticationService
    {
        Task<AuthenticationTokens> Authenticate(string email, string password);
        Task<AuthenticationTokens> GenerateTokens(int userId);
    }
}