using System;
using System.Linq;
using System.Threading.Tasks;
using DyadApp.API.Converters;
using DyadApp.API.Data;
using DyadApp.API.Services;
using DyadApp.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DyadApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DyadAppContext _context;
        private readonly IAuthenticationService _auth;
        public UsersController(DyadAppContext context, IAuthenticationService auth)
        {
            _context = context;
            _auth = auth;
        }

        public string TestAuthorization()
        {
            return "Authorized!";
        }

        
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            if (UserWithProvidedEmailAlreadyExists(model.Email))
            {
                return BadRequest("The email already exists.");
            }

            var currentDateTime = DateTime.Now;
            var user = model.ToUser();
            user.Modified = currentDateTime;
            user.ModifiedBy = 0;
            user.Created = currentDateTime;
            user.CreatedBy = 0;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = await _auth.Authenticate(model.Email, model.Password);

            return Ok(token);
        }

        private bool UserWithProvidedEmailAlreadyExists(string email)
        {
            return _context.Users.Any(x => x.Email == email);
        }
    }
}
