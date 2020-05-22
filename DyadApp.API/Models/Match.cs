using System.Collections.Generic;

namespace DyadApp.API.Models
{
	public class Match : EntityBase
	{
		public int MatchId { get; set; }
        public List<UserMatch> UserMatches { get; set; }
        public List<ChatMessage> ChatMessages { get; set; }
    }
}
