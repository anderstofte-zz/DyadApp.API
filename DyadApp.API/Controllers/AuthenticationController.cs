using System;
using System.Linq;
using System.Threading.Tasks;
using DyadApp.API.Data;
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

        public AuthenticationController(DyadAppContext context, IAuthenticationService auth)
        {
            _context = context;
            _auth = auth;
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticateUser(AuthenticationUserModel model)
        {
            var token = await _auth.Authenticate(model.Email, model.Password);

            if (token == null)
            {
                return Unauthorized();
            }

            return Ok(token);
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
    }
}
