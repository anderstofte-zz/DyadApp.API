using DyadApp.API.Data;
using DyadApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DyadApp.API.Hubs
{
	public class ChatMethods
	{
		private readonly DyadAppContext _context;

		public ChatMethods(DyadAppContext context)
		{
			_context = context;
		}

		public void AddMessage(int SenderId, int ReceiverId, string Message)
		{
			ChatMessage chatMessage = new ChatMessage();
			chatMessage.Message = Message;
			chatMessage.SenderId = SenderId;
			chatMessage.ReceiverId = ReceiverId;

			_context.ChatMessages.Add(chatMessage);
			_context.SaveChangesAsync();

		}
	}
}
