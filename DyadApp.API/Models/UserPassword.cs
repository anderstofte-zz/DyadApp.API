using System.ComponentModel.DataAnnotations;

namespace DyadApp.API.Models
{
    public class UserPassword : EntityBase
    {
        [Key]
        public int UserPasswordId { get; set; }
        public int UserId { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}