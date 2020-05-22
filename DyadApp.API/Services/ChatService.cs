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

		public void AddMessage(int senderId, int receiverId, string message)
		{
			ChatMessage chatMessage = new ChatMessage
			{
				Message = message,
				SenderId = senderId,
				ReceiverId = receiverId
			};
			_context.Add(chatMessage);
			_context.SaveChangesAsync();

		}

	}
}
