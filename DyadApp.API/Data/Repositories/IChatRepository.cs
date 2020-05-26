using System.Collections.Generic;
using System.Threading.Tasks;
using DyadApp.API.Models;

namespace DyadApp.API.Data.Repositories
{
    public interface IChatRepository
    {
        Task AddMessage(ChatMessage model);
        Task<Match> FetchChatMessages(int matchId);
        Task UpdateChatMessages(List<ChatMessage> chatMessages);
        Task<Match> FetchMatch(int matchId);
    }
}