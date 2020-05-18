using DyadApp.API.Data;
using DyadApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DyadApp.API.Services
{
	public class ChatService : IChatService
	{

		private readonly DyadAppContext _context;

		public ChatService(DyadAppContext context)
		{
			_context = context;
		}

		public void AddMessage(int SenderId, int ReceiverId, string Message)
		{
			ChatMessage chatMessage = new ChatMessage();
			chatMessage.Message = Message;
			chatMessage.SenderId = SenderId;
			chatMessage.ReceiverId = ReceiverId;
			chatMessage.Timestamp = DateTime.Now;
			_context.Add(chatMessage);
			_context.SaveChangesAsync();

		}

	}
}
