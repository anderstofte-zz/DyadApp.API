using System;
using System.Linq;
using System.Threading.Tasks;
using DyadApp.API.Converters;
using DyadApp.API.Data.Repositories;
using DyadApp.API.Extensions;
using DyadApp.API.Helpers;
using DyadApp.API.Models;
using DyadApp.API.Services;
using DyadApp.API.ViewModels;
using DyadApp.Emails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;

namespace DyadApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly IUserRepository _repository;
        private readonly IUserRepository _userRepository;
        public UsersController(IEmailService emailService, IUserRepository repository, IUserRepository userRepository)
        {
            _emailService = emailService;
            _repository = repository;
            _userRepository = userRepository;
        }

        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            var userId = User.GetUserId();
            var user = await _userRepository.GetUserById(userId);
            var userProfile = user.ToUserProfileModel();
            return Ok(userProfile);
        }

        [AllowAnonymous]
        [HttpPost("VerifyCredentials")]
        public IActionResult VerifySignupCredentials(SignupCredentialsModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.FirstError());
            }

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.FirstError());
            }

            if (await UserWithProvidedEmailAlreadyExists(model.Email))
            {
                return BadRequest("En bruger med den indtastede email findes allerede.");
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

        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] JsonPatchDocument<User> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest(ModelState);
            }

            var userId = User.GetUserId();
            var user = await _userRepository.GetUserById(userId);

            if (user == null)
            {
                return BadRequest();
            }

            patchDoc.ApplyTo(user, ModelState);
            TryValidateModel(user);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.FirstError());
            }

            if (patchDoc.Operations.Any(x => x.path == "/email"))
            {
                var existingUserWithSubmittedEmail = await _userRepository.GetByEmail(user.Email);
                if (existingUserWithSubmittedEmail != null)
                {
                    return BadRequest("Den angivede email er tilknyttet en eksisterende bruger.");
                }
            }

            await _userRepository.SaveChangesAsync();

            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("IsEmailInUse")]
        public async Task<IActionResult> CheckIfEmailIsInUse([FromBody] string email)
        {
            var isEmailInUse = await UserWithProvidedEmailAlreadyExists(email);

            return !isEmailInUse ? (IActionResult) Ok() : BadRequest();
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] PasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.FirstError());
            }

            var userId = User.GetUserId();
            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return BadRequest();
            }

            var submittedCurrentPasswordIsValid = user.ValidatePassword(model.CurrentPassword);
            if (!submittedCurrentPasswordIsValid)
            {
                return BadRequest("Den angivede nuværende adgangskode er forkert.");
            }

            var newPasswordHashed = PasswordHelper.GenerateHashedPassword(model.NewPassword);
            user.Password = newPasswordHashed.Password;
            user.Salt = newPasswordHashed.Salt;

            await _userRepository.SaveChangesAsync();

            return Ok();

        }
        private async Task<bool> UserWithProvidedEmailAlreadyExists(string email)
        {
            return await _repository.DoesUserExists(email);
        }
    }
}
