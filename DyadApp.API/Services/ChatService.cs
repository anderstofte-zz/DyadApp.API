using System.Linq;
using System.Threading.Tasks;
using DyadApp.API.Converters;
using DyadApp.API.Data.Repositories;
using DyadApp.API.Models;
using DyadApp.API.ViewModels;

namespace DyadApp.API.Services
{
	public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;

        public ChatService(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }

        public async Task AddMessage(NewChatMessageModel model)
		{
            var chatMessage = new ChatMessage
            {
                MatchId = model.MatchId, 
                Message = model.Message, 
                SenderId = model.SenderId, 
                ReceiverId = model.ReceiverId
            };

            await _chatRepository.AddMessage(chatMessage);
        }

        public async Task<MatchConversationModel> FetchChatMessages(int matchId, int userId)
        {
            var match = await _chatRepository.FetchChatMessages(userId);
            return match.ToChatMessageModels(userId);
        }

        public async Task MarkMessagesAsRead(int matchId, int userId)
        {
            var match = await _chatRepository.FetchMatch(matchId);

            var chatMessages = match.ChatMessages.Where(x => x.ReceiverId == userId).Select(x =>
            {
                x.IsRead = true;
                return x;
            }).ToList();

            await _chatRepository.UpdateChatMessages(chatMessages);
        }
    }
}
