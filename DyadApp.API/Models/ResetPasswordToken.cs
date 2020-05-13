using System;

namespace DyadApp.API.Models
{
    public class ResetPasswordToken : EntityBase
    {
        public int ResetPasswordTokenId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}