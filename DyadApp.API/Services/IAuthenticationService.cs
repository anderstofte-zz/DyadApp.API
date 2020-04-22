using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DyadApp.API.Services
{
    public interface IAuthenticationService
    {
        Task<string> Authenticate(string email, string password);
    }
}