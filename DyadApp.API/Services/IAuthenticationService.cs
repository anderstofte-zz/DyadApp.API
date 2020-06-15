using System.Threading.Tasks;
using DyadApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DyadApp.API.Services
{
    public interface IAuthenticationService
    {
        Task<IActionResult> Authenticate(string email, string password);
        Task<Signup> GetSignup(string token);
        Task<IActionResult> VerifySignup(Signup signup);
        bool IsRefreshTokenValid(RefreshToken token);
        Task<AuthenticationTokens> GenerateTokens(int userId);
    }
}