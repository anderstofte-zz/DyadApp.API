using System;

namespace DyadApp.API.Models
{
    public class User : EntityBase
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        public byte[] ProfileImage { get; set; }
    }
}