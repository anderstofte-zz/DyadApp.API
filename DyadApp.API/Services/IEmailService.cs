using System.Threading.Tasks;
using DyadApp.API.ViewModels;

namespace DyadApp.API.Services
{
    public interface IEmailService
    {
        Task<bool> SendAsync(string signupToken, CreateUserModel model);
    }
}
