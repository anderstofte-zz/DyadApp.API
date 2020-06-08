using System.Threading.Tasks;
using DyadApp.API.ViewModels;

namespace DyadApp.API.Services
{
    public interface IUserService
    {
        Task CreateUser(CreateUserModel model, string signupToken);
    }
}
