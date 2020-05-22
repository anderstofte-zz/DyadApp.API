using DyadApp.API.Data;
using DyadApp.API.Models;
using System.Threading.Tasks;
using DyadApp.API.ViewModels;

namespace DyadApp.API.Hubs
{
	public class ChatMethods
	{
		private readonly DyadAppContext _context;

		public ChatMethods(DyadAppContext context)
		{
			_context = context;
		}

		public async Task AddMessage(NewChatMessageModel model)
		{
            var chatMessage = new ChatMessage {MatchId = model.MatchId, Message = model.Message, SenderId = model.SenderId, ReceiverId = model.ReceiverId};
            _context.ChatMessages.Add(chatMessage);
			await _context.SaveChangesAsync();

		}
	}
}
