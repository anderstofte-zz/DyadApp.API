using Microsoft.AspNetCore.Authentication;

namespace DyadApp.API.Models
{
	public class ChatMessage : EntityBase
	{
		public int ChatMessageId { get; set; }
        public int MatchId { get; set; }
		public string Message { get; set; }
		public int SenderId { get; set; }
		public int ReceiverId { get; set; }
		public bool IsRead { get; set; }
	}
}
