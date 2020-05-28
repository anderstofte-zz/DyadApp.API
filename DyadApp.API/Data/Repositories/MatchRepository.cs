using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DyadApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DyadApp.API.Data.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly DyadAppContext _context;

        public MatchRepository(DyadAppContext context)
        {
            _context = context;
        }

        public async Task AddMatch(Match match)
        {
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
        }

        public async Task AddAwaitingMatch(AwaitingMatch awaitingMatch)
        {
            _context.AwaitingMatches.Add(awaitingMatch);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAwaitingMatch(AwaitingMatch awaitingMatch)
        {
            _context.AwaitingMatches.Update(awaitingMatch);
            await _context.SaveChangesAsync();
        }

        public async Task<AwaitingMatch> GetAwaitingMatchByUserId(int userId)
        {
            return await _context.AwaitingMatches.SingleOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<List<AwaitingMatch>> GetAwaitingMatches()
        {
            return await _context.AwaitingMatches.Where(x => !x.IsMatched).Include(x => x.User).ToListAsync();
        }

        public async Task<List<Match>> GetMatches(int userId)
        {
            return await _context.Matches
                .Include(x => x.UserMatches)
                .ThenInclude(um => um.User)
                .Include(x => x.ChatMessages)
                .Where(x => x.UserMatches.Any(userMatch => userMatch.UserId == userId))
                .ToListAsync();
        }
    }
}
