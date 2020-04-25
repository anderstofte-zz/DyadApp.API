using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DyadApp.API.Converters;
using DyadApp.API.Data;
using DyadApp.API.Models;
using DyadApp.API.Services;
using DyadApp.API.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MimeKit;

namespace DyadApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DyadAppContext _context;
        private readonly IEmailService _emailService;

        public UsersController(DyadAppContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        [HttpGet]
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
                // TODO: returner med en anden besked eller?
                return BadRequest("The email already exists.");
            }

            var user = model.ToUser();
            var currentDateTime = DateTime.Now;

            // TODO: Ryk det her ud i noget generisk, så man ikke skal gøre det ved hvert endpoint
            user.Modified = currentDateTime;
            user.ModifiedBy = 0;
            user.Created = currentDateTime;
            user.CreatedBy = 0;

            var signupToken = GenerateUniqueSignupToken();
            user.Signups.Add(new Signup
            {
                Token = signupToken,
                ExpirationDate = DateTime.UtcNow.AddDays(2),
                AcceptDate = null,
                Modified = currentDateTime,
                ModifiedBy = 0,
                Created = currentDateTime,
                CreatedBy = 0
            });

            _context.Users.Add(user);

            await _emailService.SendAsync(signupToken, model);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool UserWithProvidedEmailAlreadyExists(string email)
        {
            return _context.Users.Any(x => x.Email == email);
        }

        private string GenerateUniqueSignupToken()
        {
            var bytes = new byte[40];
            using (var provider = new RNGCryptoServiceProvider())
                provider.GetBytes(bytes);

            var uniqueToken = BitConverter.ToString(bytes, 0).Replace("-", "");

            return uniqueToken;
        }
    }
}
