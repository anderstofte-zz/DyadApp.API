using System;
using System.Linq;
using System.Threading.Tasks;
using DyadApp.API.Data;
using DyadApp.API.Extensions;
using DyadApp.API.Models;
using DyadApp.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DyadApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly DyadAppContext _context;
        private readonly IAuthenticationService _auth;
        private readonly ISecretKeyService _keyService;
        public AuthenticationController(DyadAppContext context, IAuthenticationService auth, ISecretKeyService keyService)
        {
            _context = context;
            _auth = auth;
            _keyService = keyService;
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticateUser(AuthenticationUserModel model)
        {
            var something = await _auth.Authenticate(model.Email, model.Password);

            if (something == null)
            {
                return Unauthorized();
            }

            return Ok(something);
        }

        [HttpPost("VerifySignupToken")]
        public async Task<bool> VerifyUser([FromBody] string token)
        {
            var signup = await _context.Signups
                .Where(s => s.Token == token && s.ExpirationDate > DateTime.UtcNow && s.AcceptDate == null)
                .SingleOrDefaultAsync();

            if (signup == null)
            {
                return false;
            }

            var user = await _context.Users.Where(u => u.UserId == signup.UserId).SingleOrDefaultAsync();

            signup.AcceptDate = DateTime.UtcNow;
            user.Verified = true;
            await _context.SaveChangesAsync();

            return true;
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> RefreshTokens([FromBody]AuthenticationTokens authenticationTokens)
        {
            int userId;
            try
            {
                userId = authenticationTokens.GetUserIdFromClaims(_keyService.GetSecretKey());
            }
            catch (Exception)
            {
                return Unauthorized("Access token is invalid.");
            }

            var refreshToken = GetRefreshToken(authenticationTokens.RefreshToken, userId);
            if (refreshToken.Result == null)
            {
                return Unauthorized("Refresh token is invalid.");
            }

            var newTokens = await _auth.GenerateTokens(userId);

            _context.RefreshTokens.Remove(refreshToken.Result);
            await _context.SaveChangesAsync();

            return Ok(newTokens);
        }

        private async Task<RefreshToken> GetRefreshToken(string refreshToken, int userId)
        {
            return await _context.RefreshTokens
                .Where(x => x.UserId == userId && x.Token == refreshToken)
                .SingleOrDefaultAsync();
        }
    }
}
