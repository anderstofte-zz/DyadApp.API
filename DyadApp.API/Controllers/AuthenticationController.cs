using System.Threading.Tasks;
using DyadApp.API.Data;
using DyadApp.API.Services;
using Microsoft.AspNetCore.Mvc;

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
    }
}
