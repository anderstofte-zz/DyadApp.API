using System.Threading.Tasks;
using DyadApp.API.ViewModels;

namespace DyadApp.API.Services
{
    public interface IEmailService
    {
        Task SendAsync(string signupToken, CreateUserModel model);
    }
}
