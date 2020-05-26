using System;

namespace DyadApp.API.Models
{
    public class Signup : EntityBase
    {
        public int SignupId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime? AcceptDate { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}