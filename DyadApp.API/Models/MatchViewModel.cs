using System;

namespace DyadApp.API.Models
{
	public class MatchViewModel
	{
		public int UserId { get; set; }
        public int MatchId { get; set; }
		public string Name { get; set; }
		public string ProfileImage { get; set; }
		public string LastMessage { get; set; }
		public DateTime LastMessageTimeStamp { get; set; }
        public DateTime MatchCreated { get; set; }
        public int UnreadMessages { get; set; }
	}
}
