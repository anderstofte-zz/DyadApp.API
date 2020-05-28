using System.Linq;
using System.Threading.Tasks;
using DyadApp.API.Converters;
using DyadApp.API.Data.Repositories;
using DyadApp.API.Extensions;
using DyadApp.API.Helpers;
using DyadApp.API.Models;
using DyadApp.API.ViewModels;
using Microsoft.Extensions.Configuration;

namespace DyadApp.API.Services
{
	public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly IConfiguration _configuration;
        public ChatService(IChatRepository chatRepository, IConfiguration configuration)
        {
            _chatRepository = chatRepository;
            _configuration = configuration;
        }

        public async Task AddMessage(NewChatMessageModel model)
        {
            var encryptionKey = _configuration.GetEncryptionKey();
            var chatMessage = new ChatMessage
            {
                MatchId = model.MatchId, 
                Message = EncryptionHelper.Encrypt(model.Message, encryptionKey), 
                SenderId = model.SenderId, 
                ReceiverId = model.ReceiverId
            };

            await _chatRepository.AddMessage(chatMessage);
        }

        public async Task<MatchConversationModel> FetchChatMessages(int matchId, int userId)
        {
            var match = await _chatRepository.FetchChatMessages(matchId);
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
