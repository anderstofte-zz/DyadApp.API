using System.Text;
using DyadApp.API.Models;
using DyadApp.API.ViewModels;

namespace DyadApp.API.Converters
{
    public static class UserConverter
    {
        public static User ToUser(this CreateUserModel model)
        {
            return new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = model.Password,
                ProfileImage = Encoding.ASCII.GetBytes(model.ProfileImage),
                DateOfBirth = model.DateOfBirth
            };
        }
    }
}
