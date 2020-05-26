using System;
using System.Threading.Tasks;
using DyadApp.API.Data.Repositories;
using DyadApp.API.Extensions;
using DyadApp.API.Helpers;
using DyadApp.API.Models;
using DyadApp.API.Services;
using DyadApp.API.ViewModels;
using DyadApp.Emails;
using Microsoft.AspNetCore.Mvc;

namespace DyadApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly ISecretKeyService _keyService;
        private readonly IEmailService _emailService;
        private readonly ILoggingService _loggingService;
        public AuthenticationController(IAuthenticationService authenticationService, ISecretKeyService keyService, IEmailService emailService, IAuthenticationRepository authenticationRepository, IUserRepository userRepository, ILoggingService loggingService)
        {
            _authenticationService = authenticationService;
            _keyService = keyService;
            _emailService = emailService;
            _authenticationRepository = authenticationRepository;
            _userRepository = userRepository;
            _loggingService = loggingService;
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticationUserModel model)
        {
            await _loggingService.SaveAuditLog("Login process initiated", AuditActionEnum.Login);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.FirstError());
            }
            
            var user = await _authenticationRepository.GetUserCredentialsByEmail(model.Email);
            if (user == null || !user.ValidatePassword(model.Password))
            {
                return BadRequest("Der findes ingen brugere med de indtastede oplysninger.");
            }

            if (!user.Verified)
            {
                return BadRequest("Kontoen er ikke verificeret. Tjek din indbakke.");
            }

            var tokens = await _authenticationService.GenerateTokens(user.UserId);
            return Ok(tokens);
        }

        [HttpPost("VerifySignupToken")]
        public async Task<IActionResult> VerifyUser([FromBody] SignupTokenModel model)
        {
            await _loggingService.SaveAuditLog($"Verifying sign up token: {model.Token}.", AuditActionEnum.Signup);
            var signup = await _authenticationRepository.GetSignupByToken(model.Token);

            if (signup == null)
            {
                return BadRequest("Signup token is invalid.");
            }

            var user = await _userRepository.GetUserById(signup.UserId);

            signup.AcceptDate = DateTime.Now;
            user.Verified = true;

            try
            {
                await _userRepository.SaveChangesAsync();
                await _authenticationRepository.SaveChangesAsync();
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpPost("Refresh")]
        public async Task<IActionResult> RefreshTokens([FromBody]AuthenticationTokens authenticationTokens)
        {
            await _loggingService.SaveAuditLog($"Refreshing tokens for user with user id {User.GetUserId()}",
                AuditActionEnum.TokenRefresh);
            int userId;
            try
            {
                userId = authenticationTokens.GetUserIdFromClaims(_keyService.GetSecretKey());
            }
            catch (Exception)
            {
                return BadRequest("Access token is invalid.");
            }

            var refreshToken = await GetRefreshToken(authenticationTokens.RefreshToken, userId);
            if (refreshToken == null)
            {
                return BadRequest("Refresh token is invalid.");
            }

            var newTokens = await _authenticationService.GenerateTokens(userId);

            await _loggingService.SaveAuditLog($"Deleting old refresh token for user with user id {userId}",
                AuditActionEnum.Delete);
            await _authenticationRepository.DeleteTokenAsync(refreshToken);

            return Ok(newTokens);
        }

        private async Task<RefreshToken> GetRefreshToken(string refreshToken, int userId)
        {
            await _loggingService.SaveAuditLog($"Reading refresh token for user with id {userId}",
                AuditActionEnum.Read);
            return await _authenticationRepository.GetRefreshToken(userId, refreshToken);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestModel model)
        {
            await _loggingService.SaveAuditLog($"Retrieving user with email {model.Email}", AuditActionEnum.Read);

            var user = await _userRepository.GetByEmail(model.Email);

            if (user == null)
            {
                return BadRequest();
            }

            var token = TokenHelper.GenerateResetPasswordToken();

            var resetPasswordToken = new ResetPasswordToken
            {
                Token = token,
                ExpirationDate = DateTime.Now.AddMinutes(30),
                UserId = user.UserId
            };

            await _loggingService.SaveAuditLog($"Creating reset-password token for user with user id {user.UserId}",
                AuditActionEnum.Create);
            await _authenticationRepository.CreateTokenAsync(resetPasswordToken);

            var resetPasswordRequest = new ResetPasswordRequest
            {
                Email = model.Email,
                Token = token
            };

            return await _emailService.SendEmail(resetPasswordRequest, EmailTypeEnum.PasswordRecovery);
        }

        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(UpdatePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.FirstError());
            }

            await _loggingService.SaveAuditLog($"Retrieving user with email {model.Email} for updating password.",
                AuditActionEnum.Read);
            var user = await _userRepository.GetUserForPasswordUpdate(model.Token, model.Email);
            if (user == null)
            {
                return BadRequest("Der findes ingen brugere med den indtastede email.");
            }

            var resetToken = user.ResetPasswordTokens.Find(x => x.Token == model.Token);
            if (resetToken == null)
            {
                return BadRequest("Din forespørgsel er udløbet. Foretag en ny.");
            }

            var hashedPassword = PasswordHelper.GenerateHashedPassword(model.NewPassword);
            user.Password = hashedPassword.Password;
            user.Salt = hashedPassword.Salt;

            await _loggingService.SaveAuditLog($"Updating user with user id {user.UserId}.", AuditActionEnum.Update);
            await _userRepository.UpdateAsync(user);
            await _loggingService.SaveAuditLog($"Deleting reset token for user with user id {user.UserId}",
                AuditActionEnum.Delete);
            await _authenticationRepository.DeleteTokenAsync(resetToken);

            return Ok();
        }
    }
}
