using System.Threading.Tasks;
using DyadApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DyadApp.API.Services
{
    public interface IAuthenticationService
    {
        Task<IActionResult> Authenticate(string email, string password);
        Task<AuthenticationTokens> GenerateTokens(int userId);
    }
}