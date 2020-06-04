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
using DyadApp.Emails.Models;
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
        private readonly IUserRepository _userRepository;
        private readonly ILoggingService _loggingService;
        private readonly IUserService _userService;

        public UsersController(IEmailService emailService, IUserRepository userRepository, ILoggingService loggingService, IUserService userService)
        {
            _emailService = emailService;
            _userRepository = userRepository;
            _loggingService = loggingService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> UserProfile()
        {
            var userId = User.GetUserId();
            await _loggingService.SaveAuditLog($"Retrieving profile for user with user id {userId}",
                AuditActionEnum.Read);

            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return BadRequest();
            }

            var userProfile = user.ToUserProfileModel();
            return Ok(userProfile);
        }

        [AllowAnonymous]
        [HttpPost("VerifyCredentials")]
        public async Task<IActionResult> VerifySignupCredentials(SignupCredentialsModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.FirstError());
            }

            if (!await IsEmailUnique(model.Email))
            {
                return BadRequest("En bruger med den indtastede email findes allerede.");
            }

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> CreateUser([FromBody] CreateUserModel model)
        {
            await _loggingService.SaveAuditLog("Creating user process initiated.", AuditActionEnum.Create);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.FirstError());
            }

            if (!await IsEmailUnique(model.Email))
            {
                return BadRequest("En bruger med den indtastede email findes allerede.");
            }
            var signupToken = TokenHelper.GenerateSignupToken();
            await _userService.CreateUser(model, signupToken);
            await _emailService.SendEmail(new EmailData(signupToken, model.Email, EmailTypeEnum.Verification));
            
            return Ok();
        }

        [HttpPatch]
        public async Task<IActionResult> Patch([FromBody] JsonPatchDocument<User> patchDoc)
        {
            var userId = User.GetUserId();

            await _loggingService.SaveAuditLog($"Updating user with user id {userId} process initiated.",
                AuditActionEnum.Update);
            if (patchDoc == null)
            {
                return BadRequest(ModelState);
            }

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
                var existingUserWithSubmittedEmail = await _userRepository.GetUserByEmail(user.Email);
                if (existingUserWithSubmittedEmail != null)
                {
                    return BadRequest("Den angivede email er tilknyttet en eksisterende bruger.");
                }
            }

            await _userRepository.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(PasswordModel model)
        {
            var userId = User.GetUserId();
            await _loggingService.SaveAuditLog($"Updating password for user with user id {userId} process initiated.",
                AuditActionEnum.Update);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.FirstError());
            }

            var user = await _userRepository.GetUserById(userId);
            if (user == null)
            {
                return BadRequest();
            }

            var submittedCurrentPasswordIsValid = user.ValidatePassword(model.CurrentPassword);
            if (!submittedCurrentPasswordIsValid)
            {
                return BadRequest("Den angivede adgangskode er forkert.");
            }

            var encryptionModel = EncryptionHelper.EncryptWithSalt(model.NewPassword);
            user.Password = encryptionModel.Text;
            user.Salt = encryptionModel.Salt;

            await _userRepository.SaveChangesAsync();

            return Ok();

        }
        private async Task<bool> IsEmailUnique(string email)
        {
            var user = await _userRepository.GetUserByEmail(email);
            return user == null;
        }
    }
}
