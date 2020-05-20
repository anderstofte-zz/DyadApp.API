namespace DyadApp.API.Models
{
	public class Match: EntityBase
	{
		public int MatchId { get; set; }
		public int PrimaryUserId { get; set; }
		public int SecondaryUserId { get; set; }
	}
}
