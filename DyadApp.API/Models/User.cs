using System;
using System.Collections.Generic;

namespace DyadApp.API.Models
{
    public class User : EntityBase
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserPassword Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte[] ProfileImage { get; set; }
        public bool Verified { get; set; }
        public List<Signup> Signups { get; set; }
    }
}