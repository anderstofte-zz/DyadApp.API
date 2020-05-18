using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DyadApp.API.Models
{
	public class ChatMessage
	{
		public int Id { get; set; }
		public string Message { get; set; }
		public DateTime Timestamp { get; set; }
		public int SenderId { get; set; }
		public int ReceiverId { get; set; }

	}
}
