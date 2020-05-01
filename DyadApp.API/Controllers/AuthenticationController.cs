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
        public AuthenticationController(IAuthenticationService authenticationService, ISecretKeyService keyService, IEmailService emailService, IAuthenticationRepository authenticationRepository, IUserRepository userRepository)
        {
            _authenticationService = authenticationService;
            _keyService = keyService;
            _emailService = emailService;
            _authenticationRepository = authenticationRepository;
            _userRepository = userRepository;
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticateUser(AuthenticationUserModel model)
        {
            var authenticationTokens = await _authenticationService.Authenticate(model.Email, model.Password);

            if (authenticationTokens == null)
            {
                return Unauthorized();
            }

            return Ok(authenticationTokens);
        }

        [HttpPost("VerifySignupToken")]
        public async Task<IActionResult> VerifyUser([FromBody] string token)
        {
            var signup = await _authenticationRepository.GetSignupByToken(token);

            if (signup == null)
            {
                return Unauthorized("Signup token is invalid.");
            }

            var user = await _userRepository.GetUserById(signup.UserId);

            signup.AcceptDate = DateTime.UtcNow;
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
            int userId;
            try
            {
                userId = authenticationTokens.GetUserIdFromClaims(_keyService.GetSecretKey());
            }
            catch (Exception)
            {
                return Unauthorized("Access token is invalid.");
            }

            var refreshToken = await GetRefreshToken(authenticationTokens.RefreshToken, userId);
            if (refreshToken == null)
            {
                return Unauthorized("Refresh token is invalid.");
            }

            var newTokens = await _authenticationService.GenerateTokens(userId);
            await _authenticationRepository.DeleteTokenAsync(refreshToken);

            return Ok(newTokens);
        }

        private async Task<RefreshToken> GetRefreshToken(string refreshToken, int userId)
        {
            return await _authenticationRepository.GetRefreshToken(userId, refreshToken);
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordRequestModel model)
        {
            var user = await _userRepository.GetByEmail(model.Email);

            if (user == null)
            {
                return BadRequest();
            }

            var token = TokenHelper.GenerateResetPasswordToken();

            var resetPasswordToken = new ResetPasswordToken
            {
                Token = token,
                ExpirationDate = DateTime.UtcNow.AddMinutes(30),
                UserId = user.UserId
            };

            await _authenticationRepository.CreateTokenAsync(resetPasswordToken);

            var resetPasswordRequest = new ResetPasswordRequest
            {
                Email = model.Email,
                Token = token
            };

            return await _emailService.SendEmail(resetPasswordRequest, EmailTypeEnum.PasswordRecovery);
        }

        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordModel model)
        {
            var resetPasswordToken = await _authenticationRepository.GetResetPasswordToken(model.Token);
            if (resetPasswordToken == null)
            {
                return BadRequest();
            }

            var userPassword = await _userRepository.GetUserPasswordByUserId(resetPasswordToken.UserId);
            if (userPassword == null)
            {
                return BadRequest();
            }

            var hashedPassword = PasswordHelper.GenerateHashedPassword(model.Password);
            userPassword.Password = hashedPassword.Password;
            userPassword.Salt = hashedPassword.Salt;

            await _authenticationRepository.DeleteTokenAsync(resetPasswordToken);

            return Ok();
        }
    }
}
