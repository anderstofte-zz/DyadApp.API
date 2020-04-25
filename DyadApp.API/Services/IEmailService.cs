using System.Threading.Tasks;
using DyadApp.API.ViewModels;

namespace DyadApp.API.Services
{
    public interface IEmailService
    {
        Task<string> SendAsync(string signupToken, CreateUserModel model);
    }
}
