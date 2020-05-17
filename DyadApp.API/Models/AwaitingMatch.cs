namespace DyadApp.API.Models
{
	public class AwaitingMatch : EntityBase
	{
		public int AwaitingMatchId { get; set; }
		public int UserId { get; set; }
		public bool IsMatched { get; set; }
	}
}
