using System;
using System.Threading.Tasks;
using DyadApp.API.Converters;
using DyadApp.API.Data.Repositories;
using DyadApp.API.Models;
using DyadApp.API.ViewModels;

namespace DyadApp.API.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task CreateUser(CreateUserModel model, string signupToken)
        {
            var user = model.ToUser();
            user.Signups.Add(new Signup
            {
                Token = signupToken,
                ExpirationDate = DateTime.Now.AddDays(2),
                AcceptDate = null
            });

            await _userRepository.CreateAsync(user);
        }
    }
}
