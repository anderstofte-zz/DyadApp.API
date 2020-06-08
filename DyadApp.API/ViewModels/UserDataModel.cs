using System;
using System.Collections.Generic;

namespace DyadApp.API.ViewModels
{
    public class UserDataModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ProfileImage { get; set; }
        public DateTime AccountCreated { get; set; }
        public List<MatchDataModel> Matches { get; set; }
    }
}
