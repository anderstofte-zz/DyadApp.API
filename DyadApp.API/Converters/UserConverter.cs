using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using DyadApp.API.Controllers;
using DyadApp.API.Helpers;
using DyadApp.API.Models;
using DyadApp.API.ViewModels;

namespace DyadApp.API.Converters
{
    public static class UserConverter
    {
        public static User ToUser(this CreateUserModel model)
        {
            var password = PasswordHelper.GenerateHashedPassword(model.Password);
            return new User
            {
                Name = model.Name,
                Email = model.Email,
                Password = password.Password,
                Salt = password.Salt,
                ProfileImage = Convert.FromBase64String(model.ProfileImage),
                DateOfBirth = model.DateOfBirth,
                Verified = false,
                Active = true,
                Signups = new List<Signup>()
            };
        }

        public static UserProfileModel ToUserProfileModel(this User user)
        {
            return new UserProfileModel
            {
                Name = user.Name,
                Email = user.Email,
                ProfileImage = Convert.ToBase64String(user.ProfileImage) 
            };
        }

        public static UserDataModel ToUserDataModel(this User user, List<Match> matches, string encryptionKey)
        {
            return new UserDataModel
            {
                Name = user.Name,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
                AccountCreated = user.Created,
                ProfileImage = Convert.ToBase64String(user.ProfileImage),
                Matches = matches.Select(x => new MatchDataModel
                {
                    Created = x.Created,
                    Messages = x.ChatMessages.Select(cm => new ChatMessageDataModel
                    {
                        Message = EncryptionHelper.Decrypt(cm.Message, encryptionKey),
                        Sent = cm.Created
                    }).ToList()
                }).ToList()
            };
        }
    }
}
