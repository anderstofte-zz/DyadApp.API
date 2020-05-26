using System;

namespace DyadApp.API.Models
{
    public class RefreshToken : EntityBase
    {
        public int RefreshTokenId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}