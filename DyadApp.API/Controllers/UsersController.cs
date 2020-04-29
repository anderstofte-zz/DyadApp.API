using System;
using System.Threading.Tasks;
using DyadApp.API.Converters;
using DyadApp.API.Data.Repositories;
using DyadApp.API.Helpers;
using DyadApp.API.Models;
using DyadApp.API.Services;
using DyadApp.API.ViewModels;
using DyadApp.Emails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DyadApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _repository;

        public UsersController(IEmailService emailService, IUserRepository repository)
        {
            _emailService = emailService;
            _repository = repository;
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
            if (!ModelState.IsValid || await UserWithProvidedEmailAlreadyExists(model.Email))
            {
                return BadRequest();
            }

            var user = model.ToUser();

            var signupToken = TokenHelper.GenerateSignupToken();
            user.Signups.Add(new Signup
            {
                Token = signupToken,
                ExpirationDate = DateTime.UtcNow.AddDays(2),
                AcceptDate = null
            });

            await _repository.CreateAsync(user);

            return await _emailService.SendEmail(user, EmailTypeEnum.Verification);
        }

        private async Task<bool> UserWithProvidedEmailAlreadyExists(string email)
        {
            return await _repository.DoesUserExists(email);
        }
    }
}
