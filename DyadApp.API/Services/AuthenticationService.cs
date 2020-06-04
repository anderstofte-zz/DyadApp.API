using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DyadApp.API.Data.Repositories;
using DyadApp.API.Extensions;
using DyadApp.API.Helpers;
using DyadApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DyadApp.API.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthenticationService(IAuthenticationRepository authenticationRepository, IConfiguration configuration, IUserRepository userRepository)
        {
            _authenticationRepository = authenticationRepository;
            _configuration = configuration;
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Authenticate(string email, string password)
        {
            var user = await _authenticationRepository.GetUserByEmail(email);
            if (user == null)
            {
                return new BadRequestObjectResult("Den indtastede email findes ikke i systemet.");
            }

            var isSubmittedPasswordValid = user.ValidatePassword(password);
            if (!isSubmittedPasswordValid)
            {
                return new BadRequestObjectResult("Den indtastede adgangskode er forkert.");
            }

            return new OkObjectResult(await GenerateTokens(user.UserId));
        }

        public async Task<Signup> GetSignup(string token)
        {
            var signup = await _authenticationRepository.GetSignupByToken(token);
            if (signup == null)
            {
                return null;
            }

            var signupIsExpired = signup.ExpirationDate < DateTime.Now;
            var signupAlreadyAccepted = signup.AcceptDate != null;
            if (signupIsExpired || signupAlreadyAccepted)
            {
                return null;
            }

            return signup;
        }

        public async Task<IActionResult> VerifySignup(Signup signup)
        {
            var user = await _userRepository.GetUserById(signup.UserId);
            signup.AcceptDate = DateTime.Now;
            user.Verified = true;

            try
            {
                await _userRepository.SaveChangesAsync();
                await _authenticationRepository.SaveChangesAsync();
                return new OkResult();
            }
            catch (Exception)
            {
                return new BadRequestResult();
            }
        }

        public async Task<AuthenticationTokens> GenerateTokens(int userId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var accessToken = tokenHandler.CreateToken(SetUpToken(userId));
            var refreshToken = RefreshTokenHelper.Generate(userId);

            await _authenticationRepository.CreateToken(refreshToken);

            var authenticationTokens = new AuthenticationTokens
            {
                AccessToken = tokenHandler.WriteToken(accessToken),
                RefreshToken = refreshToken.Token
            };

            return authenticationTokens;
        }


        private SecurityTokenDescriptor SetUpToken(int userId)
        {
            var key = Encoding.ASCII.GetBytes(_configuration.GetSecretKey());
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, userId.ToString())
                }),
                Expires = DateTime.Now.AddMinutes(30),
                IssuedAt = DateTime.Now,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
        }
    }
}
