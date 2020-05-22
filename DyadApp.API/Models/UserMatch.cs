namespace DyadApp.API.Models
{
    public class UserMatch : EntityBase
    {
        public int UserMatchId { get; set; }
        public int UserId { get; set; }
        public int MatchId { get; set; }
        public User User { get; set; }
    }
}