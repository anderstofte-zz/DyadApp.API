using System.Collections.Generic;

namespace DyadApp.API.Models
{
	public class Match: EntityBase
	{
		public int MatchId { get; set; }
		public int PrimaryUserId { get; set; }
		public int SecondaryUserId { get; set; }

		public User User { get; set; }
		public List<ChatMessage> Messages { get; set; }
	}
}
