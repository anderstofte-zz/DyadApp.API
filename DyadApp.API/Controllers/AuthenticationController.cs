using System;
using System.Linq;
using System.Threading.Tasks;
using DyadApp.API.Data;
using DyadApp.API.Data.Repositories;
using DyadApp.API.Extensions;
using DyadApp.API.Helpers;
using DyadApp.API.Models;
using DyadApp.API.Services;
using DyadApp.API.ViewModels;
using DyadApp.Emails;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        private readonly DyadAppContext _context;
        public AuthenticationController(IAuthenticationService authenticationService, ISecretKeyService keyService, IEmailService emailService, IAuthenticationRepository authenticationRepository, IUserRepository userRepository, DyadAppContext context)
        {
            _authenticationService = authenticationService;
            _keyService = keyService;
            _emailService = emailService;
            _authenticationRepository = authenticationRepository;
            _userRepository = userRepository;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AuthenticateUser([FromBody] AuthenticationUserModel model)
        {
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
            var signup = await _authenticationRepository.GetSignupByToken(model.Token);

            if (signup == null)
            {
                return BadRequest("Signup token is invalid.");
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
                return BadRequest("Access token is invalid.");
            }

            var refreshToken = await GetRefreshToken(authenticationTokens.RefreshToken, userId);
            if (refreshToken == null)
            {
                return BadRequest("Refresh token is invalid.");
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
        public async Task<IActionResult> ResetPassword(ResetPasswordRequestModel model)
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
        public async Task<IActionResult> UpdatePassword(UpdatePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.FirstError());
            }

            var user = await _userRepository.GetUserForPasswordUpdate(model.Token, model.Email);
            if (user == null)
            {
                return BadRequest("Der findes ingen brugere med den indtastede email.");
            }

            if (user.ResetPasswordTokens.Count == 0)
            {
                return BadRequest("Der opstod en fejl! Prøv igen eller kontakt support.");
            }

            var hashedPassword = PasswordHelper.GenerateHashedPassword(model.NewPassword);
            user.Password = hashedPassword.Password;
            user.Salt = hashedPassword.Salt;

            _context.ResetPasswordTokens.Remove(user.ResetPasswordTokens.First());

            await _userRepository.UpdateAsync(user);

            return Ok();
        }
    }
}
