using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DyadApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DyadApp.API.Data.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly DyadAppContext _context;

        public ChatRepository(DyadAppContext context)
        {
            _context = context;
        }

        public async Task AddMessage(ChatMessage model)
        {
            _context.ChatMessages.Add(model);
            await _context.SaveChangesAsync();
        }

        public async Task<Match> FetchChatMessages(int matchId)
        {
            return await _context.Matches
                .Include(x => x.ChatMessages)
                .Include(x => x.UserMatches)
                .ThenInclude(um => um.User)
                .Where(x => x.MatchId == matchId)
                .SingleOrDefaultAsync();
        }

        public async Task UpdateChatMessages(List<ChatMessage> chatMessages)
        {
            _context.ChatMessages.UpdateRange(chatMessages);
            await _context.SaveChangesAsync();
        }

        public Task<Match> FetchMatch(int matchId)
        {
            return _context.Matches.Include(x => x.ChatMessages).Where(x => x.MatchId == matchId)
                .SingleOrDefaultAsync();
        }
    }
}
