using System.Threading.Tasks;

namespace DyadApp.API.Services
{
    public interface IAuthenticationService
    {
        Task<string> Authenticate(string email, string password);

    }
}